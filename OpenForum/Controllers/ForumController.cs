using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using OpenForum.Core.DataAccess.Interfaces;
using OpenForum.Core.Models;
using OpenForum.Core.Properties;
using OpenForum.Core.ViewModels;
using OpenForum.Core.ViewModels.Interfaces;

namespace OpenForum.Core.Controllers
{
	public class ForumController : Controller
	{
		private static readonly byte[] IMAGE_BYTES;

		private IViewModelFactory _viewModelFactory;
		private IPostRepository _postRepository;
		private IUserRepository _userRepository;

		public bool IncludeDefaultStyles { get; set; }
		public bool IncludeValidationSummary { get; set; }
		public bool IncludeWysiwygEditor { get; set; }


		static ForumController()
		{
			using (MemoryStream stream = new MemoryStream())
			{
				Resources.nicEditorIcons.Save(stream, Resources.nicEditorIcons.RawFormat);
				stream.Position = 0;
				IMAGE_BYTES = new byte[stream.Length];
				stream.Read(IMAGE_BYTES, 0, (int)stream.Length);
			}
		}

		public ForumController(IViewModelFactory viewModelFactory, IPostRepository postRepository, IUserRepository userRepository)
		{
			_viewModelFactory = viewModelFactory;
			_postRepository = postRepository;
			_userRepository = userRepository;

			IncludeDefaultStyles = Configurations.Current.IncludeDefaultStyles;
			IncludeValidationSummary = Configurations.Current.IncludeValidationSummary;
			IncludeWysiwygEditor = Configurations.Current.IncludeWysiwygEditor;

			ItemsPerPage = 20;
		}

		public ForumController(IPostRepository postRepository)
			: this(Configurations.Current.ViewModelFactory, postRepository, Configurations.Current.UserRepository)
		{
		}

		public ForumController()
			: this(Configurations.Current.ViewModelFactory, Configurations.Current.PostRepository, Configurations.Current.UserRepository)
		{
		}

		public int ItemsPerPage { get; set; }

		public ActionResult Index(string searchQuery, int? page)
		{
			IEnumerable<Post> posts;
			string message = "";

			if (string.IsNullOrEmpty(searchQuery))
			{
				posts = _postRepository.Find();
			}
			else
			{
				posts = _postRepository.Search(searchQuery);
				message = string.Format("Search results for \"{0}\".", searchQuery);
			}

			int currentPage = page ?? 0;
			int postCount = posts.Count();
			int totalPages = (int)Math.Ceiling((double)postCount / (double)ItemsPerPage);

			if (postCount == 0)
			{
				if (string.IsNullOrEmpty(searchQuery))
				{
					message = "There are currently no posts in this forum.";
				}
				else
				{
					message = string.Format("Sorry, there are no results for \"{0}\".", searchQuery);
				}
			}

			IndexViewModel viewModel = new IndexViewModel()
			{
				PageTitle = "Forum",
				SearchQuery = searchQuery,
				Message = message,
				Posts = posts.GetPage(page, ItemsPerPage),
				CurrentPage = currentPage,
				TotalPages = totalPages,
				IncludeDefaultStyles = IncludeDefaultStyles,
				IncludeValidationSummary = IncludeValidationSummary,
			};

			return View(_viewModelFactory.GetIndexViewModel(viewModel));
		}

		public ActionResult View(int id, string title)
		{
			Post post = FindPostHelper(id);
			post.IncrementViewCount();
			_postRepository.Submit(post);

			ViewViewModel viewModel = new ViewViewModel()
			{
				PageTitle = post.Title,
				Post = post,
				CurrentUser = _userRepository.FindCurrentUser(),
				IncludeDefaultStyles = IncludeDefaultStyles,
				IncludeValidationSummary = IncludeValidationSummary,
			};

			return View(_viewModelFactory.GetViewViewModel(viewModel));
		}

		[Authorize]
		public ActionResult Post(int? id)
		{
			Post post = FindPostHelper(id);

			PostViewModel viewModel = new PostViewModel()
			{
				PageTitle = post.Title ?? "New Post",
				Post = post,
				IncludeDefaultStyles = IncludeDefaultStyles,
				IncludeValidationSummary = IncludeValidationSummary,
				IncludeWysiwygEditor = IncludeWysiwygEditor,
			};

			return View(_viewModelFactory.GetPostViewModel(viewModel));
		}

		[Authorize]
		public ActionResult Reply(int? id, int postId)
		{
			Post post = FindPostHelper(postId);
			Reply reply = FindReplyHelper(id, post);

			ReplyViewModel viewModel = new ReplyViewModel()
			{
				PageTitle = post.Title,
				Post = post,
				Reply = reply,
				IncludeDefaultStyles = IncludeDefaultStyles,
				IncludeValidationSummary = IncludeValidationSummary,
				IncludeWysiwygEditor = IncludeWysiwygEditor,
			};

			return View(_viewModelFactory.GetReplyViewModel(viewModel));
		}

		[Authorize]
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Post(int? id, string title, string body)
		{
			Post post = FindPostHelper(id);

			if (id != null && post.CreatedById != _userRepository.FindCurrentUser().Id)
			{
				throw new OpenForumException("This post cannont be editted by the current user. (Post Id = {0}; Author = {1}; Current User = {2})", post.Id, post.CreatedById, _userRepository.FindCurrentUser().Id);
			}

			post.Title = title;
			post.Body = body;

			try
			{
				post.Validate();
			}
			catch (OpenForumException exception)
			{
				ModelState.AddModelError("_FORM", exception.Message);

				PostViewModel viewModel = new PostViewModel()
				{
					PageTitle = post.Title ?? "New Post",
					Post = post,
					IncludeDefaultStyles = IncludeDefaultStyles,
					IncludeValidationSummary = IncludeValidationSummary,
					IncludeWysiwygEditor = IncludeWysiwygEditor,
				};

				return View(_viewModelFactory.GetPostViewModel(viewModel));
			}

			_postRepository.Submit(post);
			TempData["Post"] = post;

			return RedirectToAction("View", new { id = post.Id });
		}

		[Authorize]
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Reply(int? id, int postId, string body)
		{
			Post post = FindPostHelper(postId);
			Reply reply = FindReplyHelper(id, post);

			if (id != null && reply.CreatedBy.Id != _userRepository.FindCurrentUser().Id)
			{
				throw new OpenForumException("This reply cannont be editted by the current user. (Reply Id = {0}; Author = {1}; Current User = {2})", reply.Id, reply.CreatedBy.Id, _userRepository.FindCurrentUser().Id);
			}

			reply.Body = body;

			try
			{
				reply.Validate();
			}
			catch (OpenForumException exception)
			{
				ModelState.AddModelError("_FORM", exception.Message);

				ReplyViewModel viewModel = new ReplyViewModel()
				{
					PageTitle = post.Title,
					Post = post,
					Reply = reply,
					IncludeDefaultStyles = IncludeDefaultStyles,
					IncludeValidationSummary = IncludeValidationSummary,
					IncludeWysiwygEditor = IncludeWysiwygEditor,
				};

				return View(_viewModelFactory.GetReplyViewModel(viewModel));
			}

			_postRepository.Submit(post);

			return RedirectToAction("View", new { id = reply.Post.Id });
		}

		public ActionResult Script()
		{
			return File(Encoding.Default.GetBytes(Resources.nicEdit), "application/javascript");
		}

		public ActionResult Image()
		{
			return File(IMAGE_BYTES, "image/gif");
		}

		private Post FindPostHelper(int? id)
		{
			if (id == null || id == 0)
			{
				return new Post();
			}

			Post post = _postRepository.FindById(id.Value);

			if (post == null)
			{
				throw new OpenForumException("The requested forum post could not be found. (Post id = {0})", id);
			}

			return post;
		}

		private Reply FindReplyHelper(int? id, Post post)
		{
			if (id == null || id == 0)
			{
				return post.AddReply();
			}

			Reply reply = post.FindReplyById(id.Value);

			if (reply == null)
			{
				throw new OpenForumException("The requested forum reply could not be found. (Reply id = {0})", id);
			}

			return reply;
		}
	}
}

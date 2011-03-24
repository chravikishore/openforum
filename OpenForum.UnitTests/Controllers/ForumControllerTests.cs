using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OpenForum.Core;
using OpenForum.Core.Controllers;
using OpenForum.Core.DataAccess.Interfaces;
using OpenForum.Core.Models;
using OpenForum.Core.ViewModels;

namespace OpenForum.UnitTests.Controllers
{
	[TestClass]
	public class ForumControllerTests
	{
		[TestMethod]
		public void View_All_Posts_For_Index_Page()
		{
			List<Post> posts = new List<Post>()
			{
				new Post(), new Post(), new Post()
			};

			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.Find()).Returns(posts.AsQueryable());

			ForumController controller = new ForumController(postRepository.Object);

			ViewResult result = controller.Index(null, null) as ViewResult;
			IndexViewModel resultViewData = (IndexViewModel)result.ViewData.Model;

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual(3, resultViewData.Posts.Count());
			Assert.IsTrue(resultViewData.IncludeDefaultStyles);
			Assert.IsTrue(resultViewData.IncludeValidationSummary);
		}

		[TestMethod]
		public void View_Correct_Post_For_View_Page()
		{
			Post post = new Post();
			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.FindById(1)).Returns(post);

			ForumController controller = new ForumController(postRepository.Object);

			ViewResult result = controller.View(1, "title") as ViewResult;
			ViewViewModel resultViewData = (ViewViewModel)result.ViewData.Model;

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual(post, resultViewData.Post);
			Assert.IsTrue(resultViewData.IncludeDefaultStyles);
			Assert.IsTrue(resultViewData.IncludeValidationSummary);
		}

		[TestMethod]
		public void Throw_Exception_When_Requesting_An_Invalid_Post_Id()
		{
			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.FindById(1)).Returns<Post>(null);

			ForumController controller = new ForumController(postRepository.Object);

			try
			{
				controller.View(1, "title");
				Assert.Fail("Exception should have been thrown");
			}
			catch (OpenForumException exception)
			{
				Assert.AreEqual("The requested forum post could not be found. (Post id = 1)", exception.Message);
			}
		}

		[TestMethod]
		public void View_Create_New_Post_Page()
		{
			ForumController controller = new ForumController();

			ViewResult result = controller.Post(null) as ViewResult;
			PostViewModel resultViewData = (PostViewModel)result.ViewData.Model;

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual(0, resultViewData.Post.Id);
			Assert.IsNull(resultViewData.Post.Title);
			Assert.IsNull(resultViewData.Post.Body);
			Assert.IsTrue(resultViewData.IncludeDefaultStyles);
			Assert.IsTrue(resultViewData.IncludeValidationSummary);
			Assert.IsTrue(resultViewData.IncludeWysiwygEditor);
		}

		[TestMethod]
		public void View_Create_Response_Post_Page()
		{
			Post post = new Post("title", "body");
			post.SetId(1);

			Reply reply = post.AddReply();
			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.FindById(1)).Returns(post);

			ForumController controller = new ForumController(postRepository.Object);

			ViewResult result = controller.Reply(null, 1) as ViewResult;
			ReplyViewModel resultViewData = (ReplyViewModel)result.ViewData.Model;

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual(1, resultViewData.Reply.Post.Id);
			Assert.IsNull(resultViewData.Reply.Body);
			Assert.IsTrue(resultViewData.IncludeDefaultStyles);
			Assert.IsTrue(resultViewData.IncludeValidationSummary);
			Assert.IsTrue(resultViewData.IncludeWysiwygEditor);
		}

		[TestMethod]
		public void View_Edit_Post_Page()
		{
			Post post = new Post("title", "body");
			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.FindById(1)).Returns(post);

			ForumController controller = new ForumController(postRepository.Object);

			ViewResult result = controller.Post(1) as ViewResult;
			PostViewModel resultViewData = (PostViewModel)result.ViewData.Model;

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual(post, resultViewData.Post);
		}

		[TestMethod]
		public void Throw_Exception_For_Invalid_Post_Id()
		{
			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.FindById(1)).Returns<Post>(null);

			ForumController controller = new ForumController(postRepository.Object);

			try
			{
				controller.Post(1);
				Assert.Fail("Exception should have been thrown");
			}
			catch (OpenForumException exception)
			{
				Assert.AreEqual("The requested forum post could not be found. (Post id = 1)", exception.Message);
			}
		}

		[TestMethod]
		public void Throw_Exception_For_Invalid_Parent_Id()
		{
			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.FindById(1)).Returns<Post>(null);

			ForumController controller = new ForumController(postRepository.Object);

			try
			{
				controller.Reply(null, 1);
				Assert.Fail("Exception should have been thrown");
			}
			catch (OpenForumException exception)
			{
				Assert.AreEqual("The requested forum post could not be found. (Post id = 1)", exception.Message);
			}
		}

		[TestMethod]
		public void Save_New_Post()
		{
			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.Submit(It.IsAny<Post>())).Verifiable();

			ForumController controller = new ForumController(postRepository.Object);

			RedirectToRouteResult result = controller.Post(null, "title", "body") as RedirectToRouteResult;

			Post post = (Post)controller.TempData["Post"];

			Assert.AreEqual("View", result.RouteValues["action"]);
			Assert.AreEqual(0, result.RouteValues["id"]);
			Assert.AreEqual("title", post.Title);
			Assert.AreEqual("body", post.Body);
			postRepository.VerifyAll();
		}

		[TestMethod]
		public void Save_New_Reply()
		{
			Post post = new Post();
			post.SetId(1);

			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.FindById(1)).Returns(post).Verifiable();
			postRepository.Setup(x => x.Submit(post)).Verifiable();

			ForumController controller = new ForumController(postRepository.Object);

			RedirectToRouteResult result = controller.Reply(null, 1, "body") as RedirectToRouteResult;

			Assert.AreEqual("View", result.RouteValues["action"]);
			Assert.AreEqual(1, result.RouteValues["id"]);
			Assert.AreEqual(1, post.Replies.Count);
			Assert.AreEqual("body", post.Replies.ElementAt(0).Body);
			postRepository.VerifyAll();
		}

		[TestMethod]
		public void Save_Existing_Post()
		{
			Post post = new Post();

			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.FindById(1)).Returns(post).Verifiable();
			postRepository.Setup(x => x.Submit(post)).Verifiable();

			ForumController controller = new ForumController(postRepository.Object);

			RedirectToRouteResult result = controller.Post(1, "title", "body") as RedirectToRouteResult;

			postRepository.VerifyAll();
			Assert.AreEqual("View", result.RouteValues["action"]);
			Assert.AreEqual(0, result.RouteValues["id"]);
			Assert.AreEqual("title", post.Title);
			Assert.AreEqual("body", post.Body);
		}

		[TestMethod]
		public void Save_Existing_Reply()
		{
			Post post = new Post();
			post.SetId(1);

			Reply reply = post.AddReply();
			reply.SetId(2);

			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.FindById(1)).Returns(post).Verifiable();
			postRepository.Setup(x => x.Submit(post)).Verifiable();

			ForumController controller = new ForumController(postRepository.Object);

			RedirectToRouteResult result = controller.Reply(2, 1, "body") as RedirectToRouteResult;

			Assert.AreEqual("View", result.RouteValues["action"]);
			Assert.AreEqual(1, result.RouteValues["id"]);
			Assert.AreEqual("body", reply.Body);
			postRepository.VerifyAll();
		}

		[TestMethod]
		public void Throw_Exception_For_Invalid_Id_Post()
		{
			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.FindById(1)).Returns<Post>(null);

			ForumController controller = new ForumController(postRepository.Object);

			try
			{
				controller.Post(1, "title", "body");
				Assert.Fail("Exception should have been thrown");
			}
			catch (OpenForumException exception)
			{
				Assert.AreEqual("The requested forum post could not be found. (Post id = 1)", exception.Message);
			}
		}

		[TestMethod]
		public void Throw_Exception_For_Invalid_Post_Id_Post()
		{
			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.FindById(1)).Returns<Post>(null);

			ForumController controller = new ForumController(postRepository.Object);

			try
			{
				controller.Reply(null, 1, "body");
				Assert.Fail("Exception should have been thrown");
			}
			catch (OpenForumException exception)
			{
				Assert.AreEqual("The requested forum post could not be found. (Post id = 1)", exception.Message);
			}
		}

		[TestMethod]
		public void Viewing_A_Post_Increments_The_Views_Counter()
		{
			Post post = new Post();

			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.FindById(1)).Returns(post);
			postRepository.Setup(x => x.Submit(post)).Verifiable();

			ForumController controller = new ForumController(postRepository.Object);

			ViewResult result = controller.View(1, "title") as ViewResult;
			ViewViewModel resultViewData = (ViewViewModel)result.ViewData.Model;

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual(post, resultViewData.Post);
			Assert.AreEqual(1, resultViewData.Post.ViewCount);

			postRepository.VerifyAll();
		}

		[TestMethod]
		public void Failing_To_Set_The_Body_Or_Title_Of_A_Post_Causes_An_Error_Case()
		{
			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();

			ForumController controller = new ForumController(postRepository.Object);

			ViewResult result = controller.Post(null, null, null) as ViewResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual(false, controller.ModelState.IsValid);
		}

		[TestMethod]
		public void Failing_To_Set_The_Body_Or_Title_Of_A_Reply_Causes_An_Error_Case()
		{
			Post post = new Post();

			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.FindById(1)).Returns(post);

			ForumController controller = new ForumController(postRepository.Object);

			ViewResult result = controller.Reply(null, 1, null) as ViewResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual(false, controller.ModelState.IsValid);
		}

		[TestMethod]
		public void Index_Results_Can_Be_Paged()
		{
			List<Post> posts = new List<Post>()
			{
				new Post(), new Post(), new Post()
			};

			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.Find()).Returns(posts.AsQueryable());

			ForumController controller = new ForumController(postRepository.Object);
			controller.ItemsPerPage = 2;

			ViewResult result = controller.Index(null, 1) as ViewResult;
			IndexViewModel resultViewData = (IndexViewModel)result.ViewData.Model;

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual(1, resultViewData.Posts.Count());
			Assert.AreEqual(posts[2], resultViewData.Posts.ElementAt(0));
			Assert.AreEqual(1, resultViewData.CurrentPage);
			Assert.AreEqual(2, resultViewData.TotalPages);
		}

		[TestMethod]
		public void Posts_Can_Only_Be_Updated_By_The_Original_Author()
		{
			Utility.SetCurrentUser("user");

			Post post = new Post();
			Utility.SetId(post, 1);

			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.FindById(1)).Returns(post);

			Utility.SetCurrentUser("some other user");

			ForumController controller = new ForumController(postRepository.Object);

			try
			{
				controller.Post(1, "title", "body");
				Assert.Fail();
			}
			catch (OpenForumException exception)
			{
				Assert.AreEqual("This post cannont be editted by the current user. (Post Id = 1; Author = user; Current User = some other user)", exception.Message);
			}
		}

		[TestMethod]
		public void Replies_Can_Only_Be_Updated_By_The_Original_Author()
		{
			Utility.SetCurrentUser("user");

			Post post = new Post();
			Reply reply = post.AddReply();
			Utility.SetId(post, 1);
			Utility.SetId(reply, 2);

			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.FindById(1)).Returns(post);

			Utility.SetCurrentUser("some other user");

			ForumController controller = new ForumController(postRepository.Object);

			try
			{
				controller.Reply(2, 1, "body");
				Assert.Fail();
			}
			catch (OpenForumException exception)
			{
				Assert.AreEqual("This reply cannont be editted by the current user. (Reply Id = 2; Author = user; Current User = some other user)", exception.Message);
			}
		}

		[TestMethod]
		public void Request_Javascript()
		{
			ForumController controller = new ForumController();
			FileContentResult result = (FileContentResult)controller.Script();

			Assert.AreEqual("application/javascript", result.ContentType);
			Assert.AreEqual(48547, result.FileContents.Length);
		}

		[TestMethod]
		public void Request_Image()
		{
			ForumController controller = new ForumController();
			FileContentResult result = (FileContentResult)controller.Image();

			Assert.AreEqual("image/gif", result.ContentType);
			Assert.AreEqual(3370, result.FileContents.Length);
		}

		[TestMethod]
		public void Search_Returns_The_Correct_Posts()
		{
			List<Post> posts = new List<Post>()
			{
				new Post(), new Post(), new Post()
			};

			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.Search("test")).Returns(posts.AsQueryable());

			ForumController controller = new ForumController(postRepository.Object);

			ViewResult result = controller.Index("test", null) as ViewResult;
			IndexViewModel resultViewData = (IndexViewModel)result.ViewData.Model;

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual(3, resultViewData.Posts.Count());
		}

		[TestMethod]
		public void Search_Returns_The_Correct_Message()
		{
			List<Post> posts = new List<Post>()
			{
				new Post(), new Post(), new Post()
			};

			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.Search("test")).Returns(posts.AsQueryable());

			ForumController controller = new ForumController(postRepository.Object);

			ViewResult result = controller.Index("test", null) as ViewResult;
			IndexViewModel resultViewData = (IndexViewModel)result.ViewData.Model;

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual("Search results for \"test\".", resultViewData.Message);
		}

		[TestMethod]
		public void Index_With_No_Results_Returns_Correct_Message()
		{
			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.Find()).Returns(new List<Post>().AsQueryable());

			ForumController controller = new ForumController(postRepository.Object);

			ViewResult result = controller.Index(null, null) as ViewResult;
			IndexViewModel resultViewData = (IndexViewModel)result.ViewData.Model;

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual("There are currently no posts in this forum.", resultViewData.Message);
		}

		[TestMethod]
		public void Search_With_No_Results_Returns_Correct_Message()
		{
			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.Search("test")).Returns(new List<Post>().AsQueryable());

			ForumController controller = new ForumController(postRepository.Object);

			ViewResult result = controller.Index("test", null) as ViewResult;
			IndexViewModel resultViewData = (IndexViewModel)result.ViewData.Model;

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual("Sorry, there are no results for \"test\".", resultViewData.Message);
		}

		[TestMethod]
		public void Index_Title()
		{
			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.Find()).Returns(new List<Post>().AsQueryable());

			ForumController controller = new ForumController(postRepository.Object);

			ViewResult result = controller.Index(null, null) as ViewResult;
			IndexViewModel resultViewData = (IndexViewModel)result.ViewData.Model;

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual("Forum", resultViewData.PageTitle);
		}

		[TestMethod]
		public void View_Title()
		{
			Post post = new Post("some title", "body");
			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.FindById(1)).Returns(post);

			ForumController controller = new ForumController(postRepository.Object);

			ViewResult result = controller.View(1, "title") as ViewResult;
			ViewViewModel resultViewData = (ViewViewModel)result.ViewData.Model;

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual("some title", resultViewData.PageTitle);
		}

		[TestMethod]
		public void Post_Title()
		{
			Post post = new Post("some title", "body");
			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.FindById(1)).Returns(post);

			ForumController controller = new ForumController(postRepository.Object);

			ViewResult result = controller.Post(1) as ViewResult;
			PostViewModel resultViewData = (PostViewModel)result.ViewData.Model;

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual("some title", resultViewData.PageTitle);
		}

		[TestMethod]
		public void New_Post_Title()
		{
			ForumController controller = new ForumController(null);

			ViewResult result = controller.Post(null) as ViewResult;
			PostViewModel resultViewData = (PostViewModel)result.ViewData.Model;

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual("New Post", resultViewData.PageTitle);
		}

		[TestMethod]
		public void Reply_Title()
		{
			Post post = new Post("some title", "body");
			Mock<IPostRepository> postRepository = new Mock<IPostRepository>();
			postRepository.Setup(x => x.FindById(1)).Returns(post);

			ForumController controller = new ForumController(postRepository.Object);

			ViewResult result = controller.Reply(null, 1) as ViewResult;
			ReplyViewModel resultViewData = (ReplyViewModel)result.ViewData.Model;

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual("some title", resultViewData.PageTitle);
		}
	}
}

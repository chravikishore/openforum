using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenForum.Core.DataAccess.Interfaces;

namespace OpenForum.Core.Models
{
	public class Post
	{
		private IUserRepository _userRepository;

		public Post(IUserRepository userRepository)
		{
			Replies = new List<Reply>();

			_userRepository = userRepository;

			DateTime now = DateTime.Now;
			CreatedDate = now;
			LastPostDate = now;

			if (_userRepository.FindCurrentUser() != null)
			{
				string currentUserId = _userRepository.FindCurrentUser().Id;
				CreatedById = currentUserId;
				LastPostById = currentUserId;
			}
		}

		public Post()
			: this(Configurations.Current.UserRepository)
		{
		}

		public Post(string title, string body)
			: this(Configurations.Current.UserRepository, title, body)
		{
		}

		public Post(IUserRepository userRepository, string title, string body)
			: this(userRepository)
		{
			Title = title;
			Body = body;
		}

		public int Id { get; private set; }
		public DateTime CreatedDate { get; private set; }
		public string CreatedById { get; private set; }
		public DateTime LastPostDate { get; private set; }
		public string LastPostById { get; private set; }
		public string Title { get; set; }
		public int ViewCount { get; private set; }

		private string _body;
		public string Body
		{
			get { return _body; }
			set { _body = value.FixUpHtml(); }
		}

		public virtual ICollection<Reply> Replies { get; set; }


		public Reply AddReply()
		{
			Reply reply = new Reply(this);

			Replies.Add(reply);

			LastPostDate = DateTime.Now;
			LastPostById = _userRepository.FindCurrentUser().Id;

			return reply;
		}

		public void IncrementViewCount()
		{
			ViewCount++;
		}

		public Reply FindReplyById(int id)
		{
			return Replies.Single(x => x.Id == id);
		}

		public User CreatedBy
		{
			get { return _userRepository.FindById(CreatedById); }
		}

		public User LastPostBy
		{
			get { return _userRepository.FindById(LastPostById); }
		}

		// TODO: get this logic back in
		//protected void OnValidate(ChangeAction action)
		//{
		//    Validate();
		//}

		public void Validate()
		{
			if (string.IsNullOrEmpty(Title))
			{
				throw new OpenForumException("Please provide a title for your post.");
			}

			if (string.IsNullOrEmpty(Body))
			{
				throw new OpenForumException("Please provide text for your post.");
			}
			else
			{
				Body.ValidateHtml();
			}
		}

		public string GetFullText()
		{
			StringBuilder result = new StringBuilder();

			result.AppendLine(Title);
			result.AppendLine(Body);

			foreach (var item in Replies)
			{
				result.AppendLine(item.Body);
			}

			return result.ToString();
		}

		// TODO: unit test
		public string GetLatestResponse()
		{
			return Replies.Select(x => x.Body).LastOrDefault();
		}
	}
}

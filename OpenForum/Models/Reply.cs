using System;
using OpenForum.Core.DataAccess.Interfaces;

namespace OpenForum.Core.Models
{
	public class Reply
	{
		private IUserRepository _userRepository;

		public Reply(IUserRepository userRepository)
		{
			_userRepository = userRepository;

			CreatedDate = DateTime.Now;
			CreatedById = _userRepository.FindCurrentUser().Id;
		}

		public Reply()
			: this(Configurations.Current.UserRepository)
		{
		}

		internal Reply(Post post)
			: this()
		{
			Post = post;
		}

		public int Id { get; private set; }
		public Post Post { get; private set; }
		public string CreatedById { get; private set; }
		public DateTime CreatedDate { get; private set; }

		private string _body;
		public string Body
		{
			get { return _body; }
			set { _body = value.FixUpHtml(); }
		}

		public User CreatedBy
		{
			get { return _userRepository.FindById(CreatedById); }
		}


		// TODO: get this logic back in
		//protected void OnValidate(ChangeAction action)
		//{
		//    Validate();
		//}

		public void Validate()
		{
			if (string.IsNullOrEmpty(Body))
			{
				throw new OpenForumException("Please provide text for your reply.");
			}
			else
			{
				Body.ValidateHtml();
			}
		}
	}
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OpenForum.Core;
using OpenForum.Core.DataAccess;
using OpenForum.Core.Models;
using OpenForum.Core.DataAccess.Interfaces;

namespace OpenForum.UnitTests.Models
{
	[TestClass]
	public class ReplyTests
	{
		[TestMethod]
		public void Has_A_PostId()
		{
			Post post = new Post();
			Reply reply = post.AddReply();

			Assert.AreEqual(reply.Post.Id, post.Id);
		}

		[TestMethod]
		public void CreatedBy_User_Is_Lazy_Loaded()
		{
			User user = new User("1", "username");
			Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
			userRepository.Setup(x => x.FindCurrentUser()).Returns(user).Verifiable();
			userRepository.Setup(x => x.FindById("1")).Returns(user).Verifiable();

			Reply reply = new Reply(userRepository.Object);

			User createdUser = reply.CreatedBy;
			createdUser = reply.CreatedBy;

			userRepository.Verify();
		}

		[TestMethod]
		public void Body_Is_A_Required_Field()
		{
			try
			{
				Reply reply = new Reply();
				reply.Validate();
				Assert.Fail();
			}
			catch (OpenForumException exception)
			{
				Assert.AreEqual("Please provide text for your reply.", exception.Message);
			}
		}
	}
}

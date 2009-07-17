using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OpenForum.Core;
using OpenForum.Core.DataAccess;
using OpenForum.Core.Models;

namespace OpenForum.UnitTests.Models
{
    [TestClass]
    public class PostTests
    {
        [TestMethod]
        public void Has_A_Title_And_Body()
        {
            Post post = new Post("title", "body");

            Assert.AreEqual("title", post.Title);
            Assert.AreEqual("body", post.Body);
        }

        [TestMethod]
        public void Created_Date_Is_Set_When_Creating_A_Post()
        {
            Post post = new Post();

            Assert.AreEqual(DateTime.Now.Date, post.CreatedDate.Date);
        }

        [TestMethod]
        public void Created_Username_Is_Set_When_Creating_A_Post()
        {
            User user = new User("id");

            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            userRepository.Expect(x => x.FindCurrentUser()).Returns(user);
            userRepository.Expect(x => x.FindById("id")).Returns(user);

            Post post = new Post(userRepository.Object);

            Assert.AreEqual("id", post.CreatedBy.Id);
        }

        [TestMethod]
        public void Use_User_Repository_To_Get_Username_From_UserId()
        {
            User user = new User("1", "username");
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            userRepository.Expect(x => x.FindCurrentUser()).Returns(user);
            userRepository.Expect(x => x.FindById("1")).Returns(user);

            Post post = new Post(userRepository.Object);

            Assert.AreEqual("1", post.CreatedBy.Id);
            Assert.AreEqual("username", post.CreatedBy.Username);
        }

        [TestMethod]
        public void Modified_Date_Is_Set_When_Creating_A_Post()
        {
            Post post = new Post();

            Assert.AreEqual(DateTime.Now.Date, post.LastPostDate.Date);
        }

        [TestMethod]
        public void Modified_Username_Is_Set_when_Creating_A_Post()
        {
            User user = new User("id");

            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            userRepository.Expect(x => x.FindCurrentUser()).Returns(user);
            userRepository.Expect(x => x.FindById("id")).Returns(user);

            Post post = new Post(userRepository.Object);

            Assert.AreEqual("id", post.LastPostBy.Id);
        }

        [TestMethod]
        public void Use_User_Repository_To_Get_Modified_Username_From_UserId()
        {
            User user = new User("1", "username");
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            userRepository.Expect(x => x.FindCurrentUser()).Returns(user);
            userRepository.Expect(x => x.FindById("1")).Returns(user);

            Post post = new Post(userRepository.Object);

            Assert.AreEqual("1", post.LastPostBy.Id);
            Assert.AreEqual("username", post.LastPostBy.Username);
        }

        [TestMethod]
        public void CreatedBy_User_Is_Lazy_Loaded()
        {
            User user = new User("1", "username");
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            userRepository.Expect(x => x.FindCurrentUser()).Returns(user).Verifiable();
            userRepository.Expect(x => x.FindById("1")).Returns(user).AtMostOnce().Verifiable();

            Post post = new Post(userRepository.Object);

            User createdUser = post.CreatedBy;
            createdUser = post.CreatedBy;

            userRepository.Verify();
        }

        [TestMethod]
        public void ModifiedBy_User_Is_Lazy_Loaded()
        {
            User user = new User("1", "username");
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            userRepository.Expect(x => x.FindCurrentUser()).Returns(user).Verifiable();
            userRepository.Expect(x => x.FindById("1")).Returns(user).Verifiable();

            Post post = new Post(userRepository.Object);

            User createdUser = post.LastPostBy;
            createdUser = post.LastPostBy;

            userRepository.Verify();
        }

        [TestMethod]
        public void View_Count_Can_Be_Incremented()
        {
            Post post = new Post();
            post.IncrementViewCount();
            Assert.AreEqual(1, post.ViewCount);
        }

        [TestMethod]
        public void Can_Add_A_Reply()
        {
            Post post = new Post();
            Reply reply = post.AddReply();

            Assert.AreEqual(1, post.Replies.Length);
            Assert.AreEqual(reply, post.Replies[0]);
        }

        [TestMethod]
        public void Can_Find_Reply_By_Id()
        {
            Post post = new Post();

            Reply reply1 = post.AddReply();
            reply1.SetId(1);

            Reply reply2 = post.AddReply();
            reply2.SetId(2);

            Assert.AreEqual(reply2, post.FindReplyById(2));
        }

        [TestMethod]
        public void Adding_A_Reply_Updates_The_Modified_Info()
        {
            Post post = new Post();
            post.SetValue("LastPostById", "original");
            post.SetValue("LastPostDate", DateTime.Now.AddDays(-3));

            Utility.SetCurrentUser("new");

            post.AddReply();

            Assert.AreEqual("new", post.LastPostBy.Username);
            Assert.AreEqual(DateTime.Now.Date, post.LastPostDate.Date);
        }

        [TestMethod]
        public void Body_Is_A_Required_Field()
        {
            try
            {
                Post post = new Post();
                post.Title = "title";
                post.Validate();
                Assert.Fail();
            }
            catch (OpenForumException exception)
            {
                Assert.AreEqual("Please provide text for your post.", exception.Message);
            }
        }

        [TestMethod]
        public void Title_Is_A_Required_Field()
        {
            try
            {
                Post post = new Post();
                post.Validate();
                Assert.Fail();
            }
            catch (OpenForumException exception)
            {
                Assert.AreEqual("Please provide a title for your post.", exception.Message);
            }
        }

        [TestMethod]
        public void Get_Full_Text()
        {
            Post post = new Post("title", "body");
            post.AddReply().Body = "reply1";
            post.AddReply().Body = "reply2";

            Assert.AreEqual("title\r\nbody\r\nreply1\r\nreply2\r\n", post.GetFullText());
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenForum.Core.DataAccess;
using OpenForum.Core.Models;

namespace OpenForum.UnitTests.Models
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void Has_Id_And_Name()
        {
            User user = new User("Id", "Username");

            Assert.AreEqual("Id", user.Id);
            Assert.AreEqual("Username", user.Username);
        }

        [TestMethod]
        public void Default_Repository_Users_Current_Principal_Info()
        {
            Utility.SetCurrentUser("user");

            User user = new DefaultUserRepository().FindCurrentUser();

            Assert.AreEqual("user", user.Id);
            Assert.AreEqual("user", user.Username);
        }

        [TestMethod]
        public void Default_Repository_Returns_User_By_Id()
        {
            User user = new DefaultUserRepository().FindById("foundUser");

            Assert.AreEqual("foundUser", user.Id);
            Assert.AreEqual("foundUser", user.Username);
        }

        [TestMethod]
        public void Has_User_Url()
        {
            User user = new User("a", "a", "user url", "");

            Assert.IsTrue(user.HasUserUrl);
            Assert.AreEqual("user url", user.UserUrl);
        }

        [TestMethod]
        public void Does_Not_Have_User_Url()
        {
            User user = new User("a", "a", null, null);

            Assert.IsFalse(user.HasUserUrl);
        }

        [TestMethod]
        public void Has_Image_Url()
        {
            User user = new User("a", "a", null, "image url");

            Assert.IsTrue(user.HasImageUrl);
            Assert.AreEqual("image url", user.ImageUrl);
        }

        [TestMethod]
        public void Does_Not_Have_Image_Url()
        {
            User user = new User("a", "a", null, null);

            Assert.IsFalse(user.HasImageUrl);
        }
    }
}

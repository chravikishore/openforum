using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OpenForum.Core.DataAccess.Interfaces;
using OpenForum.Core.Models;
using OpenForum.Core.ViewModels;
using OpenForum.Core.ViewModels.Interfaces;
using OpenForum.Core.Views.Forum;

namespace OpenForum.UnitTests.Views
{
	[TestClass]
	public class ViewControlTests
	{
		[TestMethod]
		public void Render()
		{
			Utility.SetCurrentUser("user1");
			IViewViewModel model = new ViewViewModel();
			model.Post = new Post("post_title", "post_body");

			Utility.SetCurrentUser("user2");
			Reply reply = model.Post.AddReply();
			reply.Body = "reply_body";

			ViewHelper<_Page_Views_Forum_View_cshtml> helper = new ViewHelper<_Page_Views_Forum_View_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();

			XmlNodeList nodes = xml.SelectNodes("//table/tr");

			Assert.IsTrue(xml.InnerText.Contains("post_title"));

			Assert.AreEqual(2, nodes.Count);

			Assert.IsTrue(nodes[0].InnerText.Contains("post_body"));
			Assert.IsTrue(nodes[0].InnerText.Contains("user1"));

			Assert.IsTrue(nodes[1].InnerText.Contains("reply_body"));
			Assert.IsTrue(nodes[1].InnerText.Contains("user2"));
		}

		[TestMethod]
		public void Edit_Link_Displays_For_Current_User()
		{
			Utility.SetCurrentUser("user1");
			IViewViewModel model = new ViewViewModel();
			model.CurrentUser = new User("user1");
			model.Post = new Post("post_title", "post_body");

			Utility.SetCurrentUser("user2");
			Reply reply = model.Post.AddReply();
			reply.Body = "reply_body";

			ViewHelper<_Page_Views_Forum_View_cshtml> helper = new ViewHelper<_Page_Views_Forum_View_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();
			XmlNodeList nodes = xml.SelectNodes("//table/tr");

			Assert.IsTrue(nodes[0].InnerText.Contains("Edit"));
			Assert.IsFalse(nodes[1].InnerText.Contains("Edit"));

			model.CurrentUser = new User("user2");
			helper = new ViewHelper<_Page_Views_Forum_View_cshtml>(model);
			xml = helper.RenderToXml();
			nodes = xml.SelectNodes("//table/tr");

			Assert.IsFalse(nodes[0].InnerText.Contains("Edit"));
			Assert.IsTrue(nodes[1].InnerText.Contains("Edit"));
		}

		[TestMethod]
		public void Shows_User_Url()
		{
			User user = new User("user1", "user1", "userUrl", "imageUrl");
			Mock<IUserRepository> repository = new Mock<IUserRepository>();
			repository.Setup(x => x.FindById("user1")).Returns(user);

			Post post = new Post(repository.Object, "post_title", "post\r\nbody");
			post.SetValue("CreatedById", user.Id);

			IViewViewModel model = new ViewViewModel();
			model.Post = post;

			ViewHelper<_Page_Views_Forum_View_cshtml> helper = new ViewHelper<_Page_Views_Forum_View_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();

			XmlNode node = xml.SelectSingleNode("//a[@href = 'userUrl']");

			Assert.IsNotNull(node);
			Assert.AreEqual("user1", node.InnerText);
		}

		[TestMethod]
		public void Shows_Image_Url()
		{
			User user = new User("user1", "user1", "userUrl", "imageUrl");
			Mock<IUserRepository> repository = new Mock<IUserRepository>();
			repository.Setup(x => x.FindById("user1")).Returns(user);

			Post post = new Post(repository.Object, "post_title", "post\r\nbody");
			post.SetValue("CreatedById", user.Id);

			IViewViewModel model = new ViewViewModel();
			model.Post = post;

			ViewHelper<_Page_Views_Forum_View_cshtml> helper = new ViewHelper<_Page_Views_Forum_View_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();

			XmlNode node = xml.SelectSingleNode("//img[@src = 'imageUrl']");

			Assert.IsNotNull(node);
			Assert.AreEqual("user1", node.Attributes["alt"].Value);
		}

		[TestMethod]
		public void Can_Display_Default_Styles()
		{
			Post post = new Post();

			IViewViewModel model = new ViewViewModel();
			model.Post = post;
			model.IncludeDefaultStyles = true;

			ViewHelper<_Page_Views_Forum_View_cshtml> helper = new ViewHelper<_Page_Views_Forum_View_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNotNull(xml.SelectSingleNode("//style"));
		}

		[TestMethod]
		public void Can_Remove_Default_Styles()
		{
			Post post = new Post();

			IViewViewModel model = new ViewViewModel();
			model.Post = post;
			model.IncludeDefaultStyles = false;

			ViewHelper<_Page_Views_Forum_View_cshtml> helper = new ViewHelper<_Page_Views_Forum_View_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNull(xml.SelectSingleNode("//style"));
		}

		[TestMethod]
		public void Can_Display_Validation_Summary()
		{
			Post post = new Post();

			IViewViewModel model = new ViewViewModel();
			model.Post = post;
			model.IncludeValidationSummary = true;

			ViewHelper<_Page_Views_Forum_View_cshtml> helper = new ViewHelper<_Page_Views_Forum_View_cshtml>(model);
			helper.Control.ViewData.ModelState.AddModelError("_Form", "something bad happened");
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNotNull(xml.SelectSingleNode("//div[@class = 'validation-summary-errors']"));
		}

		[TestMethod]
		public void Can_Remove_Validation_Summary()
		{
			Post post = new Post();

			IViewViewModel model = new ViewViewModel();
			model.Post = post;
			model.IncludeValidationSummary = false;

			ViewHelper<_Page_Views_Forum_View_cshtml> helper = new ViewHelper<_Page_Views_Forum_View_cshtml>(model);
			helper.Control.ViewData.ModelState.AddModelError("_Form", "something bad happened");
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNull(xml.SelectSingleNode("//ul[@class = 'validation-summary-errors']"));
		}
	}
}

using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenForum.Core.Models;
using OpenForum.Core.ViewModels;
using OpenForum.Core.ViewModels.Interfaces;
using OpenForum.Core.Views.Forum;

namespace OpenForum.UnitTests.Views
{
	[TestClass]
	public class PostControlTests
	{
		[TestMethod]
		public void Render()
		{
			IPostViewModel model = new PostViewModel();
			model.Post = new Post("post_title", "post_body");
			model.Post.SetId(3);

			ViewHelper<_Page_Views_Forum_Post_cshtml> helper = new ViewHelper<_Page_Views_Forum_Post_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNotNull(xml.SelectSingleNode("//input[@id = 'title' and @value = 'post_title']"));
			Assert.IsNotNull(xml.SelectSingleNode("//textarea[@id = 'body' and . = '\r\npost_body']"));
			Assert.IsNotNull(xml.SelectSingleNode("//input[@id = 'id' and @value = '3']"));
		}

		[TestMethod]
		public void Can_Display_Wysiwyg_Editor()
		{
			IPostViewModel model = new PostViewModel();
			model.Post = new Post("post_title", "post_body");
			model.Post.SetId(3);
			model.IncludeWysiwygEditor = true;

			ViewHelper<_Page_Views_Forum_Post_cshtml> helper = new ViewHelper<_Page_Views_Forum_Post_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNotNull(xml.SelectSingleNode("//script[@type='text/javascript' and @src='']"));
		}

		[TestMethod]
		public void Can_Remove_Wysiwyg_Editor()
		{
			IPostViewModel model = new PostViewModel();
			model.Post = new Post("post_title", "post_body");
			model.Post.SetId(3);
			model.IncludeWysiwygEditor = false;

			ViewHelper<_Page_Views_Forum_Post_cshtml> helper = new ViewHelper<_Page_Views_Forum_Post_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNull(xml.SelectSingleNode("//script"));
		}

		[TestMethod]
		public void Can_Display_Default_Styles()
		{
			IPostViewModel model = new PostViewModel();
			model.Post = new Post("post_title", "post_body");
			model.Post.SetId(3);
			model.IncludeDefaultStyles = true;

			ViewHelper<_Page_Views_Forum_Post_cshtml> helper = new ViewHelper<_Page_Views_Forum_Post_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNotNull(xml.SelectSingleNode("//style"));
		}

		[TestMethod]
		public void Can_Remove_Default_Styles()
		{
			IPostViewModel model = new PostViewModel();
			model.Post = new Post("post_title", "post_body");
			model.Post.SetId(3);
			model.IncludeDefaultStyles = false;

			ViewHelper<_Page_Views_Forum_Post_cshtml> helper = new ViewHelper<_Page_Views_Forum_Post_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNull(xml.SelectSingleNode("//style"));
		}

		[TestMethod]
		public void Can_Display_Validation_Summary()
		{
			IPostViewModel model = new PostViewModel();
			model.Post = new Post("post_title", "post_body");
			model.Post.SetId(3);
			model.IncludeValidationSummary = true;

			ViewHelper<_Page_Views_Forum_Post_cshtml> helper = new ViewHelper<_Page_Views_Forum_Post_cshtml>(model);
			helper.Control.ViewData.ModelState.AddModelError("_Form", "something bad happened");
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNotNull(xml.SelectSingleNode("//div[@class = 'validation-summary-errors']"));
		}

		[TestMethod]
		public void Can_Remove_Validation_Summary()
		{
			IPostViewModel model = new PostViewModel();
			model.Post = new Post("post_title", "post_body");
			model.Post.SetId(3);
			model.IncludeValidationSummary = false;

			ViewHelper<_Page_Views_Forum_Post_cshtml> helper = new ViewHelper<_Page_Views_Forum_Post_cshtml>(model);
			helper.Control.ViewData.ModelState.AddModelError("_Form", "something bad happened");
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNull(xml.SelectSingleNode("//ul[@class = 'validation-summary-errors']"));
		}
	}
}

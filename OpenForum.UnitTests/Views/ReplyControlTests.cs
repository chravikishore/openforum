using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenForum.Core.Models;
using OpenForum.Core.ViewModels;
using OpenForum.Core.ViewModels.Interfaces;
using OpenForum.Core.Views.Forum;

namespace OpenForum.UnitTests.Views
{
	[TestClass]
	public class ReplyControlTests
	{
		[TestMethod]
		public void Render()
		{
			Post post = new Post();
			post.SetId(3);

			IReplyViewModel model = new ReplyViewModel();
			model.Post = post;
			model.Reply = post.AddReply();
			model.Reply.SetId(4);
			model.Reply.Body = "reply_body";

			ViewHelper<_Page_Views_Forum_Reply_cshtml> helper = new ViewHelper<_Page_Views_Forum_Reply_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNotNull(xml.SelectSingleNode("//textarea[@id = 'body' and . = '\r\nreply_body']"));
			Assert.IsNotNull(xml.SelectSingleNode("//input[@id = 'id' and @value = '4']"));
			Assert.IsNotNull(xml.SelectSingleNode("//input[@id = 'postId' and @value = '3']"));
		}

		[TestMethod]
		public void Can_Display_Wysiwyg_Editor()
		{
			Post post = new Post();
			post.SetId(3);

			IReplyViewModel model = new ReplyViewModel();
			model.Post = post;
			model.Reply = post.AddReply();
			model.Reply.SetId(4);
			model.Reply.Body = "reply_body";
			model.IncludeWysiwygEditor = true;

			ViewHelper<_Page_Views_Forum_Reply_cshtml> helper = new ViewHelper<_Page_Views_Forum_Reply_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNotNull(xml.SelectSingleNode("//script[@type='text/javascript' and @src='']"));
		}

		[TestMethod]
		public void Can_Remove_Wysiwyg_Editor()
		{
			Post post = new Post();
			post.SetId(3);

			IReplyViewModel model = new ReplyViewModel();
			model.Post = post;
			model.Reply = post.AddReply();
			model.Reply.SetId(4);
			model.Reply.Body = "reply_body";
			model.IncludeWysiwygEditor = false;

			ViewHelper<_Page_Views_Forum_Reply_cshtml> helper = new ViewHelper<_Page_Views_Forum_Reply_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNull(xml.SelectSingleNode("//script"));
		}

		[TestMethod]
		public void Can_Display_Default_Styles()
		{
			Post post = new Post();
			post.SetId(3);

			IReplyViewModel model = new ReplyViewModel();
			model.Post = post;
			model.Reply = post.AddReply();
			model.Reply.SetId(4);
			model.Reply.Body = "reply_body";
			model.IncludeDefaultStyles = true;

			ViewHelper<_Page_Views_Forum_Reply_cshtml> helper = new ViewHelper<_Page_Views_Forum_Reply_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNotNull(xml.SelectSingleNode("//style"));
		}

		[TestMethod]
		public void Can_Remove_Default_Styles()
		{
			Post post = new Post();
			post.SetId(3);

			IReplyViewModel model = new ReplyViewModel();
			model.Post = post;
			model.Reply = post.AddReply();
			model.Reply.SetId(4);
			model.Reply.Body = "reply_body";
			model.IncludeDefaultStyles = false;

			ViewHelper<_Page_Views_Forum_Reply_cshtml> helper = new ViewHelper<_Page_Views_Forum_Reply_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNull(xml.SelectSingleNode("//style"));
		}

		[TestMethod]
		public void Can_Display_Validation_Summary()
		{
			Post post = new Post();
			post.SetId(3);

			IReplyViewModel model = new ReplyViewModel();
			model.Post = post;
			model.Reply = post.AddReply();
			model.Reply.SetId(4);
			model.Reply.Body = "reply_body";
			model.IncludeValidationSummary = true;

			ViewHelper<_Page_Views_Forum_Reply_cshtml> helper = new ViewHelper<_Page_Views_Forum_Reply_cshtml>(model);
			helper.Control.ViewData.ModelState.AddModelError("_Form", "something bad happened");
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNotNull(xml.SelectSingleNode("//div[@class = 'validation-summary-errors']"));
		}

		[TestMethod]
		public void Can_Remove_Validation_Summary()
		{
			Post post = new Post();
			post.SetId(3);

			IReplyViewModel model = new ReplyViewModel();
			model.Post = post;
			model.Reply = post.AddReply();
			model.Reply.SetId(4);
			model.Reply.Body = "reply_body";
			model.IncludeValidationSummary = false;

			ViewHelper<_Page_Views_Forum_Reply_cshtml> helper = new ViewHelper<_Page_Views_Forum_Reply_cshtml>(model);
			helper.Control.ViewData.ModelState.AddModelError("_Form", "something bad happened");
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNull(xml.SelectSingleNode("//div[@class = 'validation-summary-errors']"));
		}
	}
}

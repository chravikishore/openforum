using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenForum.Core.Models;
using OpenForum.Core.ViewModels;
using OpenForum.Core.ViewModels.Interfaces;
using OpenForum.Core.Views.Forum;

namespace OpenForum.UnitTests.Views
{
	[TestClass]
	public class IndexControlTests
	{
		[TestMethod]
		public void Render_Post_List()
		{
			Utility.SetCurrentUser("user1");
			Post post1 = new Post("title1", "body1");
			post1.SetValue("ViewCount", 14);
			Utility.SetCurrentUser("user2");
			Reply reply = post1.AddReply();
			reply.Body = "reply1";

			Post post2 = new Post("title2", "body2");

			IIndexViewModel model = new IndexViewModel();
			model.Posts = new List<Post>() { post1, post2 };

			ViewHelper<_Page_Views_Forum_Index_cshtml> helper = new ViewHelper<_Page_Views_Forum_Index_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();

			XmlNodeList postNodes = xml.SelectNodes("//div[@class = 'openforum_item']");
			Assert.AreEqual(2, postNodes.Count);

			Assert.IsTrue(postNodes[0].InnerText.Contains("title1"));
			Assert.IsTrue(postNodes[0].InnerText.Contains("14 Views"));
			Assert.IsTrue(postNodes[0].InnerText.Contains("1 Posts"));
			Assert.IsTrue(postNodes[0].InnerText.Contains("Original post: user1 wrote - body1"));
			Assert.IsTrue(postNodes[0].InnerText.Contains("Latest post: user2 wrote - reply1"));
		}

		[TestMethod]
		public void Has_Paging_Info()
		{
			IIndexViewModel model = new IndexViewModel();
			model.Posts = new List<Post>() { new Post("title", "body") };
			model.CurrentPage = 2;
			model.TotalPages = 14;

			ViewHelper<_Page_Views_Forum_Index_cshtml> helper = new ViewHelper<_Page_Views_Forum_Index_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();

			XmlNode node = xml.SelectSingleNode("//div/div[@class = 'openforum_index_paging']");
			Assert.IsTrue(node.InnerText.Contains("Page 3 of 14"));
			Assert.IsTrue(node.InnerXml.Contains("&gt;&gt;&gt;"));
			Assert.IsTrue(node.InnerXml.Contains("&lt;&lt;&lt;"));

		}

		[TestMethod]
		public void Can_Display_Default_Styles()
		{
			ViewHelper<_Page_Views_Forum_Index_cshtml> helper = new ViewHelper<_Page_Views_Forum_Index_cshtml>(new IndexViewModel() { IncludeDefaultStyles = true });
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNotNull(xml.SelectSingleNode("//style"));
		}

		[TestMethod]
		public void Can_Remove_Default_Styles()
		{
			ViewHelper<_Page_Views_Forum_Index_cshtml> helper = new ViewHelper<_Page_Views_Forum_Index_cshtml>(new IndexViewModel() { IncludeValidationSummary = false });
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNull(xml.SelectSingleNode("//style"));
		}

		[TestMethod]
		public void Can_Display_Validation_Summary()
		{
			ViewHelper<_Page_Views_Forum_Index_cshtml> helper = new ViewHelper<_Page_Views_Forum_Index_cshtml>(new IndexViewModel() { IncludeValidationSummary = true });
			helper.Control.ViewData.ModelState.AddModelError("_Form", "something bad happened");
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNotNull(xml.SelectSingleNode("//div[@class = 'validation-summary-errors']"));
		}

		[TestMethod]
		public void Can_Remove_Validation_Summary()
		{
			ViewHelper<_Page_Views_Forum_Index_cshtml> helper = new ViewHelper<_Page_Views_Forum_Index_cshtml>(new IndexViewModel() { IncludeValidationSummary = false });
			helper.Control.ViewData.ModelState.AddModelError("_Form", "something bad happened");
			XmlDocument xml = helper.RenderToXml();

			Assert.IsNull(xml.SelectSingleNode("//ul[@class = 'validation-summary-errors']"));
		}

		[TestMethod]
		public void Render_Message()
		{
			IIndexViewModel model = new IndexViewModel();
			model.Message = "Test test test.";

			ViewHelper<_Page_Views_Forum_Index_cshtml> helper = new ViewHelper<_Page_Views_Forum_Index_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();

			Assert.AreEqual("Test test test.", xml.SelectSingleNode("//div/div[@class = 'openforum_message']").InnerText);
		}

		[TestMethod]
		public void Render_Search_Text()
		{
			IIndexViewModel model = new IndexViewModel();
			model.SearchQuery = "search text";

			ViewHelper<_Page_Views_Forum_Index_cshtml> helper = new ViewHelper<_Page_Views_Forum_Index_cshtml>(model);
			XmlDocument xml = helper.RenderToXml();

			Assert.AreEqual("search text", xml.SelectSingleNode("//input[@name = 'searchQuery']").Attributes["value"].Value);
		}
	}
}

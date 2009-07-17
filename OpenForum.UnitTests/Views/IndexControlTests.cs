using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenForum.Core;
using OpenForum.Core.Models;
using OpenForum.Core.ViewModels;
using OpenForum.Core.Views;

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
            post1.AddReply();

            Post post2 = new Post("title2", "body2");
            
            IIndexViewModel model = new IndexViewModel();
            model.Posts = new List<Post>() { post1, post2 };
            
            ViewHelper helper = new ViewHelper("index",  model);
            XmlDocument xml = helper.RenderToXml();

            XmlNodeList postNodes = xml.SelectNodes("//div/table[@class = 'openforum_maincontent']/tr");
            Assert.AreEqual(2, postNodes.Count);

            Assert.IsTrue(postNodes[0].InnerText.Contains("title1"));
            Assert.IsTrue(postNodes[0].InnerText.Contains("Views: 14"));
            Assert.IsTrue(postNodes[0].InnerText.Contains("Replies: 1"));
            Assert.IsTrue(postNodes[0].InnerText.Contains("created by user1"));
            Assert.IsTrue(postNodes[0].InnerText.Contains("last post by user2"));
        }

        [TestMethod]
        public void Has_Paging_Info()
        {
            IIndexViewModel model = new IndexViewModel();
            model.Posts = new List<Post>() { new Post("title", "body") };
            model.CurrentPage = 2;
            model.TotalPages = 14;

            ViewHelper helper = new ViewHelper("index", model);
            XmlDocument xml = helper.RenderToXml();

            XmlNode node = xml.SelectSingleNode("//div/div[@class = 'openforum_index_paging']");
            Assert.IsTrue(node.InnerText.Contains("Page 3 of 14"));
            Assert.IsTrue(node.InnerXml.Contains("&gt;&gt;&gt;"));
            Assert.IsTrue(node.InnerXml.Contains("&lt;&lt;&lt;"));

        }

        [TestMethod]
        public void Can_Display_Default_Styles()
        {
            ViewHelper helper = new ViewHelper("index", new IndexViewModel() { IncludeDefaultStyles = true });
            XmlDocument xml = helper.RenderToXml();

            Assert.IsNotNull(xml.SelectSingleNode("//style"));
        }

        [TestMethod]
        public void Can_Remove_Default_Styles()
        {
            ViewHelper helper = new ViewHelper("index", new IndexViewModel() { IncludeValidationSummary = false });
            XmlDocument xml = helper.RenderToXml();

            Assert.IsNull(xml.SelectSingleNode("//style"));
        }
        
        [TestMethod]
        public void Can_Display_Validation_Summary()
        {
            ViewHelper helper = new ViewHelper("index", new IndexViewModel() { IncludeValidationSummary = true });
            helper.Control.ViewData.ModelState.AddModelError("_Form", "something bad happened");
            XmlDocument xml = helper.RenderToXml();

            Assert.IsNotNull(xml.SelectSingleNode("//ul[@class = 'validation-summary-errors']"));
        }

        [TestMethod]
        public void Can_Remove_Validation_Summary()
        {
            ViewHelper helper = new ViewHelper("index", new IndexViewModel() { IncludeValidationSummary = false });
            helper.Control.ViewData.ModelState.AddModelError("_Form", "something bad happened");
            XmlDocument xml = helper.RenderToXml();

            Assert.IsNull(xml.SelectSingleNode("//ul[@class = 'validation-summary-errors']"));
        }

        [TestMethod]
        public void Render_Message()
        {
            IIndexViewModel model = new IndexViewModel();
            model.Message = "Test test test.";

            ViewHelper helper = new ViewHelper("index", model);
            XmlDocument xml = helper.RenderToXml();

            Assert.AreEqual("Test test test.", xml.SelectSingleNode("//div/div[@class = 'openforum_message']").InnerText);
        }

        [TestMethod]
        public void Render_Search_Text()
        {
            IIndexViewModel model = new IndexViewModel();
            model.SearchQuery = "search text";

            ViewHelper helper = new ViewHelper("index", model);
            XmlDocument xml = helper.RenderToXml();

            Assert.AreEqual("search text", xml.SelectSingleNode("//input[@name = 'searchQuery']").Attributes["value"].Value);
        }
    }
}

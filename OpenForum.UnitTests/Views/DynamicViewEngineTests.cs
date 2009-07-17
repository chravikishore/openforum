using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenForum.Core.Views;

namespace OpenForum.UnitTests.Views
{
    [TestClass]
    public class DynamicViewEngineTests
    {
        [TestMethod]
        public void Does_Not_Support_Partial_Views()
        {
            ControllerContext controllerContext = Utility.CreateMockControllerContext();
            DynamicViewEngine viewEngine = new DynamicViewEngine();
            ViewEngineResult result = viewEngine.FindPartialView(controllerContext, "somename", false);

            Assert.IsNull(result.View);
            Assert.IsNull(result.ViewEngine);
            Assert.AreEqual(1, result.SearchedLocations.Count());
            Assert.AreEqual("Dynamic Forum Views", result.SearchedLocations.ElementAt(0));
        }

        [TestMethod]
        public void Does_Not_Return_View_For_None_Forum_Controllers()
        {
            ControllerContext controllerContext = Utility.CreateMockControllerContext();
            DynamicViewEngine viewEngine = new DynamicViewEngine();
            ViewEngineResult result = viewEngine.FindView(controllerContext, "somename", "", false);

            Assert.IsNull(result.View);
            Assert.IsNull(result.ViewEngine);
            Assert.AreEqual(1, result.SearchedLocations.Count());
            Assert.AreEqual("Dynamic Forum Views", result.SearchedLocations.ElementAt(0));
        }

        [TestMethod]
        public void Does_Not_Return_View_For_Invalid_View_Names()
        {
            ControllerContext controllerContext = Utility.CreateMockControllerContext();
            DynamicViewEngine viewEngine = new DynamicViewEngine();
            ViewEngineResult result = viewEngine.FindView(controllerContext, "something_invalid", "", false);

            Assert.IsNull(result.View);
            Assert.IsNull(result.ViewEngine);
            Assert.AreEqual(1, result.SearchedLocations.Count());
            Assert.AreEqual("Dynamic Forum Views", result.SearchedLocations.ElementAt(0));
        }

        [TestMethod]
        public void Can_Find_Forum_Index_View()
        {
            ControllerContext controllerContext = Utility.CreateMockControllerContext();
            DynamicViewEngine viewEngine = new DynamicViewEngine();
            ViewEngineResult result = viewEngine.FindView(controllerContext, "index", "", false);
            DynamicView view = result.View as DynamicView;
            
            Assert.IsNotNull(view);
            Assert.AreEqual(viewEngine, result.ViewEngine);
            Assert.AreEqual("index", view.ViewName);
        }

        [TestMethod]
        public void Can_Find_Forum_View_View()
        {
            ControllerContext controllerContext = Utility.CreateMockControllerContext();
            DynamicViewEngine viewEngine = new DynamicViewEngine();
            ViewEngineResult result = viewEngine.FindView(controllerContext, "view", "", false);
            DynamicView view = result.View as DynamicView;

            Assert.IsNotNull(view);
            Assert.AreEqual(viewEngine, result.ViewEngine);
            Assert.AreEqual("view", view.ViewName);
        }

        [TestMethod]
        public void Can_Find_Forum_Post_View()
        {
            ControllerContext controllerContext = Utility.CreateMockControllerContext();
            DynamicViewEngine viewEngine = new DynamicViewEngine();

            ViewEngineResult result = viewEngine.FindView(controllerContext, "Post", "", false);
            DynamicView view = result.View as DynamicView;

            Assert.IsNotNull(view);
            Assert.AreEqual(viewEngine, result.ViewEngine);
            Assert.AreEqual("Post", view.ViewName);
        }

        [TestMethod]
        public void Can_Find_Forum_Reply_View()
        {
            ControllerContext controllerContext = Utility.CreateMockControllerContext();
            DynamicViewEngine viewEngine = new DynamicViewEngine();

            ViewEngineResult result = viewEngine.FindView(controllerContext, "Reply", "", false);
            DynamicView view = result.View as DynamicView;

            Assert.IsNotNull(view);
            Assert.AreEqual(viewEngine, result.ViewEngine);
            Assert.AreEqual("Reply", view.ViewName);
        }
    }
}

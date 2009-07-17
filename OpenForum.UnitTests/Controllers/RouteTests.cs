using System;
using System.Web;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OpenForum.Core;

namespace OpenForum.UnitTests.Controllers
{
    [TestClass]
    public class RouteTests
    {
        private static HttpContextBase CreateContext(string url)
        {
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            request.ExpectGet(x => x.AppRelativeCurrentExecutionFilePath).Returns("//");
            request.ExpectGet(x => x.PathInfo).Returns(url);
            request.ExpectGet(x => x.Url).Returns(new Uri("", UriKind.Relative));

            Mock<HttpResponseBase> response = new Mock<HttpResponseBase>();
            response.Expect(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(x => x);

            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            context.ExpectGet(x => x.Request).Returns(request.Object);
            context.ExpectGet(x => x.Response).Returns(response.Object);

            return context.Object;
        }

        [TestMethod]
        public void Register_Default_Routing_Rules()
        {
            RouteCollection routes = new RouteCollection();
            OpenForumManager.RegisterRoutingRules(routes);

            Assert.AreEqual(6, routes.Count);
        }

        [TestMethod]
        public void Get_Index_RouteData()
        {
            RouteCollection routes = new RouteCollection();
            OpenForumManager.RegisterRoutingRules(routes);

            HttpContextBase context = CreateContext("forum");

            RouteData routeData = routes.GetRouteData(context);

            Assert.IsNotNull(routeData);
            Assert.AreEqual(2, routeData.Values.Count);
            Assert.AreEqual("Forum", routeData.Values["controller"]);
            Assert.AreEqual("Index", routeData.Values["action"]);
        }

        [TestMethod]
        public void Get_Index_Path()
        {
            RouteCollection routes = new RouteCollection();
            OpenForumManager.RegisterRoutingRules(routes);

            HttpContextBase context = CreateContext("forum");
            RouteData routeData = routes.GetRouteData(context);

            VirtualPathData path = routes.GetVirtualPath(
                new RequestContext(context, routeData),
                new RouteValueDictionary(new { controller = "Forum", action = "Index" }));

            Assert.AreEqual("/Forum", path.VirtualPath);
        }

        [TestMethod]
        public void Get_View_RouteData()
        {
            RouteCollection routes = new RouteCollection();
            OpenForumManager.RegisterRoutingRules(routes);

            HttpContextBase context = CreateContext("forum/view/3/some-title");
            RouteData routeData = routes.GetRouteData(context);

            Assert.IsNotNull(routeData);
            Assert.AreEqual(4, routeData.Values.Count);
            Assert.AreEqual("Forum", routeData.Values["controller"]);
            Assert.AreEqual("View", routeData.Values["action"]);
            Assert.AreEqual("3", routeData.Values["id"]);
            Assert.AreEqual("some-title", routeData.Values["title"]);
        }

        [TestMethod]
        public void Get_View_Path()
        {
            RouteCollection routes = new RouteCollection();
            OpenForumManager.RegisterRoutingRules(routes);

            HttpContextBase context = CreateContext("forum");
            RouteData routeData = routes.GetRouteData(context);

            VirtualPathData path = routes.GetVirtualPath(
                new RequestContext(context, routeData),
                new RouteValueDictionary(new { controller = "Forum", action = "View", id = 3, title = "some-title" }));

            Assert.AreEqual("/Forum/View/3/some-title", path.VirtualPath);
        }

        [TestMethod]
        public void Get_Post_RouteData()
        {
            RouteCollection routes = new RouteCollection();
            OpenForumManager.RegisterRoutingRules(routes);

            HttpContextBase context = CreateContext("forum/post");
            RouteData routeData = routes.GetRouteData(context);

            Assert.IsNotNull(routeData);
            Assert.AreEqual(3, routeData.Values.Count);
            Assert.AreEqual("Forum", routeData.Values["controller"]);
            Assert.AreEqual("Post", routeData.Values["action"]);
            Assert.AreEqual("", routeData.Values["id"]);
        }

        [TestMethod]
        public void Get_Post_Path()
        {
            RouteCollection routes = new RouteCollection();
            OpenForumManager.RegisterRoutingRules(routes);

            HttpContextBase context = CreateContext("forum");
            RouteData routeData = routes.GetRouteData(context);

            VirtualPathData path = routes.GetVirtualPath(
                new RequestContext(context, routeData),
                new RouteValueDictionary(new { controller = "Forum", action = "Post" }));

            Assert.AreEqual("/Forum/Post", path.VirtualPath);
        }

        [TestMethod]
        public void Get_Post_RouteData_With_Id()
        {
            RouteCollection routes = new RouteCollection();
            OpenForumManager.RegisterRoutingRules(routes);

            HttpContextBase context = CreateContext("forum/post/14");
            RouteData routeData = routes.GetRouteData(context);

            Assert.IsNotNull(routeData);
            Assert.AreEqual(3, routeData.Values.Count);
            Assert.AreEqual("Forum", routeData.Values["controller"]);
            Assert.AreEqual("Post", routeData.Values["action"]);
            Assert.AreEqual("14", routeData.Values["id"]);
        }

        [TestMethod]
        public void Get_Post_Path_With_Id()
        {
            RouteCollection routes = new RouteCollection();
            OpenForumManager.RegisterRoutingRules(routes);

            HttpContextBase context = CreateContext("forum");
            RouteData routeData = routes.GetRouteData(context);

            VirtualPathData path = routes.GetVirtualPath(
                new RequestContext(context, routeData),
                new RouteValueDictionary(new { controller = "Forum", action = "Post", id = 94 }));

            Assert.AreEqual("/Forum/Post/94", path.VirtualPath);
        }

        [TestMethod]
        public void Get_Reply_RouteData()
        {
            RouteCollection routes = new RouteCollection();
            OpenForumManager.RegisterRoutingRules(routes);

            HttpContextBase context = CreateContext("forum/reply/4");
            RouteData routeData = routes.GetRouteData(context);

            Assert.IsNotNull(routeData);
            Assert.AreEqual(5, routeData.Values.Count);
            Assert.AreEqual("Forum", routeData.Values["controller"]);
            Assert.AreEqual("Reply", routeData.Values["action"]);
            Assert.AreEqual("4", routeData.Values["postId"]);
            Assert.AreEqual("", routeData.Values["id"]);
            Assert.AreEqual("", routeData.Values["title"]);
        }

        [TestMethod]
        public void Get_Reply_Path()
        {
            RouteCollection routes = new RouteCollection();
            OpenForumManager.RegisterRoutingRules(routes);

            HttpContextBase context = CreateContext("forum");
            RouteData routeData = routes.GetRouteData(context);

            VirtualPathData path = routes.GetVirtualPath(
                new RequestContext(context, routeData),
                new RouteValueDictionary(new { controller = "Forum", action = "Reply", postId = 7 }));

            Assert.AreEqual("/Forum/Reply/7", path.VirtualPath);
        }

        [TestMethod]
        public void Get_Reply_RouteData_With_Id()
        {
            RouteCollection routes = new RouteCollection();
            OpenForumManager.RegisterRoutingRules(routes);

            HttpContextBase context = CreateContext("forum/Reply/4/3/some-title");
            RouteData routeData = routes.GetRouteData(context);

            Assert.IsNotNull(routeData);
            Assert.AreEqual(5, routeData.Values.Count);
            Assert.AreEqual("Forum", routeData.Values["controller"]);
            Assert.AreEqual("Reply", routeData.Values["action"]);
            Assert.AreEqual("4", routeData.Values["postId"]);
            Assert.AreEqual("3", routeData.Values["id"]);
            Assert.AreEqual("some-title", routeData.Values["title"]);
        }

        [TestMethod]
        public void Get_Reply_Path_With_Id()
        {
            RouteCollection routes = new RouteCollection();
            OpenForumManager.RegisterRoutingRules(routes);

            HttpContextBase context = CreateContext("forum");
            RouteData routeData = routes.GetRouteData(context);

            VirtualPathData path = routes.GetVirtualPath(
                new RequestContext(context, routeData),
                new RouteValueDictionary(new { controller = "Forum", action = "Reply", id = 43, postId = 9 }));

            Assert.AreEqual("/Forum/Reply/9/43", path.VirtualPath);
        }

        [TestMethod]
        public void Get_Script_RouteData()
        {
            RouteCollection routes = new RouteCollection();
            OpenForumManager.RegisterRoutingRules(routes);

            HttpContextBase context = CreateContext("forum/script");

            RouteData routeData = routes.GetRouteData(context);

            Assert.IsNotNull(routeData);
            Assert.AreEqual(2, routeData.Values.Count);
            Assert.AreEqual("Forum", routeData.Values["controller"]);
            Assert.AreEqual("Script", routeData.Values["action"]);
        }

        [TestMethod]
        public void Get_Script_Path()
        {
            RouteCollection routes = new RouteCollection();
            OpenForumManager.RegisterRoutingRules(routes);

            HttpContextBase context = CreateContext("forum");
            RouteData routeData = routes.GetRouteData(context);

            VirtualPathData path = routes.GetVirtualPath(
                new RequestContext(context, routeData),
                new RouteValueDictionary(new { controller = "Forum", action = "Script" }));

            Assert.AreEqual("/Forum/Script", path.VirtualPath);
        }

        [TestMethod]
        public void Get_Image_RouteData()
        {
            RouteCollection routes = new RouteCollection();
            OpenForumManager.RegisterRoutingRules(routes);

            HttpContextBase context = CreateContext("forum/image");

            RouteData routeData = routes.GetRouteData(context);

            Assert.IsNotNull(routeData);
            Assert.AreEqual(2, routeData.Values.Count);
            Assert.AreEqual("Forum", routeData.Values["controller"]);
            Assert.AreEqual("Image", routeData.Values["action"]);
        }

        [TestMethod]
        public void Get_Image_Path()
        {
            RouteCollection routes = new RouteCollection();
            OpenForumManager.RegisterRoutingRules(routes);

            HttpContextBase context = CreateContext("forum");
            RouteData routeData = routes.GetRouteData(context);

            VirtualPathData path = routes.GetVirtualPath(
                new RequestContext(context, routeData),
                new RouteValueDictionary(new { controller = "Forum", action = "Image" }));

            Assert.AreEqual("/Forum/Image", path.VirtualPath);
        }
    }
}

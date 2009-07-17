using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OpenForum.Core.Controllers;

namespace OpenForum.UnitTests
{
    public static class Utility
    {
        public static void SetValue(this object item, string fieldOrPropertyName, object value)
        {
            PrivateObject privateObject = new PrivateObject(item);
            privateObject.SetFieldOrProperty(fieldOrPropertyName, value);
        }

        public static void SetId(this object item, int id)
        {
            item.SetValue("Id", id);
        }

        public static void SetCurrentUser(string username)
        {
            GenericIdentity identity = new GenericIdentity(username);
            GenericPrincipal principal = new GenericPrincipal(identity, new string[0]);
            Thread.CurrentPrincipal = principal;
        }

        public static ControllerContext CreateMockControllerContext()
        {
            RouteData routeData = new RouteData();
            routeData.Values["controller"] = "forum";

            Mock<HttpContextBase> contextBase = new Mock<HttpContextBase>();

            RequestContext requestContext = new RequestContext(contextBase.Object, routeData);

            return new ControllerContext(requestContext, new ForumController());
        }
    }
}

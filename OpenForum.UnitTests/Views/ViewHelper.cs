using System;
using System.Text;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using System.Xml;
using Moq;

namespace OpenForum.UnitTests.Views
{
    public class ViewHelper : ViewPage
    {
        private StringWriter _writer = new StringWriter();
        public ViewUserControl Control { get; private set; }

        public ViewHelper(string view, object model)
        {
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            Mock<HttpResponseBase> response = new Mock<HttpResponseBase>();

            response.Expect(x => x.Write(It.IsAny<string>())).Callback<string>(x => _writer.Write(x));

            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            context.ExpectGet(x => x.Request).Returns(request.Object);
            context.ExpectGet(x => x.Response).Returns(response.Object);

            RequestContext requestContext = new RequestContext(context.Object, new RouteData());
            Url = new UrlHelper(requestContext);
            
            Type type = Type.GetType(string.Format("ASP.views_forum_{0}_ascx, OpenForum, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", view));
            ViewUserControl control = (ViewUserControl)Activator.CreateInstance(type);

            control.ViewContext = new ViewContext();
            control.ViewData = new ViewDataDictionary(model);
            control.ViewContext.HttpContext = context.Object;

            Controls.Add(control);

            control.GetType().GetMethod("__BuildControlTree", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(control, new [] { control });
            Control = control;
        }

        public XmlDocument RenderToXml()
        {
            Render(new HtmlTextWriter(_writer));

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(_writer.ToString());

            return xml;
        }
    }
}

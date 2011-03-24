using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using System.Xml;
using Moq;

namespace OpenForum.UnitTests.Views
{
	public class ViewHelper<T> where T : WebViewPage, new()
	{
		public T Control { get; private set; }

		public ViewHelper(object model)
		{
			Control = new T();

			ViewContext viewContext = new ViewContext(new ControllerContext(), Mock.Of<IView>(), new ViewDataDictionary(model), new TempDataDictionary(), new StringWriter());

			Mock<HttpContextBase> context = new Mock<HttpContextBase>();
			context.SetupGet(x => x.Request).Returns(Mock.Of<HttpRequestBase>());
			context.SetupGet(x => x.Response).Returns(Mock.Of<HttpResponseBase>());
			context.SetupGet(x => x.Items).Returns(new Dictionary<object, object>());

			viewContext.HttpContext = context.Object;

			Control.ViewContext = viewContext;
			Control.ViewData = viewContext.ViewData;
			Control.InitHelpers();
		}

		public XmlDocument RenderToXml()
		{
			XmlDocument result = new XmlDocument();

			StringWriter writer = new StringWriter();
			Control.ExecutePageHierarchy(new WebPageContext(), writer);
			result.LoadXml(writer.ToString());

			return result;
		}
	}
}

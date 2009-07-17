using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OpenForum.Core.Views;
using OpenForum.Core.ViewModels;

namespace OpenForum.UnitTests.Views
{
    [TestClass]
    public class ForumViewHelperTests
    {
        [TestMethod]
        public void Creates_Url_Friendly_Titles()
        {
            Assert.AreEqual("A_Nice_Title1", ForumViewHelper.ToUrlFriendlyTitle("@$#A Nice_Title1#@$#@"));
        }

        [TestMethod]
        public void Include_Editor_Script()
        {
            StringWriter writer = new StringWriter();

            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            Mock<HttpResponseBase> response = new Mock<HttpResponseBase>();

            response.Expect(x => x.Write(It.IsAny<string>())).Callback<string>(x => writer.Write(x));

            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            context.ExpectGet(x => x.Request).Returns(request.Object);
            context.ExpectGet(x => x.Response).Returns(response.Object);

            RequestContext requestContext = new RequestContext(context.Object, new RouteData());

            string result = ForumViewHelper.GetWysiwygEditorText(new UrlHelper(requestContext), "textAreaId");

            string expectedResult = @"
<script type=""text/javascript"" src=""""></script>
<script type=""text/javascript"">
    //<![CDATA[
    bkLib.onDomLoaded(function() { new nicEditor({ iconsPath : '', buttonList: ['bold', 'italic', 'underline', 'link'] }).panelInstance('textAreaId'); });
    //]]>
</script>
";

            Assert.AreEqual(expectedResult.Substring(0, 200), result.Substring(0, 200));
        }

        [TestMethod]
        public void Get_Default_Styles()
        {
            Assert.IsTrue(ForumViewHelper.GetDefaultStyles().StartsWith("\r\n<style type=\"text/css\">"));
        }
    }
}

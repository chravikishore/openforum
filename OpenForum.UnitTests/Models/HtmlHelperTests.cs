using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenForum.Core;
using OpenForum.Core.Models;
using HtmlAgilityPack;

namespace OpenForum.UnitTests.Models
{
    [TestClass]
    public class HtmlHelperTests
    {
        [TestMethod]
        public void Validate_Malformed_Html()
        {
            string html = "<div>no closing tag";

            try
            {
                HtmlHelper.Validate(html);
                Assert.Fail();
            }
            catch (OpenForumException exception)
            {
                Assert.AreEqual("Malformed html.", exception.Message);
            }
        }

        [TestMethod]
        public void Html_With_No_Tags_Is_Valid()
        {
            string html = "here's some text";
            HtmlHelper.Validate(html);
        }

        [TestMethod]
        public void Html_With_Allowed_Elements_Is_Valid()
        {
            string html = "<a href='test.com' /><br><br/><b><i><p><span>test</span></p></i></b>";
            HtmlHelper.Validate(html);
        }

        [TestMethod]
        public void Null_Html_Is_Valid()
        {
            HtmlHelper.Validate(null);
        }

        [TestMethod]
        public void FixUp_Removes_Unsafe_Attributes()
        {
            string html = HtmlHelper.FixUp("<span style=\"font-weight: bold;\">test</span>");
            Assert.AreEqual("<span style=\"font-weight: bold;\">test</span>", html);
        }

        [TestMethod]
        public void FixUp_Adds_Rel_Attributes_To_Anchors()
        {
            string html = HtmlHelper.FixUp("<a href=\"http://test.com\">link</a>");
            Assert.AreEqual("<a href=\"http://test.com\" rel=\"nofollow\">link</a>", html);
        }

        [TestMethod]
        public void FixUp_Fixes_Bad_Rel_Attributes()
        {
            string html = HtmlHelper.FixUp("<a href=\"http://test.com\" rel=\"follow\">link</a>");
            Assert.AreEqual("<a href=\"http://test.com\" rel=\"nofollow\">link</a>", html);
        }

        [TestMethod]
        public void Unsafe_Elements_Are_Removed()
        {
            string html = HtmlHelper.FixUp("here <script>is</script> <div>some <span>content</span></div>");
            Assert.AreEqual("here is some <span>content</span>", html);
        }

        [TestMethod]
        public void Styles_With_Background_Images_Are_Removed()
        {
            string html = HtmlHelper.FixUp("<span style=\"background-image: someurl;\">test</span>");
            Assert.AreEqual("<span>test</span>", html);
        }

        [TestMethod]
        public void Anchors_Can_Not_Execute_Javascript()
        {
            string html = HtmlHelper.FixUp("<a href='javascript:alert(\"test\")'>text</a>");
            Assert.AreEqual("<a rel=\"nofollow\">text</a>", html);
        }
    }
}

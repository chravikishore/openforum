using System.Text.RegularExpressions;
using System.Web.Mvc;
using OpenForum.Core.ViewModels;

namespace OpenForum.Core.Views
{
    public static class ForumViewHelper
    {
        public static void RenderForumIndexControl(this HtmlHelper html, IIndexViewModel model)
        {
            RenderForumControl(html, "index", model);
        }

        public static void RenderForumViewControl(this HtmlHelper html, IViewViewModel model)
        {
            RenderForumControl(html, "view", model);
        }

        public static void RenderPostIndexControl(this HtmlHelper html, IPostViewModel model)
        {
            RenderForumControl(html, "post", model);
        }

        public static void RenderReplyIndexControl(this HtmlHelper html, IPostViewModel model)
        {
            RenderForumControl(html, "reply", model);
        }

        internal static void RenderForumControl(HtmlHelper html, string viewName, object model)
        {
            ViewUserControl control = ViewFinder.GetControl(viewName, "forum");
            control.ViewData = new ViewDataDictionary(model);
            control.RenderView(html.ViewContext);
        }

        public static string ToUrlFriendlyTitle(string title)
        {
            title = (title ?? "").Replace(' ', '_');
            return Regex.Replace(title, @"\W*", "");
        }

        public static string GetWysiwygEditorText(UrlHelper url, string textareaId)
        {
            string script = @"
<script type=""text/javascript"" src=""{0}""></script>
<script type=""text/javascript"">
    //<![CDATA[
    bkLib.onDomLoaded(function() {{ new nicEditor({{ iconsPath : '{1}', buttonList: ['bold', 'italic', 'underline', 'link'] }}).panelInstance('{2}'); }});
    //]]>
</script>";

            return string.Format(script, url.Action("Script"), url.Action("Image"), textareaId);
        }

        public static string GetDefaultStyles()
        {
            return @"
<style type=""text/css"">    
.openforum_maincontent { width: 100%; }
.openforum_actions, .openforum_index_paging { margin: 13px 0; }
.openforum_title { font-weight: bold; }
div.openforum_title { font-size: 1.4em; padding: 0; margin: 0 0 10px 0; }
.openforum_user, .openforum_modified, .openforum_replies, .openforum_views, .openforum_body, .openforum_title { padding: 10px; }
.openforum_user, .openforum_user_image, .openforum_modified, .openforum_replies, .openforum_views { font-size: 0.75em; vertical-align: top; }
.openforum_user, .openforum_modified { width: 150px; }
.openforum_user_image { width:30px; height:30px; }
.openforum_user_image img { width:30px; height:30px; }
input[type=""text""].openforum_textbox { width: 800px; margin: 0 0 10px 0; }
.openforum_textarea { width: 800px; height: 400px; margin: 0 0 10px 0; } 
.openforum_message { padding: 10px 0 10px 0; }
.openforum_index_paging form { display: inline }
</style>
";
        }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.431
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OpenForum.Core.Views.Forum
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel.DataAnnotations;
    using System.Configuration;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Caching;
    using System.Web.DynamicData;
    using System.Web.SessionState;
    using System.Web.Profile;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;
    using System.Xml.Linq;
    using OpenForum.Core.Views;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MvcRazorClassGenerator", "1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Forum/Post.cshtml")]
    public class _Page_Views_Forum_Post_cshtml : System.Web.Mvc.WebViewPage<OpenForum.Core.ViewModels.Interfaces.IPostViewModel>
    {
#line hidden

        public _Page_Views_Forum_Post_cshtml()
        {
        }
        protected System.Web.HttpApplication ApplicationInstance
        {
            get
            {
                return ((System.Web.HttpApplication)(Context.ApplicationInstance));
            }
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


WriteLiteral("\r\n<div class=\"openforum_container\">\r\n    \r\n    ");


Write(Model.IncludeDefaultStyles ? Html.Raw(ForumViewHelper.GetDefaultStyles()) : MvcHtmlString.Empty);

WriteLiteral("\r\n    ");


Write(Model.IncludeValidationSummary ? Html.ValidationSummary() : MvcHtmlString.Empty);

WriteLiteral("\r\n    ");


Write(Model.IncludeWysiwygEditor ? Html.Raw(ForumViewHelper.GetWysiwygEditorText(Url, "body")) : MvcHtmlString.Empty);

WriteLiteral("\r\n    \r\n");


     using (Html.BeginForm()) 
    { 

WriteLiteral("    <div class=\"openforum_maincontent\">\r\n    \r\n        <label>Title</label>\r\n    " +
"    <div>");


        Write(Html.TextBox("title", Model.Post.Title, new { @class = "openforum_textbox" }));

WriteLiteral("</div>\r\n        \r\n        <label>Question</label>\r\n        <div>");


        Write(Html.TextArea("body", Model.Post.Body, new { @class = "openforum_textarea" }));

WriteLiteral("</div>\r\n        \r\n        <div class=\"openforum_actions\"><input type=\"submit\" val" +
"ue=\"Submit\" /> ");


                                                                         Write(Html.ActionLink("Cancel", "Index"));

WriteLiteral("</div>\r\n    \r\n    </div>\r\n");



    
Write(Html.Hidden("id", Model.Post.Id));

                                     
    }

WriteLiteral("    \r\n</div>");


        }
    }
}

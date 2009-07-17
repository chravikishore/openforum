<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OpenForum.Core.ViewModels.ReplyViewModel>" %>


<div class="openforum_container">
    
    <%= (Model.IncludeDefaultStyles) ? ForumViewHelper.GetDefaultStyles() : ""%>
    <%= (Model.IncludeValidationSummary) ? Html.ValidationSummary() : ""%>
    <%= (Model.IncludeWysiwygEditor) ? ForumViewHelper.GetWysiwygEditorText(Url, "body") : ""%>
    
    <% Html.BeginForm(); %>

    <div class="openforum_maincontent">

        <label>Reply</label>
        <div><%= Html.TextArea("body", Model.Reply.Body, new { @class = "openforum_textarea" }) %></div>

        <div class="openforum_actions"><input type="submit" value="Submit" /> <%= Html.ActionLink("Cancel", "View", new { id = Model.Reply.PostId, title = ForumViewHelper.ToUrlFriendlyTitle(Model.Post.Title) })%></div>
        
        <%= Html.Hidden("id", Model.Reply.Id) %>
        <%= Html.Hidden("postId", Model.Reply.PostId) %>

    </div>

    <% Html.EndForm(); %>
    
</div>
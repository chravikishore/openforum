<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OpenForum.Core.ViewModels.PostViewModel>" %>


<div class="openforum_container">
    
    <%= (Model.IncludeDefaultStyles) ? ForumViewHelper.GetDefaultStyles() : "" %>
    <%= (Model.IncludeValidationSummary) ? Html.ValidationSummary() : ""%>
    <%= (Model.IncludeWysiwygEditor) ? ForumViewHelper.GetWysiwygEditorText(Url, "body") : ""%>
    
    <% Html.BeginForm(); %>

    <div class="openforum_maincontent">
    
        <label>Title</label>
        <div><%= Html.TextBox("title", Model.Post.Title, new { @class = "openforum_textbox" }) %></div>
        
        <label>Question</label>
        <div><%= Html.TextArea("body", Model.Post.Body, new { @class = "openforum_textarea" }) %></div>
        
        <div class="openforum_actions"><input type="submit" value="Submit" /> <%= Html.ActionLink("Cancel", "Index") %></div>
    
    </div>

    <%= Html.Hidden("id", Model.Post.Id) %>

    <% Html.EndForm(); %>
    
</div>
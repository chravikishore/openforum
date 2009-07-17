<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OpenForum.Core.ViewModels.IndexViewModel>" %>

<div class="openforum_container">
    
    <%= (Model.IncludeDefaultStyles) ? ForumViewHelper.GetDefaultStyles() : ""%>
    <%= (Model.IncludeValidationSummary) ? Html.ValidationSummary() : ""%>
    
    
    <div class="openforum_actions">
        <%= Html.ActionLink("Write A New Post", "Post") %>
    </div>            

    <div class="openforum_search">
        <% Html.BeginForm(); %>

            Search: <%= Html.TextBox("searchQuery") %> <input type="submit" value="go" />
            
        <% Html.EndForm(); %> 
    </div>
    
    <div class="openforum_message"><%= Model.Message %></div>
        
    <table class="openforum_maincontent">
        
        <% foreach(var item in Model.Posts ?? new Post[0]) { %>
        
        <tr>
            <td class="openforum_title"><%= Html.ActionLink(item.Title, "view", new { id = item.Id, title = ForumViewHelper.ToUrlFriendlyTitle(item.Title) })%></td>
            <td class="openforum_user">
                <div>created by <%= Html.Encode(item.CreatedBy.Username) %></div>
                <div><%= item.CreatedDate.ToString("MM/dd/yyyy hh:mm tt") %></div>
            </td>
            <td class="openforum_modified">
                <div>last post by <%= Html.Encode(item.LastPostBy.Username) %></div>
                <div><%= item.LastPostDate.ToString("MM/dd/yyyy hh:mm tt") %></div>
            </td>
            <td class="openforum_replies">Replies: <%= item.Replies.Length %></td>
            <td class="openforum_views">Views: <%= item.ViewCount %></td>
        </tr>
        
        <% } %>
        
    </table>
    
    <div class="openforum_index_paging">
        <% if ((Model.Posts ?? new Post[0]).Count() > 0) { %>
            <span>Page <%= Model.CurrentPage + 1 %> of <%= Model.TotalPages %></span>
        <% } %>
            
        <% if (Model.CurrentPage > 0) { %>
            
            <% Html.BeginForm(); %>
            <input type="submit" value="&lt;&lt;&lt;" />
            <input type="hidden" name="searchQuery" value="<%= Model.SearchQuery %>" />
            <input type="hidden" name="page" value="<%= Model.CurrentPage - 1 %>" />
            <% Html.EndForm(); %>
        
        <% } %>
        
        <% if (Model.CurrentPage < Model.TotalPages - 1) { %>
            
            <% Html.BeginForm(); %>
            <input type="submit" value="&gt;&gt;&gt;" />
            <input type="hidden" name="searchQuery" value="<%= Model.SearchQuery %>" />
            <input type="hidden" name="page" value="<%= Model.CurrentPage + 1 %>" />
            <% Html.EndForm(); %> 
            
        <% } %>
        
    </div>
    
    
</div>
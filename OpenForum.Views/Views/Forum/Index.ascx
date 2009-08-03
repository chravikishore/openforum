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
      
    <div class="openforum_maincontent">
        
        <% foreach(var item in Model.Posts ?? new Post[0]) { %>
        
        <div class="openforum_item">
            <div class="openforum_stats">
                <div>
                    <div><%= item.LastPostDate.ToString("M/d/yy") %></div>
                    <div><%= item.LastPostDate.ToString("h:mm tt") %></div>
                </div>
                <div>
                    <div><%= item.ViewCount%> Views</div>
                    <div><%= item.Replies.Length %> Posts</div>
                </div>
            </div>
            <div>
                <h2 class="openforum_title">
                    <%= Html.ActionLink(item.Title, "view", new { id = item.Id, title = ForumViewHelper.ToUrlFriendlyTitle(item.Title) })%>
                </h2>
            </div>
            <div class="openforum_postpreview">
                First post: <%= Html.Encode(item.CreatedBy.Username) %> wrote: <%= item.Body %>
            </div>
            <div class="openforum_postpreview">
                Latest post: <%= Html.Encode(item.LastPostBy.Username) %> wrote: FIX THIS!!!!!
            </div>
            <span></span>
        </div>
        
        <div class="openforum_clear"></div>
        
        <% } %>
    
    </div>
        
        
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
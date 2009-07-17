<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OpenForum.Core.ViewModels.ViewViewModel>" %>


<div class="openforum_container">
    
    <%= (Model.IncludeDefaultStyles) ? ForumViewHelper.GetDefaultStyles() : ""%>
    <%= (Model.IncludeValidationSummary) ? Html.ValidationSummary() : ""%>
    
    <div class="openforum_title"><%= Html.Encode(Model.Post.Title) %></div>

    <table class="openforum_maincontent">
        <tr>
            <td class="openforum_body"><%= Model.Post.Body %></td>

            <% if (Model.Post.CreatedBy.HasImageUrl) { %>
            <td class="openforum_user_image">
                <div><img src="<%= Model.Post.CreatedBy.ImageUrl %>" alt="<%= Html.Encode(Model.Post.CreatedBy.Username) %>" /></div>
            </td>
            <% } %>

            <td class="openforum_user">
                <div>                
                    <% if (Model.Post.CreatedBy.HasUserUrl) { %>
                    <a href="<%= Model.Post.CreatedBy.UserUrl %>"><%= Html.Encode(Model.Post.CreatedBy.Username)%></a>
                    <% } else { %>
                    <%= Html.Encode(Model.Post.CreatedBy.Username)%>
                    <% } %>                
                </div>
                
                <div><%= Model.Post.CreatedDate.ToString("MM/dd/yyyy HH:mm tt") %></div>

                <%= (Model.CurrentUser != null && Model.Post.CreatedBy.Id == Model.CurrentUser.Id) ? Html.ActionLink("Edit", "Post", new { id = Model.Post.Id }) : ""%>                

            </td>
        </tr>

        <% foreach (var item in Model.Post.Replies) { %>
            
        <tr>
            <td class="openforum_body"><%= item.Body%></td>

            <% if (item.CreatedBy.HasImageUrl) { %>
            <td class="openforum_user_image">
                <div><img src="<%= item.CreatedBy.ImageUrl %>" alt="<%= Html.Encode(item.CreatedBy.Username) %>" /></div>
            </td>
            <% } %>

            <td class="openforum_user">
                <div>
                
                    <% if (item.CreatedBy.HasUserUrl) { %>
                    <a href="<%= item.CreatedBy.UserUrl %>"><%= Html.Encode(item.CreatedBy.Username)%></a>
                    <% } else { %>
                    <%= Html.Encode(item.CreatedBy.Username)%>
                    <% } %>
                
                </div>
                
                <div><%= item.CreatedDate.ToString("MM/dd/yyyy HH:mm tt")%></div>

                <%= (Model.CurrentUser != null && item.CreatedBy.Id == Model.CurrentUser.Id) ? Html.ActionLink("Edit", "Reply", new { id = item.Id, postId = Model.Post.Id }) : ""%>                

            </td>
        </tr>
            
        <% } %>

    </table>

    <div class="openforum_actions">
        <%= Html.ActionLink("Back", "index") %>
        <%= Html.ActionLink("Reply", "Reply", new { id = "", postId = Model.Post.Id, title = ForumViewHelper.ToUrlFriendlyTitle(Model.Post.Title) }) %>
    </div>
</div>
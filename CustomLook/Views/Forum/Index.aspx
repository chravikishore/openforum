<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Site.Master" Inherits="System.Web.Mvc.ViewPage<CustomLook.CustomIndexViewModel>" %>
<%@ Import Namespace="OpenForum.Core.Views" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MyTitleContentArea" runat="server">
	Custom Page Title
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MyMainContentArea" runat="server">

    <h2><%= Model.Header %></h2>

    <% Html.RenderForumIndexControl(Model.IndexViewModel); %>

    <h2><%= Model.Footer %></h2>
    
</asp:Content>

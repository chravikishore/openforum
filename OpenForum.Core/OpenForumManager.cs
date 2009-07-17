using System.Web.Mvc;
using System.Web.Routing;
using OpenForum.Core.Views;

namespace OpenForum.Core
{
    public static class OpenForumManager
    {
        private static bool _areConfigurationsSet = false;

        public static void SimpleInitialize()
        {
            SimpleInitialize(Configurations.Current);
        }

        public static void SimpleInitialize(Configurations configurations)
        {
            SetConfigurations(configurations);
            RegisterRoutingRules(RouteTable.Routes);
            RegisterViewEngine();
            configurations.PostRepository.RebuildSearchIndex();
        }

        public static void SetConfigurations(Configurations configurations)
        {
            if (_areConfigurationsSet)
            {
                throw new OpenForumException("Configurations have already been set. Configurations cannont be set more than once.");
            }

            _areConfigurationsSet = true;
            Configurations.Current = configurations;
        }

        public static void RegisterViewEngine()
        {
            ViewEngines.Engines.Add(new DynamicViewEngine());
        }

        public static void RegisterRoutingRules(RouteCollection routes)
        {
            routes.MapRoute("ForumIndex", "Forum", new { controller = "Forum", action = "Index" });
            routes.MapRoute("ForumView", "Forum/View/{id}/{title}", new { controller = "Forum", action = "View", id = "", title = "" });
            routes.MapRoute("ForumSubmitPost", "Forum/Post/{id}", new { controller = "Forum", action = "Post", id = "" });
            routes.MapRoute("ForumSubmitReply", "Forum/Reply/{postId}/{id}/{title}", new { controller = "Forum", action = "Reply", postId = "", id = "", title = "" });
            routes.MapRoute("ForumScript", "Forum/Script", new { controller = "Forum", action = "Script" });
            routes.MapRoute("ForumImage", "Forum/Image", new { controller = "Forum", action = "Image" });
        }
    }
}

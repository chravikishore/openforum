using System.IO;
using System.Web.Mvc;
using System.Web.UI;
using OpenForum.Core.Models;
using OpenForum.Core.Views;
using OpenForum.Core.ViewModels;

namespace OpenForum.Core.Views
{
    public class DynamicView : IView
    {
        public string PrimaryContentPlaceHolderId { get; set; }
        public string TitleContentPlaceHolderId { get; set; }
        public string MasterLocation { get; set; }
        public string ViewName { get; set; }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            DynamicViewPage viewPage = new DynamicViewPage();
            viewPage.AppRelativeVirtualPath = "/";
            viewPage.MasterLocation = MasterLocation;
            viewPage.ViewData = viewContext.ViewData;


            ITitledViewModel titledViewModel = viewContext.ViewData.Model as ITitledViewModel;
            if (TitleContentPlaceHolderId != null && titledViewModel != null)
            {
                viewPage.AddContentControl(TitleContentPlaceHolderId, (w, p) => w.Write(titledViewModel.PageTitle));
            }

            viewPage.AddContentControl(PrimaryContentPlaceHolderId, (w, p) => ForumViewHelper.RenderForumControl(viewPage.Html, ViewName, viewPage.ViewData.Model));
            
            viewPage.RenderView(viewContext);
        }

        private class DynamicViewPage : ViewPage
        {
            public void AddContentControl(string contentPlaceHolderId, RenderMethod renderMethod)
            {
                CompiledTemplateBuilder compiledTemplateBuilder = new CompiledTemplateBuilder(x => x.SetRenderMethodDelegate(renderMethod));
                AddContentTemplate(contentPlaceHolderId, compiledTemplateBuilder);
            }
        }
    }
}
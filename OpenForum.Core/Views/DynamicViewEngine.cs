using System;
using System.Web.Mvc;
using System.Reflection;
using System.Web.UI;
using System.Collections.Generic;

namespace OpenForum.Core.Views
{
    public class DynamicViewEngine : IViewEngine
    {
        private string _masterPageLocation;
        private string _primaryContentPlaceHolderId;
        private string _titleContentPlaceHolderId;


        public DynamicViewEngine()
        {
            _masterPageLocation = Configurations.Current.MasterPageLocation;
            _primaryContentPlaceHolderId = Configurations.Current.MasterPageContentPlaceHolderId;
            _titleContentPlaceHolderId = Configurations.Current.MasterPageTitlePlaceHolderId;
        }

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return new ViewEngineResult(new string[] { "Dynamic Forum Views" });
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            string controllerName = (string)controllerContext.RouteData.Values["controller"];
            
            if (!ViewFinder.HasView(viewName, controllerName))
            {
                return new ViewEngineResult(new string[] { "Dynamic Forum Views" });
            }

            DynamicView view = new DynamicView();
            view.MasterLocation = _masterPageLocation;
            view.PrimaryContentPlaceHolderId = _primaryContentPlaceHolderId;
            view.TitleContentPlaceHolderId = _titleContentPlaceHolderId;
            view.ViewName = viewName;

            return new ViewEngineResult(view, this);
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            IDisposable disposable = view as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace OpenForum.Core.Views
{
    internal static class ViewFinder
    {
        private static Dictionary<string, Type> _views = new Dictionary<string, Type>();

        static ViewFinder()
        {
            foreach (var item in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (item.FullName.StartsWith("ASP."))
                {
                    _views.Add(item.FullName.ToLower(), item);
                }
            }
        }

        public static bool HasView(string viewName, string controllerName)
        {
            return GetControlType(viewName, controllerName) != null;
        }

        public static ViewUserControl GetControl(string viewName, string controllerName)
        {
            Type type = GetControlType(viewName, controllerName);

            if (type == null)
            {
                return null;
            }

            return (ViewUserControl)Activator.CreateInstance(type); ;
        }

        private static Type GetControlType(string viewName, string controllerName)
        {
            Type type;
            string viewTypeName = string.Format("ASP.views_{0}_{1}_ascx", controllerName, viewName);
            
            _views.TryGetValue(viewTypeName.ToLower(), out type);

            return type;
        }
    }
}

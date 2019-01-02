using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SystemSetup
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.IgnoreRoute("*/{*path}");

            routes.MapRoute(
                name: "Default",                
                url: "{controller}/{action}/{id}", 
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "SystemSetup.SystemSetup.Areas.UserManagement.Controllers" }
                //namespaces: new string[] { "SystemSetup.Controllers" }
            ).DataTokens.Add("area", "UserManagement");
        }

        public class RazorViewFactory : RazorViewEngine
        {
            public RazorViewFactory()
            {
                //MasterLocationFormats = new[] { "~/Views/Shared/{0}.cshtml" };

                //ViewLocationFormats = new[]{
                //    "~/Areas/Customer/Views/{1}/{0}.cshtml",
                //    "~/Areas/Estimate/Views/{1}/{0}.cshtml",
                //    "~/Areas/UserManagement/Views/{1}/{0}.cshtml",
                //    "~/Areas/Common/Views/{1}/{0}.cshtml",
                //    "~/Areas/PartnerStaff/Views/{1}/{0}.cshtml",
                //    "~/Areas/SendMail/Views/{1}/{0}.cshtml",
                //    "~/Views/{1}/{0}.cshtml",
                //    "~/Views/Shared/{0}.cshtml"
                //    };
            }
        }
    }
}
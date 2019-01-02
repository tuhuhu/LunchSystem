using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SystemSetup.Areas.UserManagement.Controllers;
using SystemSetup.UtilityServices.ActionLogService;
using SystemSetup.UtilityServices.LogService;

namespace SystemSetup
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    using System.Configuration;
    using System.IO;
    using SystemSetup.Constants;
    using SystemSetup.Models;
    using SystemSetup.UtilityServices;

    public class MvcApplication : System.Web.HttpApplication
    {
        private Dictionary<string, Dictionary<string, object>> shortLiveCache
                                            = new Dictionary<string, Dictionary<string, object>>();

        protected void Application_Start()
        {
            // log4net setting.
            //log4net.GlobalContext.Properties["LogFileName"] = "ngaht1.txt";
            //log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Configs/log.config")));

            AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //AuthConfig.RegisterAuth();

            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            //ViewEngines.Engines.Clear();
            //ViewEngines.Engines.Add(new SystemSetup.RouteConfig.RazorViewFactory());
            Application["shortLiveCache"] = shortLiveCache;
            var clientDataTypeProvider = ModelValidatorProviders.Providers.FirstOrDefault(p => p.GetType().Equals(typeof(ClientDataTypeModelValidatorProvider)));
            ModelValidatorProviders.Providers.Remove(clientDataTypeProvider);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            CmnEntityModel cmnEntityModel = (CmnEntityModel)CacheUtil.GetCache<CmnEntityModel>("CmnEntityModel");

            string companyCd = String.Empty;
            string userID = String.Empty;
            string userName = String.Empty;
            long userSegNo;

            if (cmnEntityModel != null)
            {
                try
                {
                    companyCd = cmnEntityModel.CompanyCd;
                    userID = cmnEntityModel.UserID;
                    userName = cmnEntityModel.UserName;
                    userSegNo = cmnEntityModel.UserSegNo;
                }
                catch (Exception)
                {
                }
            }
            companyCd = string.IsNullOrEmpty(companyCd) ? "NONE" : companyCd;
            userID = string.IsNullOrEmpty(userID) ? "NONE" : userID;
            userName = string.IsNullOrEmpty(userName) ? "NONE" : userName;
            UtilityServices.UtilityServices.ProcFatalException(ex, companyCd, userID, Server.MapPath("~/Configs/log.config"));
            
            HttpContext.Current.Session["ERROR"] = ex;
            UtilityServices.UtilityServices.NoticeException(ex, companyCd, userID);
            Response.Redirect("/Common/Common/CloseIframe/");
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            if (!Request.IsSecureConnection)
            {
                if (RedirectHttps)
                {
                    var url = Request.Url.AbsoluteUri;
                    var secureUrl = Regex.Replace(url, @"^\w+(?=://)", Uri.UriSchemeHttps);

                    if (PermanentHttps)
                    {
                        Response.RedirectPermanent(secureUrl, true);
                    }
                    else
                    {
                        Response.Redirect(secureUrl, true);
                    }
                }
            }
        }

        static bool RedirectHttps
        {
            get { return Convert.ToBoolean(ConfigurationManager.AppSettings["app:RedirectHttps"]); }
        }

        static bool PermanentHttps
        {
            get { return Convert.ToBoolean(ConfigurationManager.AppSettings["app:PermanentHttps"]); }
        }

    }
}
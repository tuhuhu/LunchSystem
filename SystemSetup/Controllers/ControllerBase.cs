using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SystemSetup.Models;
using SystemSetup.Constants;
using SystemSetup.Constants.Resources;
using SystemSetup.UtilityServices;
using SystemSetup.UtilityServices.LogService;
using System.Web.Routing;
using SystemSetup.UtilityServices.ActionLogService;

namespace SystemSetup.Controllers
{
    /// <summary>
    /// BaseController for all the controllers
    /// </summary>
    [Authorize]
    [ValidateInput(false)]
    public class BaseController : Controller
    {
        protected static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SESSION_SITEMAP = "SESSION_SITEMAP";
        private CmnEntityModel cmnEntityModel = null;

        /// <summary>
        /// Gets common entity model.
        /// </summary>
        /// <value>
        /// The common entity model.
        /// </value>
        public CmnEntityModel CmnEntityModel
        {
            get
            {
                if (cmnEntityModel == null)
                {
                    if (CacheUtil.GetCache<CmnEntityModel>("CmnEntityModel") == null)
                    {
                        CacheUtil.SaveCache("CmnEntityModel", new CmnEntityModel());
                    }
                    cmnEntityModel = (CmnEntityModel)CacheUtil.GetCache<CmnEntityModel>("CmnEntityModel");
                }
                return cmnEntityModel;
            }
        }

        /// <summary>
        /// Check authority key in list authority of setting user
        /// </summary>
        /// <param name="AUTHORITY_KEY"></param>
        /// <returns></returns>
        public bool IsInAuthorityList(string AUTHORITY_KEY)
        {            
            if (CmnEntityModel == null)
            {
                FormsAuthentication.SignOut();
                RemoveCache("CmnEntityModel");                
                Session.Clear();
                return false;
            }
            var authorityList = CmnEntityModel.AuthorityList;            

            return (authorityList.IndexOf(AUTHORITY_KEY) != -1);
        }

        /// <summary>
        /// Called before the action method is invoked.
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Request.IsAjaxRequest())
            {
                SetBuildInfo(Request);

                var routeData = filterContext.RouteData;
                var area = routeData.DataTokens["area"].ToString();
                var controller = routeData.Values["controller"].ToString();
                var action = routeData.Values["action"].ToString();
                
                if (CmnEntityModel != null
                     && area != Constants.ScreenId.UserManagement
                     && controller != Constants.ScreenId.Login
                     && controller != Constants.ScreenId.ChangePass
                     && controller != Constants.ScreenId.Common
                     && action != Constants.ScreenId.Error
                     && action != Constants.ScreenId.AuthentTimeout)
                {
                    string authorityKey = area + "/" + controller + "/" + action;
                    //if (!IsInAuthorityList(authorityKey))
                    //{
                    //    filterContext.Result = new RedirectToRouteResult(
                    //        new RouteValueDictionary
                    //        {
                    //            {"Controller", "Login"},
                    //            {"Action", "Logout"},
                    //            {"area", "UserManagement"}
                    //        });
                    //}
                    //else
                    //{
                    //    string browserType = "NONE";
                    //    string browserVersion = "NONE";
                    //    var browser = Request.Browser;
                    //    if (browser != null)
                    //    {
                    //        browserType = browser.Type;
                    //        browserVersion = browser.Version;
                    //    }
                    //    var url = Request.Url;
                    //    string requestedUrl = url != null ? url.ToString() : string.Empty;
                    //    ActionLogService.ActionLog(authorityKey, CmnEntityModel, Request.UserAgent, browserType, browserVersion, requestedUrl);
                    //}
                }

                int pos = Sitemap.FindIndex(item => item.AreaName == area && item.ControllerName == controller && item.ActionName == action);               

                if (0 <= pos)
                {
                    Sitemap.RemoveRange(pos, 0);
                }
                else
                {
                    var item = new SitemapItem
                    {
                        AreaName = area,
                        ControllerName = controller,
                        ActionName = action,
                        RestoreData = null
                    };

                    Sitemap.Insert(0, item);
                }
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Called after the action method is invoked.
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (Request.IsAjaxRequest())
            {
                if (!ModelState.IsValid)
                {
                    filterContext.Result = Json(
                        new
                        {
                            ErrorMessages = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)
                        }
                    );
                }
            }
        }

        /// <summary>
        /// Called when [exception].
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.Exception != null)
            {
                LogService.Fatal(logger, "Exceptionが発生しました。\n \n ", filterContext.Exception);

                string companyCd = "NONE";
                string userID = "NONE";
                try
                {
                    if (CmnEntityModel != null)
                    {
                        companyCd = CmnEntityModel.CompanyCd;
                        userID = CmnEntityModel.UserID;
                    }
                    UtilityServices.UtilityServices.ProcFatalException(filterContext.Exception, companyCd, userID, Server.MapPath("~/Configs/log.config"));
                }
                catch (Exception)
                {
                }
            }
            else
            {
                LogService.Fatal(logger, "処理中にシステムエラーが発生しました（Exception情報なし）\n \n ");
            }

            LogService.Debug(logger, "      ------ 出力 終わり ------\n \n ");
            base.OnException(filterContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected object GetRestoreData()
        {
            object data = null;

            var area = RouteData.DataTokens["area"].ToString();
            var controller = RouteData.Values["controller"].ToString();
            var action = RouteData.Values["action"].ToString();

            int pos = Sitemap.FindIndex(item => item.AreaName == area && item.ControllerName == controller && item.ActionName == action);

            if (0 <= pos)
            {
                data = Sitemap[pos].RestoreData;
                //Sitemap[pos].RestoreData = null;
            }

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        protected void SaveRestoreData(object data)
        {
            if (data != null)
            {
                var area = RouteData.DataTokens["area"].ToString();
                var controller = RouteData.Values["controller"].ToString();
                var action = RouteData.Values["action"].ToString();

                int pos = Sitemap.FindIndex(item => item.AreaName == area && item.ControllerName == controller && item.ActionName == action);

                if (0 <= pos)
                {
                    Sitemap[pos].RestoreData = data;
                }
            }
        }

        private string GetWindowName()
        {
            var controller = RouteData.Values["controller"].ToString();

            string windowName = Constants.WindowName.MAIN;

            HttpCookie cookie = Request.Cookies[Constants.WindowName.COOKIE_NAME];
            if (cookie != null)
            {
                if (Constants.WindowName.Items.Contains(controller))
                {
                    cookie.Value = Constants.WindowName.Items[controller] as string;
                }

                windowName = cookie.Value;
            }

            return windowName;
        }

        /// <summary>
        /// 
        /// </summary>
        private List<SitemapItem> Sitemap
        {
            get
            {
                var windowName = GetWindowName();
                var sitemaps = Session[SESSION_SITEMAP] as IDictionary<string, List<SitemapItem>>;

                if (sitemaps == null)
                {
                    sitemaps = new Dictionary<string, List<SitemapItem>>();
                    foreach (string name in Constants.WindowName.Items.Values)
                    {
                        sitemaps.Add(name, new List<SitemapItem>());
                    }

                    Session[SESSION_SITEMAP] = sitemaps;
                }

                var sitemap = sitemaps[windowName];

                return sitemap;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        private class SitemapItem
        {
            public string AreaName { get; set; }

            public string ControllerName { get; set; }

            public string ActionName { get; set; }

            public object RestoreData { get; set; }
        }

        private void PrintSitemap()
        {
            var windowName = GetWindowName();
            var sitemaps = Session[SESSION_SITEMAP] as IDictionary<string, List<SitemapItem>>;

            var sitemap = sitemaps[windowName];

            var sb = new System.Text.StringBuilder();
            sb.Append(string.Format("\nSitemap stack [{0}]\n", windowName));
            foreach (var item in sitemap.ToArray().Reverse())
            {
                sb.AppendFormat("\t{0}\n", item.ControllerName);
            }
            sb.Append("---------");

            logger.Debug(sb.ToString());
        }

        /// <summary>
        /// Return the login user object
        /// </summary>
        /// <returns></returns>
        protected int GetScrollTop()
        {
            return (int)Session[Constant.SESSION_SCROLL_TOP];
        }

        /// <summary>
        /// Set the login user object
        /// </summary>
        /// <param name="user"></param>
        protected void SetScrollTop(int value)
        {
            Session[Constant.SESSION_SCROLL_TOP] = value;
        }

        /// <summary>
        /// BaseController
        /// </summary>
        public BaseController() : base()
        {
            ViewBag.BuildInfo = "";
            ViewBag.CoreCenter_ScreenID = this.GetType().Name.Replace("Controller", string.Empty);
        }

        private void SetBuildInfo(HttpRequestBase request)
        {
            var url = request.Url;
            string requestedUrl = url != null ? url.ToString() : string.Empty;

            //// Get deploy tag
            //string deployTag = "develop";
            //deployTag = requestedUrl.Contains("localhost") ? "develop" : deployTag;
            //deployTag = requestedUrl.Contains("staging") ? "staging" : deployTag;

           
            //// Get revision info
            //string commitHash = System.IO.File.ReadAllText(
            //    System.Web.HttpContext.Current.Server.MapPath("~/App_Data/." + deployTag),
            //    System.Text.Encoding.Default);
            //commitHash = commitHash.Equals(string.Empty) ? string.Empty : new string(commitHash.Take(7).ToArray());

            //// Get build date.
            //System.IO.FileInfo file = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //string html = @"<div id=""build_info"">Build Info: [{1}] {0:yyyy/MM/dd HH:mm}</div>";
            //ViewBag.BuildInfo = String.Format(html, Utility.ConvertDateTimeToJst(file.LastWriteTimeUtc), commitHash);

        }

        #region Cache

        /// <summary>
        /// Saves the cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SaveCache(string key, object value)
        {
            CacheUtil.SaveCache(key, value);
        }

        /// <summary>
        /// Gets the cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public T GetCache<T>(string key, object defaultValue = null)
        {
            return CacheUtil.GetCache<T>(key, defaultValue);
        }

        /// <summary>
        /// Removes the cache.
        /// </summary>
        /// <param name="key">The key.</param>
        public void RemoveCache(string key)
        {
            CacheUtil.RemoveCache(key);
        }

        #endregion Cache
    }
}

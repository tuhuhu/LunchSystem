using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iEnterAsia.iseiQ.Controllers.Attributes
{
    public class CommonActionAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            System.Diagnostics.Debug.Print("メソッド前:{0}",
                filterContext.ActionDescriptor.ActionName);

            var httpContext = filterContext.HttpContext;
            if (httpContext.Request.IsAjaxRequest())
            {
                SetNoCacheByUserAgent(httpContext);
            }
        }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            System.Diagnostics.Debug.Print("メソッド後:{0}",
                filterContext.ActionDescriptor.ActionName);
        }

        protected void SetNoCacheByUserAgent(HttpContextBase httpContext)
        {
            string ua = httpContext.Request.UserAgent;
            if (ua.IndexOf("AppleWebKit", StringComparison.OrdinalIgnoreCase) >= 0 ||
                ua.IndexOf("iPhone", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                // httpContext.Response.Headers.Set("Cache-Control", "no-cache");
                httpContext.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            }
        }



    }
}

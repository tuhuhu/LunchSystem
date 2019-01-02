using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SystemSetup.Constants;

namespace SystemSetup
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new HandleAntiforgeryTokenErrorAttribute() { ExceptionType = typeof(HttpAntiForgeryException) }
            );
        }
    }

    public class HandleAntiforgeryTokenErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {            
            if (filterContext.Exception.HResult == Constant.TOKEN_ERROR_CODE)
            {
                filterContext.ExceptionHandled = true;
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new { action = "Login", controller = "Login", area = "UserManagement" }));
            }            
        }
    }
}
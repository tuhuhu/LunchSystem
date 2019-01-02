using System.Web.Mvc;
using System.Web.Security;

namespace SystemSetup.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/
        public ActionResult Index()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return View("Error");
        }
    }
}

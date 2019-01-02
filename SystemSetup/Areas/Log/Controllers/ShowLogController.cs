using SystemSetup.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemSetup.Areas.Log.Controllers
{
    public class ShowLogController : BaseController
    {
        //
        // GET: /Log/ShowLog/

        public ActionResult ShowLog()
        {
            return View();
        }
        public ActionResult ShowException()
        {
            return View();
        }
    }
}

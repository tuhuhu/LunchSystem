using iEnterAsia.iseiQ.Areas.PrototypeEstimate.Models;
using iEnterAsia.iseiQ.Areas.PrototypeEstimate.ViewModels;
using iEnterAsia.iseiQ.Controllers;
using iEnterAsia.iseiQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iEnterAsia.iseiQ.Areas.PrototypeEstimate.Controllers
{
    public class RegisterController : BaseController
    {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RegisterController()
#if PROTOTYPE
            : base(new EstimateRepository())
#else
            : base(new NormalRepository())
#endif
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="repository"></param>
        public RegisterController(IRepository repository)
            : base(repository)
        {
        }


        // GET: PrototypeEstimate/Register
        public ActionResult Create()
        {
            var vm = new EstimateViewModel();
            return this.View("Create", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(EstimateViewModel model)
        {
            return this.RedirectToAction("Index");
        }

    }
}
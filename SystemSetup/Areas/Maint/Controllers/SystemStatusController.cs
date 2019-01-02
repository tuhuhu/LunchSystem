using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SystemSetup.Models;
using SystemSetup.Controllers;
using SystemSetup.BusinessServices;
using SystemSetup.UtilityServices;

namespace SystemSetup.Areas.Maint.Controllers
{
    public class SystemStatusController : BaseController
    {
        //
        // GET: /Maint/SystemStatus/

        public ActionResult Index()
        {
            return View();
        }

        //運転制御画面
        #region SystemStatusRegister

        /// <summary>
        /// Cust Maint Register
        /// </summary>
        /// <returns></returns>
        public ActionResult SystemStatusEdit(SystemStatusModel ED)
        {
            var model = CreateEditViewModel();
            return View("SystemStatusRegister", model);
        }

        /// <summary>
        /// Create edit view model(新規作成ボタン)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SystemStatusModel CreateEditViewModel()
        {
            SystemStatusModel model = null;

            using (SystemStatusServices service = new SystemStatusServices())
            {
                model = service.GetSystemStatus();

                //新規の場合
                if (model == null)
                {
                    service.InsertSystemStatus();

                    model = service.GetSystemStatus();
                }
            }
            return model;
        }

        /// <summary>
        /// Cust Maint Register
        /// </summary>
        /// <returns></returns>
        public ActionResult SystemStatusRegister(SystemStatusModel ED)
        {
            var model = CreateEditViewModel();

            //セレクトボックスの情報を取得
            //運転モード
            model.SYSTEM_OPERATION_MODE_LIST = new List<SelectListItem>()
                {
	                new SelectListItem() { Value = "0", Text = "停止" },
	                new SelectListItem() { Value = "1",   Text = "通常運転" },
                    //new SelectListItem() { Value = "2",  Text = "縮退運転" },
                };

            return View("SystemStatusRegister", model);
        }

        /// <summary>
        /// Create/Update SystemStatus(登録ボタン)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(SystemStatusEntity model)
        {
            try
            {
                using (SystemStatusServices service = new SystemStatusServices())
                {
                    if (ModelState.IsValid)
                    {
                        model.UPD_DATE = Utility.GetCurrentDateTime();
                        service.UpdateSystemStatus(model);
                        JsonResult result = Json(new { statusCode = Constants.Constant.CREATED }, JsonRequestBehavior.AllowGet);
                        return result;
                    }
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                System.Web.HttpContext.Current.Session["ERROR"] = ex;
                return new EmptyResult();
            }
        }
        #endregion
    }
}

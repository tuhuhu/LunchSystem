using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemSetup.Models;
using SystemSetup.Controllers;
using SystemSetup.BusinessServices;
using SystemSetup.UtilityServices;

namespace SystemSetup.Areas.Payment.Controllers
{
    public class PaymentController : BaseController
    {
        //
        // GET: /Payment/Payment/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PaymentList(PaymentListModel model = null)
        {
            if (model == null)
            {
                model = new PaymentListModel();
            }

            return View("PaymentList", model);
        }

        public ActionResult PaymentSearch(DataTablesModel dt, PaymentListModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    using (LunchServices service = new LunchServices())
                    {
                        int total_row;
                        var dataList = service.PaymentSearch(dt, ref model, out total_row);
                        //int order = 1;

                        var result = Json(
                        new
                        {
                            sEcho = dt.sEcho,
                            iTotalRecords = total_row,
                            iTotalDisplayRecords = total_row,
                            aaData = (from i in dataList
                                      select new object[]
                                {
                                    i.PAYMENT_CD,
                                    i.SETUP_USER_SEQ_CD,
                                    i.SETUP_USER_NAME,
                                    i.BALANCE
                                })
                        });
                        return result;
                    }
                }
            }
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult EditPaymentClear(PaymentListModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid )
                {
                    using (LunchServices service = new LunchServices())
                    {
                        if (service.UpdatePaymentClear(model) > 0)
                        {
                            JsonResult result = Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
                            return result;
                        }
                    }
                }
            }
            return new EmptyResult();
        }
    }
}

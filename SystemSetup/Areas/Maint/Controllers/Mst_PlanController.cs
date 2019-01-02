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
    public class Mst_PlanController : BaseController
    {
        //
        // GET: /Maint/Mst_Plan/

        public ActionResult Index()
        {
            return View();
        }

        #region Mst_PlanMaintList

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Mst_PlanMaintList(Mst_PlanModel model = null)
        {
            if (model == null)
            {
                model = new Mst_PlanModel();
            }
            //共通サービス初期化
            CommonService service = new CommonService();
            return View("Mst_PlanMaintList", model);
        }

        /// <summary>
        /// Cust Maint Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Mst_PlanMaintSearch(DataTablesModel dt, Mst_PlanModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    using (Mst_PlanMaintServices service = new Mst_PlanMaintServices())
                    {
                        int total_row;
                        var dataList = service.Mst_PlanMaintSearch(dt, ref model, out total_row);
                        int order = 1;

                        var result = Json(
                        new
                        {
                            sEcho = dt.sEcho,
                            iTotalRecords = total_row,
                            iTotalDisplayRecords = total_row,
                            aaData = (from i in dataList
                                      select new object[]
                                {
                                    i.PLAN_SEQ_NO,
                                    order++,
                                    i.PLAN_CD,
                                    i.PLAN_NAME  != null ? HttpUtility.HtmlEncode(i.PLAN_NAME) : String.Empty,
                                    i.UPD_DATE.HasValue ? i.UPD_DATE.Value.ToString("yyyy/MM/dd HH:mm") : String.Empty,
                                    i.UPD_USER_ID != null ? HttpUtility.HtmlEncode(i.UPD_USER_ID) : String.Empty
                                })
                        });
                        return result;
                    }
                }
            }
            return new EmptyResult();
        }

        /// <summary>
        /// DeleteBeforeCheck
        /// </summary>
        /// <param name="SEQNO"></param>
        /// <returns></returns>
        public ActionResult DeleteBeforeCheck(String PLAN_SEQ_NO)
        {
            using (Mst_PlanMaintServices service = new Mst_PlanMaintServices())
            {
                Dictionary<String, Object> obj = new Dictionary<String, Object>();
                if (service.DeleteBeforeCheck(PLAN_SEQ_NO))
                {
                    obj.Add("statusCode", true);
                }
                else
                {
                    obj.Add("statusCode", false);
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Delete estimation
        /// </summary>
        /// <param name="SEQNO"></param>
        /// <returns></returns>
        public ActionResult DELETE(String PLAN_SEQ_NO)
        {
            using (Mst_PlanMaintServices service = new Mst_PlanMaintServices())
            {
                int result = service.DeleteMst_PlanMaint(PLAN_SEQ_NO);
                if (result > 0)
                {
                    Dictionary<String, Object> obj = new Dictionary<String, Object>();
                    obj.Add("success", result);

                    if (obj.Count > 0)
                    {
                        return Json(obj, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return new EmptyResult();
                    }
                }

                return new EmptyResult();
            }
        }

        #endregion


    }
}

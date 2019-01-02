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
    public class PlanMaintController : BaseController
    {
        //
        // GET: /Maint/PlanMaint/

        public ActionResult Index()
        {
            return View();
        }

        #region PlanMaintList

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult PlanMaintList(PlanMaintModel model = null)
        {
            if (model == null)
            {
                model = new PlanMaintModel();
            }
            //共通サービス初期化
            CommonService service = new CommonService();
            return View("PlanMaintList", model);
        }

        /// <summary>
        /// Cust Maint Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult PlanMaintSearch(DataTablesModel dt, PlanMaintModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    using (PlanMaintServices service = new PlanMaintServices())
                    {
                        int total_row;
                        var dataList = service.PlanMaintSearch(dt, ref model, out total_row);
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
                                    i.PLAN_NAME != null ? HttpUtility.HtmlEncode(i.PLAN_NAME) : String.Empty,
                                    HttpUtility.HtmlEncode(i.PLAN_BASE_PRICE),
                                    HttpUtility.HtmlEncode(i.LOGIN_ACCOUNT_UPPER),
                                    HttpUtility.HtmlEncode(i.MONTHLY_BILL_DATA_UPPER),
                                    i.DISABLE_FLG == "0" ? string.Empty : i.DISABLE_FLG == "1" ? "無効" : string.Empty
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
            using (PlanMaintServices service = new PlanMaintServices())
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
            using (PlanMaintServices service = new PlanMaintServices())
            {
                int result = service.DeletePlanMaint(PLAN_SEQ_NO);
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

        //ここから下は契約プラン登録画面
        #region PlanMaintRegister

        /// <summary>
        /// Cust Maint Register
        /// </summary>
        /// <returns></returns>
        public ActionResult PlanMaintEdit(PlanMaintModel ED)
        {
            var model = CreateEditViewModel(ED.PLAN_SEQ_NO);
            return View("PlanMaintRegister", model);
        }

        /// <summary>
        /// Create edit view model(新規作成ボタン)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PlanMaintModel CreateEditViewModel(String planseqno)
        {
            CommonService comService = new CommonService();
            PlanMaintModel model = new PlanMaintModel();

            using (PlanMaintServices service = new PlanMaintServices())
            {
                //更新の場合
                if (planseqno != "0")
                {
                    model = service.GetPlanMaint(planseqno);
                }

                //新規の場合
                if (planseqno == "0")
                {
                    model.PLAN_BASE_PRICE = 0;
                    model.LOGIN_ACCOUNT_UPPER = 0;
                    model.MONTHLY_BILL_DATA_UPPER = 0;
                }
            }

            return model;
        }

        /// <summary>
        /// Create/Update Tbl_Information(登録ボタン)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(PlanMaintEntity model)
        {
            try
            {
                using (PlanMaintServices service = new PlanMaintServices())
                {
                    if (ModelState.IsValid)
                    {
                        bool isNew = false;

                        model.PLAN_BASE_PRICE = model.PLAN_BASE_PRICE.HasValue ? model.PLAN_BASE_PRICE.Value : 0;
                        model.LOGIN_ACCOUNT_UPPER = model.LOGIN_ACCOUNT_UPPER.HasValue ? model.LOGIN_ACCOUNT_UPPER.Value : 0;
                        model.MONTHLY_BILL_DATA_UPPER = model.MONTHLY_BILL_DATA_UPPER.HasValue ? model.MONTHLY_BILL_DATA_UPPER.Value : 0;

                        if (model.PLAN_SEQ_NO == "0")
                        {
                            //Check exist PLAN_CD
                            var exist_name = service.CheckExist(model);
                            if (!exist_name)
                            {
                                isNew = true;

                                model.INS_USER_ID = 0;
                                model.INS_DATE = Utility.GetCurrentDateTime();
                                model.UPD_DATE = Utility.GetCurrentDateTime();
                                service.InsertPlanMaint(model);
                                JsonResult result = Json(new
                                {
                                    statusCode = Constants.Constant.CREATED,
                                    isNew = isNew
                                },
                                 JsonRequestBehavior.AllowGet);
                                return result;
                            }
                            else
                            {
                                ModelState.AddModelError("", Constants.Resources.Messages.DuplicateName);
                                JsonResult duplicate = Json(
                                    new
                                    {
                                        statusCode = Constants.Constant.INTERNAL_SERVER_ERROR
                                    },
                                    JsonRequestBehavior.AllowGet);
                                return duplicate;
                            }
                        }
                        else
                        {

                            model.UPD_DATE = Utility.GetCurrentDateTime();
                            model.UPD_USER_ID = base.CmnEntityModel.UserSegNo;

                            service.UpdatePlanMaint(model);
                            JsonResult result = Json(new
                            {
                                statusCode = Constants.Constant.CREATED,
                                isNew = isNew
                            },
                            JsonRequestBehavior.AllowGet);
                            return result;
                        }
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

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
    public class TaxRateMaintController : BaseController
    {
        //
        // GET: /Contract/TaxRateMaint/

        public ActionResult Index()
        {
            return View();
        }

        #region TaxRateMaintList

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult TaxRateMaintList(TaxRateMaintModel model = null)
        {
            if (model == null)
            {
                model = new TaxRateMaintModel();
            }

            return View("TaxRateMaintList", model);
        }

        /// <summary>
        /// TaxRate Maint Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult TaxRateMaintSearch(DataTablesModel dt, TaxRateMaintModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    using (TaxRateMaintServices service = new TaxRateMaintServices())
                    {
                        int total_row;
                        var dataList = service.TaxRateMaintSearch(dt, ref model, out total_row);
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
                                    i.TAX_RATE_ID,
                                    order++,
                                    i.APPLY_DATE_FROM.ToString("yyyy/MM/dd"),
                                    i.APPLY_DATE_TO.ToString("yyyy/MM/dd"),
                                    i.TAX_RATE, 
                                    i.UPD_DATE.HasValue ? i.UPD_DATE.Value.ToString("yyyy/MM/dd HH:mm") : String.Empty,
                                    i.UPD_USERNAME != null ? HttpUtility.HtmlEncode(i.UPD_USERNAME) : String.Empty
                                })
                        });
                        return result;
                    }
                }
            }
            return new EmptyResult();
        }

        /// <summary>
        /// Delete TaxRate
        /// </summary>
        /// <param name="SEQNO"></param>
        /// <returns></returns>
        public ActionResult DELETE(int TAX_RATE_ID = 0)
        {
            using (TaxRateMaintServices service = new TaxRateMaintServices())
            {
                int result = service.DeleteTaxRateMaint(TAX_RATE_ID);
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

        #region TaxRateMaintRegister

        /// <summary>
        /// TaxRate Maint Register
        /// </summary>
        /// <returns></returns>
        public ActionResult TaxRateMaintEdit(TaxRateMaintModel REGMODEL = null)
        {
            TaxRateMaintModel model = CreateEditViewModel(REGMODEL);
            return View("TaxRateMaintRegister", model);

        }

        /// <summary>
        /// Create edit view model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public TaxRateMaintModel CreateEditViewModel(TaxRateMaintModel REGMODEL)
        {
            CommonService comService = new CommonService();
            TaxRateMaintModel model = new TaxRateMaintModel();

            long TaxRateID = REGMODEL.REGIST_PARAM.TAX_RATE_ID;

            using (TaxRateMaintServices service = new TaxRateMaintServices())
            {
                if (TaxRateID != 0)
                {
                    model = service.GetTaxRateMaint(TaxRateID);
                }
                else
                {

                }
            }

            return model;
        }

        /// <summary>
        /// Create/Update Tbl_Information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(TaxRateEntity model)
        {
            try
            {
                using (TaxRateMaintServices service = new TaxRateMaintServices())
                {
                    if (ModelState.IsValid)
                    {
                        var exist = service.TaxRateMaintCheckDate(model);
                        if (exist)
                        {
                            bool isNew = false;
                            if (model.TAX_RATE_ID == 0)
                            {
                                isNew = true;
                                model.INS_USER_ID = 0;//base.CmnEntityModel.UserSegNo;
                                //model.UPD_USER_ID = 0;
                                model.INS_DATE = Utility.GetCurrentDateTime();
                                model.UPD_DATE = Utility.GetCurrentDateTime();
                                service.InsertTaxRateMaint(model);
                                JsonResult result = Json(new {
                                    statusCode = true,
                                    isNew = isNew 
                                }, JsonRequestBehavior.AllowGet);
                                return result;
                            }
                            else
                            {
                                //model.UPD_USER_ID = 0;
                                model.UPD_DATE = Utility.GetCurrentDateTime();
                                service.UpdateTaxRateMaint(model);
                                JsonResult result = Json(new {
                                    statusCode = true,
                                    isNew = isNew 
                                }, JsonRequestBehavior.AllowGet);
                                return result;
                            }
                        }
                        else
                        {
                            JsonResult duplicate = Json(
                            new
                            {
                                statusCode = false
                            },
                            JsonRequestBehavior.AllowGet);

                            return duplicate;
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

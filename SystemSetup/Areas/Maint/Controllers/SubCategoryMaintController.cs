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
    public class SubCategoryMaintController : BaseController
    {
        #region SubCategoryMaintList

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SubCategoryMaintList(SubCategoryMaintModel model = null)
        {
            if (model == null)
            {
                model = new SubCategoryMaintModel();
            }

            //共通サービス初期化
            CommonService comService = new CommonService();

            SubCategoryMaintServices service = new SubCategoryMaintServices();

            //契約企業
            model.CONTRACT_FIRM_LIST = service.GetContractFirmMasterByOutside().Select(
                f => new SelectListItem
                {
                    Value = f.COMPANY_CD.ToString(),
                    Text = f.COMPANY_CD.ToString() + ": " + f.COMPANY_NAME.ToString()
                }
                ).ToList();

            //大カテゴリ
            model.CONTRACT_TYPE_LIST = comService.GetContractTypeMasterByOutside().Select(
                f => new SelectListItem
                {
                    Value = f.COMPANY_CD + "_" + f.CONTRACT_TYPE.ToString(),
                    Text = f.CONTRACT_TYPE_DISP_NAME
                }).ToList();

            //中カテゴリ
            model.CONTRACT_SUB_TYPE_LIST = comService.GetSubContractTypeMasterByOutside().Select(
                f => new SelectListItem
                {
                    Value = f.COMPANY_CD + "_" + f.CONTRACT_TYPE.ToString() + "_" + f.CONTRACT_TYPE_CLASS.ToString(),
                    Text = f.CONTRACT_TYPE_CLASS_DISP_NAME
                }).ToList();

            return View("SubCategoryMaintList", model);
        }

        /// <summary>
        /// Cust Maint Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult SubCategoryMaintSearch(DataTablesModel dt, SubCategoryMaintModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    using (SubCategoryMaintServices service = new SubCategoryMaintServices())
                    {
                        int total_row;
                        var dataList = service.SubCategoryMaintSearch(dt, ref model, out total_row);
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
                                    i.CONTRACT_TYPE,
                                    i.CONTRACT_TYPE_CLASS,
                                    order++,
                                    i.COMPANY_CD,
                                    i.CONTRACT_TYPE_DISP_NAME != null ? HttpUtility.HtmlEncode(i.CONTRACT_TYPE_DISP_NAME) : String.Empty,
                                    i.CONTRACT_TYPE_CLASS_DISP_NAME != null ? HttpUtility.HtmlEncode(i.CONTRACT_TYPE_CLASS_DISP_NAME) : String.Empty,
                                    i.DSP_PRIORITY,
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
        /// Delete estimation
        /// </summary>
        /// <param name="SEQNO"></param>
        /// <returns></returns>
        public ActionResult DELETE(string COMPANY_CD = "", int CONTRACT_TYPE = 0, int CONTRACT_TYPE_CLASS = 0)
        {
            using (SubCategoryMaintServices service = new SubCategoryMaintServices())
            {
                int result = service.DeleteSubCategoryMaint(COMPANY_CD, CONTRACT_TYPE, CONTRACT_TYPE_CLASS);
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

        #region SubCategoryMaintRegister

        /// <summary>
        /// Cust Maint Register
        /// </summary>
        /// <returns></returns>
        public ActionResult SubCategoryMaintEdit(SubCategoryMaintModel REGMODEL = null)
        {
            SubCategoryMaintModel model = CreateEditViewModel(REGMODEL);
            return View("SubCategoryMaintRegister", model);

        }

        /// <summary>
        /// Create edit view model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SubCategoryMaintModel CreateEditViewModel(SubCategoryMaintModel REGMODEL)
        {
            CommonService comService = new CommonService();
            SubCategoryMaintModel model = new SubCategoryMaintModel();

            string companyCd = REGMODEL.REGIST_PARAM.COMPANY_CD;
            int contractType = REGMODEL.REGIST_PARAM.CONTRACT_TYPE;
            int contractTypeClass = REGMODEL.REGIST_PARAM.CONTRACT_TYPE_CLASS;

            using (SubCategoryMaintServices service = new SubCategoryMaintServices())
            {
                if (contractType > 0 && contractTypeClass > 0)
                {
                    model = service.GetSubCategoryMaint(companyCd, contractType, contractTypeClass);
                }
                else
                {
                    model.COMPANY_CD = companyCd;
                    //大カテゴリ
                    model.CONTRACT_TYPE_LIST = comService.GetContractTypeMasterByCompanycode(companyCd).Select(
                        f => new SelectListItem
                        {
                            Value = f.COMPANY_CD + "_" + f.CONTRACT_TYPE.ToString(),
                            Text = f.CONTRACT_TYPE_DISP_NAME
                        }).ToList();
                }
            }

            return model;
        }

        /// <summary>
        /// GetDefaultSubCategory
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDefaultSubCategory(string COMPANY_CD, int CONTRACT_TYPE, int CONTRACT_TYPE_CLASS)
        {
            SubCategoryMaintServices service = new SubCategoryMaintServices();
            var list = service.GetSubCategory(COMPANY_CD, CONTRACT_TYPE, CONTRACT_TYPE_CLASS);

            return Json(new
            {
                IsExited = list
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Create/Update Tbl_Information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(FirmContractClassEntity model, string ACTION_TYPE)
        {
            try
            {
                using (SubCategoryMaintServices service = new SubCategoryMaintServices())
                {
                    if (ModelState.IsValid)
                    {
                        bool isNew = false;
                        if (ACTION_TYPE.Equals("0"))
                        {
                            isNew = true;
                            model.INS_USER_ID = 0;
                            model.INS_DATE = Utility.GetCurrentDateTime();
                            model.UPD_DATE = Utility.GetCurrentDateTime();
                            model.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
                            model.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
                            model.UPD_USER_ID = 0;

                            service.InsertSubCategoryMaint(model);
                            JsonResult result = Json(new { isNew = isNew }, JsonRequestBehavior.AllowGet);
                            return result;
                        }
                        else
                        {
                            model.UPD_DATE = Utility.GetCurrentDateTime();
                            model.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;

                            service.UpdateSubCategoryMaint(model);
                            JsonResult result = Json(new { isNew = isNew }, JsonRequestBehavior.AllowGet);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SystemSetup.Models;
using SystemSetup.Controllers;
using SystemSetup.BusinessServices;
using SystemSetup.UtilityServices;
using SystemSetup.Constants.Resources;
using SystemSetup.Constants;

namespace SystemSetup.Areas.Maint.Controllers
{
    public class CategoryMaintController : BaseController
    {
        #region SubCategoryMaintList

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult CategoryMaintList(CategoryMaintModel model = null)
        {
            if (model == null)
            {
                model = new CategoryMaintModel();
            }

            //共通サービス初期化
            CommonService comService = new CommonService();

            //契約企業
            model.CONTRACT_FIRM_LIST = comService.GetContractFirmMasterByOutside().Select(
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

            return View("CategoryMaintList", model);
        }

        /// <summary>
        /// Cust Maint Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult CategoryMaintSearch(DataTablesModel dt, CategoryMaintModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    using (CategoryMaintServices service = new CategoryMaintServices())
                    {
                        int total_row;
                        var dataList = service.CategoryMaintSearch(dt, ref model, out total_row);
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
                                    i.CONTRACT_LEVEL_TYPE,
                                    i.CONTRACT_TYPE,
                                    order++,
                                    i.COMPANY_CD,
                                    i.CONTRACT_TYPE_DISP_NAME != null ? HttpUtility.HtmlEncode(i.CONTRACT_TYPE_DISP_NAME) : String.Empty,
                                    i.DSP_PRIORITY,
                                    i.EST_NO_PREFIX,
                                    string.IsNullOrEmpty(i.DELIVERY_NO_PREFIX) ? "" : HttpUtility.HtmlEncode(i.DELIVERY_NO_PREFIX),
                                    string.IsNullOrEmpty(i.PLC_ORDER_NO_PREFIX) ? "" : HttpUtility.HtmlEncode(i.PLC_ORDER_NO_PREFIX),
                                    getEffectiveType(i.EST_EFFECTIVE_TYPE),
                                    i.EST_EFFECTIVE_INTERVAL,
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

        private string getEffectiveType(string type)
        {
            string result = "";
            if (type.Equals("0"))
            {
                result = "日";
            }
            if(type.Equals("1")) 
            {
                result = "月";
            }
            else if (type.Equals("2"))
            {
                result = "年";
            }

            return result;
        }

        /// <summary>
        /// Delete estimation
        /// </summary>
        /// <param name="SEQNO"></param>
        /// <returns></returns>
        public ActionResult DELETE(string COMPANY_CD = "", int CONTRACT_TYPE = 0, string CONTRACT_LEVEL_TYPE = "")
        {
            using (CategoryMaintServices service = new CategoryMaintServices())
            {
                int result = service.DeleteCategoryMaint(COMPANY_CD, CONTRACT_TYPE, CONTRACT_LEVEL_TYPE);
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
        public ActionResult CategoryMaintEdit(CategoryMaintModel REGMODEL = null)
        {
            CategoryMaintModel model = CreateEditViewModel(REGMODEL);
            return View("CategoryMaintRegister", model);

        }

        /// <summary>
        /// Create edit view model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CategoryMaintModel CreateEditViewModel(CategoryMaintModel REGMODEL)
        {
            CategoryMaintModel model = new CategoryMaintModel();

            string companyCd = REGMODEL.REGIST_PARAM.COMPANY_CD;
            int contractType = REGMODEL.REGIST_PARAM.CONTRACT_TYPE;

            using (CategoryMaintServices service = new CategoryMaintServices())
            {
                if (contractType > 0)
                {
                    model = service.GetCategoryMaint(companyCd, contractType);
                }
                else
                {
                    model.COMPANY_CD = companyCd;
                }

                model.CONTRACT_LEVEL_TYPE = service.GetContractLevelType(companyCd);
            }

            return model;
        }

        /// <summary>
        /// Create/Update Tbl_Information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(CategoryMaintModel model)
        {
            try
            {
                using (CategoryMaintServices service = new CategoryMaintServices())
                {
                    if (ModelState.IsValid)
                    {
                        bool isNew = false;

                        model.UPD_DATE = Utility.GetCurrentDateTime();
                        model.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;

                        if (model.CONTRACT_TYPE == 0)
                        {
                            isNew = true;
                            model.INS_USER_ID = 0;
                            model.INS_DATE = Utility.GetCurrentDateTime();
                            model.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
                            model.UPD_USER_ID = 0;

                            service.InsertCategoryMaint(model);
                            JsonResult result = Json(new { isNew = isNew }, JsonRequestBehavior.AllowGet);
                            return result;
                        }
                        else
                        {
                            service.UpdateCategoryMaint(model);
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

        #region CategorySetDuplicate
        [HttpGet]
        public ActionResult CategorySetDuplicate(CategorySetDuplicateModel model = null)
        {
            if (model == null)
            {
                model = new CategorySetDuplicateModel();
            }

            //共通サービス初期化
            CategorySetDuplicateService CategoryService = new CategorySetDuplicateService();

            //契約企業
            model.SOURCE_COMPANY_CODE_LIST = CategoryService.GetSourceCompanyCode().Select(
                f => new SelectListItem
                {
                    Value = f.COMPANY_CD.ToString(),
                    Text = f.COMPANY_CD.ToString() + ": " + f.COMPANY_NAME.ToString()
                }
                ).ToList();

            //大カテゴリ
            model.DESTINATION_COMPANY_CODE_LIST = CategoryService.GetDestinationCompanyCode().Select(
                f => new SelectListItem
                {
                    Value = f.COMPANY_CD.ToString(),
                    Text = f.COMPANY_CD.ToString() + ": " + f.COMPANY_NAME.ToString()
                }
                ).ToList();

            return View("CategorySetDuplicate", model);
        }

        /// <summary>
        /// Validate CategorySetDuplicate
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool ValidateCategorySetDuplicate(CategorySetDuplicateModel model)
        {
            bool isValid = true;

            CategorySetDuplicateService CategoryService = new CategorySetDuplicateService();

            //契約企業
            model.SOURCE_COMPANY_CODE_LIST = CategoryService.GetSourceCompanyCode().Select(
                f => new SelectListItem
                {
                    Value = f.COMPANY_CD.ToString(),
                    Text = f.COMPANY_CD.ToString() + ": " + f.COMPANY_NAME.ToString()
                }
                ).ToList();

            //大カテゴリ
            model.DESTINATION_COMPANY_CODE_LIST = CategoryService.GetDestinationCompanyCode().Select(
                f => new SelectListItem
                {
                    Value = f.COMPANY_CD.ToString(),
                    Text = f.COMPANY_CD.ToString() + ": " + f.COMPANY_NAME.ToString()
                }
                ).ToList();

            if (model.SOURCE_COMPANY_CODE_LIST.FirstOrDefault() == null)
            {
                ModelState.AddModelError("", SystemSetup.Constants.Resources.Maint.NotSelectedSourceCompany);
                isValid = false;
            }

            if (model.DESTINATION_COMPANY_CODE_LIST.FirstOrDefault() == null)
            {
                ModelState.AddModelError("", SystemSetup.Constants.Resources.Maint.NotSelectedDestinationCompany);
                isValid = false;
            }
           
            return isValid;
        }

        [HttpPost]
        public ActionResult CategorySettingDuplicate(CategorySetDuplicateModel model)
        {
            try
            {
                using (CategorySetDuplicateService service = new CategorySetDuplicateService())
                {
                    if (ModelState.IsValid && ValidateCategorySetDuplicate(model))
                    {
                        model.INS_DATE = Utility.GetCurrentDateTime();
                        model.UPD_DATE = Utility.GetCurrentDateTime();
                        model.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
                        model.DEL_FLG = Constants.DeleteFlag.NON_DELETE;

                        service.Delete(model.DESTINATION_COMPANY_CODE);
                        service.CopyDuplicate(model);

                        JsonResult result = Json(new { success = "Success" }, JsonRequestBehavior.AllowGet);
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

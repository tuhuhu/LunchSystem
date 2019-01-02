using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SystemSetup.Models;
using SystemSetup.Controllers;
using SystemSetup.BusinessServices;
using SystemSetup.UtilityServices;
using SystemSetup.Constants;

namespace SystemSetup.Areas.Maint.Controllers
{
    public class ProductMaintController : BaseController
    {
        #region ProductMaintList

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductMaintList(ProductMaintModel model = null)
        {
            if (model == null)
            {
                model = new ProductMaintModel();
            }
            else
            {
                model.COMPANY_CD = model.SEARCH_COMPANY_CD;
                model.CONTRACT_TYPE_EDIT = model.SEARCH_CONTRACT_TYPE_EDIT;
                model.CONTRACT_TYPE_CLASS_EDIT = model.SEARCH_CONTRACT_TYPE_CLASS_EDIT;
                model.PRODUCT_SEQ_NO_EDIT = model.SEARCH_PRODUCT_SEQ_NO_EDIT;
            }

            //共通サービス初期化
            CommonService service = new CommonService();

            //契約企業
            model.CONTRACT_FIRM_LIST = service.GetContractFirmMasterByOutside().Select(
                f => new SelectListItem
                {
                    Value = f.COMPANY_CD.ToString(),
                    Text = f.COMPANY_CD.ToString() + ": " + f.COMPANY_NAME.ToString()
                }
                ).ToList();

            //大カテゴリ
            model.CONTRACT_TYPE_LIST = service.GetContractTypeMasterByOutside().Select(
                f => new SelectListItem
                {
                    Value = f.COMPANY_CD + "_" + f.CONTRACT_TYPE.ToString(),
                    Text = f.CONTRACT_TYPE_DISP_NAME
                }).ToList();

            //中カテゴリ
            model.CONTRACT_SUB_TYPE_LIST = service.GetSubContractTypeMasterByOutside().Select(
                f => new SelectListItem
                {
                    Value = f.COMPANY_CD + "_" + f.CONTRACT_TYPE.ToString() + "_" + f.CONTRACT_TYPE_CLASS.ToString(),
                    Text = f.CONTRACT_TYPE_CLASS_DISP_NAME
                }).ToList();

            //小カテゴリ
            model.PRODUCT_LIST = service.GetProductMasterByOutside().Select(
                f => new SelectListItem
                {
                    Value = f.PRODUCT_SEQ_NO.ToString(),
                    Text = f.PRODUCT_NAME
                }).ToList();

            return View("ProductMaintList", model);
        }

        /// <summary>
        /// Cust Maint Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ProductMaintSearch(DataTablesModel dt, ProductMaintModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    using (ProductMaintServices service = new ProductMaintServices())
                    {
                        int total_row;
                        var dataList = service.ProductMaintSearch(dt, ref model, out total_row);
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
                                    i.PRODUCT_SEQ_NO,
                                    order++,
                                    i.COMPANY_CD,
                                    i.CONTRACT_TYPE != null ? HttpUtility.HtmlEncode(i.CONTRACT_TYPE_DISP_NAME) : String.Empty,
                                    i.CONTRACT_TYPE_CLASS != null ? HttpUtility.HtmlEncode(i.CONTRACT_TYPE_CLASS_DISP_NAME) : String.Empty,
                                    i.PRODUCT_NAME != null ? HttpUtility.HtmlEncode(i.PRODUCT_NAME) : String.Empty,
                                    i.DSP_PRIORITY,
                                    i.UPD_DATE != DateTime.MinValue ? i.UPD_DATE.ToString("yyyy/MM/dd HH:mm") : String.Empty,
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
        public ActionResult DELETE(int PRODUCT_SEQ_NO = 0)
        {
            using (ProductMaintServices service = new ProductMaintServices())
            {
                int result = service.DeleteProductMaint(PRODUCT_SEQ_NO);
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

        #region ProductMaintRegister

        /// <summary>
        /// Cust Maint Register
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductMaintEdit(ProductMaintModel REGMODEL = null)
        {
            ProductMaintModel model = CreateEditViewModel(REGMODEL);
            return View("ProductMaintRegister", model);
        }

        private string GetContractInputTypeName(string contractInputType)
        {
            string name = "";

            switch (contractInputType)
            {
                case ContractInputType.OUTSOURCING:
                    name = "外注";
                    break;
                case ContractInputType.STAFF:
                    name = "社員";
                    break;
                case ContractInputType.CONTRACT:
                    name = "契約";
                    break;
                case ContractInputType.GENERAL_DISPATCH:
                    name = "派遣";
                    break;
                case ContractInputType.SPECIFIC_DISPATCH:
                    name = "特定派遣";
                    break;
                case ContractInputType.TRUSTEE:
                    name = "受託";
                    break;
                case ContractInputType.PRODUCT_SALE:
                    name = "物販";
                    break;
                case ContractInputType.DELEGATION:
                    name = "委任";
                    break;
                case ContractInputType.MAINTAINANCE:
                    name = "保守";
                    break;
                case ContractInputType.PRODUCT_SALE_MONTH:
                    name = "月額";
                    break;
                default:
                    break;
            }

            return name;
        }

        /// <summary>
        /// Create edit view model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ProductMaintModel CreateEditViewModel(ProductMaintModel REGMODEL)
        {
            CommonService comService = new CommonService();
            ProductMaintModel model = new ProductMaintModel();

            long productSeqNo = REGMODEL.REGIST_PARAM.PRODUCT_SEQ_NO;
            string companyCd = REGMODEL.REGIST_PARAM.COMPANY_CD;
            string contractTypeEdit = REGMODEL.SEARCH_CONTRACT_TYPE_EDIT;
            string contractTypeClassEdit = REGMODEL.SEARCH_CONTRACT_TYPE_CLASS_EDIT;
            int productNo = REGMODEL.SEARCH_PRODUCT_SEQ_NO_EDIT != null ? Convert.ToInt32(REGMODEL.SEARCH_PRODUCT_SEQ_NO_EDIT) : 0;

            using (ProductMaintServices service = new ProductMaintServices())
            {
                if (productSeqNo > 0)
                {
                    model = service.GetProductMaint(productSeqNo);
                    model.CONTRACT_INPUT_TYPE_NAME = GetContractInputTypeName(model.CONTRACT_INPUT_TYPE);
                }
                else
                {
                    model.COMPANY_CD = companyCd;
                    model.PJT_NAME_INIT_VALUE = "システム開発支援";
                    model.WORKING_CONTENT_INIT_VALUE = "システム開発支援作業";
                    model.WORKING_LOCATION_INIT_VALUE = "貴社ご指定場所";
                    model.DELIVERABLES_INIT_VALUE = "作業実績結果報告";
                    model.DELIVERY_LOCATION_INIT_VALUE = "貴社ご指定場所";
                   
                    if (contractTypeEdit != null)
                    {
                        model.CONTRACT_TYPE_EDIT = contractTypeEdit;
                    }
                    if (contractTypeClassEdit != null)
                    {
                        model.CONTRACT_TYPE_CLASS_EDIT = contractTypeClassEdit;
                    }
                    //大カテゴリ
                    model.CONTRACT_TYPE_LIST = comService.GetContractTypeMasterByCompanycode(companyCd).Select(
                        f => new SelectListItem
                        {
                            Value = f.COMPANY_CD + "_" + f.CONTRACT_TYPE.ToString(),
                            Text = f.CONTRACT_TYPE_DISP_NAME
                        }).ToList();
                   
                    int contractType = model.CONTRACT_TYPE_EDIT != null ? Convert.ToInt32(model.CONTRACT_TYPE_EDIT.Split('_')[1]) : 0;
                    int contractClaseType = model.CONTRACT_TYPE_CLASS_EDIT != null ? Convert.ToInt32(model.CONTRACT_TYPE_CLASS_EDIT.Split('_')[2]) : 0;            
                    //中カテゴリ
                    model.CONTRACT_SUB_TYPE_LIST = comService.GetSubContractTypeMasterByOutsideCompanycode(companyCd, contractType).Select(
                        f => new SelectListItem
                        {
                            Value = f.COMPANY_CD + "_" + f.CONTRACT_TYPE.ToString() + "_" + f.CONTRACT_TYPE_CLASS.ToString(),
                            Text = f.CONTRACT_TYPE_CLASS_DISP_NAME
                        }).ToList();
                    if (productNo > 0)
                    {
                        model.PRODUCT_NAME = comService.SearchProductName(contractType, contractClaseType, productNo);
                    }                  
                  
                }
            }
            return model;
        }

        /// <summary>
        /// GetDefaultProduct
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDefaultProduct(string COMPANY_CD, int CONTRACT_TYPE, int CONTRACT_TYPE_CLASS, string PRODUCT_NAME)
        {
            ProductMaintServices service = new ProductMaintServices();
            var list = service.GetProduct(COMPANY_CD, CONTRACT_TYPE, CONTRACT_TYPE_CLASS, PRODUCT_NAME);

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
        public ActionResult Edit(ProductEntity model)
        {
            try
            {
                using (ProductMaintServices service = new ProductMaintServices())
                {
                    if (ModelState.IsValid)
                    {
                        bool isNew = false;
                        List<int> errNo = new List<int>();
                        //CheckEnvironmentDependentCharacters
                        for (int i = 0; i < 3; i++)
                        {
                            string CHECK_PATH = null;
                            if (i == 0) { CHECK_PATH = model.EST_FORMAT_PATH; };
                            if (i == 1) { CHECK_PATH = model.ORD_FORMAT_PATH; };
                            if (i == 2) { CHECK_PATH = model.INV_FORMAT_PATH; };
                            if (!CheckEnvironmentDependentCharacters(CHECK_PATH))
                            {
                                errNo.Add(i);
                            }
                        }
                        if (errNo.Count() == 0)
                        {
                            if (model.PRODUCT_SEQ_NO == 0)
                            {
                                isNew = true;
                                model.INS_DATE = Utility.GetCurrentDateTime();
                                model.UPD_DATE = Utility.GetCurrentDateTime();
                                service.InsertProductMaint(model);
                                JsonResult result = Json(
                                    new 
                                    {
                                        statusCode = Constants.Constant.CREATED,
                                        isNew = isNew 
                                    },
                                    JsonRequestBehavior.AllowGet);
                                return result;
                            }
                            else
                            {
                                model.UPD_DATE = Utility.GetCurrentDateTime();
                                service.UpdateProductMaint(model);
                                JsonResult result = Json(
                                    new
                                    {
                                        statusCode = Constants.Constant.CREATED,
                                        isNew = isNew 
                                    }, 
                                    JsonRequestBehavior.AllowGet);
                                return result;
                            }
                        }
                        else
                        {
                            JsonResult result = Json(
                                new
                                {
                                    statusCode = Constants.Constant.INTERNAL_SERVER_ERROR,
                                    errNo = errNo
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

        /// <summary>
        /// Check ENVIRONMENT_DEPENDENT_CHARACTERS
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CheckEnvironmentDependentCharacters(string CHECK_CHAR)
        {

            for (int i = 0; i < CHECK_CHAR.Length; i++)
            {
                if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS1.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS2.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS3.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS4.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS5.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS6.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS7.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS8.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS9.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS10.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS11.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS12.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS13.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS14.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS15.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS16.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS17.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS18.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS19.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
            };
            return true;
        }

        public ActionResult GetSubContractTypeList(string companyCode, int contractTypeId)
        {
            using (CommonService service = new CommonService())
            {
                var subcontractType = service.GetSubContractTypeMasterByOutsideCompanycode(companyCode, contractTypeId);
                var result = (from s in subcontractType
                              select new
                              {
                                  key = s.COMPANY_CD + "_" + s.CONTRACT_TYPE.ToString() + "_" + s.CONTRACT_TYPE_CLASS.ToString(),                                 
                                  value = s.CONTRACT_TYPE_CLASS_DISP_NAME
                              }).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}

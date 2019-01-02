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
    public class AccountingSubjectGroupMaintController : BaseController
    {
        // GET: /Contract/AccountingSubjectGroupMaint/

        public ActionResult Index()
        {
            return View();
        }

        #region AccountingSubjectGroupMaintList

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountingSubjectGroupMaintList(AccountingSubjectGroupMaintModel model = null)
        {
            if (model == null)
            {
                model = new AccountingSubjectGroupMaintModel();
            }
            //共通サービス初期化
            CommonService service = new CommonService();
            return View("AccountingSubjectGroupMaintList" , model);
        }

        /// <summary>
        /// Cust Maint Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AccountingSubjectGroupMaintSearch(DataTablesModel dt, AccountingSubjectGroupMaintModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    using (AccountingSubjectGroupMaintServices service = new AccountingSubjectGroupMaintServices())
                    {
                        int total_row;
                        var dataList = service.AccountingSubjectGroupMaintSearch(dt, ref model, out total_row);
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
                                    i.ACCOUNT_GROUP_SEQ_NO,
                                    order++,
                                    i.ACCOUNT_GROUP_CD,
                                    i.ACCOUNT_GROUP_NAME  != null ? HttpUtility.HtmlEncode(i.ACCOUNT_GROUP_NAME) : String.Empty,
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
        /// DeleteBeforeCheck
        /// </summary>
        /// <param name="SEQNO"></param>
        /// <returns></returns>
        public ActionResult DeleteBeforeCheck(String ACCOUNT_GROUP_SEQ_NO)
        {
            using (AccountingSubjectGroupMaintServices service = new AccountingSubjectGroupMaintServices())
            {
                Dictionary<String, Object> obj = new Dictionary<String, Object>();
                if (service.DeleteBeforeCheck(ACCOUNT_GROUP_SEQ_NO))
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
        public ActionResult DELETE(String ACCOUNT_GROUP_SEQ_NO)
        {
            using (AccountingSubjectGroupMaintServices service = new AccountingSubjectGroupMaintServices())
            {
                int result = service.DeleteAccountingSubjectGroupMaint(ACCOUNT_GROUP_SEQ_NO);
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

        #region AccountingSubjectGroupMaintRegister

        /// <summary>
        /// Cust Maint Register
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountingSubjectGroupMaintEdit(AccountingSubjectGroupMaintModel ED )
        {
            var model = CreateEditViewModel(ED.ACCOUNT_GROUP_SEQ_NO);
            return View("AccountingSubjectGroupMaintRegister", model);
        }

        /// <summary>
        /// Create edit view model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public AccountingSubjectGroupMaintModel CreateEditViewModel(String contractuserseqno)
        {
            CommonService comService = new CommonService();
            AccountingSubjectGroupMaintModel model = new AccountingSubjectGroupMaintModel();

            using (AccountingSubjectGroupMaintServices service = new AccountingSubjectGroupMaintServices())
            {
                if (contractuserseqno != "0")
                {
                    model = service.GetAccountingSubjectGroupMaint(contractuserseqno);
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
        public ActionResult Edit(AccountGroupEntity model)
        {
            try
            {
                using (AccountingSubjectGroupMaintServices service = new AccountingSubjectGroupMaintServices())
                {
                    if (ModelState.IsValid)
                    {
                        bool isNew = false;
                        if (model.ACCOUNT_GROUP_SEQ_NO == "0")
                        {
                            //Check exist ACCOUNT_GROUP_CD
                            var exist_name = service.CheckExist(model);
                            if (!exist_name)
                            {
                                isNew = true;
                                model.INS_USER_ID = 0;
                                model.INS_DATE = Utility.GetCurrentDateTime();
                                model.UPD_DATE = Utility.GetCurrentDateTime();
                                service.InsertAccountingSubjectGroupMaint(model);
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
                            service.UpdateAccountingSubjectGroupMaint(model);
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

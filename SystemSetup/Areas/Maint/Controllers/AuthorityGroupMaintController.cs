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
    public class AuthorityGroupMaintController : BaseController
    {
        #region AuthorityGroupMaintList

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AuthorityGroupMaintList(AuthorityGroupMaintModel model = null)
        {
            if (model == null)
            {
                model = new AuthorityGroupMaintModel();
            }

            //共通サービス初期化
            CommonService comService = new CommonService();

            AuthorityGroupMaintServices service = new AuthorityGroupMaintServices();

            //契約企業
            model.CONTRACT_FIRM_LIST = comService.GetContractFirmMasterByOutside().Select(
                f => new SelectListItem
                {
                    Value = f.COMPANY_CD.ToString(),
                    Text = f.COMPANY_CD.ToString() + ": " + f.COMPANY_NAME.ToString()
                }
                ).ToList();

            return View("AuthorityGroupMaintList", model);
        }

        /// <summary>
        /// AuthorityGroup Maint Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AuthorityGroupMaintSearch(DataTablesModel dt, AuthorityGroupMaintModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    using (AuthorityGroupMaintServices service = new AuthorityGroupMaintServices())
                    {
                        int total_row;
                        var dataList = service.AuthorityGroupMaintSearch(dt, ref model, out total_row);
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
                                    order++,
                                    i.COMPANY_CD,
                                    i.AUTHORITY_GROUP_CD,
                                    i.AUTHORITY_GROUP_NAME != null ? HttpUtility.HtmlEncode(i.AUTHORITY_GROUP_NAME) : String.Empty,
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
        public ActionResult DeleteBeforeCheck(string COMPANY_CD = "", int AUTHORITY_GROUP_CD = -1)
        {
            using (AuthorityGroupMaintServices service = new AuthorityGroupMaintServices())
            {
                Dictionary<String, Object> obj = new Dictionary<String, Object>();
                if (service.DeleteBeforeCheck(COMPANY_CD, AUTHORITY_GROUP_CD))
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
        /// Delete AuthorityGroup and Authority
        /// </summary>
        /// <param name="SEQNO"></param>
        /// <returns></returns>
        public ActionResult DELETE(string COMPANY_CD = "", int AUTHORITY_GROUP_CD = -1)
        {
            using (AuthorityGroupMaintServices service = new AuthorityGroupMaintServices())
            {
                int result = service.DeleteAuthorityGroupMaint(COMPANY_CD, AUTHORITY_GROUP_CD);
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

        #region AuthorityGroupMaintRegister

        /// <summary>
        /// AuthorityGroup Maint Register
        /// </summary>
        /// <returns></returns>
        public ActionResult AuthorityGroupMaintEdit(AuthorityGroupMaintModel REGMODEL = null)
        {
            AuthorityGroupMaintModel model = CreateEditViewModel(REGMODEL);
            return View("AuthorityGroupMaintRegister", model);

        }

        /// <summary>
        /// Create edit view model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public AuthorityGroupMaintModel CreateEditViewModel(AuthorityGroupMaintModel REGMODEL)
        {
            CommonService comService = new CommonService();
            AuthorityGroupMaintModel model = new AuthorityGroupMaintModel();

            string ActionType = REGMODEL.ACTION_TYPE;
            string companyCd = REGMODEL.REGIST_PARAM.COMPANY_CD;

            using (AuthorityGroupMaintServices service = new AuthorityGroupMaintServices())
            {
                if (ActionType == "1" && companyCd != null)
                {
                    int AuthorityGroupCd = (int)REGMODEL.REGIST_PARAM.AUTHORITY_GROUP_CD;
                    model = service.GetAuthorityGroupMaint(companyCd, AuthorityGroupCd);
                    model.VIEW_DISABLE_FLG = model.DISABLE_FLG == "1" ? true : false;
                    model.AUTHORITY_GROUP_CD = AuthorityGroupCd;
                }
            }
            model.ACTION_TYPE = ActionType;
            model.COMPANY_CD = companyCd;

            return model;
        }

        /// <summary>
        /// Create/Update Tbl_Information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(AuthorityGroupMaintModel model)
        {
            try
            {
                using (AuthorityGroupMaintServices service = new AuthorityGroupMaintServices())
                {
                    if (ModelState.IsValid)
                    {
                        bool isNew = false;
                        if (model.ACTION_TYPE.Equals("0"))
                        {
                            isNew = true;

                            service.InsertAuthorityGroupMaint(model);
                            JsonResult result = Json(new { isNew = isNew }, JsonRequestBehavior.AllowGet);
                            return result;
                        }
                        else
                        {
                            service.UpdateAuthorityGroupMaint(model);
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

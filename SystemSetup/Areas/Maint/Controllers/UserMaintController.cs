namespace SystemSetup.Areas.Maint.Controllers
{
    using SystemSetup.BusinessServices;
    using SystemSetup.Controllers;
    using SystemSetup.Models;
    using SystemSetup.UtilityServices;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;
    using SystemSetup.Constants;

    public class UserMaintController : BaseController
    {
        #region TOP Maint
 
        /// <summary>
        /// Convert url to link
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string ConvertUrlsToLinks(string url)
        {
            string regex = @"((www\.|(http|https)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
            Regex r = new Regex(regex, RegexOptions.IgnoreCase);
            return r.Replace(url, "<a href=\"$1\" title=\"$1\" target=\"&#95;blank\">$1</a>").Replace("href=\"www", "href=\"http://www");
        }
        #endregion

        #region User Maint

        /// <summary>
        /// User Maint List
        /// </summary>
        /// <returns></returns>
        public ActionResult UserMaintList(UserMaintModel REGMODEL)
        {
            UserMaintListModel model = new UserMaintListModel();
            UserMaintServices service = new UserMaintServices();
            List<SelectListItem> items = new List<SelectListItem>();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            model.DISPLAY_START = REGMODEL.DISPLAY_START;
            model.DISPLAY_LENGTH = REGMODEL.DISPLAY_LENGTH;

            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                items.Add(new SelectListItem() { Text = pair.Key, Value = pair.Value, Selected = false });
            }

            ViewBag.DurationType = new SelectList(items, "Value", "Text");

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

            //Get list of Contract Type
            model.AUTHORITY_GROUP_LIST = service.GetGroupMaster("").Select(
                f => new SelectListItem
                {
                    Value = f.COMPANY_CD + "_" + f.AUTHORITY_GROUP_CD.ToString(),
                    Text = f.AUTHORITY_GROUP_NAME
                }).ToList();


            return View("UserMaintList", model);
        }

        /// <summary>
        /// User Maint Register
        /// </summary>
        /// <returns></returns>
        public ActionResult UserMaintEdit(UserMaintListModel REGMODEL = null)
        {
            var model = CreateEditViewModel(REGMODEL.SEARCH_PARAM.CONTRACT_USER_SEQ_CD, REGMODEL.SEARCH_PARAM.COMPANY_CD);
            model.COMPANY_CD = REGMODEL.SEARCH_PARAM.COMPANY_CD;
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            List<SelectListItem> items = new List<SelectListItem>();

            dictionary.Add(Constants.Resources.Maint.All_company, Authority_Type.All_company);
            dictionary.Add(Constants.Resources.Maint.Organization, Authority_Type.Organization);
            dictionary.Add(Constants.Resources.Maint.Person, Authority_Type.Person);

            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                items.Add(new SelectListItem() { Text = pair.Key, Value = pair.Value, Selected = false });
            }

            ViewBag.AuthorityType = new SelectList(items, "Value", "Text");
            ViewBag.SalesAnalysisAuthorityType = new SelectList(items, "Value", "Text");

            return View("UserMaintRegister", model);

        }

        /// <summary>
        /// Create edit view model
        /// </summary>
        /// <param name="infoSeqNo"></param>
        /// <returns></returns>
        public UserMaintModel CreateEditViewModel(long contractuserseqno, string companycd)
        {
            CommonService comService = new CommonService();
            UserMaintModel model = new UserMaintModel();
            using (UserMaintServices service = new UserMaintServices())
            {
                if (contractuserseqno > 0)
                {
                    model = service.GetUserMaint(contractuserseqno);
                    model.INS_USERNAME = model.INS_USER_ID != 0 ? comService.GetContractUserName(model.INS_USER_ID) : String.Empty;
                    model.UPD_USERNAME = model.UPD_USER_ID != 0 ? comService.GetContractUserName(model.UPD_USER_ID) : String.Empty;
                    model.CONTRACT_USER_PASSWORD_REPEAT = model.CONTRACT_USER_PASSWORD;
                    
                }
                else
                {
                    model.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
                }

                //Get list of Contract Type
                model.AUTHORITY_GROUP_LIST = service.GetGroupMaster(companycd).Select(
                    f => new SelectListItem
                    {
                        Value = f.AUTHORITY_GROUP_CD.ToString(),
                        Text = f.AUTHORITY_GROUP_NAME
                    }).ToList();
            }

            return model;
        }

        /// <summary>
        /// Get data for select list
        /// </summary>
        /// <param name="companycd"></param>
        /// <returns></returns>
        public ActionResult GetAuthorityGroupCdSelectList(string companycd)
        {
            using (UserMaintServices service = new UserMaintServices())
            {
                var contractType = service.GetGroupMaster(companycd);
                var result = (from s in contractType
                              select new
                              {
                                  key = s.AUTHORITY_GROUP_CD.ToString(),
                                  value = s.AUTHORITY_GROUP_NAME
                              }).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        } 

        /// <summary>
        /// User Maint Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult UserMaintSearch(DataTablesModel dt, UserMaintListModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    using (UserMaintServices service = new UserMaintServices())
                    {
                        int total_row;
                        var dataList = service.UserMaintSearch(dt, ref model, out total_row);
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
                                    i.CONTRACT_USER_SEQ_CD,
                                    order++,
                                    i.COMPANY_CD,
                                    i.CONTRACT_USER_ACCOUNT != null ? HttpUtility.HtmlEncode(i.CONTRACT_USER_ACCOUNT) : String.Empty,
                                    i.CONTRACT_USER_NAME != null ? HttpUtility.HtmlEncode(i.CONTRACT_USER_NAME) : String.Empty,
                                    i.STAFF_ID != null ? HttpUtility.HtmlEncode(i.STAFF_ID) : String.Empty,
                                    i.AUTHORITY_GROUP_NAME,
                                    i.PASSWORD_LAST_UPDATE_DATE.HasValue ? i.PASSWORD_LAST_UPDATE_DATE.Value.ToString("yyyy/MM/dd HH:mm") : String.Empty,
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
        /// Create/Update Tbl_Information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(UserMaintEntity model , string ACTION_TYPE)
        {
            try
            {
                using (UserMaintServices service = new UserMaintServices())
                {
                    if (ModelState.IsValid)
                    {
                        bool isNew = false;
                        if (model.CONTRACT_USER_SEQ_CD == 0)
                        {
                            isNew = true;
                            model.INS_USER_ID = 0;
                            model.INS_DATE = Utility.GetCurrentDateTime();
                            model.UPD_DATE = Utility.GetCurrentDateTime();
                            service.InsertUserMaint(model);
                            JsonResult result = Json(new { isNew = isNew }, JsonRequestBehavior.AllowGet);
                            return result;
                        }
                        else
                        {
                            model.UPD_DATE = Utility.GetCurrentDateTime();

                            if(ACTION_TYPE == "0")
                            {
                                service.UpdateUserMaint(model);
                            }
                            else
                            {
                                service.UpdatePassword(model);
                            }
                                                      
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

        /// <summary>
        /// Delete estimation
        /// </summary>
        /// <param name="SEQNO"></param>
        /// <returns></returns>
        public ActionResult DELETE(long SEQNO = 0)
        {
            using (UserMaintServices service = new UserMaintServices())
            {
                int result = service.DeleteUserMaint(SEQNO);
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

        /// <summary>
        /// GetDefaultAccount
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDefaultAccount(string CompanyCd, string SeqNo, string Account, string Del)
        {
            //string companyCd = base.CmnEntityModel.CompanyCd;
            UserMaintServices UserServices = new UserMaintServices();
            var list = UserServices.GetAuxiliaryCode(CompanyCd, SeqNo, Account, Del);

            return Json(new
            {
                IsExited = list
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }

}

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

    public class SetUpUserMaintController : BaseController
    {


        #region Top Maint
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

        /// <summary>
        /// 設定ユーザリスト
        /// </summary>
        /// <param name="REGMODEL"></param>
        /// <returns></returns>
        public ActionResult SetUpUserMaintList(SetUpUserMaintModel REGMODEL)
        {
            SetUpUserMaintListModel model = new SetUpUserMaintListModel();
            SetUpUserMaintServices service = new SetUpUserMaintServices();
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

            return View("SetUpUserMaintList", model);

        }

        /// <summary>
        /// ユーザ　検索
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult SetUpUserMaintSearch(DataTablesModel dt, SetUpUserMaintListModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    using (SetUpUserMaintServices service = new SetUpUserMaintServices())
                    {
                        int total_row;
                        var dataList = service.SetUpUserMaintSearch(dt, ref model, out total_row);
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
                                    i.SETUP_USER_SEQ_CD,
                                    order++,
                                    i.SETUP_USER_ACCOUNT != null ? HttpUtility.HtmlEncode(i.SETUP_USER_ACCOUNT) : String.Empty,
                                    i.SETUP_USER_NAME != null ? HttpUtility.HtmlEncode(i.SETUP_USER_NAME) : String.Empty,
                                    i.PASSWORD_LAST_UPDATE_DATE.HasValue ? i.PASSWORD_LAST_UPDATE_DATE.Value.ToString("yyyy/MM/dd") : String.Empty,
                                    i.LOGIN_LOCK_FLG == "0" ? "" : "制限",
                                    i.DISABLE_FLG == "0" ? "" : "無効",
                                })
                        });
                        return result;
                    }
                }
            }
            return new EmptyResult();

        }

        /// <summary>
        /// アカウント取得
        /// </summary>
        /// <param name="SeqNo"></param>
        /// <param name="Account"></param>
        /// <param name="Del"></param>
        /// <returns></returns>
        public ActionResult GetDefaultAccount(String SeqNo, string Account, string Del)
        {
            string companyCd = base.CmnEntityModel.CompanyCd;
            SetUpUserMaintServices SetUpUserServices = new SetUpUserMaintServices();
            var list = SetUpUserServices.GetAuxiliaryCode(companyCd, SeqNo, Account, Del);

            return Json(new
            {
                IsExited = list
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SetupUserMaint(SetUpUserMaintModel REGMODEL)
        {
            SetUpUserMaintListModel model = new SetUpUserMaintListModel();
            SetUpUserMaintServices service = new SetUpUserMaintServices();
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

            return View("SetUpUserMaintList", model);
        }

        public ActionResult SetUpUserMaintEdit(SetUpUserMaintListModel REGMODEL = null)
        {
            var model = CreateEditViewModel(REGMODEL.SEARCH_PARAM.SETUP_USER_SEQ_CD);
            return View("SetUpUserMaintRegistrer", model);
        }

        public SetUpUserMaintModel CreateEditViewModel(long contractuserseqno)
        {
            CommonService comService = new CommonService();
            SetUpUserMaintModel model = new SetUpUserMaintModel();
            using (SetUpUserMaintServices service = new SetUpUserMaintServices())
            {
                if (contractuserseqno > 0)
                {
                    model = service.GetSetUpUserMaint(contractuserseqno);
                    model.INS_USERNAME = model.INS_USER_ID != 0 ? comService.GetContractUserName(model.INS_USER_ID) : String.Empty;
                    model.UPD_USERNAME = model.UPD_USER_ID != 0 ? comService.GetContractUserName(model.UPD_USER_ID) : String.Empty;
                    model.SETUP_USER_PASSWORD_REPEAT = model.SETUP_USER_PASSWORD;
                }
                else
                {
                    model.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
                }

            }
            return model;
        }

        [HttpPost]
        public ActionResult Edit(SetUpUserMaintEntity model, string ACTION_TYPE)
        {
            try
            {
                using (SetUpUserMaintServices service = new SetUpUserMaintServices())
                {
                    if (ModelState.IsValid)
                    {
                        bool isNew = false;
                        if (model.SETUP_USER_SEQ_CD == 0)
                        {
                            isNew = true;
                            model.INS_USER_ID = 0;
                            model.INS_DATE = Utility.GetCurrentDateTime();
                            model.UPD_DATE = Utility.GetCurrentDateTime();
                            service.InsertSetUpUserMaint(model);
                            JsonResult result = Json(new { isNew = isNew }, JsonRequestBehavior.AllowGet);
                            return result;
                        }
                        else
                        {
                            model.UPD_DATE = Utility.GetCurrentDateTime();

                            if (ACTION_TYPE == "0")
                            {
                                service.UpdateSetUpUserMaint(model);
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

        public ActionResult DELETE(long SEQNO = 0)
        {
            using (SetUpUserMaintServices service = new SetUpUserMaintServices())
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

    }
}

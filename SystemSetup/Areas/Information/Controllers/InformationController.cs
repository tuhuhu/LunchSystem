namespace SystemSetup.Areas.Information.Controllers
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
    using SystemSetup.Constants.Resources;
    using SystemSetup.Constants;

    public class InformationController : BaseController
    {
        #region TOP Information
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View("Index");
        }

        /// <summary>
        /// Get data information
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadData()
        {
            string companyCd = base.CmnEntityModel.CompanyCd;
            InformationServices inforServices = new InformationServices();
            var list = inforServices.GetInformation(companyCd);
            var infoList = new List<dynamic>();

            foreach (var data in list)
            {
                infoList.Add(new
                {
                    PUBLISH_DATE_START = data.PUBLISH_DATE_START.Value.ToString("yyyy/MM/dd"),
                    CONTENT = ConvertUrlsToLinks(HttpUtility.HtmlEncode(data.CONTENT))
                });
            }

            JsonResult result = Json(
                infoList,
                JsonRequestBehavior.AllowGet);
            return result;
        }

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

        #region Post Information
        /// <summary>
        /// Post Information List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PostInformationList()
        {
            PostInformationListModel model = new PostInformationListModel();

            var tmpCondition = GetRestoreData() as PostInformationListModel;
            if (tmpCondition != null)
            {
                model = tmpCondition;
            }

            return View("PostInformationList", model);
        }

        /// <summary>
        /// Post Information Register
        /// </summary>
        /// <returns></returns>
        public ActionResult PostInformationEdit(long INFO_SEQ_NO = 0)
        {
            InformationEntityPlus model = CreateEditViewModel(INFO_SEQ_NO);
            return View("PostInformationRegister", model);
        }

        /// <summary>
        /// Create edit view model
        /// </summary>
        /// <param name="infoSeqNo"></param>
        /// <returns></returns>
        public InformationEntityPlus CreateEditViewModel(long infoSeqNo)
        {
            CommonService comService = new CommonService();
            InformationEntityPlus model = new InformationEntityPlus();
            using (InformationServices service = new InformationServices())
            {
                if (infoSeqNo > 0)
                {
                    model = service.GetInformation(infoSeqNo);
                    model.INS_USERNAME = model.INS_USER_ID != 0 ? comService.GetContractUserName(model.INS_USER_ID) : String.Empty;
                    model.UPD_USERNAME = model.UPD_USER_ID != 0 ? comService.GetContractUserName(model.UPD_USER_ID) : String.Empty;
                }
                else
                {
                    model.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
                }
            }
            return model;
        }

        /// <summary>
        /// Post Information Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostInformationList(DataTablesModel dt, PostInformationListModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid && ValidatePostInformationList(model))
                {
                    using (InformationServices service = new InformationServices())
                    {
                        int total_row;
                        var dataList = service.PostInformationSearch(dt, ref model, out total_row);
                        int order = 1;

                        this.SaveRestoreData(model);

                        var result = Json(
                        new
                        {
                            sEcho = dt.sEcho,
                            iTotalRecords = total_row,
                            iTotalDisplayRecords = total_row,
                            aaData = (from i in dataList
                                      select new object[]
                                {
                                    i.INFO_SEQ_NO,
                                    order++,
                                    i.CONTENT != null ? HttpUtility.HtmlEncode(i.CONTENT) : String.Empty,
                                    i.PUBLISH_DATE_START.HasValue ? i.PUBLISH_DATE_START.Value.ToString("yyyy/MM/dd") : String.Empty,
                                    i.PUBLISH_DATE_END.HasValue ? i.PUBLISH_DATE_END.Value.ToString("yyyy/MM/dd") : String.Empty,
                                    i.DSP_PRIORITY,
                                    i.UPD_DATE.HasValue ? i.UPD_DATE.Value.ToString("yyyy/MM/dd HH:mm") : String.Empty,
                                    i.UPD_USERNAME ?? String.Empty
                                })
                        });
                        return result;
                    }
                }
            }
            return new EmptyResult();
        }

        /// <summary>
        /// Validate data for PostInformationList screen
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ValidatePostInformationList(PostInformationListModel model)
        {
            bool isOK = true;

            if (model.CONTENT.Length > 4000)
            {
                ModelState.AddModelError(String.Empty, string.Format(Messages.MaxLength, Information.CONTENT, Constant.NVARCHAR_MAX_MAX_LENGTH));
                isOK = false;
            }

            if (model.START_DATE.HasValue && model.END_DATE.HasValue && model.START_DATE.Value > model.END_DATE.Value)
            {
                ModelState.AddModelError(String.Empty, Messages.EndDateLessStartDateWarning);
                isOK = false;
            }

            return isOK;
        }

        /// <summary>
        /// Create/Update Tbl_Information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(InformationEntity model)
        {
            try
            {
                using (InformationServices service = new InformationServices())
                {
                    if (ModelState.IsValid)
                    {
                        bool isNew = false;
                        if (model.INFO_SEQ_NO == 0)
                        {
                            isNew = true;
                            model.INS_USER_ID = base.CmnEntityModel.UserSegNo;
                            model.INS_DATE = Utility.GetCurrentDateTime();
                            model.UPD_DATE = Utility.GetCurrentDateTime();
                            service.InsertInformation(model);
                            JsonResult result = Json(new { isNew = isNew }, JsonRequestBehavior.AllowGet);
                            return result;
                        }
                        else
                        {
                            model.UPD_DATE = Utility.GetCurrentDateTime();
                            service.UpdateInformation(model);
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

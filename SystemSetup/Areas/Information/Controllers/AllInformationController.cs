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

namespace SystemSetup.Areas.Information.Controllers
{
    public class AllInformationController : BaseController
    {

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #region AllInformationList

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AllInformationList(AllInformationListModel model = null)
        {
            if (model == null)
            {
                model = new AllInformationListModel();
            }
            //共通サービス初期化
            CommonService service = new CommonService();
            return View("AllInformationList", model);
        }

        /// <summary>
        /// AllInformation Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AllInformationSearch(DataTablesModel dt, AllInformationListModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    using (AllInformationServices service = new AllInformationServices())
                    {
                        int total_row;
                        var dataList = service.AllInformationSearch(dt, ref model, out total_row);
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
                                    i.CONTENT_TYPE != null ? Constants.ContentType.Items[i.CONTENT_TYPE].ToString() : String.Empty,
                                    i.TITLE != null ? HttpUtility.HtmlEncode(i.TITLE) : String.Empty,
                                    i.CONTENT != null ? HttpUtility.HtmlEncode(i.CONTENT) : String.Empty,
                                    i.PUBLISH_DATE_START.HasValue ? i.PUBLISH_DATE_START.Value.ToString("yyyy/MM/dd") : String.Empty,
                                    i.PUBLISH_DATE_END.HasValue ? i.PUBLISH_DATE_END.Value.ToString("yyyy/MM/dd") : String.Empty,
                                    i.DSP_PRIORITY,
                                    i.DEL_FLG == "0" ? "" : "○"
                                })
                        });

                        return result;
                    }
                }
            }
            return new EmptyResult();
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

        /// <summary>
        /// Post Information Register
        /// </summary>
        /// <returns></returns>
        public ActionResult AllInformationEdit(long INFO_SEQ_NO = 0)
        {
            AllInformationListModel model = CreateEditViewModel(INFO_SEQ_NO);
            return View("AllInformationRegister", model);
        }

        /// <summary>
        /// Create edit view model
        /// </summary>
        /// <param name="infoSeqNo"></param>
        /// <returns></returns>
        public AllInformationListModel CreateEditViewModel(long infoSeqNo)
        {
            CommonService comService = new CommonService();
            AllInformationListModel model = new AllInformationListModel();

            using (AllInformationServices service = new AllInformationServices())
            {
                if (infoSeqNo > 0)
                {
                    model = service.GetAllInformation(infoSeqNo);
                }
                else
                {
                    model.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
                }
            }

            //var content1 = model.CONTENT;
            //var content = content1.Replace("\r\n", @"\r\n");
            //model.CONTENT = content;

            return model;
        }

        /// <summary>
        /// Validate data for AllInformationList screen
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ValidateAllInformationList(PostInformationListModel model)
        {
            bool isOK = true;

            if (model.CONTENT.Length > 4000)
            {
                ModelState.AddModelError(String.Empty, string.Format(Messages.MaxLength, Constants.Resources.Information.CONTENT, Constant.NVARCHAR_MAX_MAX_LENGTH));
                isOK = false;
            }

            if (model.START_DATE.HasValue && model.END_DATE.HasValue && model.START_DATE.Value > model.END_DATE.Value)
            {
                ModelState.AddModelError(String.Empty, Messages.EndDateLessStartDateWarning);
                isOK = false;
            }
            return isOK;
        }

        #region

        /// <summary>
        /// Validate AllInformationEdit screen
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ValidateAllInformationEdit(AllInformationEntity model)
        {
            bool isOK = true;

            if (String.IsNullOrEmpty(model.CONTENT))
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.Required, AllInformation.CONTENT));
                isOK = false;
            }

            if (String.IsNullOrEmpty(model.TITLE))
            {
                ModelState.AddModelError("TITLE", Messages.PostInformationTitleRequired);
                isOK = false;
            }

            if (model.TITLE.Length > Constant.TITLE_MAX_LENGTH)
            {
                ModelState.AddModelError(String.Empty, String.Format(Messages.MaxLength, AllInformation.lblTitle, Constant.TITLE_MAX_LENGTH));
                isOK = false;
            }


            if (model.CONTENT.Length > Constant.NVARCHAR_MAX_MAX_LENGTH)
            {
                ModelState.AddModelError(String.Empty, String.Format(Messages.MaxLength, AllInformation.CONTENT, Constant.NVARCHAR_MAX_MAX_LENGTH));
                isOK = false;
            }
            if (!model.PUBLISH_DATE_START.HasValue)
            {
                ModelState.AddModelError(String.Empty, String.Format(Messages.Required, AllInformation.PUBLISH_DATE_START));
                isOK = false;
            }

            if (!model.PUBLISH_DATE_END.HasValue)
            {
                ModelState.AddModelError(String.Empty, String.Format(Messages.Required, AllInformation.PUBLISH_DATE_END));
                isOK = false;
            }

            if (model.PUBLISH_DATE_START.Value > model.PUBLISH_DATE_END.Value)
            {
                ModelState.AddModelError(String.Empty, AllInformation.AllInformationStartDateMustBeEarlierThanEndDate);
                isOK = false;
            }

            return isOK;
        }

        /// <summary>
        /// Create/Update AllInformation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(AllInformationEntity model)
        {
            try
            {
                using (AllInformationServices service = new AllInformationServices())
                {
                    if (ModelState.IsValid && ValidateAllInformationEdit(model))
                    {
                        bool isNew = false;
                       
                        if (model.INFO_SEQ_NO == 0)
                        {
                            //Check exist
                            var exist_name = service.CheckExist(model);
                            if (!exist_name)
                            {
                                isNew = true;
                                model.INS_USER_ID = base.CmnEntityModel.UserSegNo;
                                model.INS_DATE = Utility.GetCurrentDateTime();
                                model.UPD_DATE = Utility.GetCurrentDateTime();
                                model.COMPANY_CD = ("     ");
                                service.InsertAllInformation(model);
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
                            model.COMPANY_CD = ("     ");
                            service.UpdateAllInformation(model);
                            JsonResult result = Json(new { statusCode = Constants.Constant.CREATED,  isNew = isNew }, JsonRequestBehavior.AllowGet);
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

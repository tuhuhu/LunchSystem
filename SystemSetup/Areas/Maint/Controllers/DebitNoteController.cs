using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemSetup.BusinessServices;
using SystemSetup.Constants;
using SystemSetup.Constants.Resources;
using SystemSetup.Controllers;
using SystemSetup.Models;


namespace SystemSetup.Areas.Maint.Controllers
{
    public class DebitNoteController : BaseController
    {
        //
        // GET: /Maint/DebitNote/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DebitNoteFormat(DebitNoteFormatModel model = null)
        {
            if (model == null)
            {
                model = new DebitNoteFormatModel();
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

            return View("DebitNoteFormat", model);
        }
        /// <summary>
        /// Search debit note
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult DebitNoteFormatSearch(DataTablesModel dt, DebitNoteFormatModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    using (DebitNoteMaintService service = new DebitNoteMaintService())
                    {
                        int total_row;
                        var dataList = service.DebitNoteFormatSearch(dt, ref model, out total_row);
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
                                    i.BILLING_ADD_FORMAT_SEQ_NO,
                                    order++,
                                    i.DEL_FLG,
                                    "",
                                    i.BILLING_TYPE,
                                    i.BILLING_FORMAT_DISP_NAME != null ? HttpUtility.HtmlEncode(i.BILLING_FORMAT_DISP_NAME) : String.Empty,
                                    i.BILLING_FORMAT_PATH != null ? HttpUtility.HtmlEncode(i.BILLING_FORMAT_PATH) : String.Empty,
                                    i.BILLING_FORMAT_DETAIL_LINE,
                                    i.DISABLE_FLG
                                })
                        });
                        return result;
                    }
                }
            }
            return new EmptyResult();
        }
        /// <summary>
        /// Update debit note format list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DebitNoteFormatEdit(DebitNoteFormatModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid && ValidateData(model))
                {

                    using (DebitNoteMaintService service = new DebitNoteMaintService())
                    {
                        if (service.UpdateDebitNote(model) > 0)
                        {
                            JsonResult result = Json(
                                    new
                                    {
                                        message = "Update success"
                                    },
                                    JsonRequestBehavior.AllowGet);

                            return result;
                        }
                    }
                }
            }

            return new EmptyResult();
        }
        /// <summary>
        /// Validate data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ValidateData(DebitNoteFormatModel model)
        {
            var isValid = true;
            var isName = true;
            var isPath = true;
            var isLine = true;
            foreach (var data in model.DEBIT_NOTE_FORMAT_LIST)
            {
                if (isName)
                {
                    if (string.IsNullOrEmpty(data.BILLING_FORMAT_DISP_NAME))
                    {
                        ModelState.AddModelError(string.Empty, string.Format(Messages.Required, Constants.Resources.Maint.FormatDisplayName));
                        isName = false;
                    }
                    else if (data.BILLING_FORMAT_DISP_NAME.Length > Constant.MAX_FORMAT_NAME)
                    {
                        ModelState.AddModelError(string.Empty, string.Format(Messages.MaxLength, Constants.Resources.Maint.FormatDisplayName, Constant.MAX_FORMAT_NAME));
                        isName = false;
                    }
                }

                if (isPath)
                {
                    if (string.IsNullOrEmpty(data.BILLING_FORMAT_PATH))
                    {
                        ModelState.AddModelError(string.Empty, string.Format(Messages.Required, Constants.Resources.Maint.FormatPath));
                        isPath = false;
                    }
                    else if (data.BILLING_FORMAT_PATH.Length > Constant.MAX_FORMAT_PATH)
                    {
                        ModelState.AddModelError(string.Empty, string.Format(Messages.MaxLength, Constants.Resources.Maint.FormatPath, Constant.MAX_FORMAT_PATH));
                        isPath = false;
                    }
                }

                if (isLine)
                {
                    if (data.BILLING_FORMAT_DETAIL_LINE == null)
                    {
                        ModelState.AddModelError(string.Empty, string.Format(Messages.Required, Constants.Resources.Maint.LineDetail));
                        isLine = false;
                    }
                    else if (data.BILLING_FORMAT_DETAIL_LINE.ToString().Length > Constant.MAX_DETAIL_LINE)
                    {
                        ModelState.AddModelError(string.Empty, string.Format(Messages.MaxLength, Constants.Resources.Maint.LineDetail, Constant.MAX_DETAIL_LINE));
                        isLine = false;
                    }
                }

                if (!isName && !isPath && !isLine)
                {
                    break;
                }
            }
            if (!isName || !isPath || !isLine)
            {
                isValid = false;
            }

            return isValid;
        }

    }
}

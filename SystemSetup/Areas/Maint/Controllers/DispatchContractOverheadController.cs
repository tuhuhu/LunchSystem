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
using SystemSetup.Constants.Resources;

namespace SystemSetup.Areas.Maint.Controllers
{
    public class DispatchContractOverheadController : BaseController
    {
        //
        // GET: /Maint/DispatchContractOverhead/

        /// <summary>
        /// Dispatch Contract Overhead List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult DispatchContractOverheadList(DispatchContractOverheadModel model = null)
        {
            if (model == null)
            {
                model = new DispatchContractOverheadModel();
            }

            CommonService service = new CommonService();

            model.CONTRACT_FIRM_LIST = service.GetContractFirmMasterByOutside().Select(
                f => new SelectListItem
                {
                    Value = f.COMPANY_CD.ToString(),
                    Text = f.COMPANY_CD.ToString() + ":" + f.COMPANY_NAME.ToString()
                }).ToList();
            return View("DispatchContractOverheadList", model);
        }

        /// <summary>
        /// Dispatch Contract Overhead Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult DispatchContractOverheadSearch(DataTablesModel dt, DispatchContractOverheadModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    using (var service = new DispatchContractOverheadServices())
                    {
                        int total_row;
                        var dataList = service.DispatchContractOverheadSearch(dt, ref model, out total_row);
                        int order = 1;
                        var result = Json(new
                        {
                            sEcho = dt.sEcho,
                            iTotalRecords = total_row,
                            iTotalDisplayRecords = total_row,
                            aaData = (from i in dataList
                                      select new object[]{
                                i.FORMAT_SEQ_NO,
                                order++,
                                i.DEL_FLG,
                                "",
                                 HttpUtility.HtmlEncode(i.FORMAT_DISP_NAME),
                                 HttpUtility.HtmlEncode(i.FORMAT_PATH),
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
        /// Dispatch Contract Overhead Edit
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DispatchContractOverheadEdit(DispatchContractOverheadModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid && ValidateData(model))
                {
                    using (var service = new DispatchContractOverheadServices())
                    {
                        if (service.UpdateDispatchContractOverhead(model) > 0)
                        {
                            JsonResult result = Json(new
                            {
                                message = "Update success"
                            }, JsonRequestBehavior.AllowGet);

                            return result;
                        }
                    }
                }
                else
                {
                    JsonResult result = Json(new
                    {
                        message = "Update failed"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return new EmptyResult();
        }

        /// <summary>
        /// Validate Dispatch Contract Overhead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ValidateData(DispatchContractOverheadModel model)
        {
            var isValid = true;
            var isName = true;
            var isPath = true;
            foreach (var data in model.DISPATCH_CONTRACT_OVERHEAD_LIST)
            {
                if (isName)
                {
                    if (string.IsNullOrEmpty(data.FORMAT_DISP_NAME))
                    {
                        ModelState.AddModelError(string.Empty, string.Format(Messages.Required, Constants.Resources.Maint.FormatDisplayName));
                        isName = false;
                    }
                    else if (data.FORMAT_DISP_NAME.Length > FormatType.MAX_FORMAT_NAME)
                    {
                        ModelState.AddModelError(string.Empty, string.Format(Messages.MaxLength, Constants.Resources.Maint.FormatDisplayName, FormatType.MAX_FORMAT_NAME));
                        isName = false;
                    }
                }

                if (isPath)
                {
                    if (string.IsNullOrEmpty(data.FORMAT_PATH))
                    {
                        ModelState.AddModelError(string.Empty, string.Format(Messages.Required, Constants.Resources.Maint.FormatPath));
                        isPath = false;
                    }
                    else if (System.Text.ASCIIEncoding.UTF8.GetByteCount(data.FORMAT_PATH) > FormatType.MAX_FORMAT_PATH)
                    {
                        ModelState.AddModelError(string.Empty, string.Format(Messages.MaxByte, Constants.Resources.Maint.FormatPath));
                        isPath = false;
                    }
                }
                // Name and path is false, break
                if (!isName && !isPath)
                {
                    break;
                }
            }
            // Name or path is false -> isvalid = true
            if (!isName || !isPath)
            {
                isValid = false;
            }
            return isValid;
        }
    }
}

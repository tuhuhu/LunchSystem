using System.Data;
using System.Text;
using SystemSetup.Constants.Resources;

namespace SystemSetup.Areas.Maint.Controllers
{
    using SystemSetup.BusinessServices;
    using SystemSetup.Controllers;
    using SystemSetup.Models;
    using SystemSetup.UtilityServices;
    using SystemSetup.Constants;
    using SystemSetup.ReportServices;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;

    public class BillingNumberMaintController : BaseController
    {
        #region Billing Number Maint

        /// <summary>
        /// Billing Number Maint List
        /// </summary>
        /// <returns></returns>
        public ActionResult BillingNumberMaintList(BillingNumberMaintModel REGMODEL)
        {
            BillingNumberMaintModel model = new BillingNumberMaintModel();
            List<SelectListItem> items = new List<SelectListItem>();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            model.DISPLAY_START = REGMODEL.DISPLAY_START;
            model.DISPLAY_LENGTH = REGMODEL.DISPLAY_LENGTH;

            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                items.Add(new SelectListItem() { Text = pair.Key, Value = pair.Value, Selected = false });
            }

            ViewBag.DurationType = new SelectList(items, "Value", "Text");
            return View("BillingNumberMaintList", model);
        }

        /// <summary>
        /// User Maint Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult BillingNumberMaintSearch(DataTablesModel dt, BillingNumberMaintModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    using (BillingNumberMaintServices service = new BillingNumberMaintServices())
                    {
                        int totalRow;
                        var dataList = service.BillingNumberMaintSearch(dt, ref model, out totalRow);
                        int order = 1;

                        var result = Json(
                        new
                        {
                            sEcho = dt.sEcho,
                            iTotalRecords = totalRow,
                            iTotalDisplayRecords = totalRow,
                            aaData = (from i in dataList
                                      select new object[]
                                {
                                    order++,
                                    i.COMPANY_NAME != null ? HttpUtility.HtmlEncode(i.COMPANY_NAME) : String.Empty,
                                    i.PLAN_NAME != null ? HttpUtility.HtmlEncode(i.PLAN_NAME) : String.Empty,
                                    i.MONTHLY_BILL_DATA_UPPER != null ? i.MONTHLY_BILL_DATA_UPPER.ToString("#,##0") + "件／月" : "0 件／月",
                                    i.BILLING_NUMBER_DATA_PREV_MONTH.ToString("#,##0") + "件",
                                    i.BILLING_NUMBER_DATA_THIS_MONTH.ToString("#,##0") + "件",
                                    i.BILLING_NUMBER_DATA_NEXT_MONTH.ToString("#,##0") + "件",
                                    i.FILE_SIZE_TOTAL
                                })
                        });
                        return result;
                    }
                }
            }
            return new EmptyResult();
        }

        /// <summary>
        /// Export billing number data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ExportBillingDataCsv(BillingNumberMaintModel model)
        {
            string[] columns = CreateColumnsList();
            using (BillingNumberMaintServices service = new BillingNumberMaintServices())
            {
                var data = service.GetBillingNumberData(model);
                DataTable dt = this.ConvertToDataTableForExport(data, columns.ToArray());
                string fileName = Constants.Resources.Maint.ContractBillingNumberMaintList + Utility.GetCurrentDateTime().ToString("yyyyMMddHHmmss") + ".csv";
                ReportServices.ExportToCsvData(this, dt, fileName);
            }
            return new EmptyResult();
        }

        /// <summary>
        /// Creat Column List
        /// </summary>
        /// <returns></returns>
        private string[] CreateColumnsList()
        {
            List<string> columns = new List<string>();
            columns.Add(BillingNumberList.TH_SerialNumber);
            columns.Add(BillingNumberList.TH_ContractCompanyName);
            columns.Add(BillingNumberList.TH_ContractPlanName);
            columns.Add(BillingNumberList.TH_MaximumNumberData);
            columns.Add(BillingNumberList.TH_BillingNumberDataPrevMonth);
            columns.Add(BillingNumberList.TH_BillingNumberDataThisMonth);
            columns.Add(BillingNumberList.TH_BillingNumberDataNextMonth);
            columns.Add(BillingNumberList.TH_FileSizeTotal);
            return columns.ToArray();
        }

        /// <summary>
        /// Convert To Data Table For Export
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        private DataTable ConvertToDataTableForExport(IEnumerable<BillingNumberMaintEntityPlus> dataList, string[] columns)
        {
            DataTable dataTable = new DataTable("Dynamic");
            //Get all the properties
            for (int i = 0; i < columns.Length; i++)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(columns[i]);
            }
            var count = 1;
            foreach (BillingNumberMaintEntityPlus data in dataList)
            {
                var values = new object[columns.Length];
                var i = 0;
                values[i++] = count++;
                values[i++] = ReportServices.EscapeStringCsv(data.COMPANY_NAME);
                values[i++] = ReportServices.EscapeStringCsv(data.PLAN_NAME);
                values[i++] = data.MONTHLY_BILL_DATA_UPPER != null ? data.MONTHLY_BILL_DATA_UPPER : 0;
                values[i++] = data.BILLING_NUMBER_DATA_PREV_MONTH;
                values[i++] = data.BILLING_NUMBER_DATA_THIS_MONTH;
                values[i++] = data.BILLING_NUMBER_DATA_NEXT_MONTH;
                values[i++] = data.FILE_SIZE_TOTAL;
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        #endregion
    }
}

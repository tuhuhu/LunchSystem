//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/17
// Comment		: Create new
//------------------------------------------------------------------------

namespace SystemSetup.ReportServices
{
    using AdvanceSoftware.ExcelCreator;
    using AdvanceSoftware.ExcelCreator.Xlsx;
    using SystemSetup.BusinessServices;
    using SystemSetup.Constants;
    using SystemSetup.Constants.Resources;
    using SystemSetup.Models;
    using SystemSetup.UtilityServices;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;

    /// <summary>
    /// Report services
    /// </summary>
    public class ReportServices
    {
        /// <summary>
        /// Get Header Name Row 
        /// </summary>
        /// <param name="listVarName"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        private static string GetNameRow(CellPositionCollection listVarName, string itemName)
        {
            string result = string.Empty;
            for (int i = 0; i < listVarName.Count; i++)
            {
                if (listVarName[i].VarName == itemName)
                {
                    result = Regex.Replace(listVarName[i].CellString, @"\d", "");
                    //string next = ((char)(productname_row[0] + 1)).ToString();
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Get Payment Type
        /// </summary>
        /// <param name="pay_site_type"></param>
        /// <returns></returns>
        private static string GetPaymentType(string pay_site_type)
        {
            return Constants.PaymentSiteType.Items[Convert.ToInt32(pay_site_type)].ToString();
        }

        /// <summary>
        /// Get payment term
        /// </summary>
        /// <param name="clo_site_type"></param>
        /// <param name="clo_day"></param>
        /// <param name="pay_site_type"></param>
        /// <param name="pay_day"></param>
        /// <returns></returns>
        private static string GetPaymentTerm(int clo_site_type, int clo_day, int pay_site_type, int pay_day)
        {
            string result = string.Empty;

            if (clo_site_type == ClosingSiteType.None)
            {
                if ((PaymentSiteType.Equals(pay_site_type.ToString(), PaymentSiteType.None)) || (PaymentSiteType.Equals(pay_day.ToString(), PaymentSiteType.None)))
                {
                    result = "編集なし";
                    return result;
                }
                else
                {
                    if (PaymentSiteType.Equals(pay_site_type.ToString(), PaymentSiteType.ThisMonth))
                    {
                        result = "当月" + pay_day + "日払";
                        return result;
                    }
                    else
                    {
                        result = GetMonthLabel(pay_site_type) + pay_day + "日払";
                        return result;
                    }
                }
            }
            else
            {
                if (PaymentSiteType.Equals(pay_site_type.ToString(), PaymentSiteType.None))
                {
                    result = clo_site_type + "月" + clo_day + "日締";
                    return result;
                }
                if ((PaymentSiteType.Equals(pay_day.ToString(), PaymentSiteType.None)) && (ClosingDay.Equals(clo_day.ToString(), ClosingDay.Day31)))
                {
                    result = clo_site_type + "月" + "末日締";
                    return result;
                }
            }

            if (clo_day == Convert.ToInt32(ClosingDay.None))
            {
                if ((PaymentSiteType.Equals(pay_site_type.ToString(), PaymentSiteType.None)) || (PaymentSiteType.Equals(pay_day.ToString(), PaymentSiteType.None)))
                {
                    result = "編集なし";
                    return result;
                }
                else
                {
                    if (pay_day == Convert.ToInt32(PaymentDayType.Day31))
                    {
                        result = GetMonthLabel(pay_site_type) + "末日払";
                        return result;
                    }
                }
            }
            else
            {
                if ((clo_day == Convert.ToInt32(ClosingDay.Day31)) && (pay_day == Convert.ToInt32(PaymentDayType.Day31)) && (clo_site_type != ClosingSiteType.None) && (pay_site_type > Convert.ToInt32(PaymentSiteType.ThisMonth)))
                {
                    result = clo_site_type + "月末日締 " + GetMonthLabel(pay_site_type) + "末日払";
                    return result;
                }
                else
                {
                    result = clo_site_type + "月" + clo_day + "日締 " + GetMonthLabel(pay_site_type) + pay_day + "日払";
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Get month label
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private static string GetMonthLabel(int index)
        {
            string result = string.Empty;
            switch (index)
            {
                case 1:
                    result = "当月";
                    break;
                case 2:
                    result = "翌月";
                    break;
                case 3:
                    result = "翌々月";
                    break;
                case 4:
                    result = "翌々々月";
                    break;
                case 5:
                    result = "翌々々々月";
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Exports the PDF.
        /// </summary>
        /// <param name="reportID">The report identifier.</param>
        /// <param name="dataSource">The data source.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public MemoryStream ExportPDF(string reportID, DataSet dataSource)
        {

            MemoryStream result = null;

            return result;


        }

        /// <summary>
        /// Create CSV File
        /// </summary>
        /// <param name="dt">Data Table</param>
        /// <returns>
        /// byte array
        /// </returns>
        public byte[] ExportCSV(DataTable dt, string header = "")
        {
            // Create the CSV file to which grid data will be exported.
            MemoryStream ms = new MemoryStream();

            StreamWriter sw = new StreamWriter(ms, Encoding.GetEncoding("shift-jis"));

            dt = dt == null ? new DataTable() : dt;

            // First we will write the headers.
            int colCount = dt.Columns.Count;

            if (string.IsNullOrEmpty(header))
            {
                for (int i = 0; i < colCount; i++)
                {
                    sw.Write("\"" + dt.Columns[i] + "\"");

                    if (i < colCount - 1)
                    {
                        sw.Write(",");
                    }
                }
            }
            else
            {
                sw.Write(header);
            }

            sw.Write(sw.NewLine);

            // Now write all the rows.
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < colCount; i++)
                {
                    sw.Write("\"" + Convert.ToString(dr[i]) + "\"");
                    if (i < colCount - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Flush();
            byte[] bytes = ms.ToArray();
            ms.Close();
            return bytes;
        }

        #region Export file Csv
        /// <summary>
        /// Using Data form datatable to export to csv file
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        public static void ExportToCsvData(Controller controller, DataTable dt, string fileName = "data.csv")
        {
            string delimiter = ",";

            controller.Response.Clear();
            controller.Response.ContentType = "text/csv;";
            controller.Response.ContentEncoding = System.Text.Encoding.GetEncoding("Shift-jis");
            controller.Response.AppendHeader("Content-type", "application/x-download");
            DownloadUtil.AddHeaderContentDisposition(HttpContext.Current.Request, controller.Response, fileName);

            string value = "";
            var builder = new StringBuilder();

            //write the csv column headers
            for (int i = 0; i < dt.Columns.Count; i++)
            {

                value = dt.Columns[i].ColumnName;
                // Implement special handling for values that contain comma or quote
                // Enclose in quotes and double up any double quotes
                if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
                    builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                else
                {
                    builder.Append(value);
                }

                controller.Response.Write(builder.ToString());
                controller.Response.Write((i < dt.Columns.Count - 1) ? delimiter : Environment.NewLine);
                builder.Clear();
            }
            //write the data
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    controller.Response.Write(row[i].ToString());
                    controller.Response.Write((i < dt.Columns.Count - 1) ? delimiter : Environment.NewLine);
                }
            }

            try
            {
                controller.Response.End();
            }
            catch (HttpException)
            {
                // When canceled.
                // The remote host closed the connection. The error code is 0x800704CD
            }
        }

        public static object FormatDynamicDecimalCsv(dynamic value)
        {
            if (value == null)
            {
                return null;
            }
            decimal tmpOut;
            return Decimal.TryParse(value.ToString(), out tmpOut) ? value : EscapeStringCsv(value);
        }

        public static string FormatDynamicDateTimeCsv(dynamic value)
        {
            if (value == null)
            {
                return null;
            }
            DateTime tmpDate;
            return DateTime.TryParse(value.ToString(), out tmpDate) ? tmpDate.ToString("yyyy/MM/dd") : EscapeStringCsv(value);
        }

        public static string EscapeStringCsv(string value)
        {
            if (value == null)
            {
                return null;
            }
            if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
            {
                return string.Format("\"{0}\"", value.Replace("\"", "\"\""));
            }
            return "\"" + value + "\"";
        }

        public static string FormatDateCsv(object dateValue)
        {
            if (dateValue == null)
            {
                return null;
            }
            return ((DateTime)dateValue).ToString("yyyy/MM/dd");
        }

        /// <summary>
        /// Convert from IList<T> to DataTable
        /// </summary>
        /// <param name="items"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static DataTable ToDataTableT<T>(IList<T> items, string[] columns)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < columns.Length; i++)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(columns[i]);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    var tmpValue = Props[i].GetValue(item, null);
                    decimal tmpDecimal = 0;
                    if (tmpValue == null)
                    {
                        values[i] = tmpValue;
                    }
                    else if (Props[i].PropertyType == typeof(Nullable<System.DateTime>)
                        || Props[i].PropertyType == typeof(System.DateTime))
                    {
                        // Format datetime values
                        values[i] = FormatDateCsv(tmpValue);
                    }
                    else if (Props[i].PropertyType == typeof(string)
                        || Props[i].PropertyType == typeof(System.String)
                        || (Props[i].PropertyType == typeof(System.Object) && !Decimal.TryParse(tmpValue.ToString(), out tmpDecimal)))
                    {
                        values[i] = EscapeStringCsv(tmpValue.ToString());
                    }
                    else
                    {
                        // orther values
                        values[i] = tmpValue;
                    }
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        #endregion
    }
}
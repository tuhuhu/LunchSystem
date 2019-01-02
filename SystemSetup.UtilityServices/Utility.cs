//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/17
// Comment		: Create new
//------------------------------------------------------------------------

//using SystemSetup.DataAccess;
//using SystemSetup.Models.Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Resources;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using SystemSetup.Constants;
using SystemSetup.Constants.Resources;

namespace SystemSetup.UtilityServices
{
    public class UtilityServices
	{
		#region Static paramater

		/// <summary>
		/// List time stamp
		/// </summary>
		private Dictionary<string, Int64> listTimeStamp = new Dictionary<string, Int64>();		

		#endregion

        public string GeneratePassword()
        {
            string strPwdchar = "abcdefghijklmnopqrstuvwxyz0123456789#+@&$ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string strPwd = "";
            Random rnd = new Random();
            for (int i = 0; i <= 7; i++)
            {
                int iRandom = rnd.Next(0, strPwdchar.Length - 1);
                strPwd += strPwdchar.Substring(iRandom, 1);
            }
            return strPwd;
        }

		public string ConvertHalfToFullsize(string target)
		{
			string result = target;
			ResourceManager managerResource = new ResourceManager(typeof(Constants.Resources.LibFullHalf));
			ResourceSet resourceSet = managerResource.GetResourceSet(CultureInfo.CurrentCulture, true, true);
			foreach (DictionaryEntry entry in resourceSet)
			{
				if (result.Contains(entry.Key.ToString()))
				{
					result = result.Replace(entry.Key.ToString(), entry.Value.ToString());
				}
			}
			return result;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetMonthLabel(int index)
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
        /// Get Closing Display
        /// </summary>
        /// <param name="CLOSING_DAY"></param>
        /// <param name="CLOSING_SITE_TYPE"></param>
        /// <returns></returns>
        public static string GetClosingDisplay(int? CLOSING_DAY, int? CLOSING_SITE_TYPE)
        {
            string result = string.Empty;

            if (CLOSING_DAY != Convert.ToInt32(ClosingDay.None))
            {
                string closingday = string.Empty;
                if (CLOSING_SITE_TYPE != ClosingSiteType.None)
                {
                    if (CLOSING_DAY == Convert.ToInt32(Constants.ClosingDayType.Day31))
                    {
                        closingday = Constant.CLOSE_LASTDAY_MONTH;
                    }
                    else
                    {
                        closingday = CLOSING_DAY + Constant.CLOSE_DAY;
                    }
                    result = CLOSING_SITE_TYPE + "月" + closingday;
                }
                else
                {
                    if (CLOSING_DAY == Convert.ToInt32(Constants.ClosingDayType.Day31))
                    {
                        closingday = Constant.CLOSE_LASTDAY_MONTH;
                    }
                    else
                    {
                        closingday = CLOSING_DAY + Constant.CLOSE_DAY;
                    }
                    result = closingday;
                }
            }

            return result;
        }

        /// <summary>
        /// Get Payment Display
        /// </summary>
        /// <param name="PAYMENT_DAY"></param>
        /// <param name="PAYMENT_SITE_TYPE"></param>
        /// <returns></returns>
        public static string GetPaymentDisplay(int? PAYMENT_DAY, string PAYMENT_SITE_TYPE)
        {
            string result = string.Empty;

            if (PAYMENT_DAY != Convert.ToInt32(PaymentDayType.None))
            {
                string paymentday = string.Empty;
                if (!PaymentSiteType.None.Equals(PAYMENT_SITE_TYPE))
                {
                    if (PAYMENT_DAY == Convert.ToInt32(Constants.PaymentDayType.Day31))
                    {
                        paymentday = Constant.PAY_LASTDAY_MONTH;
                    }
                    else
                    {
                        paymentday = PAYMENT_DAY + Constant.PAY_DAY;
                    }
                    result = GetPaymentType(PAYMENT_SITE_TYPE) + paymentday;
                }
                else
                {
                    if (PAYMENT_DAY == Convert.ToInt32(Constants.PaymentDayType.Day31))
                    {
                        paymentday = Constant.PAY_LASTDAY_MONTH;
                    }
                    else
                    {
                        paymentday = PAYMENT_DAY + Constant.PAY_DAY;
                    }
                    result = paymentday;
                }
            }

            return result;
        }

        /// <summary>
        /// Get Payment Type
        /// </summary>
        /// <param name="pay_site_type"></param>
        /// <returns></returns>
        public static string GetPaymentType(string pay_site_type)
        {
            return Constants.PaymentSiteType.Items[Convert.ToInt32(pay_site_type)].ToString();
        }

        /// <summary>
        /// Get Status
        /// </summary>
        /// <param name="prjState"></param>
        /// <param name="contState"></param>
        /// <param name="billState"></param>
        /// <returns></returns>
        public static string GetDisplayStatusString(string prjState, string contState, string billState)
        {
            string status = string.Empty;
            switch (prjState)
            {
                case ProjectState.CREATE_NEW:
                    switch (contState)
                    {
                        case ContractState.CREATE_NEW: //00
                            status = EstimationStatus.CreateNew;
                            break;
                        case ContractState.CREATE_ESTIMATE: //10
                            status = EstimationStatus.CreateEstimate;
                            break;
                        case ContractState.ISSUE_ESTIMATE: //15
                            status = EstimationStatus.IssueEstimate;
                            break;
                        case ContractState.EDIT_ESTIMATE: //17
                            status = EstimationStatus.EditEstimate;
                            break;
                        case ContractState.REISSUE_ESTIMATE: //19
                            status = EstimationStatus.ReissueEstimate;
                            break;
                        case ContractState.CREATE_CONTRACT: //30
                            status = EstimationStatus.CreateContract;
                            break;
                        case ContractState.IMPLEMENT_CONTRACT: //35
                            status = EstimationStatus.ImplementContract;
                            break;
                        case ContractState.CHANGE_CONTRACT: //37
                            status = EstimationStatus.ChangeContract;
                            break;
                        case ContractState.FINISH_IN_MIDDLE: //95
                            if (billState == BillingState.FINISH_BILLING)
                            {
                                status = EstimationStatus.FinishBillingFinishInMiddle;
                            }
                            else
                            {
                                status = EstimationStatus.FinishInMiddle;
                            }
                            break;
                        case ContractState.FINISH_EXPAND: //97
                            if (billState == BillingState.FINISH_BILLING)
                            {
                                status = EstimationStatus.FinishExpand;
                            }
                            else
                            {
                                status = EstimationStatus.FinishBillingExtensionContinues;
                            }
                            break;
                        case ContractState.FINISH_COMPLETE: //99
                            if (billState == BillingState.FINISH_BILLING)
                            {
                                status = EstimationStatus.FinishBilling;
                            }
                            else
                            {
                                status = EstimationStatus.FinishComplete;
                            }
                            break;
                        default:
                            break;
                    }
                    break;

                case ProjectState.DELETE_ESTIMATE:
                    status = EstimationStatus.DeleteEstimate;
                    break;
                case ProjectState.DELETE_CONTRACT:
                    status = EstimationStatus.DeleteContract;
                    break;
                case ProjectState.FINISH_PROJECT:
                    status = EstimationStatus.FinishProject;
                    break;
                default:
                    break;
            }
            return status;
        }

		private bool IsContentMatch(string ListItems, string Item)
		{
			int ListItemsLength = ListItems.Length;
			int ItemLength = Item.Length;
			System.Globalization.CompareInfo ci = System.Globalization.CompareInfo.GetCompareInfo("ja-JP");

			if (ListItemsLength < ItemLength)
				return false;
			else
			{
				string index = string.Empty;
				for (int i = 0; i <= ListItemsLength - ItemLength; i++)
				{
					index = ListItems.Substring(i, ItemLength);
					if (ci.Compare(index, Item, System.Globalization.CompareOptions.IgnoreWidth | System.Globalization.CompareOptions.IgnoreKanaType | System.Globalization.CompareOptions.IgnoreCase) == 0)
					{
						return true;
					}
				}
				return false;
			}
		}

        public static decimal RoundByType(string roundType, decimal number)
        {
            if (number % 1 == 0)
            {
                return number;
            }
            switch (roundType)
            {
                case RoundingType.RoundDown:
                    // 0:小数点以下切り捨て: Round Down
                    number = number < 0 && number != Math.Floor(number) ? Math.Floor(number) + 1 : Math.Floor(number);
                    break;
                case RoundingType.RoundUp:
                    // 1：小数点第一位切り上げ: Round Up
                    number = number > 0 && number != Math.Floor(number) ? Math.Floor(number) + 1 : Math.Floor(number);
                    break;
                case RoundingType.RoundOff:
                    // 2：小数点第一位四捨五入: Round C#
                    number = Math.Round(number, 0, MidpointRounding.AwayFromZero);
                    break;
            }
            return number;
        }

        /// <summary>
        /// Round hour by minute unit
        /// </summary>
        /// <param name="value">input value</param>
        /// <param name="unit">minute unit</param>
        /// <returns></returns>
        public static decimal RoundTimeByUnit(decimal value, int unit) {
            decimal roundValue = RoundByType(RoundingType.RoundDown, value);

            // minute overbalance
            decimal minute = RoundByType(RoundingType.RoundOff, value % 1 * 60);

            if (minute != 0 && unit != 0 && minute != unit && minute % unit != 0) {

                // return value by cut all decimal of working time
                if (minute < unit)
                {
                    return roundValue;
                }

                // convert minute to hour
                decimal newMinute = RoundByType(RoundingType.RoundDown, minute / unit) * unit; // minute rounded (round down)
                var hourOverbalanceArr = (newMinute / 60).ToString().Split('.');
                string hourOverbalance = hourOverbalanceArr.Count() > 1 ? (hourOverbalanceArr.Last().Length > 2 ? hourOverbalanceArr.Last().Substring(0, 2) : hourOverbalanceArr.Last()) : "00";

                value = Convert.ToDecimal(roundValue.ToString() + '.' + hourOverbalance);
            }

            return value;
        }

        public static void ProcFatalException(Exception ex, string companyCd, string userId, string logConfigFileName)
        {
            log4net.GlobalContext.Properties["LogFileName"] = string.Format("{0}_{1}", companyCd, ConfigurationManager.AppSettings[ConfigurationKeys.LOG_FILE_ERROR]);
            log4net.Config.XmlConfigurator.Configure(new FileInfo(logConfigFileName));
            log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            StringBuilder strlog = new StringBuilder();

            if (ex != null)
            {
                strlog.AppendLine("Exceptionが発生しました。\n \n ");
            }
            else
            {
                strlog.AppendLine("処理中にシステムエラーが発生しました（Exception情報なし）\n \n ");
            }

            strlog.AppendLine(" 企業コード: " + "\t" + companyCd);
            strlog.AppendLine(string.Format(" User ID: \t{0}", userId));

            var req = HttpContext.Current.Request;
            // Get value and push into log
            string requestedUrl = string.IsNullOrEmpty(req.Url.ToString()) ? "NONE" : req.Url.ToString();
            string browserType = "NONE";
            string browserVersion = "NONE";

            var browser = req.Browser;
            if (browser != null)
            {
                browserType = string.IsNullOrEmpty(browser.Type) ? "NONE" : browser.Type;
                browserVersion = string.IsNullOrEmpty(browser.Version) ? "NONE" : browser.Version;
            }
            // Create string log
            strlog.AppendLine(string.Format(" Requested Url: {0}", requestedUrl));
            strlog.AppendLine(string.Format(" Webブラウザ種別:\t{0} {1}", browserType, browserVersion));

            LogService.LogService.Fatal(logger, strlog.ToString(), ex);
            LogService.LogService.Debug(logger, "      ------ 出力 終わり ------\n \n ");
        }

        public static bool NoticeException(Exception ex, string companyCd, string userId)
        {
            try
            {
                if (ex == null) return false;
                string message = "Exceptionが発生しました。";
                var req = HttpContext.Current.Request;
                string requestedUrl = req.Url.ToString();
                string userAgent = req.UserAgent;
                string browserType = String.Empty;
                string browserVersion = String.Empty;
                if (requestedUrl.Contains("localhost"))
                {
                    return false;
                }
                if (requestedUrl.Contains("i-seiq2-test") && (requestedUrl.Contains("EstimateListIndex") || requestedUrl.Contains("ShowLog")))
                {
                    return false;
                }
                var browser = req.Browser;
                if (browser != null)
                {
                    browserType = browser.Type;
                    browserVersion = browser.Version;
                }

                requestedUrl = string.IsNullOrEmpty(requestedUrl) ? "NONE" : requestedUrl;
                userAgent = string.IsNullOrEmpty(userAgent) ? "NONE" : userAgent;
                browserType = string.IsNullOrEmpty(browserType) ? "NONE" : browserType;
                browserVersion = string.IsNullOrEmpty(browserVersion) ? "NONE" : browserVersion;


                if (ex.GetType() == typeof(HttpException))
                {
                    int statusCode = (ex as HttpException).GetHttpCode();
                    if (statusCode == Constant.NOT_FOUND)
                    {
                        message = "Http 404 Not Found.";
                    }
                }

                string exMessage = string.Empty;
                try
                {
                    exMessage = ex.Message;
                    if (string.IsNullOrEmpty(exMessage))
                    {
                        exMessage = ex.GetType().Name;
                    }
                    exMessage = new string(exMessage.Take(1800).ToArray());
                }
                catch (Exception)
                {
                }

                message = string.Format(@"{0} URL: {1}\nCOMPANY_CD: {2} userId: {3} Browser: {4} \n{5}", message,
                    requestedUrl, companyCd, userId, browserType, exMessage);

                string url = string.Format(@"{0}?username={1}&icon_emoji={2}&msg={3}"
                    , @"https://b9qnzgym0l.execute-api.ap-northeast-1.amazonaws.com/prod/iseiq"
                    , HttpUtility.UrlPathEncode("i-seiQ")
                    , HttpUtility.UrlPathEncode(":scream:")
                    , HttpUtility.UrlPathEncode(message));

                try
                {
                    url = new string(url.Take(2000).ToArray());
                }
                catch (Exception)
                {
                }

                var request = System.Net.WebRequest.Create(url) as System.Net.HttpWebRequest;
                if (request != null)
                {
                    using (var response = request.GetResponse())
                    {
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Create select list item for billing create status
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> BillingCreateStatusSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            Dictionary<string, string> createStatus = new Dictionary<string, string>();

            createStatus.Add(Constants.BillingCreateStatus.AllText, Constants.BillingCreateStatus.All);
            createStatus.Add(Constants.BillingCreateStatus.NotCreatedText, Constants.BillingCreateStatus.NotCreated);
            createStatus.Add(Constants.BillingCreateStatus.CreatedText, Constants.BillingCreateStatus.Created);

            foreach (KeyValuePair<string, string> pair in createStatus)
            {
                list.Add(new SelectListItem() { Text = pair.Key, Value = pair.Value, Selected = false });
            }

            return list;
        }

        /// <summary>
        /// Create select list item for billing auto update status
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> BillingAutoUpdateSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            Dictionary<string, string> automaticUpdate = new Dictionary<string, string>();

            automaticUpdate.Add(Constants.BillingAutoUpdate.AllText, Constants.BillingAutoUpdate.All);
            automaticUpdate.Add(Constants.BillingAutoUpdate.NotAutoText, Constants.BillingAutoUpdate.NotAuto);
            automaticUpdate.Add(Constants.BillingAutoUpdate.OnlyAutoText, Constants.BillingAutoUpdate.OnlyAuto);

            foreach (KeyValuePair<string, string> pair in automaticUpdate)
            {
                list.Add(new SelectListItem() { Text = pair.Key, Value = pair.Value, Selected = false });
            }

            return list;
        }
	}
}
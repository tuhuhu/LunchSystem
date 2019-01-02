//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/17
// Comment		: Create new
//------------------------------------------------------------------------

namespace SystemSetup.UtilityServices
{
    using SystemSetup.Constants;
    using System;
    using System.Resources;

    public static partial class Utility
    {
        /// <summary>
        /// Convert time string
        /// </summary>
        /// <param name="timeDiv"></param>
        /// <returns></returns>
        public static string ConvertTimeString(string timeDiv)
        {
            ResourceManager resourceManager = new ResourceManager(typeof(SystemSetup.Constants.Resources.TimeZone));
            string result = string.Empty;
            if (string.IsNullOrEmpty(timeDiv))
            {
                result = string.Empty;
            }
            else
            {
                result = resourceManager.GetString(timeDiv);
            }
            return result;
        }

        /// <summary>
        /// Function to convert time zone Japanese
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime ConvertDateTimeToJst(DateTime target)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(target.ToUniversalTime(), TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time"));
        }

        /// <summary>
        /// Get current Date and Time
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentDateTime()
        {
            return ConvertDateTimeToJst(DateTime.Now);
        }

        /// <summary>
        /// Get current date only
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentDateOnly()
        {
            return ConvertDateTimeToJst(DateTime.Now).Date;
        }

        /// <summary>
        /// Get Effective Date
        /// </summary>
        /// <param name="EST_EFFECTIVE_TYPE"></param>
        /// <param name="EST_EFFECTIVE_INTERVAL"></param>
        /// <param name="ISSUE_DATE"></param>
        /// <returns></returns>
        public static DateTime GetEffectiveDate(string EST_EFFECTIVE_TYPE, int EST_EFFECTIVE_INTERVAL, DateTime ISSUE_DATE)
        {
            var effectiveDate = new DateTime();

            switch (EST_EFFECTIVE_TYPE)
            {
                case Constants.EffectiveType.Day:
                    effectiveDate = ISSUE_DATE.AddDays(EST_EFFECTIVE_INTERVAL);
                    break;

                case Constants.EffectiveType.Month:
                    effectiveDate = ISSUE_DATE.AddMonths(EST_EFFECTIVE_INTERVAL);
                    break;

                case Constants.EffectiveType.Year:
                    effectiveDate = ISSUE_DATE.AddYears(EST_EFFECTIVE_INTERVAL);
                    break;

                default:
                    break;
            }

            return effectiveDate;
        }

        /// <summary>
        /// Get payment term
        /// </summary>
        /// <param name="clo_site_type"></param>
        /// <param name="clo_day"></param>
        /// <param name="pay_site_type"></param>
        /// <param name="pay_day"></param>
        /// <returns></returns>
        public static DateTime GetPaymentTerm(int clo_site_type, int clo_day, int pay_site_type, int pay_day)
        {
            DateTime paymentTime = GetCurrentDateTime();

            if (clo_site_type == ClosingSiteType.None)
            {
                if ((PaymentSiteType.Equals(pay_site_type.ToString(), PaymentSiteType.None)) || (PaymentDayType.Equals(pay_day.ToString(), PaymentDayType.None)))
                {
                    // PT1 & PT5
                    return paymentTime;
                }
                else
                {

                    if (PaymentSiteType.Equals(pay_site_type.ToString(), PaymentSiteType.ThisMonth))
                    {
                        // PT8
                        paymentTime = new DateTime(GetCurrentDateTime().Year, GetCurrentDateTime().Month, pay_day);
                        return paymentTime;
                    }
                    else
                    {
                        // PT9
                        paymentTime = new DateTime(GetCurrentDateTime().Year, GetCurrentDateTime().Month, pay_day).AddMonths(pay_site_type - 1);
                        return paymentTime;
                    }
                }
            }
            else
            {
                if ((PaymentSiteType.Equals(pay_site_type.ToString(), PaymentSiteType.None)) && (clo_day != Convert.ToInt32(ClosingDay.None)))
                {
                    //PT6
                    paymentTime = new DateTime(GetCurrentDateTime().Year, clo_site_type, clo_day);
                    return paymentTime;
                }
                if ((pay_day == Convert.ToInt32(PaymentDayType.None)) && (clo_day > Convert.ToInt32(Constants.ClosingDay.Day27)))
                {
                    //PT7
                    paymentTime = new DateTime(GetCurrentDateTime().Year, clo_site_type, DateTime.DaysInMonth(GetCurrentDateTime().Year, clo_site_type));
                    return paymentTime;
                }
            }

            if (clo_day == Convert.ToInt32(ClosingDay.None))
            {
                if ((PaymentSiteType.Equals(pay_site_type.ToString(), PaymentSiteType.None)) || (PaymentDayType.Equals(pay_day.ToString(), PaymentDayType.None)))
                {
                    //PT2 & PT4
                    return paymentTime;
                }
                else
                {
                    if (pay_day > Convert.ToInt32(Constants.PaymentDayType.Day27))
                    {
                        //PT10
                        paymentTime = GetSameDayAddMonths(GetCurrentDateTime().Year, GetCurrentDateTime().Month, pay_day, pay_site_type - 1);
                        return paymentTime;
                    }
                }
            }
            else
            {
                if ((clo_day > Convert.ToInt32(Constants.ClosingDay.Day27)) && (pay_day > Convert.ToInt32(Constants.PaymentDayType.Day27)) && (clo_site_type != ClosingSiteType.None) && (pay_site_type > Convert.ToInt32(PaymentSiteType.ThisMonth)))
                {
                    // PT12
                    paymentTime = GetSameDayAddMonths(GetCurrentDateTime().Year, GetCurrentDateTime().Month, pay_day, pay_site_type - 1);
                    return paymentTime;
                }
                else
                {
                    // PT11
                    if (pay_day != Convert.ToInt32(PaymentDayType.None))
                    {
                        paymentTime = new DateTime(GetCurrentDateTime().Year, clo_site_type, pay_day);
                        return paymentTime;
                    }

                }
            }

            return paymentTime;
        }

        /// <summary>
        /// Count billing months
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="closingDay"></param>
        /// <returns></returns>
        public static int CountBillingMonths(DateTime startDate, DateTime endDate, int closingDay) 
        {
            var countYM = 0;
            for (var tmpDate = GetFirstClosingDate(startDate, startDate, closingDay);
                tmpDate < GetSameDayAddMonths(endDate.Year, endDate.Month, endDate.Day, 1);
                tmpDate = GetSameDayAddMonths(tmpDate.Year, tmpDate.Month, tmpDate.Day, 1))
            {
                countYM++;
                if (tmpDate >= endDate)
                {
                    break;
                }
            }
            return countYM;
        }

        /// <summary>
        /// Get the same day in the future months
        /// </summary>
        /// <param name="currentDate"></param>
        /// <param name="addMonths"></param>
        /// <returns></returns>
        public static DateTime GetSameDayAddMonths(int year, int month, int day, int addMonths = 0)
        {
            if (day == 0)
            {
                day = 1;
            }
            if (day >= DateTime.DaysInMonth(year, month))
            {
                return new DateTime(year, month, 1).AddMonths(addMonths + 1).AddDays(-1); // End of month
            }
            return new DateTime(year, month, day).AddMonths(addMonths);
        }

        /// <summary>
        /// Get number of days for［サイト］field on email content.
        /// </summary>
        /// <returns></returns>
        public static int GetNumberOfDays(int paymentDay, int closingDay, string paymentSiteType)
        {
            int paymentDayValue = paymentDay == Convert.ToInt32(PaymentDayType.Day31) ? PaymentDayType.StandardValue : paymentDay;
            int closingDayValue = closingDay == Convert.ToInt32(ClosingDayType.Day31) ? ClosingDayType.StandardValue : closingDay;
            int paymentSiteTypeValue = Convert.ToInt32(paymentSiteType) <= Convert.ToInt32(PaymentSiteType.ThisMonth)
                ? Convert.ToInt32(PaymentSiteType.None)
                : (Convert.ToInt32(paymentSiteType) - 1) * 30;
            return paymentDayValue - closingDayValue + paymentSiteTypeValue;
        }

        /// <summary>
        /// Get first closing date
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="closingDay"></param>
        /// <returns></returns>
        public static DateTime GetFirstClosingDate(DateTime startDate, DateTime endDate, int closingDay)
        {
            DateTime firstClosingDate;
            if (closingDay == Convert.ToInt32(Constants.ClosingDay.Day31))
            {
                firstClosingDate = new DateTime(startDate.Year, startDate.Month, 1).AddMonths(1).AddDays(-1);
            }
            else if (closingDay >= startDate.Day)
            {
                firstClosingDate = new DateTime(startDate.Year, startDate.Month, closingDay);
            }
            else // closingDay < startDate.Day
            {
                firstClosingDate = new DateTime(startDate.Year, startDate.Month, closingDay).AddMonths(1);
            }
            return firstClosingDate;
        }
    }
}

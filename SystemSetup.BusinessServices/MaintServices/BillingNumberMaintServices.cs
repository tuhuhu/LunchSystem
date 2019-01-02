using SystemSetup.DataAccess;
using SystemSetup.DataAccess.Common;
using SystemSetup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SystemSetup.UtilityServices;

namespace SystemSetup.BusinessServices
{
    public class BillingNumberMaintServices : BaseServices
    {
        #region READ

        /// <summary>
        /// Billing Number Maint Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<BillingNumberMaintEntityPlus> BillingNumberMaintSearch(DataTablesModel dt, ref BillingNumberMaintModel searchCondition, out int totalrow)
        {
            // Declare new DataAccess object
            BillingNumberMaintDa dataAccess = new BillingNumberMaintDa();
            DateTime dtNow = Utility.GetCurrentDateOnly();
            string yearMonth = dtNow.ToString("yyyyMM");
            string prevYearMonth = dtNow.AddMonths(-1).ToString("yyyyMM");
            string nextYearMonth = dtNow.AddMonths(1).ToString("yyyyMM");
            IEnumerable<BillingNumberMaintEntityPlus> results = dataAccess.BillingNumberMaintSearch(dt, ref searchCondition, out totalrow);
            foreach (var result in results)
            {
                result.BILLING_NUMBER_DATA_THIS_MONTH = dataAccess.GetBillingRowCountByYearMonth(result.COMPANY_CD, yearMonth);
                result.BILLING_NUMBER_DATA_PREV_MONTH = dataAccess.GetBillingRowCountByYearMonth(result.COMPANY_CD, prevYearMonth);
                result.BILLING_NUMBER_DATA_NEXT_MONTH = dataAccess.GetBillingRowCountByYearMonth(result.COMPANY_CD, nextYearMonth);
                result.FILE_SIZE_TOTAL = dataAccess.GetFileSizeTotalbyCompany(result.COMPANY_CD);
            }
            if (results == null)
            {
                base.CmnEntityModel.ErrorMsgCd = Constants.MessageCd.W0015;
            }
            else
            {
                base.CmnEntityModel.ErrorMsgCd = string.Empty;
            }

            return results;
        }

        /// <summary>
        /// Get Billing Number Data To Export
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <returns></returns>
        public IEnumerable<BillingNumberMaintEntityPlus> GetBillingNumberData(BillingNumberMaintModel searchCondition)
        {
            // Declare new DataAccess object
            BillingNumberMaintDa dataAccess = new BillingNumberMaintDa();
            DateTime dtNow = Utility.GetCurrentDateOnly();
            string yearMonth = dtNow.ToString("yyyyMM");
            string prevYearMonth = dtNow.AddMonths(-1).ToString("yyyyMM");
            string nextYearMonth = dtNow.AddMonths(1).ToString("yyyyMM");
            IEnumerable<BillingNumberMaintEntityPlus> results = dataAccess.GetBillingNumberData(searchCondition);
            foreach (var result in results)
            {
                result.BILLING_NUMBER_DATA_THIS_MONTH = dataAccess.GetBillingRowCountByYearMonth(result.COMPANY_CD, yearMonth);
                result.BILLING_NUMBER_DATA_PREV_MONTH = dataAccess.GetBillingRowCountByYearMonth(result.COMPANY_CD, prevYearMonth);
                result.BILLING_NUMBER_DATA_NEXT_MONTH = dataAccess.GetBillingRowCountByYearMonth(result.COMPANY_CD, nextYearMonth);
                result.FILE_SIZE_TOTAL = dataAccess.GetFileSizeTotalbyCompany(result.COMPANY_CD);
            }
            if (results == null)
            {
                base.CmnEntityModel.ErrorMsgCd = Constants.MessageCd.W0015;
            }
            else
            {
                base.CmnEntityModel.ErrorMsgCd = string.Empty;
            }

            return results;
        }
        #endregion
    }
}

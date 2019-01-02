using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemSetup.Models;
using SystemSetup.DataAccess;
using System.Transactions;

namespace SystemSetup.BusinessServices
{
    public class TaxRateMaintServices : BaseServices
    {
        #region READ
        /// <summary>
        /// Post Information Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<TaxRateMaintModel> TaxRateMaintSearch(DataTablesModel dt, ref TaxRateMaintModel searchCondition, out int totalrow)
        {
            // Declare new DataAccess object
            TaxRateMaintDa dataAccess = new TaxRateMaintDa();
            IEnumerable<TaxRateMaintModel> results = dataAccess.TaxRateMaintSearch(dt, ref searchCondition, out totalrow);

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
        /// Get a row from Tbl_Information based on INFO_SEQ_NO
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public TaxRateMaintModel GetTaxRateMaint(long TaxRateID)
        {
            // Declare new DataAccess object
            TaxRateMaintDa dataAccess = new TaxRateMaintDa();
            //
            TaxRateMaintModel result = dataAccess.GetInformation(TaxRateID);

            base.CmnEntityModel.ErrorMsgCd = (result == null) ? Constants.MessageCd.W0015 : String.Empty;
            return result;
        }

        #endregion

        #region UPDATE
        public long UpdateTaxRateMaint(TaxRateEntity model)
        {
            long result = 0;
            // Declare new DataAccess object
            TaxRateMaintDa dataAccess = new TaxRateMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.UpdateTaxRateMaintModel(model);

                if (result > 0)
                    transaction.Complete();
            }

            if (result == 0)
            {
                base.CmnEntityModel.ErrorMsgCd = Constants.MessageCd.W0015;
            }
            else
            {
                base.CmnEntityModel.ErrorMsgCd = string.Empty;
            }

            return result;
        }
        #endregion

        #region DELETE
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SEQNO"></param>
        /// <returns></returns>
        public int DeleteTaxRateMaint(int TAX_RATE_ID = 0)
        {
            int result = 0;
            // Declare new DataAccess object
            TaxRateMaintDa dataAccess = new TaxRateMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.DeleteTaxRateMaint(TAX_RATE_ID);

                if (result > 0)
                    transaction.Complete();
            }

            if (result == 0)
            {
                base.CmnEntityModel.ErrorMsgCd = Constants.MessageCd.W0015;
            }
            else
            {
                base.CmnEntityModel.ErrorMsgCd = string.Empty;
            }

            return result;
        }
        #endregion

        #region CREATE
        /// <summary>
        /// Insert into Tbl_Information
        /// </summary>
        /// <param name="Maint"></param>
        /// <returns></returns>
        public long InsertTaxRateMaint(TaxRateEntity Maint)
        {
            long result = 0;
            // Declare new DataAccess object
            TaxRateMaintDa dataAccess = new TaxRateMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.InsertTaxRateMaint(Maint);

                if (result > 0)
                    transaction.Complete();
            }

            if (result == 0)
            {
                base.CmnEntityModel.ErrorMsgCd = Constants.MessageCd.W0015;
            }
            else
            {
                base.CmnEntityModel.ErrorMsgCd = string.Empty;
            }

            return result;
        }
        #endregion

        #region CheckDate
        /// <summary>
        /// TaxRateMaintCheckDate
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public bool TaxRateMaintCheckDate(TaxRateEntity searchCondition)
        {
            // Declare new DataAccess object
            TaxRateMaintDa dataAccess = new TaxRateMaintDa();
            IEnumerable<TaxRateMaintModel> results = dataAccess.TaxRateMaintCheckDate(searchCondition);

            if (results == null)
            {
                base.CmnEntityModel.ErrorMsgCd = Constants.MessageCd.W0015;
            }
            else
            {
                base.CmnEntityModel.ErrorMsgCd = string.Empty;
                if (results.Count() == 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}

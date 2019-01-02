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
    public class AccountingSubjectMaintServices : BaseServices
    {
        #region READ
        /// <summary>
        /// Post Information Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<AccountingSubjectMaintModel> AccountingSubjectMaintSearch(DataTablesModel dt, ref AccountingSubjectMaintModel searchCondition, out int totalrow)
        {
            // Declare new DataAccess object
            AccountingSubjectMaintDa dataAccess = new AccountingSubjectMaintDa();
            IEnumerable<AccountingSubjectMaintModel> results = dataAccess.AccountingSubjectMaintSearch(dt, ref searchCondition, out totalrow);

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
        public AccountingSubjectMaintModel GetAccountingSubjectMaint(string accountCd)
        {
            // Declare new DataAccess object
            AccountingSubjectMaintDa dataAccess = new AccountingSubjectMaintDa();
            //
            AccountingSubjectMaintModel result = dataAccess.GetInformation(accountCd);

            base.CmnEntityModel.ErrorMsgCd = (result == null) ? Constants.MessageCd.W0015 : String.Empty;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public AccountingSubjectMaintModel NewAccountingSubjectMaint(AccountingSubjectMaintModel model)
        {
            AccountingSubjectMaintModel result = new AccountingSubjectMaintModel();

            result.ACCOUNT_GROUP_SEQ_NO = (long)model.SEARCH_ACCOUNT_GROUP_SEQ_NO;

            return result;
        }

        /// <summary>
        ///   GetContractCompany
        /// </summary>
        /// <returns></returns>
        public IList<AccountingSubjectMaintModel> GetContractCompany(string accountCd)
        {
            // Declare new DataAccess object
            AccountingSubjectMaintDa dataAccess = new AccountingSubjectMaintDa();
            IList<AccountingSubjectMaintModel> results = dataAccess.GetContractCompany(accountCd);
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

        #region UPDATE
        public long UpdateAccountingSubjectMaint(AccountCodeEntity model,string BEFORE_JIS_ACCOUNT_CD)
        {
            long result = 0;
            // Declare new DataAccess object
            AccountingSubjectMaintDa dataAccess = new AccountingSubjectMaintDa();

            using (var transaction = new TransactionScope())
            {
                result = dataAccess.UpdateAccountingSubjectMaintModel(model);
                // Update issue flag            
                if (result > 0)
                {
                    transaction.Complete();
                }
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
        /// <param name="ACCOUNT_CD"></param>
        /// <returns></returns>
        public int DeleteAccountingSubjectMaint(string ACCOUNT_CD)
        {
            int result = 0;
            // Declare new DataAccess object
            AccountingSubjectMaintDa dataAccess = new AccountingSubjectMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.DeleteAccountingSubjectMaint(ACCOUNT_CD);

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
        public long InsertAccountingSubjectMaint(AccountCodeEntity Maint)
        {
            long result = 0;
            // Declare new DataAccess object
            AccountingSubjectMaintDa dataAccess = new AccountingSubjectMaintDa();

            using (var transaction = new TransactionScope())
            {
                result = dataAccess.InsertAccountingSubjectMaint(Maint);
                if (result > 0)
                {
                    transaction.Complete();
                }
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

        /// <summary>
        /// Check exist ACCOUNT_CD
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool CheckExistAccount(AccountCodeEntity Entity)
        {
            AccountingSubjectMaintDa dataAccess = new AccountingSubjectMaintDa();
            return dataAccess.CheckAccount(Entity);
        }

        #region DeleteBeforeCheck
        /// <summary>
        /// DeleteBeforeCheck
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public bool DeleteBeforeCheck(string ACCOUNT_CD)
        {
            AccountingSubjectMaintDa dataAccess = new AccountingSubjectMaintDa();
            return dataAccess.DeleteBeforeCheck(ACCOUNT_CD);
        }
        #endregion
    }
}

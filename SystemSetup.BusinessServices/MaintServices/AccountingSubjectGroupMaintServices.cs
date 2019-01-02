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
    public class AccountingSubjectGroupMaintServices : BaseServices
    {
        #region READ
        /// <summary>
        /// Post Information Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<AccountingSubjectGroupMaintModel> AccountingSubjectGroupMaintSearch(DataTablesModel dt, ref AccountingSubjectGroupMaintModel searchCondition, out int totalrow)
        {
            // Declare new DataAccess object
            AccountingSubjectGroupMaintDa dataAccess = new AccountingSubjectGroupMaintDa();
            IEnumerable<AccountingSubjectGroupMaintModel> results = dataAccess.AccountingSubjectGroupMaintSearch(dt, ref searchCondition, out totalrow);

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
        public AccountingSubjectGroupMaintModel GetAccountingSubjectGroupMaint(String infoSeqNo)
        {
             //Declare new DataAccess object
            AccountingSubjectGroupMaintDa dataAccess = new AccountingSubjectGroupMaintDa();
            AccountingSubjectGroupMaintModel result = dataAccess.GetInformation(infoSeqNo);
            base.CmnEntityModel.ErrorMsgCd = (result == null) ? Constants.MessageCd.W0015 : String.Empty;
            return result;
        }



        #endregion

        #region UPDATE
        public long UpdateAccountingSubjectGroupMaint(AccountGroupEntity model)
        {
            long result = 0;
            // Declare new DataAccess object
            AccountingSubjectGroupMaintDa dataAccess = new AccountingSubjectGroupMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.UpdateAccountingSubjectGroupMaintModel(model);

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
        public int DeleteAccountingSubjectGroupMaint(String infoSeqNo)
        {
            int result = 0;
            // Declare new DataAccess object
            AccountingSubjectGroupMaintDa dataAccess = new AccountingSubjectGroupMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.Delete(infoSeqNo);

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
        public long InsertAccountingSubjectGroupMaint(AccountGroupEntity Maint)
        {
            long result = 0;
            // Declare new DataAccess object
            AccountingSubjectGroupMaintDa dataAccess = new AccountingSubjectGroupMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.InsertAccountingSubjectGroupMaint(Maint);

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

        /// <summary>
        ///   GetAuxiliaryCode
        /// </summary>
        /// <returns></returns>
        public IList<AccountingSubjectGroupMaintModel> GetAuxiliaryCode(string accountGroupCd, string accountGroupName)
        {
            // Declare new DataAccess object
            AccountingSubjectGroupMaintDa dataAccess = new AccountingSubjectGroupMaintDa();
            IList<AccountingSubjectGroupMaintModel> results = dataAccess.GetAuxiliaryCode(accountGroupCd, accountGroupName);
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

        #region Check exist ACCOUNT_GROUP_CD
        /// <summary>
        /// Check exist ACCOUNT_GROUP_CD
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public bool CheckExist(AccountGroupEntity model)
        {
            AccountingSubjectGroupMaintDa dataAccess = new AccountingSubjectGroupMaintDa();
            return dataAccess.CheckExist(model);
        }
        #endregion

        #region DeleteBeforeCheck
        /// <summary>
        /// DeleteBeforeCheck
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public bool DeleteBeforeCheck(String ACCOUNT_GROUP_SEQ_NO)
        {
            AccountingSubjectGroupMaintDa dataAccess = new AccountingSubjectGroupMaintDa();
            return dataAccess.DeleteBeforeCheck(ACCOUNT_GROUP_SEQ_NO);
        }
        #endregion
    }
}

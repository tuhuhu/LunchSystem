using SystemSetup.DataAccess;
using SystemSetup.DataAccess.Common;
using SystemSetup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SystemSetup.BusinessServices
{
    public class UserMaintServices : BaseServices
    {
        #region CREATE
        /// <summary>
        /// Insert into Tbl_Information
        /// </summary>
        /// <param name="UserMaintModel"></param>
        /// <returns></returns>
        public long InsertUserMaint(UserMaintEntity UserMaint)
        {
            var UserMaintDa = new UserMaintDa();

            long result = 0;
            // Declare new DataAccess object
            UserMaintDa dataAccess = new UserMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.InsertUserMaint(UserMaint);

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

        #region READ
        /// <summary>
        /// Get a row from Tbl_Information based on INFO_SEQ_NO
        /// </summary>
        /// <param name="infoSeqNo"></param>
        /// <returns></returns>
        public UserMaintModel GetUserMaint(long infoSeqNo)
        {
            // Declare new DataAccess object
            UserMaintDa dataAccess = new UserMaintDa();
            UserMaintModel result = dataAccess.GetUserMaintInformation(infoSeqNo);
            base.CmnEntityModel.ErrorMsgCd = (result == null) ? Constants.MessageCd.W0015 : String.Empty;

            result.AUTHORITY_GROUP_CD_STRING = result.AUTHORITY_GROUP_CD.ToString();
            
            return result;
        }

        /// <summary>
        /// Post Information Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<UserMaintEntityPlus> UserMaintSearch(DataTablesModel dt, ref UserMaintListModel searchCondition, out int totalrow)
        {
            // Declare new DataAccess object
            UserMaintDa dataAccess = new UserMaintDa();
            IEnumerable<UserMaintEntityPlus> results = dataAccess.UserMaintSearch(dt, ref searchCondition, out totalrow);

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
        public long UpdateUserMaint(UserMaintEntity UserMaint)
        {
            var UserMaintDa = new UserMaintDa();

            long result = 0;
            // Declare new DataAccess object
            UserMaintDa dataAccess = new UserMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.UpdateUserMaint(UserMaint);

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
        public long UpdatePassword(UserMaintEntity UserMaint)
        {
            var UserMaintDa = new UserMaintDa();

            long result = 0;
            // Declare new DataAccess object
            UserMaintDa dataAccess = new UserMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.UpdatePassword(UserMaint);

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
        public int DeleteUserMaint(long SEQNO)
        {
            int result = 0;
            // Declare new DataAccess object
            UserMaintDa dataAccess = new UserMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.DeleteUserMaint(SEQNO);

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
        /// Get contract type master
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserMaintListModel> GetGroupMaster(string companycd)
        {
            // Declare new DataAccess object
            UserMaintDa dataAccess = new UserMaintDa();
            IEnumerable<UserMaintListModel> results;
            // Get PIC master            
            results = dataAccess.GetListGroupWithChilds(companycd);

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
        ///   GetAuxiliaryCode
        /// </summary>
        /// <returns></returns>
        public IList<UserMaintModel> GetAuxiliaryCode(string companyCd, string SeqNo, string Account, string Del)
        {
            // Declare new DataAccess object
            UserMaintDa dataAccess = new UserMaintDa();
            IList<UserMaintModel> results = dataAccess.GetAuxiliaryCode(companyCd, SeqNo, Account, Del);
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
    }
}

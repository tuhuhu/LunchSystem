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
    public class AuthorityGroupMaintServices : BaseServices
    {
        #region READ
        /// <summary>
        /// Post Information Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<AuthorityGroupMaintModel> AuthorityGroupMaintSearch(DataTablesModel dt, ref AuthorityGroupMaintModel searchCondition, out int totalrow)
        {
            // Declare new DataAccess object
            AuthorityGroupMaintDa dataAccess = new AuthorityGroupMaintDa();
            IEnumerable<AuthorityGroupMaintModel> results = dataAccess.AuthorityGroupMaintSearch(dt, ref searchCondition, out totalrow);

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
        public AuthorityGroupMaintModel GetAuthorityGroupMaint(string companyCd, int AuthorityGroupCd)
        {
            // Declare new DataAccess object
            AuthorityGroupMaintDa dataAccess = new AuthorityGroupMaintDa();
            //
            AuthorityGroupMaintModel result = dataAccess.GetInformation(companyCd, AuthorityGroupCd);

            base.CmnEntityModel.ErrorMsgCd = (result == null) ? Constants.MessageCd.W0015 : String.Empty;
            return result;
        }

        /// <summary>
        /// Get contract firm master
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractFirmEntity> GetContractFirmByContractTypeLevelExceptZero()
        {
            // Declare new DataAccess object
            AuthorityGroupMaintDa dataAccess = new AuthorityGroupMaintDa();
            IEnumerable<ContractFirmEntity> results;
            // Get PIC master            
            results = dataAccess.GetContractFirmByContractTypeLevelExceptZero();

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
        public long UpdateAuthorityGroupMaint(AuthorityGroupMaintModel model)
        {
            long result = 0;
            // Declare new DataAccess object
            AuthorityGroupMaintDa dataAccess = new AuthorityGroupMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.UpdateAuthorityGroupMaintModel(model);

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
        public int DeleteAuthorityGroupMaint(string COMPANY_CD, int AUTHORITY_GROUP_CD)
        {
            int result = 0;
            // Declare new DataAccess object
            AuthorityGroupMaintDa dataAccess = new AuthorityGroupMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.DeleteAuthorityGroupMaint(COMPANY_CD, AUTHORITY_GROUP_CD);

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
        public long InsertAuthorityGroupMaint(AuthorityGroupMaintModel Maint)
        {
            long result = 0;
            // Declare new DataAccess object
            AuthorityGroupMaintDa dataAccess = new AuthorityGroupMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.InsertAuthorityGroupMaint(Maint);

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

        #region DeleteBeforeCheck
        /// <summary>
        /// DeleteBeforeCheck
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public bool DeleteBeforeCheck(string COMPANY_CD, int AUTHORITY_GROUP_CD)
        {
            AuthorityGroupMaintDa dataAccess = new AuthorityGroupMaintDa();
            return dataAccess.DeleteBeforeCheck(COMPANY_CD, AUTHORITY_GROUP_CD);
        }
        #endregion
    }
}

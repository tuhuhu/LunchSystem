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
    public class SubCategoryMaintServices : BaseServices
    {
        #region READ
        /// <summary>
        /// Post Information Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<SubCategoryMaintModel> SubCategoryMaintSearch(DataTablesModel dt, ref SubCategoryMaintModel searchCondition, out int totalrow)
        {
            // Declare new DataAccess object
            SubCategoryMaintDa dataAccess = new SubCategoryMaintDa();
            IEnumerable<SubCategoryMaintModel> results = dataAccess.SubCategoryMaintSearch(dt, ref searchCondition, out totalrow);

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
        public SubCategoryMaintModel GetSubCategoryMaint(string companyCd, int contractType, int contractTypeClass)
        {
            // Declare new DataAccess object
            SubCategoryMaintDa dataAccess = new SubCategoryMaintDa();
            //
            SubCategoryMaintModel result = dataAccess.GetInformation(companyCd, contractType, contractTypeClass);

            base.CmnEntityModel.ErrorMsgCd = (result == null) ? Constants.MessageCd.W0015 : String.Empty;
            return result;
        }

        /// <summary>
        ///   GetSubCategory
        /// </summary>
        /// <returns></returns>
        public IList<SubCategoryMaintModel> GetSubCategory(string companyCd, int contractType, int contractTypeClass)
        {
            // Declare new DataAccess object
            SubCategoryMaintDa dataAccess = new SubCategoryMaintDa();
            IList<SubCategoryMaintModel> results = dataAccess.GetSubCategory(companyCd, contractType, contractTypeClass);
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
        /// Get contract firm master
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractFirmEntity> GetContractFirmByContractTypeLevelExceptZero()
        {
            // Declare new DataAccess object
            SubCategoryMaintDa dataAccess = new SubCategoryMaintDa();
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

        /// <summary>
        /// Get contract firm master
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractFirmEntity> GetContractFirmMasterByOutside()
        {
            // Declare new DataAccess object
            SubCategoryMaintDa dataAccess = new SubCategoryMaintDa();
            IEnumerable<ContractFirmEntity> results;
            // Get PIC master            
            results = dataAccess.GetContractFirmMasterByOutside();

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
        public long UpdateSubCategoryMaint(FirmContractClassEntity model)
        {
            long result = 0;
            // Declare new DataAccess object
            SubCategoryMaintDa dataAccess = new SubCategoryMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.UpdateSubCategoryMaintModel(model);

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
        public int DeleteSubCategoryMaint(string COMPANY_CD, int CONTRACT_TYPE, int CONTRACT_TYPE_CLASS)
        {
            int result = 0;
            // Declare new DataAccess object
            SubCategoryMaintDa dataAccess = new SubCategoryMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.DeleteSubCategoryMaint(COMPANY_CD, CONTRACT_TYPE, CONTRACT_TYPE_CLASS);

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
        public long InsertSubCategoryMaint(FirmContractClassEntity Maint)
        {
            long result = 0;
            // Declare new DataAccess object
            SubCategoryMaintDa dataAccess = new SubCategoryMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.InsertSubCategoryMaint(Maint);

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
    }
}

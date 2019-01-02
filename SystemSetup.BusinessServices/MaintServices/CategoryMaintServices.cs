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
    public class CategoryMaintServices : BaseServices
    {
        #region READ
        /// <summary>
        /// Post Information Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<CategoryMaintModel> CategoryMaintSearch(DataTablesModel dt, ref CategoryMaintModel searchCondition, out int totalrow)
        {
            // Declare new DataAccess object
            CategoryMaintDa dataAccess = new CategoryMaintDa();
            IEnumerable<CategoryMaintModel> results = dataAccess.CategoryMaintSearch(dt, ref searchCondition, out totalrow);

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
        public CategoryMaintModel GetCategoryMaint(string companyCd, int contractType)
        {
            // Declare new DataAccess object
            CategoryMaintDa dataAccess = new CategoryMaintDa();
            //
            CategoryMaintModel result = dataAccess.GetInformation(companyCd, contractType);

            base.CmnEntityModel.ErrorMsgCd = (result == null) ? Constants.MessageCd.W0015 : String.Empty;
            return result;
        }

        public string GetContractLevelType(string companyCd)
        {
            string result = "";
            // Declare new DataAccess object
            CategoryMaintDa dataAccess = new CategoryMaintDa();

            CategoryMaintModel model = dataAccess.GetContractLevelType(companyCd);
            result = model.CONTRACT_LEVEL_TYPE;

            return result;
        }
            
        #endregion

        #region UPDATE
        public long UpdateCategoryMaint(CategoryMaintModel model)
        {
            long result = 0;
            // Declare new DataAccess object
            CategoryMaintDa dataAccess = new CategoryMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.UpdateCategoryMaintModel(model);

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
        public int DeleteCategoryMaint(string COMPANY_CD, int CONTRACT_TYPE, string CONTRACT_LEVEL_TYPE)
        {
            int result = 0;
            // Declare new DataAccess object
            CategoryMaintDa dataAccess = new CategoryMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.DeleteCategoryMaint(COMPANY_CD, CONTRACT_TYPE, CONTRACT_LEVEL_TYPE);

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
        public long InsertCategoryMaint(CategoryMaintModel model)
        {
            long result = 0;
            // Declare new DataAccess object
            CategoryMaintDa dataAccess = new CategoryMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.InsertCategoryMaint(model);

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

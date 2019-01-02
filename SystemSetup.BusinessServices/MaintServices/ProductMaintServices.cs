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
    public class ProductMaintServices : BaseServices
    {
        #region READ
        /// <summary>
        /// Post Information Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<ProductMaintModel> ProductMaintSearch(DataTablesModel dt, ref ProductMaintModel searchCondition, out int totalrow)
        {
            // Declare new DataAccess object
            ProductMaintDa dataAccess = new ProductMaintDa();
            IEnumerable<ProductMaintModel> results = dataAccess.ProductMaintSearch(dt, ref searchCondition, out totalrow);

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
        public ProductMaintModel GetProductMaint(long productSeqNo)
        {
            // Declare new DataAccess object
            ProductMaintDa dataAccess = new ProductMaintDa();
            //
            ProductMaintModel result = dataAccess.GetInformation(productSeqNo);

            base.CmnEntityModel.ErrorMsgCd = (result == null) ? Constants.MessageCd.W0015 : String.Empty;
            return result;
        }

        /// <summary>
        ///   GetProduct
        /// </summary>
        /// <returns></returns>
        public IList<ProductMaintModel> GetProduct(string companyCd, int contractType, int contractTypeClass, string productName)
        {
            // Declare new DataAccess object
            ProductMaintDa dataAccess = new ProductMaintDa();
            IList<ProductMaintModel> results = dataAccess.GetProduct(companyCd, contractType, contractTypeClass, productName);
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


        #endregion

        #region UPDATE
        public long UpdateProductMaint(ProductEntity model)
        {
            long result = 0;
            // Declare new DataAccess object
            ProductMaintDa dataAccess = new ProductMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.UpdateProductMaintModel(model);

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
        public int DeleteProductMaint(long PRODUCT_SEQ_NO)
        {
            int result = 0;
            // Declare new DataAccess object
            ProductMaintDa dataAccess = new ProductMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.DeleteProductMaint(PRODUCT_SEQ_NO);

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
        public long InsertProductMaint(ProductEntity Maint)
        {
            long result = 0;
            // Declare new DataAccess object
            ProductMaintDa dataAccess = new ProductMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.InsertProductMaint(Maint);

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

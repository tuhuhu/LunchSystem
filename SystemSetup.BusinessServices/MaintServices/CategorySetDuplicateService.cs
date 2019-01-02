using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SystemSetup.DataAccess;
using SystemSetup.Models;

namespace SystemSetup.BusinessServices
{
    public class CategorySetDuplicateService: BaseServices
    {
        #region READ
        /// <summary>
        /// Get Source Company Code
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractFirmEntity> GetSourceCompanyCode()
        {
            // Declare new DataAccess object
            CategorySetDuplicateDa dataAccess = new CategorySetDuplicateDa();
            IEnumerable<ContractFirmEntity> results;
            // Get PIC master            
            results = dataAccess.GetSourceCompanyCode();

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
        /// Get Destination Company Code
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractFirmEntity> GetDestinationCompanyCode()
        {
            // Declare new DataAccess object
            CategorySetDuplicateDa dataAccess = new CategorySetDuplicateDa();
            IEnumerable<ContractFirmEntity> results;
            // Get PIC master            
            results = dataAccess.GetDestinationCompanyCode();

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

        #region DELETE
        public int Delete(string COMPANY_CD)
        {
            var result = 0;
            // Declare new DataAccess object
            CategorySetDuplicateDa dataAccess = new CategorySetDuplicateDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                int result1 = dataAccess.DeleteFirmContractConfig(COMPANY_CD);
                int result2 = dataAccess.DeleteFirmContractClass(COMPANY_CD);
                int result3 = dataAccess.DeleteProduct(COMPANY_CD);
                result = result1 > 0 ? result1 : result2 > 0 ? result2 : result3 > 0 ? result3 : 0;
                if (result > 0)
                    transaction.Complete();
            }
            
            return result;
        }
        #endregion

        #region COPY DUPLICATE
        public long CopyDuplicate(CategorySetDuplicateModel model)
        {
            long result = 0;
            CategorySetDuplicateDa dataAccess = new CategorySetDuplicateDa();

            IList<FirmContractConfigEntity> FIRM_CONTRACT_CONFIG = dataAccess.GetFirmContractConfigList(model.SOURCE_COMPANY_CODE);
            IList<FirmContractClassEntity> FIRM_CONTRACT_CLASS = dataAccess.GetFirmContractClassList(model.SOURCE_COMPANY_CODE);
            IList<ProductEntity> PRODUCT_LIST = dataAccess.GetProductList(model.SOURCE_COMPANY_CODE);
            foreach (var data in FIRM_CONTRACT_CONFIG)
            {
                data.COMPANY_CD = model.DESTINATION_COMPANY_CODE;
                data.DEL_FLG = model.DEL_FLG;
                data.INS_DATE = model.INS_DATE;
                data.INS_USER_ID = Constants.Constant.DEFAULT_USER_ID;
                data.UPD_DATE = model.UPD_DATE;
                data.UPD_USER_ID = Constants.Constant.DEFAULT_USER_ID;
                data.UPD_PROG_ID = model.UPD_PROG_ID;

                result = dataAccess.CopyFirmContractConfig(data);
            }

            foreach (var data in FIRM_CONTRACT_CLASS)
            {
                data.COMPANY_CD = model.DESTINATION_COMPANY_CODE;
                data.DEL_FLG = model.DEL_FLG;
                data.INS_DATE = model.INS_DATE;
                data.INS_USER_ID = Constants.Constant.DEFAULT_USER_ID;
                data.UPD_DATE = model.UPD_DATE;
                data.UPD_USER_ID = Constants.Constant.DEFAULT_USER_ID;
                data.UPD_PROG_ID = model.UPD_PROG_ID;

                result = dataAccess.CopyFirmContractClass(data);
            }

            foreach (var data in PRODUCT_LIST)
            {
                data.COMPANY_CD = model.DESTINATION_COMPANY_CODE;
                data.DEL_FLG = model.DEL_FLG;
                data.INS_DATE = model.INS_DATE;
                data.INS_USER_ID = Constants.Constant.DEFAULT_USER_ID;
                data.UPD_DATE = model.UPD_DATE;
                data.UPD_USER_ID = Constants.Constant.DEFAULT_USER_ID;
                data.UPD_PROG_ID = model.UPD_PROG_ID;

                result = dataAccess.CopyProduct(data);
            }

            return result;

        }
        #endregion
    }
}

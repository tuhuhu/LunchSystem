using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemSetup.Models;
using SystemSetup.DataAccess;
using System.Transactions;
using SystemSetup.UtilityServices.SafePassword;

namespace SystemSetup.BusinessServices
{
    public class ContractCompanyMaintServices : BaseServices
    {
        #region READ
        /// <summary>
        /// Post Information Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<ContractCompanyMaintModel> ContractCompanyMaintSearch(DataTablesModel dt, ref ContractCompanyMaintModel searchCondition, out int totalrow)
        {
            // Declare new DataAccess object
            ContractCompanyMaintDa dataAccess = new ContractCompanyMaintDa();
            IEnumerable<ContractCompanyMaintModel> results = dataAccess.ContractCompanyMaintSearch(dt, ref searchCondition, out totalrow);

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
        public ContractCompanyMaintModel GetContractCompanyMaint(string companyCd)
        {
            // Declare new DataAccess object
            ContractCompanyMaintDa dataAccess = new ContractCompanyMaintDa();
            //
            ContractCompanyMaintModel result = dataAccess.GetInformation(companyCd);

            base.CmnEntityModel.ErrorMsgCd = (result == null) ? Constants.MessageCd.W0015 : String.Empty;
            return result;
        }

        /// <summary>
        ///   GetContractCompany
        /// </summary>
        /// <returns></returns>
        public IList<ContractCompanyMaintModel> GetContractCompany(string companyCd)
        {
            // Declare new DataAccess object
            ContractCompanyMaintDa dataAccess = new ContractCompanyMaintDa();
            IList<ContractCompanyMaintModel> results = dataAccess.GetContractCompany(companyCd);
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
        public long UpdateContractCompanyMaint(ContractFirmEntity model)
        {
            long result = 0;
            // Declare new DataAccess object
            ContractCompanyMaintDa dataAccess = new ContractCompanyMaintDa();

            model.BILL_FORMAT_PATH = string.IsNullOrEmpty(model.BILL_FORMAT_PATH) ? "" : model.BILL_FORMAT_PATH.Trim();
            model.BILL_FORMAT_SES_PATH = string.IsNullOrEmpty(model.BILL_FORMAT_SES_PATH) ? "" : model.BILL_FORMAT_SES_PATH.Trim();
            model.BILL_FORMAT_TEMP_PATH = string.IsNullOrEmpty(model.BILL_FORMAT_TEMP_PATH) ? "" : model.BILL_FORMAT_TEMP_PATH.Trim();

            if (!model.UPLOAD_FILE_PASSWORD.Equals(Constants.Constant.DISPLAY_PASSWORD))
            {
                if (!String.IsNullOrEmpty(model.UPLOAD_FILE_PASSWORD))
                {
                    model.UPLOAD_FILE_PASSWORD = SafePassword.GetSaltedPassword(model.UPLOAD_FILE_PASSWORD);
                }
            }

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.UpdateContractCompanyMaintModel(model);

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

        public long UpdateRelContractFirm(ContractFirmEntity model)
        {
            long result = 0;
            // Declare new DataAccess object
            ContractCompanyMaintDa dataAccess = new ContractCompanyMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.UpdateRelContractFirm(model);

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
        public int DeleteContractCompanyMaint(string COMPANY_CD)
        {
            int result = 0;
            // Declare new DataAccess object
            ContractCompanyMaintDa dataAccess = new ContractCompanyMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.DeleteContractCompanyMaint(COMPANY_CD);

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
        /// <param name="model"></param>
        /// <returns></returns>
        public long InsertContractCompanyMaint(ContractFirmEntity model)
        {
            long result = 0;
            long result2 = 0;
            // Declare new DataAccess object
            ContractCompanyMaintDa dataAccess = new ContractCompanyMaintDa();

            model.BILL_FORMAT_PATH = string.IsNullOrEmpty(model.BILL_FORMAT_PATH) ? "" : model.BILL_FORMAT_PATH.Trim();
            model.BILL_FORMAT_SES_PATH = string.IsNullOrEmpty(model.BILL_FORMAT_SES_PATH) ? "" : model.BILL_FORMAT_SES_PATH.Trim();
            model.BILL_FORMAT_TEMP_PATH = string.IsNullOrEmpty(model.BILL_FORMAT_TEMP_PATH) ? "" : model.BILL_FORMAT_TEMP_PATH.Trim();
            if (!String.IsNullOrEmpty(model.UPLOAD_FILE_PASSWORD))
            {
                model.UPLOAD_FILE_PASSWORD = SafePassword.GetSaltedPassword(model.UPLOAD_FILE_PASSWORD);
            }
            using (var transaction = new TransactionScope())
            {
                result = dataAccess.InsertContractCompanyMaint(model);
                result2 = dataAccess.InsertBusinessPartner(model);

                if (result > 0 && result2 > 0)
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
        /// Check exist COMPANY_CD
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool CheckExistCustomer(ContractFirmEntity Entity)
        {
            ContractCompanyMaintDa dataAccess = new ContractCompanyMaintDa();
            return dataAccess.CheckExistCustomer(Entity);
        }

        /// <summary>
        /// Get current files password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetCurrentFilePassword(string companyCd)
        {
            ContractCompanyMaintDa dataAccess = new ContractCompanyMaintDa();
            return dataAccess.GetCurrentFilePassword(companyCd);
        }
    
    
    }
}

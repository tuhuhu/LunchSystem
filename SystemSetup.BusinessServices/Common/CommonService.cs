//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/03
// Comment		: Create new
//------------------------------------------------------------------------

namespace SystemSetup.BusinessServices
{
    using SystemSetup.Constants;
    using SystemSetup.DataAccess.Common;
    using SystemSetup.Models;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Resources;
    using System.Text;
    using System.Web.Mvc;

	public class CommonService : BaseServices
	{
        /// <summary>
		/// Get Resource Data
		/// </summary>
		/// <param name="resourceName">resource Name</param>
		/// <returns>
		/// List [SelectListItem]
		/// </returns>
		public static List<SelectListItem> GetResourceData(string resourceName)
		{
			List<SelectListItem> resourcedata = new List<SelectListItem>();
			resourcedata.Add(new SelectListItem());
			try
			{
				ResourceManager managerResource = new ResourceManager(resourceName, typeof(LogDiv).Assembly);
				ResourceSet resourceSet = managerResource.GetResourceSet(CultureInfo.CurrentCulture, true, true);

				foreach (DictionaryEntry entry in resourceSet)
				{
					resourcedata.Add(new SelectListItem() { Text = entry.Value.ToString(), Value = entry.Key.ToString() });
				}
			}
			catch (Exception)
			{
				resourcedata = new List<SelectListItem>();
			}

			return resourcedata.OrderBy(x => x.Value).ToList();
		}

		/// <summary>
		/// Gets the resource data.
		/// </summary>
		/// <param name="resourceType">Type of the resource.</param>
		/// <returns>List of SelectListItem </returns>
		public static List<SelectListItem> GetResourceData(Type resourceType)
		{
			List<SelectListItem> resourcedata = new List<SelectListItem>();
			resourcedata.Add(new SelectListItem());
			try
			{
				ResourceManager managerResource = new ResourceManager(resourceType);
				ResourceSet resourceSet = managerResource.GetResourceSet(CultureInfo.CurrentCulture, true, true);
				foreach (DictionaryEntry entry in resourceSet)
				{
					resourcedata.Add(new SelectListItem() { Text = entry.Value.ToString(), Value = entry.Key.ToString() });
				}
			}
			catch (Exception)
			{
				resourcedata = new List<SelectListItem>();
			}
			return resourcedata.OrderBy(x => x.Value).ToList();
		}

		/// <summary>
		/// Truncates the text.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="length">The length.</param>
		/// <returns> String of truncate text</returns>
		public static string TruncateText(string input, int length)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
			StringBuilder result = new StringBuilder();
			if (input.Length <= length)
			{
				result.Append("<p>");
				result.Append(input);
				result.Append("</p>");
				return result.ToString();
			}
			else
			{
				result.Append("<p title=\"");
				result.Append(input);
				result.Append("\">");
				result.Append(input.Substring(0, length));
				result.Append("</p>");
				return result.ToString();
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemSegNo"></param>
        /// <returns></returns>
        public ItemEntity GetItemProductByItemSegNo(long itemSegNo)
        {
            // Declare new DataAccess object
            CommonDa dataAccess = new CommonDa();
            ItemEntity result = new ItemEntity();

            // Get list customer info
            result = dataAccess.GetItemProductByItemSegNo(itemSegNo);

            //Return list customer info
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accSegNo"></param>
        /// <returns></returns>
        public ContractFirmBankAccountInfoEntity GetItemBankAccountBySegNo(long accSegNo)
        {
            // Declare new DataAccess object
            CommonDa dataAccess = new CommonDa();
            ContractFirmBankAccountInfoEntity result = new ContractFirmBankAccountInfoEntity();

            // Get list customer info
            result = dataAccess.GetItemBankAccountBySegNo(accSegNo);

            //Return list customer info
            return result;
        }

        /// <summary>
        /// Get Default Contract Firm Bank Account Info
        /// </summary>
        /// <returns></returns>
        public ContractFirmBankAccountInfoEntity GetDefaultContractFirmBankAccountInfo()
        {
            CommonDa dataAccess = new CommonDa();
            return dataAccess.GetDefaultContractFirmBankAccountInfo();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="debitSegNo"></param>
        /// <returns></returns>
        public DebitAccountEntity GetDebitAccountBySegNo(long debitSegNo)
        {
            // Declare new DataAccess object
            CommonDa dataAccess = new CommonDa();
            DebitAccountEntity result = new DebitAccountEntity();

            // Get list customer info
            result = dataAccess.GetDebitAccountBySegNo(debitSegNo);

            //Return list customer info
            return result;
        }        

        /// <summary>
        /// Search Item Product
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <param name="total_row"></param>
        /// <returns></returns>
        public IEnumerable<ItemEntity> SearchItemProduct(DataTablesModel dt, ref SearchProductModel model, out int total_row)
        {
            // Declare new DataAccess object
            CommonDa dataAccess = new CommonDa();
            IEnumerable<ItemEntity> results;

            // Get list customer info
            results = dataAccess.SearchItemProduct(dt, ref model, out total_row);

            if (results == null)
            {
                base.CmnEntityModel.ErrorMsgCd = Constants.MessageCd.W0015;
            }
            else
            {
                base.CmnEntityModel.ErrorMsgCd = string.Empty;
            }

            //Return list customer info
            return results;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<ContractFirmBankAccountInfoEntity> SearchBankAccountInfo(DataTablesModel dt, out int totalrow)
        {
            // Declare new DataAccess object
            CommonDa dataAccess = new CommonDa();
            IEnumerable<ContractFirmBankAccountInfoEntity> results;

            // Get list customer info
            results = dataAccess.SearchBankAccountInfo(dt, out totalrow);

            if (results == null)
            {
                base.CmnEntityModel.ErrorMsgCd = Constants.MessageCd.W0015;
            }
            else
            {
                base.CmnEntityModel.ErrorMsgCd = string.Empty;
            }

            //Return list customer info
            return results;
        }        
        
        /// <summary>
        /// Get Contract User Name
        /// </summary>
        /// <param name="UserSegCd"></param>
        /// <returns></returns>
        public string GetContractUserName(long UserSegCd)
        {
            string username = string.Empty;
            // Declare new DataAccess object
            CommonDa dataAccess = new CommonDa();
            // Get Contract input type            
            username = dataAccess.GetContractUserName(UserSegCd);

            if (string.IsNullOrEmpty(username))
            {
                base.CmnEntityModel.ErrorMsgCd = Constants.MessageCd.W0015;
            }
            else
            {
                base.CmnEntityModel.ErrorMsgCd = String.Empty;
            }
            return username;
        }

        /// <summary>
        /// Get contract firm master
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractFirmEntity> GetContractFirmMasterByOutside()
        {
            // Declare new DataAccess object
            CommonDa dataAccess = new CommonDa();
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

        /// <summary>
        /// Get contract type master
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractTypeMasterModel> GetContractTypeMaster()
        {
            // Declare new DataAccess object
            CommonDa dataAccess = new CommonDa();
            IEnumerable<ContractTypeMasterModel> results;
            // Get PIC master            
            results = dataAccess.GetContractTypeMaster();

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
        /// Get all subcontract type in master table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractTypeMasterModel> GetSubContractTypeMaster()
        {
            CommonDa dataAccess = new CommonDa();
            IEnumerable<ContractTypeMasterModel> results;

            results = dataAccess.GetSubContractTypeMaster();

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

        #region Services for Mst_Product
        /// <summary>
        /// Get estimation file path
        /// </summary>
        /// <param name="companycd"></param>
        /// <param name="contracttype"></param>
        /// <returns></returns>
        public ProductEntity GetProductMaster(string companycd, long productSegNo)
        {
            // Declare new DataAccess object
            CommonDa dataAccess = new CommonDa();

            // Get list customer info
            var result = dataAccess.GetProductMaster(companycd, productSegNo);

            //Return list customer info
            return result;
        }

        /// <summary>
        /// Get Product by ContractType and ContractTypeClass
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="contractyTypeClass"></param>
        /// <returns></returns>
        public IEnumerable<ProductEntity> GetProductByContractType_ContractTypeClass(int contractType, int contractyTypeClass)
        {
            // Declare new DataAccess object
            CommonDa dataAccess = new CommonDa();
            IEnumerable<ProductEntity> results;
            // Get PIC master            
            results = dataAccess.GetProductByContractType_ContractTypeClass(contractType, contractyTypeClass);

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
        /// Get Product by ContractType and ContractTypeClass
        /// </summary>
        /// <param name="contractFirm"></param>
        /// <param name="contractType"></param>
        /// <param name="contractyTypeClass"></param>
        /// <returns></returns>
        public IEnumerable<ProductEntity> GetProductByContractType_ContractTypeClass_Outside(string contractFirm, int contractType, int contractyTypeClass)
        {
            // Declare new DataAccess object
            CommonDa dataAccess = new CommonDa();
            IEnumerable<ProductEntity> results;
            // Get PIC master            
            results = dataAccess.GetProductByContractType_ContractTypeClass_Outside(contractFirm, contractType, contractyTypeClass);

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
        /// Get all product in master table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductEntity> GetProductMaster()
        {
            CommonDa dataAccess = new CommonDa();
            IEnumerable<ProductEntity> results;

            results = dataAccess.GetProductMaster();

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
        /// Search Product Name
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public string SearchProductName(int contractType, int contractSubType, int productSeqNo)
        {
            // Declare new DataAccess object
            CommonDa dataAccess = new CommonDa();
            string result = string.Empty;

            // Get list customer info
            result = dataAccess.SearchProductName(contractType, contractSubType, productSeqNo);

            //Return list customer info
            return result;
        }
        #endregion

        /// <summary>
        /// Insert into Tbl_DebitAccount
        /// </summary>
        /// <param name="debitAcc"></param>
        /// <returns></returns>
        public int InsertDebitAccount(DebitAccountEntity debitAcc)
        {
            CommonDa da = new CommonDa();
            return da.InsertDebitAccount(debitAcc);
        }

        /// <summary>
        /// Get sub contract by contract type
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public IEnumerable<ContractTypeMasterModel> GetSubContractTypeByContractType(int contractType)
        {
            // Declare new DataAccess object
            CommonDa dataAccess = new CommonDa();
            IEnumerable<ContractTypeMasterModel> results;
            // Get PIC master            
            results = dataAccess.GetSubContractTypeByContractType(contractType);

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
        /// Get sub contract by contract type
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public IEnumerable<ContractTypeMasterModel> GetContractTypeSelectListByOutside(string contractFirm)
        {
            // Declare new DataAccess object
            CommonDa dataAccess = new CommonDa();
            IEnumerable<ContractTypeMasterModel> results;
            // Get PIC master            
            results = dataAccess.GetContractTypeSelectListByOutside(contractFirm);

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
        /// Get contract type master
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractTypeMasterModel> GetContractTypeMasterByOutside()
        {
            // Declare new DataAccess object
            CommonDa dataAccess = new CommonDa();
            IEnumerable<ContractTypeMasterModel> results;
            // Get PIC master            
            results = dataAccess.GetContractTypeMasterByOutside();

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
        /// Get contract type master with companycode
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractTypeMasterModel> GetContractTypeMasterByCompanycode(string companycode)
        {
            // Declare new DataAccess object
            CommonDa dataAccess = new CommonDa();
            IEnumerable<ContractTypeMasterModel> results;
            // Get PIC master            
            results = dataAccess.GetContractTypeMasterByCompanycode(companycode);

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
        /// Get all subcontract type in master table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractTypeMasterModel> GetSubContractTypeMasterByOutside()
        {
            CommonDa dataAccess = new CommonDa();
            IEnumerable<ContractTypeMasterModel> results;

            results = dataAccess.GetSubContractTypeMasterByOutside();

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
        /// Get all subcontract type in master table with companycode
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractTypeMasterModel> GetSubContractTypeMasterByOutsideCompanycode(string companycode, int contractType)
        {
            CommonDa dataAccess = new CommonDa();
            IEnumerable<ContractTypeMasterModel> results;

            results = dataAccess.GetSubContractTypeMasterByOutsideCompanycode(companycode, contractType);

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
        /// Get sub contract by contract type
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public IEnumerable<ContractTypeMasterModel> GetSubContractTypeSelectListByOutside(string contractFirm, int contractType)
        {
            // Declare new DataAccess object
            CommonDa dataAccess = new CommonDa();
            IEnumerable<ContractTypeMasterModel> results;
            // Get PIC master            
            results = dataAccess.GetSubContractTypeSelectListByOutside(contractFirm, contractType);

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
        /// Get all subcontract type in master table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductMaintModel> GetProductNameMasterByOutside()
        {
            CommonDa dataAccess = new CommonDa();
            IEnumerable<ProductMaintModel> results;

            results = dataAccess.GetProductNameMasterByOutside();

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
        /// Get all product in master table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductEntity> GetProductMasterByOutside()
        {
            CommonDa dataAccess = new CommonDa();
            IEnumerable<ProductEntity> results;

            results = dataAccess.GetProductMasterByOutside();

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
        /// Get all AccountGroup in master table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AccountGroupEntity> GetAccountGroupMasterByOutside()
        {
            CommonDa dataAccess = new CommonDa();
            IEnumerable<AccountGroupEntity> results;

            results = dataAccess.GetAccountGroupMasterByOutside();

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
        /// Get all AccountCodeJIS in master table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AccountCodeJISEntity> GetAccountCodeJISMasterByOutside()
        {
            CommonDa dataAccess = new CommonDa();
            IEnumerable<AccountCodeJISEntity> results;

            results = dataAccess.GetAccountCodeJISMasterByOutside();

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
	    /// Get Plan Master
	    /// </summary>
	    /// <returns></returns>
	    public IEnumerable<PlanMaintEntity> GetPlanMaster()
	    {
            CommonDa dataAccess = new CommonDa();
            IEnumerable<PlanMaintEntity> results;

            results = dataAccess.GetPlanMaster();

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
	    /// Insert Contract Firm Plan
	    /// </summary>
	    /// <param name="entity"></param>
	    public long InsertContractFirmPlan(ContractFirmPlanMaintEntity entity)
	    {
            CommonDa dataAccess = new CommonDa();
            long results;

            results = dataAccess.InsertContractFirmPlan(entity);

            if (results == 0)
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
        /// Delete Contract Firm Plan
        /// </summary>
        /// <param name="COMPANY_CD"></param>
        /// <returns></returns>
        public long DeleteContractFirmPlan(ContractFirmPlanMaintEntity entity)
        {
            CommonDa dataAccess = new CommonDa();
            long results;

            results = dataAccess.DeleteContractFirmPlan(entity);

            if (results == 0)
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
        /// Get Contract Firm Plan
        /// </summary>
        /// <param name="COMPANY_CD"></param>
        /// <returns></returns>
        public IEnumerable<long> GetContractFirmPlan(string COMPANY_CD)
        {
            CommonDa dataAccess = new CommonDa();
            IEnumerable<long> results;

            results = dataAccess.GetContractFirmPlan(COMPANY_CD);

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
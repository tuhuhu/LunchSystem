//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/03
// Comment		: Create new
//------------------------------------------------------------------------

namespace SystemSetup.DataAccess.Common
{
    using SystemSetup.Models;
    using SystemSetup.UtilityServices.PagingHelper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SystemSetup.UtilityServices;
    /// <summary>
	/// Common data access
	/// </summary>
	public class CommonDa : BaseDa
	{
        #region GetMasterData
        /// <summary>
        /// Get contract firm master
        /// </summary>
        /// <returns></returns>
        public ContractFirmEntity GetContractFirmMaster()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT * FROM Mst_ContractFirm WHERE COMPANY_CD = @COMPANY_CD AND DEL_FLG = @DEL_FLG ");
            return base.SingleOrDefault<ContractFirmEntity>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    COMPANY_CD = base.CmnEntityModel.CompanyCd
                });
        }                

        /// <summary>
        /// Get BAnk Acoount list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractFirmBankAccountInfoEntity> GetListBankAccount()
        {
            // Declare database connection      
            string CompanyCodeInSessiion = base.CmnEntityModel.CompanyCd;
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT * FROM Mst_ContractFirmBankAccountInfo");

            sql.Append(" WHERE COMPANY_CD = @COMPANY_CD AND DEL_FLG = @DEL_FLG");

            return base.Query<ContractFirmBankAccountInfoEntity>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    COMPANY_CD = CompanyCodeInSessiion
                }).ToList();
        }               

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemSegNo"></param>
        /// <returns></returns>
        public ItemEntity GetItemProductByItemSegNo(long itemSegNo)
        {
            // Create connection 
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT * FROM Mst_Item");

            sql.Append(" WHERE ITEM_SEQ_NO = @ITEM_SEQ_NO");
            sql.Append(" AND DEL_FLG = @DEL_FLG");

            var itemproduct = base.Query<ItemEntity>(sql.ToString(),
                new
                {
                    ITEM_SEQ_NO = itemSegNo,
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).FirstOrDefault();

            // Create variable ouput   
            return itemproduct;
        }

        /// <summary>
        /// Get bank account
        /// </summary>
        /// <param name="accSegNo"></param>
        /// <returns></returns>
        public ContractFirmBankAccountInfoEntity GetItemBankAccountBySegNo(long accSegNo)
        {
            // Create connection 
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT * FROM Mst_ContractFirmBankAccountInfo");

            sql.Append(" WHERE BANK_ACCOUNT_SEQ_NO = @BANK_ACCOUNT_SEQ_NO");
            sql.Append(" AND DEL_FLG = @DEL_FLG");

            var itemBankAccount = base.Query<ContractFirmBankAccountInfoEntity>(sql.ToString(),
                new
                {
                    BANK_ACCOUNT_SEQ_NO = accSegNo,
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).FirstOrDefault();

            // Create variable ouput   
            return itemBankAccount;
        }

        /// <summary>
        /// Get Default Contract Firm Bank Account Info
        /// </summary>
        /// <returns></returns>
        public ContractFirmBankAccountInfoEntity GetDefaultContractFirmBankAccountInfo()
        {
            string sql = @"
                SELECT *
                FROM Mst_ContractFirmBankAccountInfo
                WHERE COMPANY_CD = @COMPANY_CD
                    AND DEFAULT_ACCOUNT_FLG = @DEFAULT_ACCOUNT_FLG
                    AND DEL_FLG = @DEL_FLG";

            return base.Query<ContractFirmBankAccountInfoEntity>(sql, new
                {
                    COMPANY_CD = base.CmnEntityModel.CompanyCd,
                    DEFAULT_ACCOUNT_FLG = Constants.DefaultBankAccountFlag.On,
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accSegNo"></param>
        /// <returns></returns>
        public DebitAccountEntity GetDebitAccountBySegNo(long debitSegNo)
        {
            string sql = @"
                SELECT *
                FROM Tbl_DebitAccount
                WHERE CONT_BILLING_SEQ_NO = @CONT_BILLING_SEQ_NO
                    AND DEL_FLG = @DEL_FLG ";
            return base.Query<DebitAccountEntity>(sql, new { CONT_BILLING_SEQ_NO = debitSegNo, DEL_FLG = Constants.DeleteFlag.NON_DELETE }).FirstOrDefault();
        }        

        /// <summary>
        /// Search Item Product List
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<ItemEntity> SearchItemProduct(DataTablesModel dt, ref SearchProductModel model, out int totalrow)
        {
            string CompanyCodeInSessiion = base.CmnEntityModel.CompanyCd;
            StringBuilder sql = new StringBuilder();
            // Define row to display
            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            // SQL発行
            sql.Append(@" 
                SELECT 
                    tbItem.* 
                FROM (
                    SELECT 
                        mbp.ITEM_SEQ_NO,
                        mbp.ITEM_NO,
                        mbp.NOMEN,
                        mbp.UNIT_PRICE,
                        mbp.UNIT_TYPE                        
                    FROM
                        Mst_Item mbp                        
                    WHERE mbp.COMPANY_CD = @CompanyCode
                        AND mbp.CONTRACT_TYPE = @CONTRACT_TYPE
                        AND mbp.CONTRACT_TYPE_CLASS = @CONTRACT_TYPE_CLASS
                        AND mbp.PRODUCT_NO = @PRODUCT_NO
                        AND mbp.DEL_FLG = @DEL_FLG");

            //if (searchcustomerentity.BP_NAME != null)
            //{
            //    sql.Append(" AND ((mbp.BP_NAME LIKE @CustomerName) OR (mbp.BP_NAME_KANA LIKE @CustomerName))");
            //}
            sql.Append(" ) tbItem");

            if (dt.iSortCol_0.HasValue && !string.IsNullOrEmpty(dt.sColumns))
            {
                string[] sCol = dt.sColumns.Split(',');

                if (dt.iSortCol_0 < sCol.Length && !string.IsNullOrEmpty(sCol[dt.iSortCol_0.Value]))
                {
                    sql.Append(string.Format(" ORDER BY {0} {1}", sCol[dt.iSortCol_0.Value], dt.sSortDir_0));
                }
            }

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;

            // Count data list
            totalrow = base.ExecuteScalar<int>(sqlcount, new
            {
                CompanyCode = CompanyCodeInSessiion,
                CONTRACT_TYPE = model.CONTRACT_TYPE,
                CONTRACT_TYPE_CLASS = model.CONTRACT_TYPE_CLASS,
                PRODUCT_NO = model.PRODUCT_NO,
                pageindex = lower,
                pagesize = upper,
                DEL_FLG = Constants.DeleteFlag.NON_DELETE
            });

            // data tu return
            return base.Query<ItemEntity>(sqlpage, new
            {
                CompanyCode = CompanyCodeInSessiion,
                CONTRACT_TYPE = model.CONTRACT_TYPE,
                CONTRACT_TYPE_CLASS = model.CONTRACT_TYPE_CLASS,
                PRODUCT_NO = model.PRODUCT_NO,
                pageindex = lower,
                pagesize = upper,
                DEL_FLG = Constants.DeleteFlag.NON_DELETE
            }).ToList();
        }

        /// <summary>
        /// Search bank account info
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<ContractFirmBankAccountInfoEntity> SearchBankAccountInfo(DataTablesModel dt, out int totalrow)
        {
            string CompanyCodeInSessiion = base.CmnEntityModel.CompanyCd;
            StringBuilder sql = new StringBuilder();
            // Define row to display
            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            // SQL発行
            sql.Append(@" 
                SELECT 
                    tbItem.* 
                FROM (
                    SELECT 
                        BANK_ACCOUNT_SEQ_NO,
                        DSP_PRIORITY,
                        BANK_ACCOUNT_DISP_NAME,
                        FINANCIAL_INST_CD,
                        FINANCIAL_INST_NAME,
                        BANK_ACCOUNT_BRANCH_CD,
                        BANK_ACCOUNT_BRANCH_NAME,
                        BANK_ACCOUNT_TYPE,
                        BANK_ACCOUNT_NO,
                        BANK_ACCOUNT_HOLDER,
                        SWIFT_BIC_CD
                    FROM
                        Mst_ContractFirmBankAccountInfo                         
                    WHERE COMPANY_CD = @CompanyCode                        
                        AND DEL_FLG = @DEL_FLG");            
            sql.Append(" ) tbItem");

            if (dt.iSortCol_0.HasValue && !string.IsNullOrEmpty(dt.sColumns))
            {
                string[] sCol = dt.sColumns.Split(',');

                if (dt.iSortCol_0 < sCol.Length && !string.IsNullOrEmpty(sCol[dt.iSortCol_0.Value]))
                {
                    sql.Append(string.Format(" ORDER BY {0} {1}", sCol[dt.iSortCol_0.Value], dt.sSortDir_0));
                }
            }

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;

            // Count data list
            totalrow = base.ExecuteScalar<int>(sqlcount, new
            {
                CompanyCode = CompanyCodeInSessiion,               
                pageindex = lower,
                pagesize = upper,
                DEL_FLG = Constants.DeleteFlag.NON_DELETE
            });

            // data return
            return base.Query<ContractFirmBankAccountInfoEntity>(sqlpage, new
            {
                CompanyCode = CompanyCodeInSessiion,                
                pageindex = lower,
                pagesize = upper,
                DEL_FLG = Constants.DeleteFlag.NON_DELETE
            }).ToList();
        }
               
        /// <summary>
        /// Get contract user name
        /// </summary>
        /// <param name="UserSegCd"></param>
        /// <returns></returns>
        public string GetContractUserName(long UserSegCd)
        {
            // Declare database connection      
            StringBuilder sqlselect = new StringBuilder();

            sqlselect.Append(" SELECT   CONTRACT_USER_NAME ");
            sqlselect.Append(" FROM	    Mst_ContractFirmUser ");
            sqlselect.Append(" WHERE    CONTRACT_USER_SEQ_CD = @CONTRACT_USER_SEQ_CD ");
            sqlselect.Append(" AND      DEL_FLG = @DEL_FLG ");

            // Execute sql command
            var user = base.Query<ContractFirmUserEntity>(sqlselect.ToString(), new
            {
                CONTRACT_USER_SEQ_CD = UserSegCd,
                DEL_FLG = Constants.DeleteFlag.NON_DELETE
            }).FirstOrDefault();

            if (user != null)
            {
                return user.CONTRACT_USER_NAME;
            }
            else
            {
                return String.Empty;
            }
        }

        #region Mst_Product
        /// <summary>
        /// Get all product in master table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductEntity> GetProductMaster()
        {
            string CompanyCodeInSessiion = base.CmnEntityModel.CompanyCd;
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT * FROM Mst_Product
                WHERE COMPANY_CD = @COMPANY_CD
                AND DEL_FLG = @DEL_FLG
                ORDER BY DSP_PRIORITY");

            var data = base.Query<ProductEntity>(sql.ToString(),
                new
                {
                    COMPANY_CD = CompanyCodeInSessiion,
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).ToList();

            return data;
        }

        /// <summary>
        /// Get Product by ContractType and ContractTypeClass
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="contractyTypeClass"></param>
        /// <returns></returns>
        public IEnumerable<ProductEntity> GetProductByContractType_ContractTypeClass(int contractType, int contractyTypeClass)
        {
            // Declare database connection      
            string CompanyCodeInSessiion = base.CmnEntityModel.CompanyCd;
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT * FROM Mst_Product");
            sql.Append(" WHERE COMPANY_CD = @COMPANY_CD AND DEL_FLG = @DEL_FLG");
            sql.Append(" AND CONTRACT_TYPE = @CONTRACT_TYPE AND CONTRACT_TYPE_CLASS = @CONTRACT_TYPE_CLASS");
            sql.Append(" ORDER BY DSP_PRIORITY");

            var pics = base.Query<ProductEntity>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    CONTRACT_TYPE = contractType,
                    CONTRACT_TYPE_CLASS = contractyTypeClass,
                    COMPANY_CD = CompanyCodeInSessiion
                }).ToList();

            // Create variable ouput   
            return pics;
        }

        /// <summary>
        /// Get Product by ContractType and ContractTypeClass
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="contractyTypeClass"></param>
        /// <returns></returns>
        public IEnumerable<ProductEntity> GetProductByContractType_ContractTypeClass_Outside(string contractFirm, int contractType, int contractyTypeClass)
        {
            // Declare database connection      
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT * FROM Mst_Product");
            sql.Append(" WHERE COMPANY_CD = @COMPANY_CD AND DEL_FLG = @DEL_FLG");
            sql.Append(" AND CONTRACT_TYPE = @CONTRACT_TYPE AND CONTRACT_TYPE_CLASS = @CONTRACT_TYPE_CLASS");
            sql.Append(" ORDER BY DSP_PRIORITY");

            var pics = base.Query<ProductEntity>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    CONTRACT_TYPE = contractType,
                    CONTRACT_TYPE_CLASS = contractyTypeClass,
                    COMPANY_CD = contractFirm
                }).ToList();

            // Create variable ouput   
            return pics;
        }

        /// <summary>
        /// Get estimation file path
        /// </summary>
        /// <param name="companycd"></param>
        /// <param name="contracttype"></param>
        /// <returns></returns>
        public ProductEntity GetProductMaster(string companycd, long productSegNo)
        {
            string filepath = string.Empty;
            // Declare database connection      
            // Create connection 
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT * FROM Mst_Product");
            sql.Append(" WHERE PRODUCT_SEQ_NO = @PRODUCT_SEQ_NO");
            sql.Append(" AND COMPANY_CD = @COMPANY_CD");

            var contractmaster = base.Query<ProductEntity>(sql.ToString(),
                new
                {
                    PRODUCT_SEQ_NO = productSegNo,
                    COMPANY_CD = companycd
                }).FirstOrDefault();

            // Create variable ouput   
            return contractmaster;
        }

        /// <summary>
        /// Search Product Name
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="contractSubType"></param>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public string SearchProductName(int contractType, int contractSubType, int productSeqNo)
        {
            // Declare database connection
            string result = string.Empty;
            StringBuilder sql = new StringBuilder();
            string CompanyCodeInSessiion = base.CmnEntityModel.CompanyCd;
            // SQL発行
            sql.Append(" SELECT PRODUCT_NAME FROM Mst_Product");          
            sql.Append(" WHERE CONTRACT_TYPE = @CONTRACT_TYPE");
            sql.Append(" AND CONTRACT_TYPE_CLASS = @CONTRACT_TYPE_CLASS");
            sql.Append(" AND PRODUCT_SEQ_NO = @PRODUCT_SEQ_NO");

            // Execute sql command
            var contractconfig = base.Query<ProductEntity>(sql.ToString(),
                new
                {                    
                    CONTRACT_TYPE = contractType,
                    CONTRACT_TYPE_CLASS = contractSubType,
                    PRODUCT_SEQ_NO = productSeqNo
                }).FirstOrDefault();

            if (contractconfig != null) result = contractconfig.PRODUCT_NAME;

            // Create variable ouput
            return result;
        }

        /// <summary>
        /// Get all product in master table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductEntity> GetProductMasterByOutside()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT * FROM Mst_Product
                WHERE DEL_FLG = @DEL_FLG
                ORDER BY DSP_PRIORITY");

            var data = base.Query<ProductEntity>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).ToList();

            return data;
        }

        /// <summary>
        /// Get all AccountGroup in master table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AccountGroupEntity> GetAccountGroupMasterByOutside()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT * FROM Mst_AccountGroup
                WHERE DEL_FLG = @DEL_FLG
                ORDER BY ACCOUNT_GROUP_CD");

            var data = base.Query<AccountGroupEntity>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).ToList();

            return data;
        }


        /// <summary>
        /// Get all AccountCodeJIS in master table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AccountCodeJISEntity> GetAccountCodeJISMasterByOutside()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT RTRIM(JIS_ACCOUNT_CD) AS JIS_ACCOUNT_CD,
                       ACCOUNT_DISP_NAME
                FROM Mst_AccountCodeJIS
                WHERE DEL_FLG = @DEL_FLG
                ORDER BY JIS_ACCOUNT_CD");

            var data = base.Query<AccountCodeJISEntity>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).ToList();

            return data;
        }
        #endregion

        #endregion

        /// <summary>
        /// Insert into Tbl_DebitAccount
        /// </summary>
        /// <param name="debitAcc"></param>
        /// <returns></returns>
        public int InsertDebitAccount(DebitAccountEntity debitAcc)
        {
            string command = @"
                INSERT INTO [dbo].[Tbl_DebitAccount]
                   ([CONT_BILLING_SEQ_NO]
                   ,[FINANCIAL_INST_CD]
                   ,[FINANCIAL_INST_NAME]
                   ,[BANK_ACCOUNT_BRANCH_CD]
                   ,[BANK_ACCOUNT_BRANCH_NAME]
                   ,[BANK_ACCOUNT_TYPE]
                   ,[BANK_ACCOUNT_NO]
                   ,[BANK_ACCOUNT_HOLDER]
                   ,[SWIFT_BIC_CD]
                   ,[BANK_MANAGE_CD]
                   ,[DEL_FLG]
                   ,[INS_DATE]
                   ,[INS_USER_ID]
                   ,[UPD_DATE]
                   ,[UPD_USER_ID]
                   ,[UPD_PROG_ID])
                VALUES
                   (@CONT_BILLING_SEQ_NO
                   ,@FINANCIAL_INST_CD
                   ,@FINANCIAL_INST_NAME
                   ,@BANK_ACCOUNT_BRANCH_CD
                   ,@BANK_ACCOUNT_BRANCH_NAME
                   ,@BANK_ACCOUNT_TYPE
                   ,@BANK_ACCOUNT_NO
                   ,@BANK_ACCOUNT_HOLDER
                   ,@SWIFT_BIC_CD
                   ,@BANK_MANAGE_CD
                   ,@DEL_FLG
                   ,@INS_DATE
                   ,@INS_USER_ID
                   ,@UPD_DATE
                   ,@UPD_USER_ID
                   ,@UPD_PROG_ID ) ";
            return base.DbAdd(command, debitAcc);
        }

        /// <summary>
        /// Get contract firm master
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractFirmEntity> GetContractFirmMasterByOutside()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT * FROM Mst_ContractFirm WHERE DEL_FLG = @DEL_FLG ");
            sql.Append(" ORDER BY COMPANY_CD");

            var data = base.Query<ContractFirmEntity>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).ToList();

            return data;
        }

        /// <summary>
        /// Get contract type master model
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractTypeMasterModel> GetContractTypeMaster()
        {
            // Declare database connection      
            string CompanyCodeInSessiion = base.CmnEntityModel.CompanyCd;
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT * FROM Mst_FirmContractConfig");

            sql.Append(" WHERE COMPANY_CD = @COMPANY_CD AND DEL_FLG = @DEL_FLG");

            sql.Append(" ORDER BY DSP_PRIORITY");

            var pics = base.Query<ContractTypeMasterModel>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    COMPANY_CD = CompanyCodeInSessiion
                }).ToList();

            // Create variable ouput   
            return pics;
        }

        /// <summary>
        /// Get all subcontract type in master table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractTypeMasterModel> GetSubContractTypeMaster()
        {
            string CompanyCodeInSessiion = base.CmnEntityModel.CompanyCd;
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT * FROM Mst_FirmContractClass
                WHERE COMPANY_CD = @COMPANY_CD
                AND DEL_FLG = @DEL_FLG
                ORDER BY DSP_PRIORITY");

            var data = base.Query<ContractTypeMasterModel>(sql.ToString(),
                new
                {
                    COMPANY_CD = CompanyCodeInSessiion,
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).ToList();

            return data;
        }

        /// <summary>
        /// Get sub contract by contract type
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public IEnumerable<ContractTypeMasterModel> GetSubContractTypeByContractType(int contractType)
        {
            // Declare database connection      
            string CompanyCodeInSessiion = base.CmnEntityModel.CompanyCd;
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT * FROM Mst_FirmContractClass");

            sql.Append(" WHERE COMPANY_CD = @COMPANY_CD AND DEL_FLG = @DEL_FLG");
            sql.Append(" AND CONTRACT_TYPE = @CONTRACT_TYPE");
            sql.Append(" ORDER BY DSP_PRIORITY");

            var pics = base.Query<ContractTypeMasterModel>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    CONTRACT_TYPE = contractType,
                    COMPANY_CD = CompanyCodeInSessiion
                }).ToList();

            // Create variable ouput   
            return pics;
        }

        /// <summary>
        /// Get sub contract by contract type
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public IEnumerable<ContractTypeMasterModel> GetContractTypeSelectListByOutside(string contractFirm)
        {
            // Declare database connection      
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT * FROM Mst_FirmContractConfig");

            sql.Append(" WHERE COMPANY_CD = @COMPANY_CD AND DEL_FLG = @DEL_FLG");
            sql.Append(" ORDER BY DSP_PRIORITY");

            var pics = base.Query<ContractTypeMasterModel>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    COMPANY_CD = contractFirm
                }).ToList();

            // Create variable ouput   
            return pics;
        }

        /// <summary>
        /// Get contract type master model
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractTypeMasterModel> GetContractTypeMasterByOutside()
        {
            // Declare database connection      
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT * FROM Mst_FirmContractConfig");

            sql.Append(" WHERE DEL_FLG = @DEL_FLG");

            sql.Append(" ORDER BY DSP_PRIORITY");

            var pics = base.Query<ContractTypeMasterModel>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).ToList();

            // Create variable ouput   
            return pics;
        }

        /// <summary>
        /// Get contract type master model with companycode
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractTypeMasterModel> GetContractTypeMasterByCompanycode(string companycode)
        {
            // Declare database connection      
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT * FROM Mst_FirmContractConfig");

            sql.Append(" WHERE COMPANY_CD = @COMPANY_CD AND DEL_FLG = @DEL_FLG");

            sql.Append(" ORDER BY DSP_PRIORITY");

            var pics = base.Query<ContractTypeMasterModel>(sql.ToString(),
                new
                {
                    COMPANY_CD = companycode,
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).ToList();

            // Create variable ouput   
            return pics;
        }

        /// <summary>
        /// Get all subcontract type in master table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractTypeMasterModel> GetSubContractTypeMasterByOutside()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT * FROM Mst_FirmContractClass
                WHERE DEL_FLG = @DEL_FLG
                ORDER BY DSP_PRIORITY");

            var data = base.Query<ContractTypeMasterModel>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).ToList();

            return data;
        }

        /// <summary>
        /// Get all subcontract type in master table with companycode
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractTypeMasterModel> GetSubContractTypeMasterByOutsideCompanycode(string companycode, int contractType)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT * FROM Mst_FirmContractClass
                WHERE  COMPANY_CD = @COMPANY_CD AND DEL_FLG = @DEL_FLG AND CONTRACT_TYPE = @CONTRACT_TYPE
                ORDER BY DSP_PRIORITY");

            var data = base.Query<ContractTypeMasterModel>(sql.ToString(),
                new
                {
                    COMPANY_CD = companycode,
                    CONTRACT_TYPE = contractType,
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).ToList();

            return data;
        }

        /// <summary>
        /// Get sub contract by contract type
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public IEnumerable<ContractTypeMasterModel> GetSubContractTypeSelectListByOutside(string contractFirm, int contractType)
        {
            // Declare database connection      
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT * FROM Mst_FirmContractClass");

            sql.Append(" WHERE COMPANY_CD = @COMPANY_CD AND DEL_FLG = @DEL_FLG");
            sql.Append(" AND CONTRACT_TYPE = @CONTRACT_TYPE");
            sql.Append(" ORDER BY DSP_PRIORITY");

            var pics = base.Query<ContractTypeMasterModel>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    CONTRACT_TYPE = contractType,
                    COMPANY_CD = contractFirm
                }).ToList();

            // Create variable ouput   
            return pics;
        }
        /// <summary>
        /// Get all Product type in master table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductMaintModel> GetProductNameMasterByOutside()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT * FROM Mst_Product
                WHERE DEL_FLG = @DEL_FLG
                ORDER BY DSP_PRIORITY");

            var data = base.Query<ProductMaintModel>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).ToList();

            return data;
        }

        /// <summary>
        /// delete Mst_Item table
        /// </summary>
        /// <returns></returns>
        public int deleteMstItemByOutside
            (string COMPANY_CD, int? CONTRACT_TYPE, int? CONTRACT_TYPE_CLASS = null, int? PRODUCT_NO = null)
        {
            if (COMPANY_CD == "" || CONTRACT_TYPE == null)
            {
                return 0;
            }

            StringBuilder sqlupdate = new StringBuilder();
            sqlupdate.Append(" UPDATE Mst_Item");
            sqlupdate.Append(" SET ");
            sqlupdate.Append("    DEL_FLG = @DEL_FLG,");
            sqlupdate.Append("    UPD_DATE = @UPD_DATE,");
            sqlupdate.Append("    UPD_USER_ID = @UPD_USER_ID");
            sqlupdate.Append(" WHERE    COMPANY_CD = @COMPANY_CD");
            sqlupdate.Append("   AND    CONTRACT_TYPE = @CONTRACT_TYPE");

            if (CONTRACT_TYPE_CLASS != null)
            {
                sqlupdate.Append("   AND    CONTRACT_TYPE_CLASS = @CONTRACT_TYPE_CLASS");
            }
            if (PRODUCT_NO != null)
            {
                sqlupdate.Append("   AND    PRODUCT_NO = @PRODUCT_NO");
            }


            return base.Execute(sqlupdate.ToString(), new
            {
                COMPANY_CD = COMPANY_CD,
                CONTRACT_TYPE = CONTRACT_TYPE,
                CONTRACT_TYPE_CLASS = CONTRACT_TYPE_CLASS,
                PRODUCT_NO = PRODUCT_NO,

                DEL_FLG = Constants.DeleteFlag.DELETE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = 0
            });
        }

        /// <summary>
        /// delete Mst_Product table
        /// </summary>
        /// <returns></returns>
        public int deleteMstProductByOutside
            (string COMPANY_CD, int? CONTRACT_TYPE, int? CONTRACT_TYPE_CLASS = null, int? PRODUCT_NO = null)
        {
            if (COMPANY_CD == "" || CONTRACT_TYPE == null)
            {
                return 0;
            }

            StringBuilder sqlupdate = new StringBuilder();
            sqlupdate.Append(" UPDATE Mst_Product");
            sqlupdate.Append(" SET ");
            sqlupdate.Append("    DEL_FLG = @DEL_FLG,");
            sqlupdate.Append("    UPD_DATE = @UPD_DATE,");
            sqlupdate.Append("    UPD_USER_ID = @UPD_USER_ID");
            sqlupdate.Append(" WHERE    COMPANY_CD = @COMPANY_CD");
            sqlupdate.Append("   AND    CONTRACT_TYPE = @CONTRACT_TYPE");

            if (CONTRACT_TYPE_CLASS != null)
            {
                sqlupdate.Append("   AND    CONTRACT_TYPE_CLASS = @CONTRACT_TYPE_CLASS");
            }
            if (PRODUCT_NO != null)
            {
                sqlupdate.Append("   AND    PRODUCT_NO = @PRODUCT_NO");
            }


            return base.Execute(sqlupdate.ToString(), new
            {
                COMPANY_CD = COMPANY_CD,
                CONTRACT_TYPE = CONTRACT_TYPE,
                CONTRACT_TYPE_CLASS = CONTRACT_TYPE_CLASS,
                PRODUCT_NO = PRODUCT_NO,

                DEL_FLG = Constants.DeleteFlag.DELETE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = 0
            });
        }

        /// <summary>
        /// Get Plan Master
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PlanMaintEntity> GetPlanMaster()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT * FROM Mst_Plan
                WHERE DEL_FLG = @DEL_FLG
                ORDER BY PLAN_SEQ_NO");

            var data = base.Query<PlanMaintEntity>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).ToList();

            return data;
        }

        /// <summary>
        /// Insert Contract Firm Plan
        /// </summary>
        /// <param name="entity"></param>
        public long InsertContractFirmPlan(ContractFirmPlanMaintEntity entity)
        {
            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append(@"
                INSERT INTO Rel_ContractFirmPlan
                (
                     COMPANY_CD
                    ,PLAN_SEQ_NO
                    ,PLAN_START_DATE
                    ,DEL_FLG
                    ,INS_DATE
                    ,INS_USER_ID
                    ,UPD_DATE
                    ,UPD_USER_ID
                )
                VALUES
                (
                     @COMPANY_CD
                    ,@PLAN_SEQ_NO
                    ,@PLAN_START_DATE
                    ,@DEL_FLG    
                    ,@INS_DATE
                    ,@INS_USER_ID
                    ,@UPD_DATE 
                    ,@UPD_USER_ID               
                ) ");
            var res = base.DbAddByOutside(sqlinsert.ToString(), entity);
            if (res > 0)
            {
                var query = "SELECT ident_current('Rel_ContractFirmPlan')";
                return base.ExecuteScalar<long>(query);
            }
            return 0;
        }

        /// <summary>
        /// Delete contract firm plan
        /// </summary>
        /// <param name="COMPANY_CD"></param>
        public long DeleteContractFirmPlan(ContractFirmPlanMaintEntity entity)
        {
            StringBuilder sqlupdate = new StringBuilder();
            sqlupdate.Append(" UPDATE Rel_ContractFirmPlan");
            sqlupdate.Append(" SET ");
            sqlupdate.Append("    DEL_FLG = @DEL_FLG,");
            sqlupdate.Append("    PLAN_END_DATE = @PLAN_END_DATE");
            sqlupdate.Append(" WHERE    COMPANY_CD = @COMPANY_CD");
            sqlupdate.Append(" AND    PLAN_REL_SEQ_NO = @PLAN_REL_SEQ_NO");

            return base.Execute(sqlupdate.ToString(), new
            {
                DEL_FLG = Constants.DeleteFlag.DELETE,
                PLAN_END_DATE = Utility.GetCurrentDateTime(),
                COMPANY_CD = entity.COMPANY_CD,
                PLAN_REL_SEQ_NO = entity.PLAN_REL_SEQ_NO
            });
        }

        /// <summary>
        /// Get Contract Firm Plan
        /// </summary>
        /// <param name="COMPANY_CD"></param>
        /// <returns></returns>
        public IEnumerable<long> GetContractFirmPlan(string COMPANY_CD)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT PLAN_SEQ_NO FROM Rel_ContractFirmPlan
                WHERE DEL_FLG = @DEL_FLG
                AND PLAN_END_DATE IS NULL
                AND COMPANY_CD = @COMPANY_CD");

            var id = base.Query<long>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    COMPANY_CD = COMPANY_CD
                }).ToList();

            return id;
        }
    }
}
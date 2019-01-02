using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemSetup.Models;

namespace SystemSetup.DataAccess
{
    public class CategorySetDuplicateDa: BaseDa
    {
        #region READ
        /// <summary>
        /// Get contract firm master
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractFirmEntity> GetSourceCompanyCode()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT DISTINCT CF.* FROM Mst_ContractFirm CF");
            sql.Append(" INNER JOIN Mst_FirmContractConfig as FCC ON CF.COMPANY_CD = FCC.COMPANY_CD AND FCC.DEL_FLG = @DEL_FLG");
            sql.Append(" where CF.DEL_FLG = @DEL_FLG");
 
            sql.Append(" ORDER BY COMPANY_CD");

            var data = base.Query<ContractFirmEntity>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).ToList();

            return data;
        }

        /// <summary>
        /// Get Get Destination Company Code
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractFirmEntity> GetDestinationCompanyCode()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT DISTINCT CF.* FROM Mst_ContractFirm CF");
            sql.Append(" where CF.COMPANY_CD NOT IN ( select FCC.COMPANY_CD from Mst_FirmContractConfig FCC where  FCC.DEL_FLG = @DEL_FLG)");
            sql.Append(" and CF.DEL_FLG = @DEL_FLG");

            sql.Append(" ORDER BY COMPANY_CD");

            var data = base.Query<ContractFirmEntity>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).ToList();

            return data;
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Delete FirmContractConfig
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public int DeleteFirmContractConfig(string companyCode)
        {
            var currentuser = base.CmnEntityModel;
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
            DELETE FROM Mst_FirmContractConfig
                WHERE COMPANY_CD = @COMPANY_CD");

            return base.Execute(sql.ToString(), new
            {
                COMPANY_CD = companyCode,
            });
        }

        /// <summary>
        /// Delete FirmContractClass
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public int DeleteFirmContractClass(string companyCode)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
            DELETE FROM Mst_FirmContractClass
                WHERE COMPANY_CD = @COMPANY_CD");

            return base.Execute(sql.ToString(), new
            {
                COMPANY_CD = companyCode,
            });
        }

        /// <summary>
        /// Delete FirmContractClass
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public int DeleteProduct(string companyCode)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
            DELETE FROM Mst_Product
                WHERE COMPANY_CD = @COMPANY_CD");

            return base.Execute(sql.ToString(), new
            {
                COMPANY_CD = companyCode,
            });
        }

        #endregion

        #region COPY DUPLICATE
        /// Get List FirmContractConfig
        /// </summary>
        /// <returns></returns>
        public IList<FirmContractConfigEntity> GetFirmContractConfigList(string SOURCE_COMPANY_CD)
        {
            // Declare database connection
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT * FROM Mst_FirmContractConfig");

            sql.Append(" WHERE COMPANY_CD = @COMPANY_CD AND DEL_FLG = @DEL_FLG");

            sql.Append(" ORDER BY COMPANY_CD");

            var result = base.Query<FirmContractConfigEntity>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    COMPANY_CD = SOURCE_COMPANY_CD
                }).ToList();

            // Create variable ouput   
            return result;
        }

        /// Get List FirmContractClass
        /// </summary>
        /// <returns></returns>
        public IList<FirmContractClassEntity> GetFirmContractClassList(string SOURCE_COMPANY_CD)
        {
            // Declare database connection
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT * FROM Mst_FirmContractClass");

            sql.Append(" WHERE COMPANY_CD = @COMPANY_CD AND DEL_FLG = @DEL_FLG");

            sql.Append(" ORDER BY COMPANY_CD");

            var result = base.Query<FirmContractClassEntity>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    COMPANY_CD = SOURCE_COMPANY_CD
                }).ToList();

            // Create variable ouput   
            return result;
        }

        /// Get List FirmContractClass
        /// </summary>
        /// <returns></returns>
        public IList<ProductEntity> GetProductList(string SOURCE_COMPANY_CD)
        {
            // Declare database connection
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT * FROM Mst_Product");

            sql.Append(" WHERE COMPANY_CD = @COMPANY_CD AND DEL_FLG = @DEL_FLG");

            sql.Append(" ORDER BY PRODUCT_SEQ_NO");

            var result = base.Query<ProductEntity>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    COMPANY_CD = SOURCE_COMPANY_CD
                }).ToList();

            // Create variable ouput   
            return result;
        }
        /// <summary>
        /// Copy FirmContractConfig
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long CopyFirmContractConfig(FirmContractConfigEntity model)
        {
            int result = 0;
            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append(@" 
                    INSERT INTO [Mst_FirmContractConfig] 
                        ([COMPANY_CD]
                        ,[CONTRACT_TYPE]
                        ,[DSP_PRIORITY]
                        ,[CONTRACT_TYPE_DISP_NAME]
                        ,[EST_NO_PREFIX]
                        ,[DELIVERY_NO_PREFIX]
                        ,[PLC_ORDER_NO_PREFIX]
                        ,[EST_EFFECTIVE_TYPE]
                        ,[EST_EFFECTIVE_INTERVAL]
                        ,[DEL_FLG]
                        ,[INS_DATE]
                        ,[INS_USER_ID]
                        ,[UPD_DATE]
                        ,[UPD_USER_ID]
                        ,[UPD_PROG_ID])
                    VALUES
                        (@COMPANY_CD,
                        @CONTRACT_TYPE,
                        @DSP_PRIORITY,
                        @CONTRACT_TYPE_DISP_NAME,
                        @EST_NO_PREFIX,
                        @DELIVERY_NO_PREFIX,
                        @PLC_ORDER_NO_PREFIX,
                        @EST_EFFECTIVE_TYPE,
                        @EST_EFFECTIVE_INTERVAL,
                        @DEL_FLG,
                        @INS_DATE,
                        @INS_USER_ID,
                        @UPD_DATE,
                        @UPD_USER_ID,
                        @UPD_PROG_ID)
                        ");

            result = base.DbAddByOutside(sqlinsert.ToString(), model);
            return result;
        }
        /// <summary>
        /// Copy FirmContractClass
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long CopyFirmContractClass(FirmContractClassEntity model)
        {
            int result = 0;
            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append(@" 
                    INSERT INTO [Mst_FirmContractClass] 
                        ([COMPANY_CD]
                        ,[CONTRACT_TYPE]
                        ,[CONTRACT_TYPE_CLASS]
                        ,[DSP_PRIORITY]
                        ,[CONTRACT_TYPE_CLASS_DISP_NAME]
                        ,[DEL_FLG]
                        ,[INS_DATE]
                        ,[INS_USER_ID]
                        ,[UPD_DATE]
                        ,[UPD_USER_ID]
                        ,[UPD_PROG_ID])
                    VALUES
                        (@COMPANY_CD,
                        @CONTRACT_TYPE,
                        @CONTRACT_TYPE_CLASS,
                        @DSP_PRIORITY,
                        @CONTRACT_TYPE_CLASS_DISP_NAME,
                        @DEL_FLG,
                        @INS_DATE,
                        @INS_USER_ID,
                        @UPD_DATE,
                        @UPD_USER_ID,
                        @UPD_PROG_ID)
                        ");

            result = base.DbAddByOutside(sqlinsert.ToString(), model);
            return result;
        }
        /// <summary>
        /// Copy Product 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long CopyProduct(ProductEntity model)
        {
            int result = 0;
            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append(@" 
                    INSERT INTO [Mst_Product] 
                        ([COMPANY_CD]
                        ,[CONTRACT_TYPE]
                        ,[CONTRACT_TYPE_CLASS]
                        ,[PRODUCT_NO]
                        ,[PRODUCT_CD]
                        ,[PRODUCT_NAME]
                        ,[DSP_PRIORITY]
                        ,[CONTRACT_INPUT_TYPE]
                        ,[PJT_NAME_INIT_VALUE]
                        ,[WORKING_CONTENT_INIT_VALUE]
                        ,[WORKING_LOCATION_INIT_VALUE]
                        ,[DELIVERABLES_INIT_VALUE]
                        ,[DELIVERY_LOCATION_INIT_VALUE]
                        ,[EST_FORMAT_PATH]
                        ,[EST_FORMAT_DETAIL_LINE]
                        ,[ORD_FORMAT_PATH]
                        ,[ORD_FORMAT_DETAIL_LINE]
                        ,[INV_FORMAT_PATH]
                        ,[INV_FORMAT_DETAIL_LINE]
                        ,[PROCESSING_TYPE]
                        ,[AUTO_EXPAND_FLG]
                        ,[DEL_FLG]
                        ,[INS_DATE]
                        ,[INS_USER_ID]
                        ,[UPD_DATE]
                        ,[UPD_USER_ID]
                        ,[UPD_PROG_ID])
                    VALUES
                        (@COMPANY_CD,
                        @CONTRACT_TYPE,
                        @CONTRACT_TYPE_CLASS,
                        @PRODUCT_NO,
                        @PRODUCT_CD,
                        @PRODUCT_NAME,
                        @DSP_PRIORITY,
                        @CONTRACT_INPUT_TYPE,
                        @PJT_NAME_INIT_VALUE,
                        @WORKING_CONTENT_INIT_VALUE,
                        @WORKING_LOCATION_INIT_VALUE,
                        @DELIVERABLES_INIT_VALUE,
                        @DELIVERY_LOCATION_INIT_VALUE,
                        @EST_FORMAT_PATH,
                        @EST_FORMAT_DETAIL_LINE,
                        @ORD_FORMAT_PATH,
                        @ORD_FORMAT_DETAIL_LINE,
                        @INV_FORMAT_PATH,
                        @INV_FORMAT_DETAIL_LINE,
                        @PROCESSING_TYPE,
                        @AUTO_EXPAND_FLG,
                        @DEL_FLG,
                        @INS_DATE,
                        @INS_USER_ID,
                        @UPD_DATE,
                        @UPD_USER_ID,
                        @UPD_PROG_ID)
                        ");

            result = base.DbAddByOutside(sqlinsert.ToString(), model);
            return result;
        }

        #endregion
    }
}

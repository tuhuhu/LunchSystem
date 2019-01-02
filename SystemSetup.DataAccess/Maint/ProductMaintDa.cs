using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SystemSetup.Models;
using SystemSetup.UtilityServices;
using SystemSetup.UtilityServices.LogService;
using SystemSetup.UtilityServices.PagingHelper;

namespace SystemSetup.DataAccess
{
    public class ProductMaintDa : BaseDa
    {
        #region READ

        /// <summary>
        /// Search payment/Not payment
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<ProductMaintModel> ProductMaintSearch(DataTablesModel dt, ref ProductMaintModel searchCondition, out int totalrow)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT
                    mp.PRODUCT_SEQ_NO,
	                mp.COMPANY_CD,
					mp.CONTRACT_TYPE,
	                mp.CONTRACT_TYPE_CLASS,
	                mp.PRODUCT_NO,
	                mp.PRODUCT_CD,
	                mp.PRODUCT_NAME,
	                mp.DSP_PRIORITY,
	                mp.UPD_DATE,
	                mcfu.CONTRACT_USER_NAME AS UPD_USERNAME,
                    mfco.CONTRACT_TYPE_DISP_NAME AS CONTRACT_TYPE_DISP_NAME,
                    mfcl.CONTRACT_TYPE_CLASS_DISP_NAME AS CONTRACT_TYPE_CLASS_DISP_NAME
                FROM
	                Mst_Product mp
                        LEFT JOIN Mst_ContractFirmUser mcfu ON
                            mp.COMPANY_CD = mcfu.COMPANY_CD AND
                            mp.UPD_USER_ID = mcfu.CONTRACT_USER_SEQ_CD
                        LEFT JOIN Mst_FirmContractConfig mfco ON
                            mp.COMPANY_CD = mfco.COMPANY_CD AND
                            mp.CONTRACT_TYPE = mfco.CONTRACT_TYPE AND
                            mfco.DEL_FLG = 0
                        LEFT JOIN Mst_FirmContractClass mfcl ON
                            mp.COMPANY_CD = mfcl.COMPANY_CD AND
                            mp.CONTRACT_TYPE = mfcl.CONTRACT_TYPE AND
                            mp.CONTRACT_TYPE_CLASS = mfcl.CONTRACT_TYPE_CLASS AND
                            mfcl.DEL_FLG = 0
                WHERE
                    mp.DEL_FLG = 0

            ");

            if (!String.IsNullOrEmpty(searchCondition.COMPANY_CD))
            {
                sql.Append(@"
                    AND mp.COMPANY_CD = @COMPANY_CD  ");
            }
            string contract_type = "";
            if (!String.IsNullOrEmpty(searchCondition.CONTRACT_TYPE_EDIT))
            {
                sql.Append(@"
                    AND mp.CONTRACT_TYPE = @CONTRACT_TYPE");

                contract_type = searchCondition.CONTRACT_TYPE_EDIT.Split('_')[1];
            }
            string contract_type_class = "";
            if (!String.IsNullOrEmpty(searchCondition.CONTRACT_TYPE_CLASS_EDIT))
            {
                sql.Append(@"
                    AND mp.CONTRACT_TYPE_CLASS = @CONTRACT_TYPE_CLASS");

                contract_type_class = searchCondition.CONTRACT_TYPE_CLASS_EDIT.Split('_')[2];
            }
            if (!String.IsNullOrEmpty(searchCondition.PRODUCT_SEQ_NO_EDIT))
            {
                sql.Append(@"
                    AND mp.PRODUCT_SEQ_NO = @PRODUCT_SEQ_NO");
            }

            sql.Append(@"
                ORDER BY
	                mp.COMPANY_CD,
                    mp.CONTRACT_TYPE,
                    mp.CONTRACT_TYPE_CLASS,
                    mp.DSP_PRIORITY desc,
                    mp.PRODUCT_NO");

            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;


            var dataList = base.Query<ProductMaintModel>(sqlpage,
                new
                {
                    COMPANY_CD = searchCondition.COMPANY_CD,
                    CONTRACT_TYPE = contract_type,
                    CONTRACT_TYPE_CLASS = contract_type_class,
                    PRODUCT_SEQ_NO = searchCondition.PRODUCT_SEQ_NO_EDIT,
                    pageindex = lower,
                    pagesize = upper
                }).ToList();

            totalrow = base.Query<int>(sqlcount,
                new
                {
                    COMPANY_CD = searchCondition.COMPANY_CD,
                    CONTRACT_TYPE = contract_type,
                    CONTRACT_TYPE_CLASS = contract_type_class,
                    PRODUCT_SEQ_NO = searchCondition.PRODUCT_SEQ_NO_EDIT,
                    pageindex = lower,
                    pagesize = upper
                }).FirstOrDefault();

            return dataList;
        }

        /// <summary>
        /// Get a row from Tbl_Information based on INFO_SEQ_NO
        /// </summary>
        /// <param name="infoSeqNo"></param>
        /// <returns></returns>
        public ProductMaintModel GetInformation(long? productSeqNo)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
	                mp.*,
                    config.CONTRACT_TYPE_DISP_NAME,
					class.CONTRACT_TYPE_CLASS_DISP_NAME,
	                mcfu_upd.CONTRACT_USER_NAME AS UPD_USERNAME,
	                mcfu_ins.CONTRACT_USER_NAME AS INS_USERNAME
                FROM
	                Mst_Product mp
                        LEFT JOIN Mst_ContractFirmUser mcfu_upd ON
                            mp.COMPANY_CD = mcfu_upd.COMPANY_CD AND
                            mp.UPD_USER_ID = mcfu_upd.CONTRACT_USER_SEQ_CD
                        LEFT JOIN Mst_ContractFirmUser mcfu_ins ON
                            mp.COMPANY_CD = mcfu_ins.COMPANY_CD AND
                            mp.INS_USER_ID = mcfu_ins.CONTRACT_USER_SEQ_CD
                        LEFT JOIN Mst_FirmContractConfig config ON
                            mp.COMPANY_CD = config.COMPANY_CD AND
                            mp.CONTRACT_TYPE = config.CONTRACT_TYPE
                        LEFT JOIN Mst_FirmContractClass class ON
                            mp.COMPANY_CD = class.COMPANY_CD AND
                            mp.CONTRACT_TYPE = class.CONTRACT_TYPE AND
                            mp.CONTRACT_TYPE_CLASS = class.CONTRACT_TYPE_CLASS
                WHERE
                    mp.PRODUCT_SEQ_NO = @PRODUCT_SEQ_NO
");

            return base.Query<ProductMaintModel>(sql.ToString(), new
            {
                PRODUCT_SEQ_NO = productSeqNo,
            }).FirstOrDefault();
        }

        /// <summary>
        /// GetProduct
        /// </summary>
        /// <returns></returns>
        public IList<ProductMaintModel> GetProduct(string companyCd, int contractType, int contractTypeClass, string productName)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
                    *
                FROM
                    Mst_Product
                WHERE
                    COMPANY_CD = @COMPANY_CD AND
                    CONTRACT_TYPE = @CONTRACT_TYPE AND
                    CONTRACT_TYPE_CLASS = @CONTRACT_TYPE_CLASS
	                PRODUCT_NAME = @PRODUCT_NAME
            ");
            return base.Query<ProductMaintModel>(sql.ToString(), new
            {
                COMPANY_CD = companyCd,
                CONTRACT_TYPE = contractType,
                CONTRACT_TYPE_CLASS = contractTypeClass
            }).ToList();
        }

        #endregion

        #region UPDATE
        public long UpdateProductMaintModel(ProductEntity maint)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                UPDATE [dbo].[Mst_Product] 
                    SET [PRODUCT_CD] = @PRODUCT_CD
                    ,[CONTRACT_TYPE] = @CONTRACT_TYPE
                    ,[CONTRACT_TYPE_CLASS] = @CONTRACT_TYPE_CLASS
                    ,[PRODUCT_NO] = @PRODUCT_NO
                    ,[PRODUCT_NAME] = @PRODUCT_NAME
                    ,[DSP_PRIORITY] = @DSP_PRIORITY
                    ,[CONTRACT_INPUT_TYPE] = @CONTRACT_INPUT_TYPE
                    ,[PJT_NAME_INIT_VALUE] = @PJT_NAME_INIT_VALUE
                    ,[WORKING_CONTENT_INIT_VALUE] = @WORKING_CONTENT_INIT_VALUE
                    ,[WORKING_LOCATION_INIT_VALUE] = @WORKING_LOCATION_INIT_VALUE
                    ,[DELIVERABLES_INIT_VALUE] = @DELIVERABLES_INIT_VALUE
                    ,[DELIVERY_LOCATION_INIT_VALUE] = @DELIVERY_LOCATION_INIT_VALUE
                    ,[EST_FORMAT_PATH] = @EST_FORMAT_PATH
                    ,[EST_FORMAT_DETAIL_LINE] = @EST_FORMAT_DETAIL_LINE
                    ,[ORD_FORMAT_PATH] = @ORD_FORMAT_PATH
                    ,[ORD_FORMAT_DETAIL_LINE] = @ORD_FORMAT_DETAIL_LINE
                    ,[INV_FORMAT_PATH] = @INV_FORMAT_PATH
                    ,[INV_FORMAT_DETAIL_LINE] = @INV_FORMAT_DETAIL_LINE
                    ,[AUTO_EXPAND_FLG] = @AUTO_EXPAND_FLG
                    ,[DEL_FLG] = @DEL_FLG
                    ,[UPD_DATE] = @UPD_DATE
                    ,[UPD_USER_ID] = @UPD_USER_ID
                    ,[UPD_PROG_ID] = @UPD_PROG_ID
                WHERE PRODUCT_SEQ_NO = @PRODUCT_SEQ_NO
 
            ");


            return base.DbUpdateByOutside(sql.ToString(), maint, new
            {
                PRODUCT_SEQ_NO = maint.PRODUCT_SEQ_NO
            });
            //return base.Execute(sql.ToString(), new
            //{
            //    COMPANY_CD = maint.COMPANY_CD,
            //    CONTRACT_TYPE = maint.CONTRACT_TYPE,
            //    CONTRACT_TYPE_CLASS = maint.CONTRACT_TYPE_CLASS,
            //    PRODUCT_CD = maint.PRODUCT_CD,
            //    PRODUCT_NAME = maint.PRODUCT_NAME,
            //    DSP_PRIORITY = maint.DSP_PRIORITY,
            //    CONTRACT_INPUT_TYPE = maint.CONTRACT_INPUT_TYPE,
            //    PJT_NAME_INIT_VALUE = maint.PJT_NAME_INIT_VALUE,
            //    WORKING_CONTENT_INIT_VALUE = maint.WORKING_CONTENT_INIT_VALUE,
            //    WORKING_LOCATION_INIT_VALUE = maint.WORKING_LOCATION_INIT_VALUE,
            //    DELIVERABLES_INIT_VALUE = maint.DELIVERABLES_INIT_VALUE,
            //    DELIVERY_LOCATION_INIT_VALUE = maint.DELIVERY_LOCATION_INIT_VALUE,
            //    EST_FORMAT_PATH = maint.EST_FORMAT_PATH,
            //    EST_FORMAT_DETAIL_LINE = maint.EST_FORMAT_DETAIL_LINE,
            //    AUTO_EXPAND_FLG = maint.AUTO_EXPAND_FLG,
            //    ORD_FORMAT_PATH = maint.ORD_FORMAT_PATH,
            //    ORD_FORMAT_DETAIL_LINE = maint.ORD_FORMAT_DETAIL_LINE,
            //    UPD_DATE = maint.UPD_DATE,
            //    UPD_USER_ID = maint.UPD_USER_ID,
            //    UPD_PROG_ID = maint.UPD_PROG_ID,
            //});
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Delete Mst_Product
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int DeleteProductMaint(long PRODUCT_SEQ_NO)
        {
            int result = 0;

            #region Delete Mst_Product
            result = Delete(PRODUCT_SEQ_NO);
            #endregion

            return result;
        }

        /// <summary>
        /// Delete Mst_Product
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public int Delete(long PRODUCT_SEQ_NO)
        {
            StringBuilder sqlupdate = new StringBuilder();
            sqlupdate.Append(" UPDATE Mst_Product");
            sqlupdate.Append(" SET ");
            sqlupdate.Append("    DEL_FLG = @DEL_FLG,");
            sqlupdate.Append("    UPD_DATE = @UPD_DATE,");
            sqlupdate.Append("    UPD_USER_ID = @UPD_USER_ID");

            sqlupdate.Append(" WHERE    PRODUCT_SEQ_NO = @PRODUCT_SEQ_NO");

            var res = base.Execute(sqlupdate.ToString(), new
            {
                PRODUCT_SEQ_NO = PRODUCT_SEQ_NO,

                DEL_FLG = Constants.DeleteFlag.DELETE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = base.CmnEntityModel.UserSegNo
            });

            if(res > 0){
                Common.CommonDa cDa = new Common.CommonDa();
                StringBuilder sqlselect = new StringBuilder();
                sqlselect.Append(" SELECT ");
                sqlselect.Append("    COMPANY_CD,");
                sqlselect.Append("    CONTRACT_TYPE,");
                sqlselect.Append("    CONTRACT_TYPE_CLASS,");
                sqlselect.Append("    PRODUCT_NO");
                sqlselect.Append(" FROM Mst_Product");
                sqlselect.Append(" WHERE    PRODUCT_SEQ_NO = @PRODUCT_SEQ_NO");

                ProductEntity entity = base.Query<ProductEntity>(sqlselect.ToString(), new
                {
                    PRODUCT_SEQ_NO = PRODUCT_SEQ_NO
                }).First();
                res += cDa.deleteMstItemByOutside(entity.COMPANY_CD,entity.CONTRACT_TYPE,entity.CONTRACT_TYPE_CLASS,entity.PRODUCT_NO);
            }
            return res;
        }

        #endregion


        #region CREATE
        /// <summary>
        /// Insert into Mst_Product
        /// </summary>
        /// <param name="Maint"></param>
        /// <returns></returns>
        public long InsertProductMaint(ProductEntity Maint)
        {
            Maint.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            Maint.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append(@"
                INSERT INTO Mst_Product
                (
                    COMPANY_CD,
                    CONTRACT_TYPE,
                    CONTRACT_TYPE_CLASS,
                    PRODUCT_NO,
                    PRODUCT_CD,
                    PRODUCT_NAME,
                    DSP_PRIORITY,
                    CONTRACT_INPUT_TYPE,
                    PJT_NAME_INIT_VALUE,
                    WORKING_CONTENT_INIT_VALUE,
                    WORKING_LOCATION_INIT_VALUE,
                    DELIVERABLES_INIT_VALUE,
                    DELIVERY_LOCATION_INIT_VALUE,
                    EST_FORMAT_PATH,
                    EST_FORMAT_DETAIL_LINE,
                    INS_DATE,
                    AUTO_EXPAND_FLG,
                    ORD_FORMAT_PATH,
                    ORD_FORMAT_DETAIL_LINE,
                    INV_FORMAT_PATH,
                    INV_FORMAT_DETAIL_LINE,
                    INS_USER_ID,
                    UPD_DATE,
                    UPD_USER_ID,
                    UPD_PROG_ID
                )
                VALUES
                (
                    @COMPANY_CD,
                    @CONTRACT_TYPE,
                    @CONTRACT_TYPE_CLASS,
                    ISNULL((SELECT MAX(PRODUCT_NO) FROM [Mst_Product] WHERE [COMPANY_CD] = @COMPANY_CD AND [CONTRACT_TYPE] = @CONTRACT_TYPE AND [CONTRACT_TYPE_CLASS] = @CONTRACT_TYPE_CLASS), 0) + 1,
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
                    @INS_DATE,
                    @AUTO_EXPAND_FLG,
                    @ORD_FORMAT_PATH,
                    @ORD_FORMAT_DETAIL_LINE,
                    @INV_FORMAT_PATH,
                    @INV_FORMAT_DETAIL_LINE,
                    @INS_USER_ID,
                    @UPD_DATE,
                    @UPD_USER_ID,
                    @UPD_PROG_ID
                ) ");
            var res = base.DbAddByOutside(sqlinsert.ToString(), Maint);
            if (res > 0)
            {
                var query = "SELECT ident_current('Mst_Product')";
                return base.ExecuteScalar<long>(query);
            }
            return 0;
        }
        #endregion
    }
}

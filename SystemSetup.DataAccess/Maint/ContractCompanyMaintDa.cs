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
    public class ContractCompanyMaintDa : BaseDa
    {
        #region READ

        /// <summary>
        /// Search payment/Not payment
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<ContractCompanyMaintModel> ContractCompanyMaintSearch(DataTablesModel dt, ref ContractCompanyMaintModel searchCondition, out int totalrow)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT
					mcf.COMPANY_CD,
	                mcf.COMPANY_NAME,
	                mcf.COMPANY_GROUP_NAME,
	                mcf.COMPANY_STAFF_NAME,
	                mcf.COMPANY_CONTRACT_STATUS,
	                mcf.UPD_DATE,
	                mcfu.CONTRACT_USER_NAME AS UPD_USERNAME
                FROM
	                Mst_ContractFirm mcf
                        LEFT JOIN Mst_ContractFirmUser mcfu ON
                            mcf.UPD_USER_ID = mcfu.CONTRACT_USER_SEQ_CD
                WHERE
                    mcf.DEL_FLG = @DEL_FLG
            ");
            
            
            if (!String.IsNullOrEmpty(searchCondition.SEARCH_COMPANY_CD))
            {
                sql.Append(@"
                    AND mcf.COMPANY_CD LIKE @COMPANY_CD  ");
            }

            if (!String.IsNullOrEmpty(searchCondition.SEARCH_COMPANY_NAME))
            {
                sql.Append(@"
                    AND mcf.COMPANY_NAME LIKE @COMPANY_NAME");
            }

            if (!String.IsNullOrEmpty(searchCondition.SEARCH_COMPANY_GROUP_NAME))
            {
                sql.Append(@"
                    AND mcf.COMPANY_GROUP_NAME LIKE @COMPANY_GROUP_NAME");
            }

            if (!String.IsNullOrEmpty(searchCondition.SEARCH_COMPANY_STAFF_NAME))
            {
                sql.Append(@"
                    AND mcf.COMPANY_STAFF_NAME LIKE @COMPANY_STAFF_NAME");
            }

//            if (!searchCondition.SEARCH_COMPANY_CONTRACT_STATUS)
//            {
//                sql.Append(@"
//                    AND  mcf.COMPANY_CONTRACT_STATUS = '1'");
//            }

            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;


            var dataList = base.Query<ContractCompanyMaintModel>(sqlpage,
                new
                {
                    COMPANY_CD = '%' + searchCondition.SEARCH_COMPANY_CD + '%',
                    COMPANY_NAME = '%' + searchCondition.SEARCH_COMPANY_NAME + '%',
                    COMPANY_GROUP_NAME = '%' + searchCondition.SEARCH_COMPANY_GROUP_NAME + '%',
                    COMPANY_STAFF_NAME = '%' + searchCondition.SEARCH_COMPANY_STAFF_NAME + '%',
                    DEL_FLG = Constants.DisableFlag.Enable,
                    pageindex = lower,
                    pagesize = upper
                }).ToList();

            totalrow = base.Query<int>(sqlcount,
                new
                {
                    COMPANY_CD = '%' + searchCondition.SEARCH_COMPANY_CD + '%',
                    COMPANY_NAME = '%' + searchCondition.SEARCH_COMPANY_NAME + '%',
                    COMPANY_GROUP_NAME = '%' + searchCondition.SEARCH_COMPANY_GROUP_NAME + '%',
                    COMPANY_STAFF_NAME = '%' + searchCondition.SEARCH_COMPANY_STAFF_NAME + '%',
                    DEL_FLG = Constants.DisableFlag.Enable,
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
        public ContractCompanyMaintModel GetInformation(string companyCd)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
					mcf.*,
	                mcfu_upd.CONTRACT_USER_NAME AS UPD_USERNAME,
	                mcfu_ins.CONTRACT_USER_NAME AS INS_USERNAME
                FROM
	                Mst_ContractFirm mcf
                        LEFT JOIN Mst_ContractFirmUser mcfu_upd ON
                            mcf.UPD_USER_ID = mcfu_upd.CONTRACT_USER_SEQ_CD
                        LEFT JOIN Mst_ContractFirmUser mcfu_ins ON
                            mcf.INS_USER_ID = mcfu_ins.CONTRACT_USER_SEQ_CD
                WHERE
                    mcf.COMPANY_CD = @COMPANY_CD
            ");

            return base.Query<ContractCompanyMaintModel>(sql.ToString(), new
            {
                COMPANY_CD = companyCd,
            }).FirstOrDefault();
        }

        /// <summary>
        /// GetContractCompany
        /// </summary>
        /// <returns></returns>
        public IList<ContractCompanyMaintModel> GetContractCompany(string companyCd)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
                    *
                FROM
                    Mst_ContractFirm
                WHERE
                    COMPANY_CD = @COMPANY_CD
            ");
            return base.Query<ContractCompanyMaintModel>(sql.ToString(), new
            {
                COMPANY_CD = companyCd
            }).ToList();
        }

        #endregion

        #region UPDATE
        public long UpdateContractCompanyMaintModel(ContractFirmEntity maint)
        {
            StringBuilder sql = new StringBuilder();
            maint.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            sql.Append(@"
                UPDATE [dbo].[Mst_ContractFirm] 
                    SET
                     [COMPANY_NAME] = @COMPANY_NAME
                    ,[COMPANY_ZIP_CD] = @COMPANY_ZIP_CD
                    ,[COMPANY_ADDR_1] = @COMPANY_ADDR_1
                    ,[COMPANY_ADDR_2] = @COMPANY_ADDR_2
                    ,[COMPANY_GROUP_NAME] = @COMPANY_GROUP_NAME
                    ,[COMPANY_STAFF_NAME] = @COMPANY_STAFF_NAME
                    ,[COMPANY_STAFF_NAME_KANA] = @COMPANY_STAFF_NAME_KANA
                    ,[COMPANY_STAFF_TEL] = @COMPANY_STAFF_TEL
                    ,[COMPANY_STAFF_TEL_EXT] = @COMPANY_STAFF_TEL_EXT
                    ,[COMPANY_STAFF_EMAIL_ADDR] = @COMPANY_STAFF_EMAIL_ADDR
                    ,[ROUNDING_TYPE] = @ROUNDING_TYPE
                    ,[CONTRACT_LEVEL_TYPE] = @CONTRACT_LEVEL_TYPE
                    ,[EST_NO_USE_PREFIX] = @EST_NO_USE_PREFIX
                    ,[EST_NO_USE_YM] = @EST_NO_USE_YM

                    ,[EST_COLLECTION_NO_USE_PREFIX] = @EST_COLLECTION_NO_USE_PREFIX
                    ,[EST_COLLECTION_NO_PREFIX] = @EST_COLLECTION_NO_PREFIX
                    ,[EST_COLLECTION_NO_USE_YM] = @EST_COLLECTION_NO_USE_YM

                    ,[EST_COLLECTION_FORMAT_PATH] = @EST_COLLECTION_FORMAT_PATH
                    ,[EST_COLLECTION_FORMAT_DETAIL_LINE] = @EST_COLLECTION_FORMAT_DETAIL_LINE
                    ,[EST_COLLECTION_FORMAT_NOMEN_LINE] = @EST_COLLECTION_FORMAT_NOMEN_LINE

                    ,[EST_COLLECTION_FORMAT_SES_PATH] = @EST_COLLECTION_FORMAT_SES_PATH
                    ,[EST_COLLECTION_FORMAT_SES_DETAIL_LINE] = @EST_COLLECTION_FORMAT_SES_DETAIL_LINE
                    ,[EST_COLLECTION_FORMAT_SES_NOMEN_LINE] = @EST_COLLECTION_FORMAT_SES_NOMEN_LINE

                    ,[ORD_COLLECTION_FORMAT_PATH] = @ORD_COLLECTION_FORMAT_PATH
                    ,[ORD_COLLECTION_FORMAT_DETAIL_LINE] = @ORD_COLLECTION_FORMAT_DETAIL_LINE
                    ,[ORD_COLLECTION_FORMAT_NOMEN_LINE] = @ORD_COLLECTION_FORMAT_NOMEN_LINE

                    ,[ORD_COLLECTION_FORMAT_SES_PATH] = @ORD_COLLECTION_FORMAT_SES_PATH
                    ,[ORD_COLLECTION_FORMAT_SES_DETAIL_LINE] = @ORD_COLLECTION_FORMAT_SES_DETAIL_LINE
                    ,[ORD_COLLECTION_FORMAT_SES_NOMEN_LINE] = @ORD_COLLECTION_FORMAT_SES_NOMEN_LINE

                    ,[BILL_NO_USE_PREFIX] = @BILL_NO_USE_PREFIX
                    ,[BILL_NO_PREFIX] = @BILL_NO_PREFIX
                    ,[BILL_NO_USE_YM] = @BILL_NO_USE_YM
                    ,[BILL_FORMAT_PATH] = @BILL_FORMAT_PATH
                    ,[BILL_FORMAT_DETAIL_LINE] = @BILL_FORMAT_DETAIL_LINE
                    ,[BILL_FORMAT_SES_PATH] = @BILL_FORMAT_SES_PATH
                    ,[BILL_FORMAT_SES_DETAIL_LINE] = @BILL_FORMAT_SES_DETAIL_LINE
                    ,[BILL_FORMAT_TEMP_PATH] = @BILL_FORMAT_TEMP_PATH
                    ,[BILL_FORMAT_TEMP_DETAIL_LINE] = @BILL_FORMAT_TEMP_DETAIL_LINE
                    ,[BILL_PAYEE_TITLE] = @BILL_PAYEE_TITLE
                    ,[BILL_PAYEE_NOTE] = @BILL_PAYEE_NOTE
                    ,[BILL_DIRECT_TITLE] = @BILL_DIRECT_TITLE
                    ,[BILL_DIRECT_NOTE] = @BILL_DIRECT_NOTE
                    ,[DELIVERY_NO_USE_PREFIX] = @DELIVERY_NO_USE_PREFIX
                    ,[DELIVERY_NO_USE_YM] = @DELIVERY_NO_USE_YM
                    ,[PLC_ORDER_NO_USE_PREFIX] = @PLC_ORDER_NO_USE_PREFIX
                    ,[PLC_ORDER_NO_USE_YM] = @PLC_ORDER_NO_USE_YM
                    ,[PLC_ORDER_FORMAT_PATH] = @PLC_ORDER_FORMAT_PATH
                    ,[PLC_ORDER_FORMAT_DETAIL_LINE] = @PLC_ORDER_FORMAT_DETAIL_LINE
                    ,[PLC_PAYMENT_NOTICE_FORMAT_PATH] = @PLC_PAYMENT_NOTICE_FORMAT_PATH
                    ,[DOC_COVER_LETTER_FORMAT_PATH] = @DOC_COVER_LETTER_FORMAT_PATH
                    ,[FAX_COVER_LETTER_FORMAT_PATH] = @FAX_COVER_LETTER_FORMAT_PATH
                    ,[ENVELOPE_DESTINATION_FORMAT_PATH] = @ENVELOPE_DESTINATION_FORMAT_PATH
                    ,[AVAILABLE_PERIOD_FROM] = @AVAILABLE_PERIOD_FROM
                    ,[AVAILABLE_PERIOD_TO] = @AVAILABLE_PERIOD_TO
                    ,[COMPANY_CONTRACT_STATUS] = @COMPANY_CONTRACT_STATUS
                    ,[DEL_FLG] = @DEL_FLG
                    ,[UPD_DATE] = @UPD_DATE
                    ,[UPD_USER_ID] = @UPD_USER_ID
                    ,[UPD_PROG_ID] = @UPD_PROG_ID
                    ,[COMPANY_LOGO_IMAGE_PATH] = @COMPANY_LOGO_IMAGE_PATH
                    ,[UPLOAD_FILE_SIZE_LIMIT] = @UPLOAD_FILE_SIZE_LIMIT
                    ,[ACCOUNT_CLOSING_MONTH] = @ACCOUNT_CLOSING_MONTH
                    ,[PLAN_REL_SEQ_NO] = @PLAN_REL_SEQ_NO
            ");

            if (!maint.UPLOAD_FILE_PASSWORD.Equals(Constants.Constant.DISPLAY_PASSWORD))
            {
                sql.Append(@"
                ,[UPLOAD_FILE_PASSWORD] = @UPLOAD_FILE_PASSWORD
            ");
            }

            sql.Append(@"
                WHERE COMPANY_CD = @COMPANY_CD
            ");
            return base.DbUpdateByOutside(sql.ToString(), maint, new
            {
                COMPANY_CD = maint.COMPANY_CD
            });
        }

        public long UpdateRelContractFirm(ContractFirmEntity maint)
        {
            StringBuilder sql = new StringBuilder();
            maint.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            sql.Append(@"
                UPDATE [dbo].[Rel_ContractFirmPlan] 
                    SET                               
                     [DEL_FLG] = @DEL_FLG
                    ,[UPD_DATE] = @UPD_DATE
                    ,[UPD_USER_ID] = @UPD_USER_ID
                    ,[UPD_PROG_ID] = @UPD_PROG_ID                   
                WHERE COMPANY_CD = @COMPANY_CD
            ");
            return base.DbUpdateByOutside(sql.ToString(), maint, new
            {
                COMPANY_CD = maint.COMPANY_CD
            });
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Delete Mst_ContractFirm
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int DeleteContractCompanyMaint(string COMPANY_CD)
        {
            int result = 0;
            int result2 = 0;

            #region Delete Mst_ContractFirm
            result = Delete(COMPANY_CD);
            result2 = Delete2(COMPANY_CD);

            if (result == 0 || result2 == 0)
            {
                result = 0;
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Delete Mst_ContractFirm
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public int Delete(string COMPANY_CD)
        {
            StringBuilder sqlupdate = new StringBuilder();
            sqlupdate.Append(" UPDATE Mst_ContractFirm");
            sqlupdate.Append(" SET ");
            sqlupdate.Append("    DEL_FLG = @DEL_FLG,");
            sqlupdate.Append("    UPD_DATE = @UPD_DATE,");
            sqlupdate.Append("    UPD_USER_ID = @UPD_USER_ID");

            sqlupdate.Append(" WHERE    COMPANY_CD = @COMPANY_CD");

            return base.Execute(sqlupdate.ToString(), new
            {
                COMPANY_CD = COMPANY_CD,

                DEL_FLG = Constants.DeleteFlag.DELETE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = 0
            });
        }

        /// <summary>
        /// Delete Mst_BusinessPartner
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public int Delete2(string COMPANY_CD)
        {
            StringBuilder sqlupdate = new StringBuilder();
            sqlupdate.Append(" UPDATE Mst_BusinessPartner");
            sqlupdate.Append(" SET ");
            sqlupdate.Append("    DEL_FLG = @DEL_FLG,");
            sqlupdate.Append("    UPD_DATE = @UPD_DATE,");
            sqlupdate.Append("    UPD_USER_ID = @UPD_USER_ID");

            sqlupdate.Append(" WHERE    COMPANY_CD = @COMPANY_CD");

            return base.Execute(sqlupdate.ToString(), new
            {
                COMPANY_CD = COMPANY_CD,

                DEL_FLG = Constants.DeleteFlag.DELETE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = 0
            });
        }

        #endregion

        #region CREATE
        /// <summary>
        /// Insert into Mst_ContractFirm
        /// </summary>
        /// <param name="Maint"></param>
        /// <returns></returns>
        public long InsertContractCompanyMaint(ContractFirmEntity Maint)
        {
            Maint.UPD_PROG_ID = "0";
            Maint.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append(@"
                INSERT INTO Mst_ContractFirm
                (
                    COMPANY_CD
                    ,COMPANY_NAME
                    ,COMPANY_ZIP_CD
                    ,COMPANY_ADDR_1
                    ,COMPANY_ADDR_2
                    ,COMPANY_GROUP_NAME
                    ,COMPANY_STAFF_NAME
                    ,COMPANY_STAFF_NAME_KANA
                    ,COMPANY_STAFF_TEL
                    ,COMPANY_STAFF_TEL_EXT
                    ,COMPANY_STAFF_EMAIL_ADDR
                    ,ROUNDING_TYPE
                    ,CONTRACT_LEVEL_TYPE
                    ,EST_NO_USE_PREFIX
                    ,EST_NO_USE_YM

                    ,EST_COLLECTION_NO_USE_PREFIX
                    ,EST_COLLECTION_NO_PREFIX
                    ,EST_COLLECTION_NO_USE_YM

                    ,EST_COLLECTION_FORMAT_PATH
                    ,EST_COLLECTION_FORMAT_DETAIL_LINE
                    ,EST_COLLECTION_FORMAT_NOMEN_LINE

                    ,EST_COLLECTION_FORMAT_SES_PATH
                    ,EST_COLLECTION_FORMAT_SES_DETAIL_LINE
                    ,EST_COLLECTION_FORMAT_SES_NOMEN_LINE

                    ,ORD_COLLECTION_FORMAT_PATH
                    ,ORD_COLLECTION_FORMAT_DETAIL_LINE
                    ,ORD_COLLECTION_FORMAT_NOMEN_LINE

                    ,ORD_COLLECTION_FORMAT_SES_PATH
                    ,ORD_COLLECTION_FORMAT_SES_DETAIL_LINE
                    ,ORD_COLLECTION_FORMAT_SES_NOMEN_LINE

                    ,BILL_NO_USE_PREFIX
                    ,BILL_NO_PREFIX
                    ,BILL_NO_USE_YM
                    ,BILL_FORMAT_PATH
                    ,BILL_FORMAT_DETAIL_LINE
                    ,BILL_FORMAT_SES_PATH
                    ,BILL_FORMAT_SES_DETAIL_LINE
                    ,BILL_FORMAT_TEMP_PATH
                    ,BILL_FORMAT_TEMP_DETAIL_LINE
                    ,BILL_PAYEE_TITLE
                    ,BILL_PAYEE_NOTE
                    ,BILL_DIRECT_TITLE
                    ,BILL_DIRECT_NOTE
                    ,DELIVERY_NO_USE_PREFIX
                    ,DELIVERY_NO_USE_YM
                    ,PLC_ORDER_NO_USE_PREFIX
                    ,PLC_ORDER_NO_USE_YM
                    ,PLC_ORDER_FORMAT_PATH
                    ,PLC_ORDER_FORMAT_DETAIL_LINE
                    ,PLC_PAYMENT_NOTICE_FORMAT_PATH 
                    ,DOC_COVER_LETTER_FORMAT_PATH
                    ,FAX_COVER_LETTER_FORMAT_PATH
                    ,ENVELOPE_DESTINATION_FORMAT_PATH
                    ,AVAILABLE_PERIOD_FROM
                    ,AVAILABLE_PERIOD_TO
                    ,COMPANY_CONTRACT_STATUS
                    ,DEL_FLG
                    ,INS_DATE
                    ,INS_USER_ID
                    ,UPD_DATE
                    ,UPD_USER_ID
                    ,UPD_PROG_ID
                    ,COMPANY_LOGO_IMAGE_PATH
                    ,UPLOAD_FILE_PASSWORD
                    ,UPLOAD_FILE_SIZE_LIMIT
                    ,ACCOUNT_CLOSING_MONTH
                    ,PLAN_REL_SEQ_NO
                )
                VALUES
                (
                     @COMPANY_CD
                    ,@COMPANY_NAME
                    ,@COMPANY_ZIP_CD
                    ,@COMPANY_ADDR_1
                    ,@COMPANY_ADDR_2
                    ,@COMPANY_GROUP_NAME
                    ,@COMPANY_STAFF_NAME
                    ,@COMPANY_STAFF_NAME_KANA
                    ,@COMPANY_STAFF_TEL
                    ,@COMPANY_STAFF_TEL_EXT
                    ,@COMPANY_STAFF_EMAIL_ADDR
                    ,@ROUNDING_TYPE
                    ,@CONTRACT_LEVEL_TYPE
                    ,@EST_NO_USE_PREFIX
                    ,@EST_NO_USE_YM

                    ,@EST_COLLECTION_NO_USE_PREFIX
                    ,@EST_COLLECTION_NO_PREFIX
                    ,@EST_COLLECTION_NO_USE_YM

                    ,@EST_COLLECTION_FORMAT_PATH
                    ,@EST_COLLECTION_FORMAT_DETAIL_LINE
                    ,@EST_COLLECTION_FORMAT_NOMEN_LINE

                    ,@EST_COLLECTION_FORMAT_SES_PATH
                    ,@EST_COLLECTION_FORMAT_SES_DETAIL_LINE
                    ,@EST_COLLECTION_FORMAT_SES_NOMEN_LINE

                    ,@ORD_COLLECTION_FORMAT_PATH
                    ,@ORD_COLLECTION_FORMAT_DETAIL_LINE
                    ,@ORD_COLLECTION_FORMAT_NOMEN_LINE

                    ,@ORD_COLLECTION_FORMAT_SES_PATH
                    ,@ORD_COLLECTION_FORMAT_SES_DETAIL_LINE
                    ,@ORD_COLLECTION_FORMAT_SES_NOMEN_LINE

                    ,@BILL_NO_USE_PREFIX
                    ,@BILL_NO_PREFIX
                    ,@BILL_NO_USE_YM
                    ,@BILL_FORMAT_PATH
                    ,@BILL_FORMAT_DETAIL_LINE
                    ,@BILL_FORMAT_SES_PATH
                    ,@BILL_FORMAT_SES_DETAIL_LINE
                    ,@BILL_FORMAT_TEMP_PATH
                    ,@BILL_FORMAT_TEMP_DETAIL_LINE
                    ,@BILL_PAYEE_TITLE
                    ,@BILL_PAYEE_NOTE
                    ,@BILL_DIRECT_TITLE
                    ,@BILL_DIRECT_NOTE
                    ,@DELIVERY_NO_USE_PREFIX
                    ,@DELIVERY_NO_USE_YM
                    ,@PLC_ORDER_NO_USE_PREFIX
                    ,@PLC_ORDER_NO_USE_YM
                    ,@PLC_ORDER_FORMAT_PATH
                    ,@PLC_ORDER_FORMAT_DETAIL_LINE
                    ,@PLC_PAYMENT_NOTICE_FORMAT_PATH
                    ,@DOC_COVER_LETTER_FORMAT_PATH
                    ,@FAX_COVER_LETTER_FORMAT_PATH
                    ,@ENVELOPE_DESTINATION_FORMAT_PATH
                    ,@AVAILABLE_PERIOD_FROM
                    ,@AVAILABLE_PERIOD_TO
                    ,@COMPANY_CONTRACT_STATUS
                    ,@DEL_FLG
                    ,@INS_DATE
                    ,@INS_USER_ID
                    ,@UPD_DATE
                    ,@UPD_USER_ID
                    ,@UPD_PROG_ID
                    ,@COMPANY_LOGO_IMAGE_PATH
                    ,@UPLOAD_FILE_PASSWORD
                    ,@UPLOAD_FILE_SIZE_LIMIT
                    ,@ACCOUNT_CLOSING_MONTH
                    ,@PLAN_REL_SEQ_NO
                ) ");
            var res = base.DbAddByOutside(sqlinsert.ToString(), Maint);
            if (res > 0)
            {
//                var query = "SELECT ident_current('Mst_ContractFirm')";
//                return base.ExecuteScalar<long>(query);
                return res;
            }
            return 0;
        }
        #endregion

        #region CREATEBusinessPartner
        /// <summary>
        /// Insert into Mst_BusinessPartner
        /// </summary>
        /// <param name="Maint"></param>
        /// <returns></returns>
        public long InsertBusinessPartner(ContractFirmEntity Maint)
        {
            BusinessPartnerEntity BPModel = new BusinessPartnerEntity();
            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append(@"
                INSERT INTO [dbo].[Mst_BusinessPartner]
                        ([COMPANY_CD]
                        ,[BP_CD]
                        ,[BP_NAME]
                        ,[BP_NAME_KANA]
                        ,[BP_NAME_DISP]
                        ,[BASE_BP_SEQ_NO]
                        ,[SPECIAL_VALUE_TYPE]
                        ,[DISABLE_FLG]
                        ,[DEL_FLG]
                        ,[INS_DATE]
                        ,[INS_USER_ID]
                        ,[UPD_DATE]
                        ,[UPD_USER_ID]
                        ,[UPD_PROG_ID])
                    VALUES
                        (@COMPANY_CD,
                        @BP_CD,
                        @BP_NAME,
                        @BP_NAME_KANA,
                        @BP_NAME_DISP,
                        @BASE_BP_SEQ_NO,
                        @SPECIAL_VALUE_TYPE, 
                        @DISABLE_FLG,
                        @DEL_FLG,
                        @INS_DATE,
                        @INS_USER_ID,
                        @UPD_DATE,
                        @UPD_USER_ID,
                        @UPD_PROG_ID)
                 ");

            BPModel.COMPANY_CD = Maint.COMPANY_CD;

            BPModel.BP_CD = 0;
            BPModel.BP_NAME = "個人事業主";
            BPModel.BP_NAME_KANA = "コジンジギョウヌシ";
            BPModel.BP_NAME_DISP = "個人事業主";
            BPModel.BASE_BP_SEQ_NO = 0;
            BPModel.SPECIAL_VALUE_TYPE = "1";
            BPModel.DISABLE_FLG = "0";
            BPModel.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
            BPModel.INS_DATE = Maint.INS_DATE;
            BPModel.INS_USER_ID = Maint.INS_USER_ID;
            BPModel.UPD_DATE = Maint.INS_DATE;
            BPModel.UPD_USER_ID = Maint.INS_USER_ID;
            BPModel.UPD_PROG_ID = Maint.UPD_PROG_ID;

            var res = base.DbAddByOutside(sqlinsert.ToString(), BPModel);
            if (res > 0)
            {
                return res;
            }
            return 0;
        }
        #endregion


        /// <summary>
        /// Check exist COMPANY_CD
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool CheckExistCustomer(ContractFirmEntity Entity)
        {
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT COMPANY_CD FROM Mst_ContractFirm ");
            sql.Append(" WHERE COMPANY_CD = @COMPANY_CD");

            var cus = base.Query<string>(sql.ToString(), new
            {
                COMPANY_CD = Entity.COMPANY_CD
            });

            if (cus.Count() != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get current file's password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetCurrentFilePassword(string companyCd)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
                    UPLOAD_FILE_PASSWORD 
                FROM Mst_ContractFirm
                WHERE COMPANY_CD = @COMPANY_CD ");
            var result = base.SingleOrDefault<ContractFirmEntity>(sql.ToString(), new
            {
                COMPANY_CD = companyCd
            });
            return result.UPLOAD_FILE_PASSWORD;
        }

    }
}

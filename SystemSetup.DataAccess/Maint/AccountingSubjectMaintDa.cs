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
    public class AccountingSubjectMaintDa : BaseDa
    {
        #region READ

        /// <summary>
        /// Search payment/Not payment
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<AccountingSubjectMaintModel> AccountingSubjectMaintSearch(DataTablesModel dt, ref AccountingSubjectMaintModel searchCondition, out int totalrow)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT
					mac.ACCOUNT_CD,
	                mac.ACCOUNT_NAME,
	                mac.ACCOUNT_DISP_NAME,
	                mac.ASSETS_LIABILITIES_TYPE,
	                mac.UPD_DATE,
	                mcfu.CONTRACT_USER_NAME AS UPD_USERNAME
                FROM
	                Mst_AccountCode mac
                        LEFT JOIN Mst_ContractFirmUser mcfu ON
                            mac.UPD_USER_ID = mcfu.CONTRACT_USER_SEQ_CD
                WHERE
                    mac.DEL_FLG = @DEL_FLG
                    AND mac.ACCOUNT_GROUP_SEQ_NO = @ACCOUNT_GROUP_SEQ_NO
            ");


            if (!String.IsNullOrEmpty(searchCondition.SEARCH_ACCOUNT_CD))
            {
                sql.Append(@"
                    AND mac.ACCOUNT_CD LIKE @ACCOUNT_CD  ");
            }

            if (!String.IsNullOrEmpty(searchCondition.SEARCH_ACCOUNT_NAME))
            {
                sql.Append(@"
                    AND mac.ACCOUNT_NAME LIKE @ACCOUNT_NAME");
            }

            if (!String.IsNullOrEmpty(searchCondition.SEARCH_ACCOUNT_DISP_NAME))
            {
                sql.Append(@"
                    AND mac.ACCOUNT_DISP_NAME LIKE @ACCOUNT_DISP_NAME");
            }

            if (!String.IsNullOrEmpty(searchCondition.SEARCH_ASSETS_LIABILITIES_TYPE))
            {
                sql.Append(@"
                    AND mac.ASSETS_LIABILITIES_TYPE LIKE @ASSETS_LIABILITIES_TYPE");
            }

            if (!searchCondition.SEARCH_DISABLE_FLG)
            {
                sql.Append(@"
                    AND  mac.DISABLE_FLG = '0'");
            }

            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;


            var dataList = base.Query<AccountingSubjectMaintModel>(sqlpage,
                new
                {
                    ACCOUNT_GROUP_SEQ_NO = searchCondition.SEARCH_ACCOUNT_GROUP_SEQ_NO,
                    ACCOUNT_CD = '%' + searchCondition.SEARCH_ACCOUNT_CD + '%',
                    ACCOUNT_NAME = '%' + searchCondition.SEARCH_ACCOUNT_NAME + '%',
                    ACCOUNT_DISP_NAME = '%' + searchCondition.SEARCH_ACCOUNT_DISP_NAME + '%',
                    ASSETS_LIABILITIES_TYPE = '%' + searchCondition.SEARCH_ASSETS_LIABILITIES_TYPE + '%',
                    DEL_FLG = Constants.DisableFlag.Enable,
                    pageindex = lower,
                    pagesize = upper
                }).ToList();

            totalrow = base.Query<int>(sqlcount,
                new
                {
                    ACCOUNT_GROUP_SEQ_NO = searchCondition.SEARCH_ACCOUNT_GROUP_SEQ_NO,
                    ACCOUNT_CD = '%' + searchCondition.SEARCH_ACCOUNT_CD + '%',
                    ACCOUNT_NAME = '%' + searchCondition.SEARCH_ACCOUNT_NAME + '%',
                    ACCOUNT_DISP_NAME = '%' + searchCondition.SEARCH_ACCOUNT_DISP_NAME + '%',
                    ASSETS_LIABILITIES_TYPE = '%' + searchCondition.SEARCH_ASSETS_LIABILITIES_TYPE + '%',
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
        public AccountingSubjectMaintModel GetInformation(string accountCd)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
                    mac.ACCOUNT_CD
                   ,mac.ACCOUNT_NAME
                   ,mac.ACCOUNT_DISP_NAME
                   ,mac.ASSETS_LIABILITIES_TYPE
                   ,mac.ACCOUNT_GROUP_SEQ_NO
                   ,RTRIM(mac.JIS_ACCOUNT_CD) AS JIS_ACCOUNT_CD
                   ,mac.DISABLE_FLG
                   ,mac.DEL_FLG
                   ,mac.INS_DATE
                   ,mac.INS_USER_ID
                   ,mac.UPD_DATE
                   ,mac.UPD_USER_ID
                   ,mac.UPD_PROG_ID
	               ,mcfu_upd.CONTRACT_USER_NAME AS UPD_USERNAME
	               ,mcfu_ins.CONTRACT_USER_NAME AS INS_USERNAME
                FROM
	                Mst_AccountCode mac
                        LEFT JOIN Mst_ContractFirmUser mcfu_upd ON
                            mac.UPD_USER_ID = mcfu_upd.CONTRACT_USER_SEQ_CD
                        LEFT JOIN Mst_ContractFirmUser mcfu_ins ON
                            mac.INS_USER_ID = mcfu_ins.CONTRACT_USER_SEQ_CD
                WHERE
                    mac.ACCOUNT_CD = @ACCOUNT_CD
            ");

            return base.Query<AccountingSubjectMaintModel>(sql.ToString(), new
            {
                ACCOUNT_CD = accountCd,
            }).FirstOrDefault();
        }

        /// <summary>
        /// GetContractCompany
        /// </summary>
        /// <returns></returns>
        public IList<AccountingSubjectMaintModel> GetContractCompany(string accountCd)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
                    *
                FROM
                    Mst_AccountCode
                WHERE
                    ACCOUNT_CD = @ACCOUNT_CD
            ");
            return base.Query<AccountingSubjectMaintModel>(sql.ToString(), new
            {
                ACCOUNT_CD = accountCd
            }).ToList();
        }

        #endregion

        #region UPDATE
        public long UpdateAccountingSubjectMaintModel(AccountCodeEntity maint)
        {
            StringBuilder sql = new StringBuilder();
            maint.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            sql.Append(@"
                UPDATE [dbo].[Mst_AccountCode] 
                    SET
                     [ACCOUNT_NAME] = @ACCOUNT_NAME
                    ,[ACCOUNT_DISP_NAME] = @ACCOUNT_DISP_NAME
                    ,[ASSETS_LIABILITIES_TYPE] = @ASSETS_LIABILITIES_TYPE
                    ,[ACCOUNT_GROUP_SEQ_NO] = @ACCOUNT_GROUP_SEQ_NO
                    ,[JIS_ACCOUNT_CD] = @JIS_ACCOUNT_CD
                    ,[DISABLE_FLG] = @DISABLE_FLG
                    ,[UPD_DATE] = @UPD_DATE
                    ,[UPD_USER_ID] = @UPD_USER_ID
                    ,[UPD_PROG_ID] = @UPD_PROG_ID

                WHERE ACCOUNT_CD = @ACCOUNT_CD
            ");
            return base.DbUpdateByOutside(sql.ToString(), maint, new
            {
                ACCOUNT_CD = maint.ACCOUNT_CD
            });
        }

        #endregion

        #region DELETE
        /// <summary>
        /// Delete Mst_AccountCode and Mst_AccountCodeJIS
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int DeleteAccountingSubjectMaint(string ACCOUNT_CD)
        {
            int result = 0;

            #region Delete Mst_AccountCode
            result = DeleteAccountCode(ACCOUNT_CD);
            #endregion

            return result;
        }

        /// <summary>
        /// Delete Mst_AccountCode
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public int DeleteAccountCode(string ACCOUNT_CD)
        {
            StringBuilder sqlupdate = new StringBuilder();
            sqlupdate.Append(" UPDATE Mst_AccountCode");
            sqlupdate.Append(" SET ");
            sqlupdate.Append("    DEL_FLG = @DEL_FLG,");
            sqlupdate.Append("    UPD_DATE = @UPD_DATE,");
            sqlupdate.Append("    UPD_USER_ID = @UPD_USER_ID");

            sqlupdate.Append(" WHERE    ACCOUNT_CD = @ACCOUNT_CD");

            return base.Execute(sqlupdate.ToString(), new
            {
                ACCOUNT_CD = ACCOUNT_CD,

                DEL_FLG = Constants.DeleteFlag.DELETE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = 0
            });
        }

        #endregion

        #region CREATE
        /// <summary>
        /// Insert into Mst_AccountCode
        /// </summary>
        /// <param name="Maint"></param>
        /// <returns></returns>
        public long InsertAccountingSubjectMaint(AccountCodeEntity Maint)
        {
            Maint.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            Maint.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append(@"
                INSERT INTO Mst_AccountCode
                (
                     ACCOUNT_CD
                    ,ACCOUNT_NAME
                    ,ACCOUNT_DISP_NAME
                    ,ASSETS_LIABILITIES_TYPE
                    ,ACCOUNT_GROUP_SEQ_NO
                    ,JIS_ACCOUNT_CD
                    ,DISABLE_FLG
                    ,DEL_FLG
                    ,INS_DATE
                    ,INS_USER_ID
                    ,UPD_DATE
                    ,UPD_USER_ID
                    ,UPD_PROG_ID
                )
                VALUES
                (
                     @ACCOUNT_CD
                    ,@ACCOUNT_NAME
                    ,@ACCOUNT_DISP_NAME
                    ,@ASSETS_LIABILITIES_TYPE
                    ,@ACCOUNT_GROUP_SEQ_NO
                    ,@JIS_ACCOUNT_CD
                    ,@DISABLE_FLG
                    ,@DEL_FLG
                    ,@INS_DATE
                    ,@INS_USER_ID
                    ,@UPD_DATE
                    ,@UPD_USER_ID
                    ,@UPD_PROG_ID
                ) ");
            var res = base.DbAddByOutside(sqlinsert.ToString(), Maint);
            if (res > 0)
            {
                return res;
            }
            return 0;
        }
        #endregion

        #region Check exist ACCOUNT_CD
        /// <summary>
        /// Check exist ACCOUNT_CD
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool CheckAccount(AccountCodeEntity Entity)
        {
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT ACCOUNT_CD FROM Mst_AccountCode ");
            sql.Append(" WHERE ACCOUNT_CD = @ACCOUNT_CD");

            var cus = base.Query<string>(sql.ToString(), new
            {
                ACCOUNT_CD = Entity.ACCOUNT_CD
            });

            if (cus.Count() != 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region DeleteBeforeCheck
        /// <summary>
        /// DeleteBeforeCheck
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool DeleteBeforeCheck(string ACCOUNT_CD)
        {
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT AUXILIARY_SEQ_NO FROM Mst_AuxiliaryCode ");
            sql.Append(" WHERE ACCOUNT_CD = @ACCOUNT_CD");
            sql.Append(" AND DEL_FLG = @DEL_FLG");

            var cus = base.Query<string>(sql.ToString(), new
            {
                ACCOUNT_CD = ACCOUNT_CD,
                DEL_FLG = Constants.DeleteFlag.NON_DELETE
            });

            if (cus.Count() != 0)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}

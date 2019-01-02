using Dapper;
using DapperExtensions;
using SystemSetup.Constants;
using SystemSetup.Models;
using SystemSetup.UtilityServices;
using SystemSetup.UtilityServices.LogService;
using SystemSetup.UtilityServices.PagingHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemSetup.UtilityServices.SafePassword;

namespace SystemSetup.DataAccess
{
    public class UserMaintDa : BaseDa
    {
        #region CREATE
        /// <summary>
        /// Insert into Tbl_Information
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        public long InsertUserMaint(UserMaintEntity UserMaint)
        {
            UserMaint.COMPANY_CD = UserMaint.COMPANY_CD;
            UserMaint.AUTHORITY_GROUP_CD = UserMaint.AUTHORITY_GROUP_CD != null ? UserMaint.AUTHORITY_GROUP_CD : 0;
            UserMaint.PASSWORD_LAST_UPDATE_DATE = DateTime.Parse(Constants.Constant.SqlDateRange.MIN);
            UserMaint.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            UserMaint.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append(@"
                INSERT INTO Mst_ContractFirmUser
                (
                    COMPANY_CD,
                    CONTRACT_USER_ACCOUNT,
                    CONTRACT_USER_PASSWORD,
                    CONTRACT_USER_NAME,
                    STAFF_ID,
                    AUTHORITY_GROUP_CD,
                    PASSWORD_LAST_UPDATE_DATE,
                    SEARCH_AUTHORITY_TYPE,
                    SALES_ANALYSIS_AUTHORITY_TYPE,
                    DISABLE_FLG,
                    DEL_FLG,
                    INS_DATE,
                    INS_USER_ID,
                    UPD_DATE,
                    UPD_USER_ID,
                    UPD_PROG_ID
                )
                VALUES
                (
                    @COMPANY_CD,
                    @CONTRACT_USER_ACCOUNT,
                    @CONTRACT_USER_PASSWORD,
                    @CONTRACT_USER_NAME,
                    @STAFF_ID,
                    @AUTHORITY_GROUP_CD,
                    @PASSWORD_LAST_UPDATE_DATE,
                    @SEARCH_AUTHORITY_TYPE,
                    @SALES_ANALYSIS_AUTHORITY_TYPE,
                    @DISABLE_FLG,
                    @DEL_FLG,
                    @INS_DATE,
                    @INS_USER_ID,
                    @UPD_DATE,
                    @UPD_USER_ID,
                    @UPD_PROG_ID
                ) ");
            UserMaint.CONTRACT_USER_PASSWORD = SafePassword.GetSaltedPassword(UserMaint.CONTRACT_USER_PASSWORD);
            var res = base.DbAddByOutside(sqlinsert.ToString(), UserMaint);
            if (res > 0)
            {
                var query = "SELECT ident_current('Mst_ContractFirmUser')";
                return base.ExecuteScalar<long>(query);
            }
            return 0;
        }
        #endregion

        #region READ
        /// <summary>
        /// Get a row from Tbl_Information based on INFO_SEQ_NO
        /// </summary>
        /// <param name="infoSeqNo"></param>
        /// <returns></returns>
        public UserMaintModel GetUserMaintInformation(long infoSeqNo)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT *
                FROM Mst_ContractFirmUser 
                WHERE CONTRACT_USER_SEQ_CD = @INFO_SEQ_NO ");
            return base.Query<UserMaintModel>(sql.ToString(), new
            {
                INFO_SEQ_NO = infoSeqNo
            }).FirstOrDefault();
        }

        /// <summary>
        /// Search payment/Not payment
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<UserMaintEntityPlus> UserMaintSearch(DataTablesModel dt, ref UserMaintListModel searchCondition, out int totalrow)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
                    main.COMPANY_CD,
	                main.CONTRACT_USER_SEQ_CD,
	                main.CONTRACT_USER_ACCOUNT,
	                main.CONTRACT_USER_NAME,
	                main.STAFF_ID,
	                sub2.AUTHORITY_GROUP_NAME,
	                main.PASSWORD_LAST_UPDATE_DATE,
	                main.UPD_DATE,
	                sub.CONTRACT_USER_NAME as UPD_USERNAME
                FROM
	                Mst_ContractFirmUser main
                LEFT JOIN 
	                Mst_ContractFirmUser sub
                ON
	                main.UPD_USER_ID = sub.CONTRACT_USER_SEQ_CD
                AND
	                main.COMPANY_CD = sub.COMPANY_CD
                LEFT JOIN
	                Mst_AuthorityGroup sub2 
                ON
	                main.AUTHORITY_GROUP_CD = sub2.AUTHORITY_GROUP_CD
                AND
                    main.COMPANY_CD = sub2.COMPANY_CD
                WHERE
                    main.DEL_FLG = @DEL_FLG
                    AND main.COMPANY_CD = @COMPANY_CD ");

            if (!String.IsNullOrEmpty(searchCondition.CONTRACT_USER_ACCOUNT))
            {
                sql.Append(@"
                    AND main.CONTRACT_USER_ACCOUNT LIKE @CONTRACT_USER_ACCOUNT ");
            }
            if (!String.IsNullOrEmpty(searchCondition.CONTRACT_USER_NAME))
            {
                sql.Append(@"
                    AND main.CONTRACT_USER_NAME LIKE @CONTRACT_USER_NAME ");
            }
            if (searchCondition.AUTHORITY_GROUP_CD != null)
            {
                sql.Append(@"
                    AND main.AUTHORITY_GROUP_CD = @AUTHORITY_GROUP_CD ");
            }
            if (searchCondition.STAFF_ID != null)
            {
                sql.Append(@"
                    AND main.STAFF_ID = @STAFF_ID ");
            }
            if (false.Equals(searchCondition.INCLUDE_DISABLE))
            {
                sql.Append(@"
                    AND main.DISABLE_FLG = @DISABLE_FLG ");
            }
            sql.Append(@"
                ORDER BY main.CONTRACT_USER_SEQ_CD ASC");

            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;

            var dataList = base.Query<UserMaintEntityPlus>(sqlpage,
                new
                {
                    COMPANY_CD = searchCondition.COMPANY_CD,
                    CONTRACT_USER_ACCOUNT = '%' + searchCondition.CONTRACT_USER_ACCOUNT + '%',
                    CONTRACT_USER_NAME = '%' + searchCondition.CONTRACT_USER_NAME + '%',
                    AUTHORITY_GROUP_CD = searchCondition.AUTHORITY_GROUP_CD,
                    STAFF_ID = searchCondition.STAFF_ID,
                    DISABLE_FLG = Constants.DisableFlag.Enable,
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    pageindex = lower,
                    pagesize = upper
                }).ToList();

            totalrow = base.Query<int>(sqlcount,
                new
                {
                    COMPANY_CD = searchCondition.COMPANY_CD,
                    CONTRACT_USER_ACCOUNT = '%' + searchCondition.CONTRACT_USER_ACCOUNT + '%',
                    CONTRACT_USER_NAME = '%' + searchCondition.CONTRACT_USER_NAME + '%',
                    AUTHORITY_GROUP_CD = searchCondition.AUTHORITY_GROUP_CD,
                    STAFF_ID = searchCondition.STAFF_ID,
                    DISABLE_FLG = Constants.DisableFlag.Enable,
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    pageindex = lower,
                    pagesize = upper
                }).FirstOrDefault();

            return dataList;
        }

        public IEnumerable<UserMaintModel> GetAUTHORITY_GROUP()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            SELECT
                AUTHORITY_GROUP_CD,
                AUTHORITY_GROUP_NAME
            FROM
                Mst_AuthorityGroup
            WHERE
                COMPANY_CD = @COMPANY_CD 
                DEL_FLG = @DEL_FLG");
            return base.Query<UserMaintModel>(sql.ToString(), new
            {
                DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                COMPANY_CD = base.CmnEntityModel.CompanyCd
            }).ToList();


        }
        #endregion

        #region UPDATE
        public long UpdateUserMaint(UserMaintEntity maint)
        {
            StringBuilder sql = new StringBuilder();
            maint.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;

            if (maint.AUTHORITY_GROUP_CD == null)
            {
                maint.AUTHORITY_GROUP_CD = 0;
            }

            sql.Append(@"
             UPDATE [dbo].[Mst_ContractFirmUser] 
             SET [CONTRACT_USER_ACCOUNT] = @CONTRACT_USER_ACCOUNT
                ,[CONTRACT_USER_NAME] = @CONTRACT_USER_NAME
                ,[STAFF_ID] = @STAFF_ID
                ,[AUTHORITY_GROUP_CD] = @AUTHORITY_GROUP_CD
                ,[LOGIN_LOCK_FLG] = @LOGIN_LOCK_FLG
                ,[SEARCH_AUTHORITY_TYPE] = @SEARCH_AUTHORITY_TYPE
                ,[SALES_ANALYSIS_AUTHORITY_TYPE] = @SALES_ANALYSIS_AUTHORITY_TYPE
                ,[DISABLE_FLG] = @DISABLE_FLG
                ,[DEL_FLG] = @DEL_FLG
                ,[UPD_DATE] = @UPD_DATE
                ,[UPD_USER_ID] = @UPD_USER_ID
                ,[UPD_PROG_ID] = @UPD_PROG_ID
                WHERE CONTRACT_USER_SEQ_CD = @CONTRACT_USER_SEQ_CD ");

            return base.DbUpdateByOutside(sql.ToString(), maint, new
            {
                CONTRACT_USER_SEQ_CD = maint.CONTRACT_USER_SEQ_CD
            });
        }

        public long UpdatePassword(UserMaintEntity maint)
        {
            StringBuilder sql = new StringBuilder();
            maint.COMPANY_CD = base.CmnEntityModel.CompanyCd;
            maint.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            sql.Append(@"
             UPDATE [dbo].[Mst_ContractFirmUser] 
             SET [CONTRACT_USER_PASSWORD] = @CONTRACT_USER_PASSWORD
                ,[PASSWORD_LAST_UPDATE_DATE] = @PASSWORD_LAST_UPDATE_DATE
                ,[UPD_DATE] = @UPD_DATE
                ,[UPD_USER_ID] = @UPD_USER_ID
                ,[UPD_PROG_ID] = @UPD_PROG_ID
                WHERE CONTRACT_USER_SEQ_CD = @CONTRACT_USER_SEQ_CD ");
            return base.Execute(sql.ToString(), new 
            { 
                CONTRACT_USER_SEQ_CD = maint.CONTRACT_USER_SEQ_CD,
                CONTRACT_USER_PASSWORD = SafePassword.GetSaltedPassword(maint.CONTRACT_USER_PASSWORD),
                PASSWORD_LAST_UPDATE_DATE = DateTime.Parse(Constants.Constant.SqlDateRange.MIN),
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = CmnEntityModel.UserSegNo,
                UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE,
            });
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Delete estimate
        /// </summary>
        /// <param name="SEQNO"></param>
        /// <returns></returns>
        public int DeleteUserMaint(long SEQNO)
        {
            int result = 0;

            // Declare database connection      
            StringBuilder sqlupdate = new StringBuilder();
            DateTime uptdate = Utility.GetCurrentDateTime();
            long updateuser = base.CmnEntityModel.UserSegNo;
            long staffid = base.CmnEntityModel.UserSegNo;

            sqlupdate.Append(" SELECT ");
            sqlupdate.Append("       CONTRACT_USER_SEQ_CD ");
            sqlupdate.Append(" FROM	    Mst_ContractFirmUser");
            sqlupdate.Append(" WHERE	    CONTRACT_USER_SEQ_CD = @CONTRACT_USER_SEQ_CD");

            // Execute sql command
            var project = base.Query<UserMaintEntity>(sqlupdate.ToString(), new { CONTRACT_USER_SEQ_CD = SEQNO }).FirstOrDefault();

            #region Delete Mst_ContractFirmUser
            var UserMaintDa = new UserMaintDa();
            result = Delete(SEQNO, updateuser);
            #endregion

            return result;
        }

        /// <summary>
        /// Delete Tbl_Estimate
        /// </summary>
        /// <param name="SEQNO"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int Delete(long SEQNO, long userID)
        {
            StringBuilder sqlupdate = new StringBuilder();
            sqlupdate.Append(" UPDATE Mst_ContractFirmUser");
            sqlupdate.Append(" SET ");
            sqlupdate.Append("    DEL_FLG = @DEL_FLG,");
            sqlupdate.Append("    UPD_DATE = @UPD_DATE,");
            sqlupdate.Append("    UPD_USER_ID = @UPD_USER_ID");

            sqlupdate.Append(" WHERE    CONTRACT_USER_SEQ_CD = @CONTRACT_USER_SEQ_CD");

            return base.Execute(sqlupdate.ToString(), new
            {
                CONTRACT_USER_SEQ_CD = SEQNO,
                DEL_FLG = Constants.DeleteFlag.DELETE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = userID
            });
        }

        #endregion

        /// <summary>
        /// Get contract type master model
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserMaintListModel> GetListGroupWithChilds(string companycd)
        {
            // Declare database connection      
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(@"
            SELECT
                COMPANY_CD,
                AUTHORITY_GROUP_CD,
                AUTHORITY_GROUP_NAME
            FROM
                Mst_AuthorityGroup
            WHERE
                DEL_FLG = @DEL_FLG
            ");

            if (!companycd.Equals(""))
            {
                sql.Append(@"
                     AND COMPANY_CD = @COMPANY_CD
                ");
            }

            var pics = base.Query<UserMaintListModel>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    COMPANY_CD = companycd
                }).ToList();

            // Create variable ouput   
            return pics;
        }

        /// <summary>
        /// GetAuxiliaryCode
        /// </summary>
        /// <returns></returns>
        public IList<UserMaintModel> GetAuxiliaryCode(string companyCd, string seqno, string account, string del)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            SELECT
                *
            FROM
                Mst_ContractFirmUser
                WHERE
                COMPANY_CD = @COMPANY_CD
                AND CONTRACT_USER_SEQ_CD <> @CONTRACT_USER_SEQ_CD
                AND CONTRACT_USER_ACCOUNT = @CONTRACT_USER_ACCOUNT
                AND DEL_FLG = 0");
            return base.Query<UserMaintModel>(sql.ToString(), new
            {
                COMPANY_CD = companyCd,
                CONTRACT_USER_SEQ_CD = seqno,
                CONTRACT_USER_ACCOUNT = account,
                DEL_FLG = del,
            }).ToList();
        }
    }
}

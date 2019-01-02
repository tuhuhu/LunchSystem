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
    public class SetUpUserMaintDa : BaseDa
    {

        #region CREATE

        public long InsertSetUpUserMaint(SetUpUserMaintEntity SetUpUserMaint)
        {
            SetUpUserMaint.PASSWORD_LAST_UPDATE_DATE = DateTime.Parse(Constants.Constant.SqlDateRange.MIN);
            SetUpUserMaint.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            SetUpUserMaint.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append(@"
                INSERT INTO Mst_SystemSetupUser
                (

                    SETUP_USER_ACCOUNT,
                    SETUP_USER_PASSWORD,
                    SETUP_USER_NAME,
                    LOGIN_LOCK_FLG,
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
                    @SETUP_USER_ACCOUNT,
                    @SETUP_USER_PASSWORD,
                    @SETUP_USER_NAME,
                    0,
                    @DISABLE_FLG,
                    @DEL_FLG,
                    @INS_DATE,
                    0,
                    @UPD_DATE,
                    0,
                    0
                ) ");
            SetUpUserMaint.SETUP_USER_PASSWORD = SafePassword.GetSaltedPassword(SetUpUserMaint.SETUP_USER_PASSWORD);
            var res = base.DbAddByOutside(sqlinsert.ToString(), SetUpUserMaint);
            if (res > 0)
            {
                var query = "SELECT ident_current('Mst_SystemSetupUser')";
                return base.ExecuteScalar<long>(query);
            }
            return 0;

        }

        #endregion


        #region READ
        /// <summary>
        /// 
        /// </summary>
        /// <param name="infoSeqNo"></param>
        /// <returns></returns>
        public SetUpUserMaintModel GetSetUpUserMaintInformation(long infoSeqNo)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT *
                FROM Mst_SystemSetupUser 
                WHERE SETUP_USER_SEQ_CD = @INFO_SEQ_NO ");
            return base.Query<SetUpUserMaintModel>(sql.ToString(), new
            {
                INFO_SEQ_NO = infoSeqNo
            }).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<SetUpUserMaintEntityPlus> SetUpUserMaintSearch(DataTablesModel dt, ref SetUpUserMaintListModel searchCondition, out int totalrow)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
                    main.SETUP_USER_SEQ_CD,
                    main.SETUP_USER_ACCOUNT,
	                main.SETUP_USER_NAME,
	                main.SETUP_USER_PASSWORD,
	                main.PASSWORD_LAST_UPDATE_DATE,
	                main.LOGIN_LOCK_FLG,
                    main.DISABLE_FLG
                FROM
	                Mst_SystemSetupUser main
                WHERE
                    main.DEL_FLG = @DEL_FLG");

            if (!String.IsNullOrEmpty(searchCondition.SETUP_USER_ACCOUNT))
            {
                sql.Append(@"
                    AND main.SETUP_USER_ACCOUNT LIKE @SETUP_USER_ACCOUNT ");
            }
            if (!String.IsNullOrEmpty(searchCondition.SETUP_USER_NAME))
            {
                sql.Append(@"
                    AND main.SETUP_USER_NAME LIKE @SETUP_USER_NAME ");
            }
            sql.Append(@"
                ORDER BY main.SETUP_USER_ACCOUNT COLLATE JAPANESE_BIN");

            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;

            var dataList = base.Query<SetUpUserMaintEntityPlus>(sqlpage,
                new
                {
                    SETUP_USER_ACCOUNT = searchCondition.SETUP_USER_ACCOUNT + '%',
                    SETUP_USER_NAME = searchCondition.SETUP_USER_NAME + '%',
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    pageindex = lower,
                    pagesize = upper
                }).ToList();

            totalrow = base.Query<int>(sqlcount,
                new
                {
                    SETUP_USER_ACCOUNT = searchCondition.SETUP_USER_ACCOUNT + '%',
                    SETUP_USER_NAME = searchCondition.SETUP_USER_NAME + '%',
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    pageindex = lower,
                    pagesize = upper
                }).FirstOrDefault();

            return dataList;
        }

        #endregion


        #region UPDATE

        /// <summary>
        /// アップデータ
        /// </summary>
        /// <param name="maint"></param>
        /// <returns></returns>
        public long UpdateSetUpUserMaint(SetUpUserMaintEntity maint)
        {
            StringBuilder sql = new StringBuilder();
            maint.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;

            sql.Append(@"
           UPDATE [dbo].[Mst_SystemSetupUser] 
             SET [SETUP_USER_ACCOUNT] = @SETUP_USER_ACCOUNT
                ,[SETUP_USER_NAME] = @SETUP_USER_NAME
                ,[LOGIN_LOCK_FLG] = @LOGIN_LOCK_FLG          
                ,[DISABLE_FLG] = @DISABLE_FLG
                ,[DEL_FLG] = @DEL_FLG
                ,[UPD_DATE] = @UPD_DATE
                ,[UPD_USER_ID] = @UPD_USER_ID
                ,[UPD_PROG_ID] = @UPD_PROG_ID
                WHERE SETUP_USER_SEQ_CD = @SETUP_USER_SEQ_CD");

            return base.DbUpdateByOutside(sql.ToString(), maint, new
            {
                SETUP_USER_SEQ_CD = maint.SETUP_USER_SEQ_CD
            });

        }

        /// <summary>
        /// パスワードをアップデータ
        /// </summary>
        /// <param name="maint"></param>
        /// <returns></returns>
        public long UpdatePassword(SetUpUserMaintEntity maint)
        {
            StringBuilder sql = new StringBuilder();
            maint.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            sql.Append(@"
             UPDATE [dbo].[Mst_SystemSetupUser] 
             SET [SETUP_USER_PASSWORD] = @SETUP_USER_PASSWORD
                ,[PASSWORD_LAST_UPDATE_DATE] = @PASSWORD_LAST_UPDATE_DATE
                ,[UPD_DATE] = @UPD_DATE
                ,[UPD_USER_ID] = @UPD_USER_ID
                ,[UPD_PROG_ID] = @UPD_PROG_ID
                WHERE SETUP_USER_SEQ_CD = @SETUP_USER_SEQ_CD ");
            return base.Execute(sql.ToString(), new
                {
                    SETUP_USER_SEQ_CD = maint.SETUP_USER_SEQ_CD,
                    SETUP_USER_PASSWORD = SafePassword.GetSaltedPassword(maint.SETUP_USER_PASSWORD),
                    PASSWORD_LAST_UPDATE_DATE = DateTime.Parse("1/1/1901"),
                    UPD_DATE = Utility.GetCurrentDateTime(),
                    UPD_USER_ID = CmnEntityModel.UserSegNo,
                    UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE,
                });

        }

        #endregion


        #region DELETE

        /// <summary>
        /// デリート
        /// </summary>
        /// <param name="SEQNO"></param>
        /// <returns></returns>
        public int DeleteSetUpUserMaint(long SEQNO)
        {
            int result = 0;

            StringBuilder sqlupdate = new StringBuilder();
            DateTime update = Utility.GetCurrentDateTime();
            long updateuser = base.CmnEntityModel.UserSegNo;
            
            sqlupdate.Append(" SELECT" );
            sqlupdate.Append("     SETUP_USER_SEQ_CD");
            sqlupdate.Append(" FROM     Mst_SystemSetupUser");
            sqlupdate.Append(" WHERE        SETUP_USER_SEQ_CD = @SETUP_USER_SEQ_CD");

            var project = base.Query<SetUpUserMaintEntity>(sqlupdate.ToString(), new { SETUP_USER_SEQ_CD = SEQNO }).FirstOrDefault();

            var SetUpUserMaintDa = new SetUpUserMaintDa();
            result = Delete(SEQNO, updateuser);

            return result;

        }

        public int Delete(long SEQNO,long userID)
        {
            StringBuilder sqlupdate = new StringBuilder();
            sqlupdate.Append(" UPDATE Mst_SystemSetupUser");
            sqlupdate.Append(" SET ");
            sqlupdate.Append("      DEL_FLG = @DEL_FLG,");
            sqlupdate.Append("      UPD_DATE = @UPD_DATE,");
            sqlupdate.Append("      UPD_USER_ID = @UPD_USER_ID");

            sqlupdate.Append(" WHERE    SETUP_USER_SEQ_CD = @SETUP_USER_SEQ_CD");

            return base.Execute(sqlupdate.ToString(), new
                {
                    SETUP_USER_SEQ_CD = SEQNO,
                    DEL_FLG = Constants.DeleteFlag.DELETE,
                    UPD_DATE = Utility.GetCurrentDateTime(),
                    UPD_USER_ID = userID
                });
        }


        #endregion


        /// <summary>
        /// GetAuxiliaryCode
        /// </summary>
        /// <returns></returns>
        public IList<SetUpUserMaintModel> GetAuxiliaryCode(string companyCd, string seqno, string account, string del)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            SELECT
                *
            FROM
                Mst_SystemSetupUser
                WHERE
                SETUP_USER_SEQ_CD <> @SETUP_USER_SEQ_CD
                AND SETUP_USER_ACCOUNT = @SETUP_USER_ACCOUNT
                AND DEL_FLG = 0");
            return base.Query<SetUpUserMaintModel>(sql.ToString(), new
            {
                SETUP_USER_SEQ_CD = seqno,
                SETUP_USER_ACCOUNT = account,
                DEL_FLG = del,
            }).ToList();
        }



    }
}

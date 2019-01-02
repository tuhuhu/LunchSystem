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

namespace SystemSetup.DataAccess
{
    public class SystemStatusDa : BaseDa
    {
        #region CREATE
        /// <summary>
        /// Insert into 
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        public long InsertSystemStatus(SystemStatusEntity model)
        {
            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append(@"
                INSERT INTO Mst_SystemStatus
                (
                    SYSTEM_OPERATION_MODE,
                    SYSTEM_STOPPED_MESSAGE,
                    NOTICE_FLG,
                    NOTICE_TITLE,
                    NOTICE_MESSAGE,
                    DEL_FLG,
                    INS_DATE,
                    INS_USER_ID,
                    UPD_DATE,
                    UPD_USER_ID,
                    UPD_PROG_ID
                )
                VALUES
                (
                    @SYSTEM_OPERATION_MODE,
                    @SYSTEM_STOPPED_MESSAGE,
                    @NOTICE_FLG,
                    @NOTICE_TITLE,
                    @NOTICE_MESSAGE,
                    @DEL_FLG,
                    @INS_DATE,
                    @INS_USER_ID,
                    @UPD_DATE,
                    @UPD_USER_ID,
                    @UPD_PROG_ID
                ) ");
            var res = base.Execute(sqlinsert.ToString(), model);
            return res;
        }

        public SystemStatusModel GetSystemStatus()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
                    *
                FROM
                    Mst_SystemStatus
            ");
            return base.SingleOrDefault<SystemStatusModel>(sql.ToString());
        }
        #endregion

        //        /// <summary>
        //        /// Search payment/Not payment
        //        /// </summary>
        //        /// <param name="dt"></param>
        //        /// <param name="searchCondition"></param>
        //        /// <param name="totalrow"></param>
        //        /// <returns></returns>
        //        public IEnumerable<SystemStatusEntityPlus> SystemStatusSearch(DataTablesModel dt, ref SystemStatusListModel searchCondition, out int totalrow)
        //        {
        //            StringBuilder sql = new StringBuilder();
        //            sql.Append(@"
        //                SELECT i.INFO_SEQ_NO, i.CONTENT, i. PUBLISH_DATE_START, i.PUBLISH_DATE_END, i.DSP_PRIORITY, i.UPD_DATE, u.CONTRACT_USER_NAME as UPD_USERNAME
        //                FROM
        //                    Tbl_Information i
        //                LEFT JOIN
        //                    Mst_ContractFirmUser u
        //                    ON i.UPD_USER_ID = u.CONTRACT_USER_SEQ_CD
        //                    AND i.COMPANY_CD = u.COMPANY_CD
        //                WHERE
        //                    i.COMPANY_CD = @COMPANY_CD ");

        //            if (false.Equals(searchCondition.INCLUDE_DELETED))
        //            {
        //                sql.Append(@"
        //                    AND i.DEL_FLG = @DEL_FLG ");
        //            }
        //            sql.Append(@"
        //                ORDER BY i.INFO_SEQ_NO ASC");

        //            int lower = dt.iDisplayStart + 1;
        //            int upper = dt.iDisplayStart + dt.iDisplayLength;

        //            PagingHelper.SQLParts parts;
        //            PagingHelper.SplitSQL(sql.ToString(), out parts);

        //            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
        //            string sqlcount = parts.sqlCount;

        //            var dataList = base.Query<SystemStatusEntityPlus>(sqlpage,
        //                new
        //                {
        //                    COMPANY_CD = base.CmnEntityModel.CompanyCd,
        //                    CONTENT = '%' + searchCondition.CONTENT + '%',
        //                    START_DATE = searchCondition.START_DATE,
        //                    END_DATE = searchCondition.END_DATE,
        //                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
        //                    pageindex = lower,
        //                    pagesize = upper
        //                }).ToList();

        //            totalrow = base.Query<int>(sqlcount,
        //                new
        //                {
        //                    COMPANY_CD = base.CmnEntityModel.CompanyCd,
        //                    CONTENT = '%' + searchCondition.CONTENT + '%',
        //                    START_DATE = searchCondition.START_DATE,
        //                    END_DATE = searchCondition.END_DATE,
        //                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
        //                    pageindex = lower,
        //                    pagesize = upper
        //                }).FirstOrDefault();

        //            return dataList;
        //        }
        //        #endregion

        #region UPDATE
        public long UpdateSystemStatusModel(SystemStatusEntity systemstatus)
        {
            StringBuilder sql = new StringBuilder();
            systemstatus.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;

            sql.Append(@"
             UPDATE [dbo].[Mst_SystemStatus] 
             SET [SYSTEM_OPERATION_MODE] = @SYSTEM_OPERATION_MODE
                ,[SYSTEM_STOPPED_MESSAGE] = @SYSTEM_STOPPED_MESSAGE
                ,[NOTICE_FLG] = @NOTICE_FLG
                ,[NOTICE_TITLE] = @NOTICE_TITLE
                ,[NOTICE_MESSAGE] = @NOTICE_MESSAGE
                ,[DEL_FLG] = @DEL_FLG
                ,[UPD_DATE] = @UPD_DATE
                ,[UPD_USER_ID] = @UPD_USER_ID
                ,[UPD_PROG_ID] = @UPD_PROG_ID
                 ");
            return base.DbUpdate(sql.ToString(), systemstatus, new { });
        }
        #endregion

        /// <summary>
        /// Get a row from SystemStatus
        /// </summary>
        /// <param name="infoSeqNo"></param>
        /// <returns></returns>
        public SystemStatusModel GetInformation(string operationMode)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
                    *
                FROM
	                Mst_SystemStatus
                WHERE
                    SYSTEM_OPERATION_MODE = @SYSTEM_OPERATION_MODE;
            ");

            return base.Query<SystemStatusModel>(sql.ToString(), new
            {
                SYSTEM_OPERATION_MODE = operationMode,
            }).FirstOrDefault();
        }


        /// <summary>
        /// GetAuxiliaryCode
        /// </summary>
        /// <returns></returns>
        public IList<SystemStatusModel> GetAuxiliaryCode(int? notice, string noticeTitle)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            SELECT
                *
            FROM
                Mst_SystemStatus
                WHERE
                NOTICE_FLG = @NOTICE_FLG
                AND NOTICE_TITLE = @NOTICE_TITLE
                AND DEL_FLG = 0");
            return base.Query<SystemStatusModel>(sql.ToString(), new
            {
                NOTICE_FLG = notice,
                NOTICE_TITLE = noticeTitle,
            }).ToList();
        }
    }
}
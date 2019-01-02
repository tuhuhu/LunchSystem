using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SystemSetup.Models;
using SystemSetup.Constants;
using SystemSetup.UtilityServices.PagingHelper;

namespace SystemSetup.DataAccess
{
    public class DispatchContractOverheadDa : BaseDa
    {
        #region Dispatch Contract Overhead
        /// <summary>
        /// Search Dispatch Contract Overhead
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<DispatchContractOverheadModel> DispatchContractOverheadSearch(DataTablesModel dt, ref DispatchContractOverheadModel searchCondition, out int totalrow)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                    SELECT 
	                      [FORMAT_SEQ_NO]
                          ,[FORMAT_SUB_TYPE]
                          ,[FORMAT_DISP_NAME]
                          ,[FORMAT_PATH]
                          ,[DISABLE_FLG]
	                      ,[DEL_FLG]
                    FROM [dbo].[Mst_ReportFormat]");
            if (!String.IsNullOrEmpty(searchCondition.COMPANY_CD))
            {
                sql.Append(@"
                    WHERE COMPANY_CD = @COMPANY_CD 
                    AND DEL_FLG = @DEL_FLG 
                    AND FORMAT_TYPE = @FORMAT_TYPE
                    AND FORMAT_SUB_TYPE = @FORMAT_SUB_TYPE");
            }

            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;

            var dataList = base.Query<DispatchContractOverheadModel>(sql.ToString(),
               new
               {
                   COMPANY_CD = searchCondition.COMPANY_CD,
                   DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                   FORMAT_TYPE = Constants.FormatType.DispatchType,
                   FORMAT_SUB_TYPE = Constants.FormatSubType.Billing,
                   pageindex = lower,
                   pagesize = upper
               }).ToList();

            totalrow = base.Query<int>(sqlcount,
                new
                {
                    COMPANY_CD = searchCondition.COMPANY_CD,
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    FORMAT_TYPE = Constants.FormatType.DispatchType,
                    FORMAT_SUB_TYPE = Constants.FormatSubType.Billing,
                    pageindex = lower,
                    pagesize = upper
                }).FirstOrDefault();

            return dataList;
        }

        /// <summary>
        /// Update Dispatch Contract Overhead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long UpdateDispatchContractOverhead(DispatchContractOverheadEntity model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                        UPDATE [dbo].[Mst_ReportFormat]
                        SET 
                              [FORMAT_DISP_NAME] = @FORMAT_DISP_NAME
                              ,[FORMAT_PATH] = @FORMAT_PATH
                              ,[DISABLE_FLG] = @DISABLE_FLG
                              ,[DEL_FLG] = @DEL_FLG
	                          ,[UPD_USER_ID] = @UPD_USER_ID
                              ,[UPD_PROG_ID] = @UPD_PROG_ID
                        WHERE [FORMAT_SEQ_NO] = @FORMAT_SEQ_NO
                        ");
            return base.DbUpdate(sql.ToString(), model, new { FORMAT_SEQ_NO = model.FORMAT_SEQ_NO });
        }

        /// <summary>
        /// Insert Dispatch Contract Overhead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long InsertDispatchContractOverhead(DispatchContractOverheadEntity model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                   INSERT INTO [dbo].[Mst_ReportFormat]
                          ([COMPANY_CD]
                          ,[FORMAT_TYPE]
                          ,[FORMAT_SUB_TYPE]
                          ,[FORMAT_DISP_NAME]
                          ,[FORMAT_PATH]
                          ,[FORMAT_DETAIL_LINE]
                          ,[DISABLE_FLG]
                          ,[DEL_FLG]
                          ,[INS_DATE]
                          ,[INS_USER_ID]
                          ,[UPD_DATE]
                          ,[UPD_USER_ID]
                          ,[UPD_PROG_ID]
	                    )
                    VALUES
                          (@COMPANY_CD
                          ,@FORMAT_TYPE
                          ,@FORMAT_SUB_TYPE
                          ,@FORMAT_DISP_NAME
                          ,@FORMAT_PATH
                          ,@FORMAT_DETAIL_LINE
                          ,@DISABLE_FLG
                          ,@DEL_FLG
                          ,@INS_DATE
                          ,@INS_USER_ID
                          ,@UPD_DATE
                          ,@UPD_USER_ID
                          ,@UPD_PROG_ID
	                    )
                    ");
            return base.DbAdd(sql.ToString(), model);
        }
        #endregion

        #region Payment Dispatch Contract Overhead
        /// <summary>
        /// Search Payment Dispatch Contract Overhead
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<DispatchContractOverheadModel> PaymentDispatchContractOverheadSearch(DataTablesModel dt, ref DispatchContractOverheadModel searchCondition, out int totalrow)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                    SELECT 
	                      [FORMAT_SEQ_NO]
                          ,[FORMAT_SUB_TYPE]
                          ,[FORMAT_DISP_NAME]
                          ,[FORMAT_PATH]
                          ,[DISABLE_FLG]
	                      ,[DEL_FLG]
                    FROM [dbo].[Mst_ReportFormat]");
            if (!String.IsNullOrEmpty(searchCondition.COMPANY_CD))
            {
                sql.Append(@"
                    WHERE COMPANY_CD = @COMPANY_CD 
                    AND  DEL_FLG = @DEL_FLG
                    AND FORMAT_TYPE = @FORMAT_TYPE
                    AND FORMAT_SUB_TYPE = @FORMAT_SUB_TYPE");
            }

            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;

            var dataList = base.Query<DispatchContractOverheadModel>(sql.ToString(),
               new
               {
                   COMPANY_CD = searchCondition.COMPANY_CD,
                   DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                   FORMAT_TYPE = Constants.FormatType.DispatchType,
                   FORMAT_SUB_TYPE = Constants.FormatSubType.Payment,
                   pageindex = lower,
                   pagesize = upper
               }).ToList();

            totalrow = base.Query<int>(sqlcount,
                new
                {
                    COMPANY_CD = searchCondition.COMPANY_CD,
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    FORMAT_TYPE = Constants.FormatType.DispatchType,
                    FORMAT_SUB_TYPE = Constants.FormatSubType.Payment,
                    pageindex = lower,
                    pagesize = upper
                }).FirstOrDefault();

            return dataList;
        }
        /// <summary>
        /// Update Payment Dispatch Contract Overhead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long UpdatePaymentDispatchContractOverhead(DispatchContractOverheadEntity model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                        UPDATE [dbo].[Mst_ReportFormat]
                        SET 
                              [FORMAT_DISP_NAME] = @FORMAT_DISP_NAME
                              ,[FORMAT_PATH] = @FORMAT_PATH
                              ,[DISABLE_FLG] = @DISABLE_FLG
                              ,[DEL_FLG] = @DEL_FLG
	                          ,[UPD_USER_ID] = @UPD_USER_ID
                              ,[UPD_PROG_ID] = @UPD_PROG_ID
                        WHERE [FORMAT_SEQ_NO] = @FORMAT_SEQ_NO
                        ");
            return base.DbUpdate(sql.ToString(), model, new { FORMAT_SEQ_NO = model.FORMAT_SEQ_NO });
        }

        /// <summary>
        /// Insert Payment Dispatch Contract Overhead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long InsertPaymentDispatchContractOverhead(DispatchContractOverheadEntity model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                   INSERT INTO [dbo].[Mst_ReportFormat]
                          ([COMPANY_CD]
                          ,[FORMAT_TYPE]
                          ,[FORMAT_SUB_TYPE]
                          ,[FORMAT_DISP_NAME]
                          ,[FORMAT_PATH]
                          ,[FORMAT_DETAIL_LINE]
                          ,[DISABLE_FLG]
                          ,[DEL_FLG]
                          ,[INS_DATE]
                          ,[INS_USER_ID]
                          ,[UPD_DATE]
                          ,[UPD_USER_ID]
                          ,[UPD_PROG_ID]
	                    )
                    VALUES
                          (@COMPANY_CD
                          ,@FORMAT_TYPE
                          ,@FORMAT_SUB_TYPE
                          ,@FORMAT_DISP_NAME
                          ,@FORMAT_PATH
                          ,@FORMAT_DETAIL_LINE
                          ,@DISABLE_FLG
                          ,@DEL_FLG
                          ,@INS_DATE
                          ,@INS_USER_ID
                          ,@UPD_DATE
                          ,@UPD_USER_ID
                          ,@UPD_PROG_ID
	                    )
                    ");
            return base.DbAdd(sql.ToString(), model);
        }

        #endregion
    }
}

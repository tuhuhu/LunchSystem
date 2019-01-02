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
    public class BillingNumberMaintDa : BaseDa
    {
        #region READ
        /// <summary>
        /// Search billing number list
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<BillingNumberMaintEntityPlus> BillingNumberMaintSearch(DataTablesModel dt, ref BillingNumberMaintModel searchCondition, out int totalrow)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
               SELECT cf.COMPANY_NAME,cf.COMPANY_CD,p.*
               FROM Mst_ContractFirm cf 
               LEFT JOIN Rel_ContractFirmPlan cfp
                   ON cf.PLAN_REL_SEQ_NO = cfp.PLAN_REL_SEQ_NO   
                   AND cf.COMPANY_CD = cfp.COMPANY_CD         
                   AND cfp.DEL_FLG = @DEL_FLG
               LEFT JOIN Mst_Plan p                
                   ON cfp.PLAN_SEQ_NO =  p.PLAN_SEQ_NO       
                   AND p.DEL_FLG = @DEL_FLG
                   AND p.DISABLE_FLG = @DISABLE_FLG           
               WHERE cf.COMPANY_NAME LIKE @COMPANY_NAME
                   AND cf.DEL_FLG = @DEL_FLG
               ORDER BY cf.COMPANY_NAME");

            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;

            var dataList = base.Query<BillingNumberMaintEntityPlus>(sqlpage,
                new
                {
                    COMPANY_NAME = '%'+searchCondition.SEARCH_COMPANY_NAME+'%',
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    DISABLE_FLG = DisableFlag.Enable,
                    pageindex = lower,
                    pagesize = upper
                }).ToList();

            totalrow = base.Query<int>(sqlcount,
                new
                {
                    COMPANY_NAME = '%'+searchCondition.SEARCH_COMPANY_NAME+'%',
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    DISABLE_FLG = DisableFlag.Enable,
                    pageindex = lower,
                    pagesize = upper
                }).FirstOrDefault();

            return dataList;
        }

        /// <summary>
        /// Get Billing Number Data
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <returns></returns>
        public IEnumerable<BillingNumberMaintEntityPlus> GetBillingNumberData(BillingNumberMaintModel searchCondition)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT cf.COMPANY_NAME,cf.COMPANY_CD,p.*
                FROM Mst_ContractFirm cf 
                LEFT JOIN Rel_ContractFirmPlan cfp
                    ON cf.PLAN_REL_SEQ_NO = cfp.PLAN_REL_SEQ_NO   
                    AND cf.COMPANY_CD = cfp.COMPANY_CD         
                    AND cfp.DEL_FLG = @DEL_FLG
                LEFT JOIN Mst_Plan p                
                    ON cfp.PLAN_SEQ_NO =  p.PLAN_SEQ_NO       
                    AND p.DEL_FLG = @DEL_FLG
                    AND p.DISABLE_FLG = @DISABLE_FLG           
                WHERE cf.COMPANY_NAME LIKE @COMPANY_NAME
                    AND cf.DEL_FLG = @DEL_FLG
                ORDER BY cf.COMPANY_NAME");

            var dataList = base.Query<BillingNumberMaintEntityPlus>(sql.ToString(),
                new
                {
                    COMPANY_NAME = '%' + searchCondition.SEARCH_COMPANY_NAME + '%',
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    DISABLE_FLG = DisableFlag.Enable
                }).ToList();
            return dataList;
        }

        /// <summary>
        /// Get Billing Row Count By YearMonth
        /// </summary>
        /// <param name="companyCd"></param>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        public int GetBillingRowCountByYearMonth(string companyCd, string yearMonth)
        {
            // Declare database connection      
            StringBuilder sql = new StringBuilder();
            // SQL発行
            sql.Append(@"
                SELECT COUNT(*) FROM 
                Tbl_ContractBilling tbContractBilling                
                LEFT JOIN Tbl_Contract tbContract
                    ON tbContract.COMPANY_CD = tbContractBilling.COMPANY_CD 
                    AND tbContract.CONT_SEQ_NO = tbContractBilling.CONT_SEQ_NO                 
                LEFT JOIN Tbl_ProjectsState ps
                    ON ps.PRJ_SEQ_NO = tbContractBilling.PRJ_SEQ_NO
                    AND ps.COMPANY_CD = tbContractBilling.COMPANY_CD
                WHERE tbContractBilling.COMPANY_CD = @COMPANY_CD
                AND tbContractBilling.BILLING_YM = @BILLING_YM
                AND tbContract.CONTRACT_STATUS != @CONTRACT_STATUS
                AND CAST(ps.PRJ_STATE AS INT) < @PRJ_STATE_SELECT_LIMIT
                AND tbContractBilling.DEL_FLG = @DEL_FLG");
            var count = base.Query<int>(sql.ToString(),
                new
                {
                    COMPANY_CD = companyCd,
                    BILLING_YM = yearMonth,
                    CONTRACT_STATUS = ContractStatus.Create,
                    PRJ_STATE_SELECT_LIMIT = Constants.ProjectState.PRJ_STATE_SELECT_LIMIT,
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).ToArray();
            return count[0];
        }


        public decimal GetFileSizeTotalbyCompany(string companyCd)
        {
            // Declare database connection      
            StringBuilder sql = new StringBuilder();
            // SQL発行
            sql.Append(@"

                SELECT sum (DT.FILE_SIZE) as FILE_SIZE_TOTAL from 
                Tbl_UploadFilesDetails DT
                LEFT JOIN [Tbl_UploadFiles] UF
                ON DT.UPLOAD_FILE_SEQ_NO = UF.UPLOAD_FILE_SEQ_NO
                WHERE DT.UPLOAD_FILE_SEQ_NO IN (
                SELECT UPLOAD_FILE_SEQ_NO from Tbl_UploadFiles
                WHERE [COMPANY_CD] = @COMPANY_CD
                AND [DEL_FLG] = @DEL_FLG ) ");

            var result = base.Query<BillingNumberMaintEntity>(sql.ToString(),
                new
                {
                    COMPANY_CD = companyCd,
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).FirstOrDefault();

            decimal sizeUpload = result != null ? result.FILE_SIZE_TOTAL : 0;
            sizeUpload = (decimal)sizeUpload / 1024;
            sizeUpload = (decimal)((long)(sizeUpload * 100)) / 100;

            return sizeUpload;
        }
        #endregion
    }
}

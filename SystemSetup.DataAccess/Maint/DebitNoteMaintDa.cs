using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemSetup.Models;
using SystemSetup.UtilityServices.PagingHelper;

namespace SystemSetup.DataAccess
{
    public class DebitNoteMaintDa: BaseDa
    {
        /// <summary>
        /// Search Debit Note
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<DebitNoteFormatModel> DebitNoteFormatSearch(DataTablesModel dt, ref DebitNoteFormatModel searchCondition, out int totalrow)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                    SELECT * FROM Mst_AdditionalBillingFormat  ");
            if (!String.IsNullOrEmpty(searchCondition.COMPANY_CD))
            {
                sql.Append(@"
                    WHERE COMPANY_CD = @COMPANY_CD AND DEL_FLG = @DEL_FLG  ");
            }
            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;


            var dataList = base.Query<DebitNoteFormatModel>(sql.ToString(),
                new
                {
                    COMPANY_CD = searchCondition.COMPANY_CD,
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    pageindex = lower,
                    pagesize = upper
                }).ToList();

            totalrow = base.Query<int>(sqlcount,
                new
                {
                    COMPANY_CD = searchCondition.COMPANY_CD,
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    pageindex = lower,
                    pagesize = upper
                }).FirstOrDefault();

            return dataList;
        }
        /// <summary>
        /// Update Debit Note
        /// </summary>
        /// <param name="BillingPrepare"></param>
        /// <returns></returns>
        public int UpdateDebitNote(DebitNoteFormatEntity model)
        {
            StringBuilder sqlupdate = new StringBuilder();
            sqlupdate.Append(@"
                UPDATE [dbo].[Mst_AdditionalBillingFormat]
                   SET [BILLING_FORMAT_DISP_NAME] = @BILLING_FORMAT_DISP_NAME
                      ,[BILLING_FORMAT_PATH] = @BILLING_FORMAT_PATH
                      ,[BILLING_FORMAT_DETAIL_LINE] = @BILLING_FORMAT_DETAIL_LINE
                      ,[DISABLE_FLG] = @DISABLE_FLG
                      ,[DEL_FLG] = @DEL_FLG
                      ,[UPD_DATE] = @UPD_DATE
                      ,[UPD_USER_ID] = @UPD_USER_ID
                      ,[UPD_PROG_ID] = @UPD_PROG_ID
                 WHERE [BILLING_ADD_FORMAT_SEQ_NO] = @BILLING_ADD_FORMAT_SEQ_NO
                        ");

            return base.DbUpdate(sqlupdate.ToString(), model, new { BILLING_ADD_FORMAT_SEQ_NO = model.BILLING_ADD_FORMAT_SEQ_NO });
        }
        /// <summary>
        /// Insert Debit Note
        /// </summary>
        /// <param name="BillingPrepare"></param>
        /// <returns></returns>
        public int InsertDebitNote(DebitNoteFormatEntity model)
        {
            int result = 0;
            StringBuilder sqlinsert = new StringBuilder();
                sqlinsert.Append(@" 
                    INSERT INTO [Mst_AdditionalBillingFormat] 
                        ([COMPANY_CD]
                        ,[BILLING_TYPE]
                        ,[BILLING_FORMAT_DISP_NAME]
                        ,[BILLING_FORMAT_PATH]
                        ,[BILLING_FORMAT_DETAIL_LINE]
                        ,[DISABLE_FLG]
                        ,[DEL_FLG]
                        ,[INS_DATE]
                        ,[INS_USER_ID]
                        ,[UPD_DATE]
                        ,[UPD_USER_ID]
                        ,[UPD_PROG_ID])
                    VALUES
                        (@COMPANY_CD,
                        @BILLING_TYPE,
                        @BILLING_FORMAT_DISP_NAME,
                        @BILLING_FORMAT_PATH,
                        @BILLING_FORMAT_DETAIL_LINE,
                        @DISABLE_FLG,
                        @DEL_FLG,
                        @INS_DATE,
                        @INS_USER_ID,
                        @UPD_DATE,
                        @UPD_USER_ID,
                        @UPD_PROG_ID)
                        ");

                // Execute sql command to insert into Mst_BusinessPartnerCompany
                result = base.DbAdd(sqlinsert.ToString(), model);
                return result;
            }
    }
}

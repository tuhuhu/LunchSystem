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
    public class TaxRateMaintDa : BaseDa
    {
        #region READ

        /// <summary>
        /// Search payment/Not payment
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<TaxRateMaintModel> TaxRateMaintSearch(DataTablesModel dt, ref TaxRateMaintModel searchCondition, out int totalrow)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT
					mfcc.TAX_RATE_ID,
	                mfcc.APPLY_DATE_FROM,
	                mfcc.APPLY_DATE_TO,
	                mfcc.TAX_RATE,
	                mfcc.UPD_DATE,
	                mcfu.CONTRACT_USER_NAME AS UPD_USERNAME
                FROM
	                Mst_TaxRate mfcc
                        LEFT JOIN Mst_ContractFirmUser mcfu ON
                            mfcc.UPD_USER_ID = mcfu.CONTRACT_USER_SEQ_CD
                WHERE
                    mfcc.DEL_FLG = 0
            ");

            if (!String.IsNullOrEmpty(searchCondition.SEARCH_APPLY_DATE_FROM))
            {
                sql.Append(@"
                    AND mfcc.APPLY_DATE_TO >= @SEARCH_APPLY_DATE_FROM  ");
            }

            if (!String.IsNullOrEmpty(searchCondition.SEARCH_APPLY_DATE_TO))
            {
                sql.Append(@"
                    AND mfcc.APPLY_DATE_FROM <= @SEARCH_APPLY_DATE_TO  ");
            }

            if (!String.IsNullOrEmpty(searchCondition.SEARCH_TAX_RATE))
            {
                sql.Append(@"
                    AND mfcc.TAX_RATE = @SEARCH_TAX_RATE  ");
            }

            sql.Append(@"
                ORDER BY mfcc.APPLY_DATE_TO DESC");

            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;

            var dataList = base.Query<TaxRateMaintModel>(sqlpage,
                new
                {
                    SEARCH_APPLY_DATE_FROM = searchCondition.SEARCH_APPLY_DATE_FROM,
                    SEARCH_APPLY_DATE_TO = searchCondition.SEARCH_APPLY_DATE_TO,
                    SEARCH_TAX_RATE = searchCondition.SEARCH_TAX_RATE,
                    pageindex = lower,
                    pagesize = upper
                }).ToList();

            totalrow = base.Query<int>(sqlcount,
                new
                {
                    SEARCH_APPLY_DATE_FROM = searchCondition.SEARCH_APPLY_DATE_FROM,
                    SEARCH_APPLY_DATE_TO = searchCondition.SEARCH_APPLY_DATE_TO,
                    SEARCH_TAX_RATE = searchCondition.SEARCH_TAX_RATE,
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
        public TaxRateMaintModel GetInformation(long TaxRateID)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
					mfcc.TAX_RATE_ID,
	                mfcc.APPLY_DATE_FROM,
	                mfcc.APPLY_DATE_TO,
	                mfcc.TAX_RATE,
                    mfcc.DEL_FLG,
	                mfcc.UPD_DATE,
                    mfcc.UPD_USER_ID,
	                mcfu_upd.CONTRACT_USER_NAME AS UPD_USERNAME,
                    mfcc.INS_DATE,
                    mfcc.INS_USER_ID,
	                mcfu_ins.CONTRACT_USER_NAME AS INS_USERNAME
                FROM
	                Mst_TaxRate mfcc
                        LEFT JOIN Mst_ContractFirmUser mcfu_upd ON
                            mfcc.UPD_USER_ID = mcfu_upd.CONTRACT_USER_SEQ_CD
                        LEFT JOIN Mst_ContractFirmUser mcfu_ins ON
                            mfcc.INS_USER_ID = mcfu_ins.CONTRACT_USER_SEQ_CD
                WHERE
                    mfcc.TAX_RATE_ID = @TAX_RATE_ID
            ");

            return base.Query<TaxRateMaintModel>(sql.ToString(), new
            {
                TAX_RATE_ID = TaxRateID,
            }).FirstOrDefault();
        }

        #endregion

        #region UPDATE
        public long UpdateTaxRateMaintModel(TaxRateEntity maint)
        {
            StringBuilder sql = new StringBuilder();
            //maint.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            sql.Append(@"
                UPDATE [dbo].[Mst_TaxRate] 
                    SET [APPLY_DATE_FROM] = @APPLY_DATE_FROM
                    ,[APPLY_DATE_TO] = @APPLY_DATE_TO
                    ,[TAX_RATE] = @TAX_RATE
                    ,[UPD_DATE] = @UPD_DATE
                    ,[UPD_USER_ID] = @UPD_USER_ID
                    ,[UPD_PROG_ID] = @UPD_PROG_ID
                WHERE TAX_RATE_ID = @TAX_RATE_ID 
            ");
            return base.Execute(sql.ToString(), new
            {
                APPLY_DATE_FROM = maint.APPLY_DATE_FROM,
                APPLY_DATE_TO = maint.APPLY_DATE_TO,
                TAX_RATE = maint.TAX_RATE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = 0,
                UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE,
                TAX_RATE_ID = maint.TAX_RATE_ID,
            });
            //return base.DbUpdate(sql.ToString(), maint, new
            //{
            //    TAX_RATE_ID = maint.TAX_RATE_ID,
            //    //UPD_USER_ID = 0,
            //});
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Delete Mst_TaxRate
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int DeleteTaxRateMaint(int TAX_RATE_ID = 0)
        {
            int result = 0;

            #region Delete Mst_TaxRate
            result = Delete(TAX_RATE_ID);
            #endregion

            return result;
        }

        /// <summary>
        /// Delete Mst_TaxRate
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public int Delete(int TAX_RATE_ID = 0)
        {
            StringBuilder sqlupdate = new StringBuilder();
            sqlupdate.Append(" UPDATE Mst_TaxRate");
            sqlupdate.Append(" SET ");
            sqlupdate.Append("    DEL_FLG = @DEL_FLG,");
            sqlupdate.Append("    UPD_DATE = @UPD_DATE,");
            sqlupdate.Append("    UPD_USER_ID = @UPD_USER_ID");

            sqlupdate.Append(" WHERE    TAX_RATE_ID = @TAX_RATE_ID");

            return base.Execute(sqlupdate.ToString(), new
            {
                TAX_RATE_ID = TAX_RATE_ID,

                DEL_FLG = Constants.DeleteFlag.DELETE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = base.CmnEntityModel.UserSegNo
            });
        }

        #endregion


        #region CREATE
        /// <summary>
        /// Insert into Mst_TaxRate
        /// </summary>
        /// <param name="Maint"></param>
        /// <returns></returns>
        public long InsertTaxRateMaint(TaxRateEntity maint)
        {
            //maint.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            //maint.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append(@"
                INSERT INTO Mst_TaxRate
                (
                    APPLY_DATE_FROM,
                    APPLY_DATE_TO,
                    TAX_RATE,
                    DEL_FLG,
                    INS_DATE,
                    INS_USER_ID,
                    UPD_DATE,
                    UPD_USER_ID,
                    UPD_PROG_ID
                )
                VALUES
                (
                    @APPLY_DATE_FROM,
                    @APPLY_DATE_TO,
                    @TAX_RATE,
                    @DEL_FLG,
                    @INS_DATE,
                    @INS_USER_ID,
                    @UPD_DATE,
                    @UPD_USER_ID,
                    @UPD_PROG_ID
                ) ");
            return base.Execute(sqlinsert.ToString(), new
            {
                APPLY_DATE_FROM = maint.APPLY_DATE_FROM,
                APPLY_DATE_TO = maint.APPLY_DATE_TO,
                TAX_RATE = maint.TAX_RATE,
                DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                INS_DATE = Utility.GetCurrentDateTime(),
                INS_USER_ID = 0,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = 0,
                UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE,
            });
            //var res = base.DbAdd(sqlinsert.ToString(), Maint);
            //if (res > 0)
            //{
            //    var query = "SELECT ident_current('Mst_TaxRate')";
            //    return base.ExecuteScalar<long>(query);
            //}
            //return 0;
        }
        #endregion

        #region CheckDate

        /// <summary>
        /// TaxRateMaintCheckDate
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<TaxRateMaintModel> TaxRateMaintCheckDate(TaxRateEntity searchCondition)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT
					mfcc.TAX_RATE_ID,
	                mfcc.APPLY_DATE_FROM,
	                mfcc.APPLY_DATE_TO,
	                mfcc.TAX_RATE,
	                mfcc.UPD_DATE
                FROM
	                Mst_TaxRate mfcc

                WHERE
                    mfcc.DEL_FLG = 0
                    AND mfcc.APPLY_DATE_TO >= @APPLY_DATE_FROM
                    AND mfcc.APPLY_DATE_FROM <= @APPLY_DATE_TO  ");
            if(searchCondition.TAX_RATE_ID != 0)
            {
                sql.Append(@"
                    AND mfcc.TAX_RATE_ID != @TAX_RATE_ID  ");
            }

            var dataList = base.Query<TaxRateMaintModel>(sql.ToString(),
                new
                {
                    APPLY_DATE_FROM = searchCondition.APPLY_DATE_FROM,
                    APPLY_DATE_TO = searchCondition.APPLY_DATE_TO,
                    TAX_RATE_ID = searchCondition.TAX_RATE_ID,
                }).ToList();

            return dataList;
        }
        #endregion
    }
}

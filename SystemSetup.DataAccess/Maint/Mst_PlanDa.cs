﻿using System;
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
    public class Mst_PlanDa : BaseDa
    {
        #region READ

        /// <summary>
        /// Search payment/Not payment
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<Mst_PlanModel> Mst_PlanMaintSearch(DataTablesModel dt, ref Mst_PlanModel searchCondition, out int totalrow)
        {
            StringBuilder sql = new StringBuilder();

            //データ表示のSELECT文
            sql.Append(@"

                SELECT 
                       mag.PLAN_SEQ_NO
                       mag.PLAN_CD,
                       mag.PLAN_NAME,
                       mag.PLAN_BASE_PRICE,
                       mag.LOGIN_ACCOUNT_UPPER,
					   mag.MONTHLY_BILL_DATA_UPPER 
                FROM 
                       Mst_Plan 
                WHERE 
                       mag.DEL_FLG=0
            ");

            if (!String.IsNullOrEmpty(searchCondition.PLAN_CD))
            {
                sql.Append(@"
                    AND mag.PLAN_CD LIKE @PLAN_CD ");
            }

            if (!String.IsNullOrEmpty(searchCondition.PLAN_NAME))
            {
                sql.Append(@"
                    AND mag.PLAN_NAME LIKE @PLAN_NAME ");
            }

            if (false.Equals(searchCondition.INCLUDE_DISABLE))
            {
                sql.Append(@"
                    AND mag.DISABLE_FLG = @DISABLE_FLG ");
            }

            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;


            var dataList = base.Query<Mst_PlanModel>(sqlpage,
                new
                {
                    PLAN_CD = '%' + searchCondition.PLAN_CD + '%',
                    PLAN_NAME = '%' + searchCondition.PLAN_NAME + '%',
                    DISABLE_FLG = Constants.DisableFlag.Enable,
                    pageindex = lower,
                    pagesize = upper
                }).ToList();

            totalrow = base.Query<int>(sqlcount,
                new
                {
                    PLAN_CD = '%' + searchCondition.PLAN_CD + '%',
                    PLAN_NAME = '%' + searchCondition.PLAN_NAME + '%',
                    DISABLE_FLG = Constants.DisableFlag.Enable
                }).FirstOrDefault();

            return dataList;
        }

        /// <summary>
        /// Get a row from Tbl_Information based on INFO_SEQ_NO
        /// </summary>
        /// <param name="infoSeqNo"></param>
        /// <returns></returns>
        public Mst_PlanModel GetInformation(String infoSeqNo)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
	                mag.*,
                    mcfu_plan.PLAN_CD
                FROM
                 Mst_Plan mag
                WHERE
                    mag.PLAN_SEQ_NO = @PLAN_SEQ_NO
            ");

            return base.Query<Mst_PlanModel>(sql.ToString(), new
            {
                PLAN_SEQ_NO = infoSeqNo
            }).FirstOrDefault();
        }

        #endregion

        #region UPDATE
        public long UpdateMst_PlanModel(Mst_PlanEntity UPD)
        {
            StringBuilder sql = new StringBuilder();
            UPD.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            sql.Append(@"
                UPDATE [dbo].[Mst_Plan]
                    SET
                     [PLAN_CD] = @PLAN_CD
                    ,[PLAN_NAME] = @PLAN_NAME
                    ,[PLAN_BASE_PRICE] = @PLAN_BASE_PRICE
                    ,[LOGIN_ACCOUNT_UPPER] = @LOGIN_ACCOUNT_UPPER
                    ,[MONTHLY_BILL_DATA_UPPER] = @MONTHLY_BILL_DATA_UPPER
                    ,[DISABLE_FLG] = @DISABLE_FLG
                    ,[UPD_PROG_ID] = @UPD_PROG_ID
                WHERE 
                    PLAN_SEQ_NO = @PLAN_SEQ_NO

            ");
            return base.DbUpdateByOutside(sql.ToString(), UPD, new
            {
                PLAN_CD = UPD.PLAN_CD,
                PLAN_NAME = UPD.PLAN_NAME,
                PLAN_BASE_PRICE = UPD.PLAN_BASE_PRICE,
                LOGIN_ACCOUNT_UPPER = UPD.LOGIN_ACCOUNT_UPPER,
                MONTHLY_BILL_DATA_UPPER = UPD.MONTHLY_BILL_DATA_UPPER,
                DISABLE_FLG = UPD.DISABLE_FLG,
                UPD_PROG_ID = UPD.UPD_PROG_ID
            });
        }
        #endregion

        #region DELETE

        /// <summary>
        /// Delete Mst_Plan
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public int Delete(String infoSeqNo)
        {
            StringBuilder sqlupdate2 = new StringBuilder();
            sqlupdate2.Append(" UPDATE Mst_Plan ");
            sqlupdate2.Append(" SET ");
            sqlupdate2.Append("    DEL_FLG = @DEL_FLG,");
            sqlupdate2.Append("    UPD_DATE = @UPD_DATE,");
            sqlupdate2.Append("    UPD_USER_ID = @UPD_USER_ID");
            sqlupdate2.Append(" WHERE PLAN_SEQ_NO = @PLAN_SEQ_NO");

            base.Execute(sqlupdate2.ToString(), new
            {
                PLAN_SEQ_NO = infoSeqNo,
                DEL_FLG = Constants.DeleteFlag.DELETE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = base.CmnEntityModel.UserSegNo
            });


            StringBuilder sqlupdate = new StringBuilder();
            sqlupdate.Append(" UPDATE Mst_Plan ");
            sqlupdate.Append(" SET ");
            sqlupdate.Append("    DEL_FLG = @DEL_FLG,");
            sqlupdate.Append("    UPD_DATE = @UPD_DATE,");
            sqlupdate.Append("    UPD_USER_ID = @UPD_USER_ID");
            sqlupdate.Append(" WHERE  PLAN_SEQ_NO = @PLAN_SEQ_NO");

            return base.Execute(sqlupdate.ToString(), new
            {
                PLAN_SEQ_NO = infoSeqNo,
                DEL_FLG = Constants.DeleteFlag.DELETE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = base.CmnEntityModel.UserSegNo
            });
        }

        #endregion

        /// <summary>
        /// GetAuxiliaryCode
        /// </summary>
        /// <returns></returns>
        public IList<Mst_PlanModel> GetAuxiliaryCode(string mstPlanCd, string mstPlanName)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            SELECT
                *
            FROM
                Mst_Plan
                WHERE
                PLAN_CD = @PLAN_CD 
                AND PLAN_NAME = @PLAN_NAME
                AND DEL_FLG = 0");
            return base.Query<Mst_PlanModel>(sql.ToString(), new
            {
                ACCOUNT_GROUP_CD = mstPlanCd,
                ACCOUNT_GROUP_NAME = mstPlanName,
            }).ToList();
        }

        #region CREATE
        /// <summary>
        /// Insert into Mst_FirmContractClass
        /// </summary>
        /// <param name="Maint"></param>
        /// <returns></returns>
        public long InsertMst_PlanMaint(Mst_PlanEntity Maint)
        {
            Maint.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            Maint.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append(@"
                INSERT INTO Mst_Plan
                (
                    PLAN_CD,
                    PLAN_NAME,
                    PLAN_BASE_PRICE,
                    LOGIN_ACCOUNT_UPPER,
                    MONTHLY_BILL_DATA_UPPER,
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
                    @ACCOUNT_GROUP_CD,
                    @ACCOUNT_GROUP_NAME,
                    @PLAN_BASE_PRICE,
                    @LOGIN_ACCOUNT_UPPER,
                    @MONTHLY_BILL_DATA_UPPER,
                    @DISABLE_FLG,
                    @DEL_FLG,
                    @INS_DATE,
                    @INS_USER_ID,
                    @UPD_DATE,
                    @UPD_USER_ID,
                    @UPD_PROG_ID
                ) ");
            var res = base.DbAddByOutside(sqlinsert.ToString(), Maint);
            if (res > 0)
            {
                var query = "SELECT ident_current('Mst_Plan')";
                return base.ExecuteScalar<long>(query);
            }
            return 0;
        }
        #endregion

        #region Check exist PLAN_CD
        /// <summary>
        /// Check exist PLAN_CD
        /// </summary>
        /// <param name="Modele"></param>
        /// <returns></returns>
        public bool CheckExist(Mst_PlanEntity model)
        {
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT PLAN_CD FROM Mst_Plan ");
            sql.Append(" WHERE PLAN_CD = @PLAN_CD AND DEL_FLG = @DEL_FLG");

            var cus = base.Query<string>(sql.ToString(), new
            {
                DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                PLAN_CD = model.PLAN_CD
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
        public bool DeleteBeforeCheck(String PLAN_SEQ_NO)
        {
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT PLAN_CD FROM Mst_Plan ");
            sql.Append(" WHERE PLAN_SEQ_NO = @PLAN_SEQ_NO");
            sql.Append(" AND DEL_FLG = @DEL_FLG");

            IEnumerable<String> results = base.Query<string>(sql.ToString(), new
            {
                PLAN_SEQ_NO = PLAN_SEQ_NO,
                DEL_FLG = Constants.DeleteFlag.NON_DELETE
            });

            if (results.Count() == 0)
            {
                return false;
            }
            else
            {
                StringBuilder sql2 = new StringBuilder();
                // SQL発行  ここ契約企業プラン関係テーブルかも？
                sql2.Append(" SELECT PLAN_CD FROM Mst_Plan ");
                sql2.Append(" WHERE DEL_FLG = @DEL_FLG");
                sql2.Append(" AND PLAN_CD IN(");
                foreach (string data in results)
                {
                    sql2.Append("'" + data + "',");
                }
                sql2 = sql2.Remove(sql2.ToString().LastIndexOf(","), 1);
                sql2.Append(")");

                var cus = base.Query<string>(sql2.ToString(), new
                {
                    PLAN_SEQ_NO = PLAN_SEQ_NO,
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                });

                if (cus.Count() != 0)
                {
                    return true;
                }
                return false;
            }
        }
        #endregion
    }
}

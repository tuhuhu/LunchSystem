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
    public class AccountingSubjectGroupMaintDa : BaseDa
    {
        #region READ

        /// <summary>
        /// Search payment/Not payment
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<AccountingSubjectGroupMaintModel> AccountingSubjectGroupMaintSearch(DataTablesModel dt, ref AccountingSubjectGroupMaintModel searchCondition, out int totalrow)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT
                    mag.ACCOUNT_GROUP_SEQ_NO,
					mag.ACCOUNT_GROUP_CD,
	                mag.ACCOUNT_GROUP_NAME,
                    mag.DISABLE_FLG,
	                mag.UPD_DATE,
	                mag.UPD_USER_ID,
	                mcfu.CONTRACT_USER_NAME AS UPD_USERNAME
                FROM
	                Mst_AccountGroup mag
                        LEFT JOIN Mst_ContractFirmUser mcfu ON
                            mag.UPD_USER_ID = mcfu.CONTRACT_USER_SEQ_CD
                WHERE
                    mag.DEL_FLG = 0
            ");

            if (!String.IsNullOrEmpty(searchCondition.ACCOUNT_GROUP_CD))
            {
                sql.Append(@"
                    AND mag.ACCOUNT_GROUP_CD LIKE @ACCOUNT_GROUP_CD  ");
            }

            if (!String.IsNullOrEmpty(searchCondition.ACCOUNT_GROUP_NAME))
            {
                sql.Append(@"
                    AND mag.ACCOUNT_GROUP_NAME LIKE @ACCOUNT_GROUP_NAME  ");
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


            var dataList = base.Query<AccountingSubjectGroupMaintModel>(sqlpage,
                new
                {
                    ACCOUNT_GROUP_CD = '%' + searchCondition.ACCOUNT_GROUP_CD + '%',
                    ACCOUNT_GROUP_NAME = '%' + searchCondition.ACCOUNT_GROUP_NAME + '%',
                    DISABLE_FLG = Constants.DisableFlag.Enable,
                    pageindex = lower,
                    pagesize = upper
                }).ToList();

            totalrow = base.Query<int>(sqlcount,
                new
                {
                    ACCOUNT_GROUP_CD = '%' + searchCondition.ACCOUNT_GROUP_CD + '%',
                    ACCOUNT_GROUP_NAME = '%' + searchCondition.ACCOUNT_GROUP_NAME + '%',
                    DISABLE_FLG = Constants.DisableFlag.Enable
                }).FirstOrDefault();

            return dataList;
        }

        /// <summary>
        /// Get a row from Tbl_Information based on INFO_SEQ_NO
        /// </summary>
        /// <param name="infoSeqNo"></param>
        /// <returns></returns>
        public AccountingSubjectGroupMaintModel GetInformation(String infoSeqNo)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
	                mag.*,
                    mcfu_upd.CONTRACT_USER_NAME AS UPD_USERNAME,
                    mcfu_ins.CONTRACT_USER_NAME AS INS_USERNAME
                FROM
                 Mst_AccountGroup mag
                        LEFT JOIN Mst_ContractFirmUser mcfu_upd ON
                            mag.UPD_USER_ID = mcfu_upd.CONTRACT_USER_SEQ_CD
                        LEFT JOIN Mst_ContractFirmUser mcfu_ins ON
                            mag.INS_USER_ID = mcfu_ins.CONTRACT_USER_SEQ_CD
                WHERE
                    mag.ACCOUNT_GROUP_SEQ_NO = @ACCOUNT_GROUP_SEQ_NO
            ");

            return base.Query<AccountingSubjectGroupMaintModel>(sql.ToString(), new
            {
                ACCOUNT_GROUP_SEQ_NO = infoSeqNo
            }).FirstOrDefault();
        }

        #endregion

        #region UPDATE
        public long UpdateAccountingSubjectGroupMaintModel(AccountGroupEntity UPD)
        {
            StringBuilder sql = new StringBuilder();
            UPD.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            sql.Append(@"
                UPDATE [dbo].[Mst_AccountGroup]
                    SET 
                     [ACCOUNT_GROUP_CD] = @ACCOUNT_GROUP_CD
                    ,[ACCOUNT_GROUP_NAME] = @ACCOUNT_GROUP_NAME
                    ,[DISABLE_FLG] = @DISABLE_FLG
                    ,[UPD_DATE] = @UPD_DATE
                    ,[UPD_USER_ID] = @UPD_USER_ID
                    ,[UPD_PROG_ID] = @UPD_PROG_ID
                WHERE 
                    ACCOUNT_GROUP_SEQ_NO = @ACCOUNT_GROUP_SEQ_NO

            ");
            return base.DbUpdateByOutside(sql.ToString(), UPD, new
            {
                ACCOUNT_GROUP_CD = UPD.ACCOUNT_GROUP_CD,
                ACCOUNT_GROUP_NAME = UPD.ACCOUNT_GROUP_NAME,
                DISABLE_FLG = UPD.DISABLE_FLG,
                UPD_DATE = UPD.UPD_DATE,
                UPD_USER_ID = UPD.UPD_USER_ID,
                UPD_PROG_ID = UPD.UPD_PROG_ID,
                ACCOUNT_GROUP_SEQ_NO = UPD.ACCOUNT_GROUP_SEQ_NO
            });
        }
        #endregion

        #region DELETE

        /// <summary>
        /// Delete Mst_FirmContractClass
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public int Delete(String infoSeqNo)
        {
            StringBuilder sqlupdate2 = new StringBuilder();
            sqlupdate2.Append(" UPDATE Mst_AccountCode ");
            sqlupdate2.Append(" SET ");
            sqlupdate2.Append("    DEL_FLG = @DEL_FLG,");
            sqlupdate2.Append("    UPD_DATE = @UPD_DATE,");
            sqlupdate2.Append("    UPD_USER_ID = @UPD_USER_ID");
            sqlupdate2.Append(" WHERE ACCOUNT_GROUP_SEQ_NO = @ACCOUNT_GROUP_SEQ_NO");

            base.Execute(sqlupdate2.ToString(), new
            {
                ACCOUNT_GROUP_SEQ_NO = infoSeqNo,
                DEL_FLG = Constants.DeleteFlag.DELETE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = base.CmnEntityModel.UserSegNo
            });


            StringBuilder sqlupdate = new StringBuilder();
            sqlupdate.Append(" UPDATE Mst_AccountGroup");
            sqlupdate.Append(" SET ");
            sqlupdate.Append("    DEL_FLG = @DEL_FLG,");
            sqlupdate.Append("    UPD_DATE = @UPD_DATE,");
            sqlupdate.Append("    UPD_USER_ID = @UPD_USER_ID");
            sqlupdate.Append(" WHERE     ACCOUNT_GROUP_SEQ_NO = @ACCOUNT_GROUP_SEQ_NO");

            return base.Execute(sqlupdate.ToString(), new
            {
                ACCOUNT_GROUP_SEQ_NO = infoSeqNo,
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
        public IList<AccountingSubjectGroupMaintModel> GetAuxiliaryCode(string accountGroupCd, string accountGroupName)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            SELECT
                *
            FROM
                Mst_AccountGroup
                WHERE
                ACCOUNT_GROUP_CD = @ACCOUNT_GROUP_CD 
                AND ACCOUNT_GROUP_NAME = @ACCOUNT_GROUP_NAME
                AND DEL_FLG = 0");
            return base.Query<AccountingSubjectGroupMaintModel>(sql.ToString(), new
            {     
                ACCOUNT_GROUP_CD = accountGroupCd,
                ACCOUNT_GROUP_NAME = accountGroupName,
            }).ToList();
        }

        #region CREATE
        /// <summary>
        /// Insert into Mst_FirmContractClass
        /// </summary>
        /// <param name="Maint"></param>
        /// <returns></returns>
        public long InsertAccountingSubjectGroupMaint(AccountGroupEntity Maint)
        {
            Maint.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            Maint.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append(@"
                INSERT INTO Mst_AccountGroup
                (
                    ACCOUNT_GROUP_CD,
                    ACCOUNT_GROUP_NAME,
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
                var query = "SELECT ident_current('Mst_AccountGroup')";
                return base.ExecuteScalar<long>(query);
            }
            return 0;
        }
        #endregion

        #region Check exist ACCOUNT_GROUP_CD
        /// <summary>
        /// Check exist ACCOUNT_GROUP_CD
        /// </summary>
        /// <param name="Modele"></param>
        /// <returns></returns>
        public bool CheckExist(AccountGroupEntity model)
        {
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT ACCOUNT_GROUP_CD FROM Mst_AccountGroup ");
            sql.Append(" WHERE ACCOUNT_GROUP_CD = @ACCOUNT_GROUP_CD AND DEL_FLG = @DEL_FLG");

            var cus = base.Query<string>(sql.ToString(), new
            {
                DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                ACCOUNT_GROUP_CD = model.ACCOUNT_GROUP_CD
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
        public bool DeleteBeforeCheck(String ACCOUNT_GROUP_SEQ_NO)
        {
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT ACCOUNT_CD FROM Mst_AccountCode ");
            sql.Append(" WHERE ACCOUNT_GROUP_SEQ_NO = @ACCOUNT_GROUP_SEQ_NO");
            sql.Append(" AND DEL_FLG = @DEL_FLG");

            IEnumerable<String> results = base.Query<string>(sql.ToString(), new
            {
                ACCOUNT_GROUP_SEQ_NO = ACCOUNT_GROUP_SEQ_NO,
                DEL_FLG = Constants.DeleteFlag.NON_DELETE
            });

            if (results.Count() == 0)
            {
                return false;
            }
            else
            {
                StringBuilder sql2 = new StringBuilder();
                // SQL発行  
                sql2.Append(" SELECT ACCOUNT_CD FROM Mst_AuxiliaryCode ");
                sql2.Append(" WHERE DEL_FLG = @DEL_FLG");
                sql2.Append(" AND ACCOUNT_CD IN(");
                foreach (string data in results)
                {
                    sql2.Append("'" + data + "',");
                }
                sql2 = sql2.Remove(sql2.ToString().LastIndexOf(","), 1);
                sql2.Append(")");

                var cus = base.Query<string>(sql2.ToString(), new
                {
                    ACCOUNT_GROUP_SEQ_NO = ACCOUNT_GROUP_SEQ_NO,
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

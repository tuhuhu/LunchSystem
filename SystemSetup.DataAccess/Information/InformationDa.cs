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
    public class InformationDa : BaseDa
    {
        #region CREATE
        /// <summary>
        /// Insert into Tbl_Information
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        public long InsertInformation(InformationEntity information)
        {
            information.COMPANY_CD = base.CmnEntityModel.CompanyCd;
            information.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append(@"
                INSERT INTO Tbl_Information
                (
                    PUBLISH_DATE_START,
                    PUBLISH_DATE_END,
                    COMPANY_CD,
                    CONTENT,
                    DSP_PRIORITY,
                    DEL_FLG,
                    INS_DATE,
                    INS_USER_ID,
                    UPD_DATE,
                    UPD_USER_ID,
                    UPD_PROG_ID
                )
                VALUES
                (
                    @PUBLISH_DATE_START,
                    @PUBLISH_DATE_END,
                    @COMPANY_CD,
                    @CONTENT,
                    @DSP_PRIORITY,
                    @DEL_FLG,
                    @INS_DATE,
                    @INS_USER_ID,
                    @UPD_DATE,
                    @UPD_USER_ID,
                    @UPD_PROG_ID
                ) ");
            var res = base.DbAdd(sqlinsert.ToString(), information);
            if (res > 0)
            {
                var query = "SELECT ident_current('Tbl_Information')";
                return base.ExecuteScalar<long>(query);
            }
            return 0;
        }
        #endregion

        //全体掲載情報のInsert
        #region CREATE
        /// <summary>
        /// Insert into Tbl_Information
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        public long InsertAllInformation(AllInformationEntity information)
        {
            information.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append(@"
                INSERT INTO Tbl_Information
                (
                    PUBLISH_DATE_START,
                    PUBLISH_DATE_END,
                    COMPANY_CD,
                    CONTENT,
                    CONTENT_TYPE,
                    TITLE,
                    DSP_PRIORITY,
                    DEL_FLG,
                    INS_DATE,
                    INS_USER_ID,
                    UPD_DATE,
                    UPD_USER_ID,
                    UPD_PROG_ID
                )
                VALUES
                (
                    @PUBLISH_DATE_START,
                    @PUBLISH_DATE_END,
                    @COMPANY_CD,
                    @CONTENT,
                    @CONTENT_TYPE,
                    @TITLE,
                    @DSP_PRIORITY,
                    @DEL_FLG,
                    @INS_DATE,
                    @INS_USER_ID,
                    @UPD_DATE,
                    @UPD_USER_ID,
                    @UPD_PROG_ID
                ) ");
            var res = base.DbAdd(sqlinsert.ToString(), information);
            if (res > 0)
            {
                var query = "SELECT ident_current('Tbl_Information')";
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
        public InformationEntityPlus GetInformation(long infoSeqNo)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT *
                FROM Tbl_Information 
                WHERE INFO_SEQ_NO = @INFO_SEQ_NO
                    AND COMPANY_CD = @COMPANY_CD ");
            return base.Query<InformationEntityPlus>(sql.ToString(), new
            {
                INFO_SEQ_NO = infoSeqNo,
                COMPANY_CD = base.CmnEntityModel.CompanyCd
            }).FirstOrDefault();
        }

        /// <summary>
        /// Get a row from Tbl_Information based on INFO_SEQ_NO
        /// 全体掲載情報
        /// </summary>
        /// <param name="infoSeqNo"></param>
        /// <returns></returns>
        public AllInformationListModel GetAllInformation(long infoSeqNo)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT *
                FROM Tbl_Information 
                WHERE INFO_SEQ_NO = @INFO_SEQ_NO");
            return base.Query<AllInformationListModel>(sql.ToString(), new
            {
                INFO_SEQ_NO = infoSeqNo
            }).FirstOrDefault();
        }

        /// <summary>
        /// Get Partner Address Master
        /// </summary>
        /// <returns></returns>
        public IList<InformationEntity> GetInformation(string companyCd)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
	                *
                FROM
	                Tbl_Information 
                WHERE 
	                COMPANY_CD IN (@COMPANY_CD, '     ')
	                AND DEL_FLG = @DEL_FLG
                    AND @CurrentDate BETWEEN PUBLISH_DATE_START AND PUBLISH_DATE_END
                    ORDER BY PUBLISH_DATE_START DESC, DSP_PRIORITY DESC");
            return base.Query<InformationEntity>(sql.ToString(), new
            {
                COMPANY_CD = companyCd,
                DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                CurrentDate = Utility.GetCurrentDateOnly()
            }).ToList();
        }

        /// <summary>
        /// Get Partner Address Master
        /// 全体掲載情報
        /// </summary>
        /// <returns></returns>
        public IList<AllInformationListModel> GetAllInformation(string companyCd)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
	                *
                FROM
	                Tbl_Information 
                WHERE 
	                COMPANY_CD IN (@COMPANY_CD, '     ')
                    AND @CurrentDate BETWEEN PUBLISH_DATE_START AND PUBLISH_DATE_END
                    ORDER BY PUBLISH_DATE_START DESC, DSP_PRIORITY DESC");
            return base.Query<AllInformationListModel>(sql.ToString(), new
            {
                COMPANY_CD = companyCd,
                DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                CurrentDate = Utility.GetCurrentDateOnly()
            }).ToList();
        }
         /// <summary>
        /// Search payment/Not payment
        /// 全体掲載情報
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<AllInformationListModel> AllInformationSearch(DataTablesModel dt, ref AllInformationListModel searchCondition, out int totalrow)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT i.INFO_SEQ_NO, 
                       i.COMPANY_CD,
                       i.CONTENT,
                       i.CONTENT_TYPE,
                       i.TITLE,
                       i.PUBLISH_DATE_START,
                       i.PUBLISH_DATE_END,
                       i.DSP_PRIORITY,
                       i.UPD_DATE,
                       i.DEL_FLG
                FROM
                    Tbl_Information i
                WHERE ");

                if (true.Equals(searchCondition.INCLUDE_DELETED))
                {
                    sql.Append(@"
                       (i.DEL_FLG = 0
                       OR i.DEL_FLG = 1)");
                }
                else
                {
                    sql.Append(@"
                       i.DEL_FLG = 0");
                }

            if (!String.IsNullOrEmpty(searchCondition.CONTENT_DATE))
            {
                sql.Append(@"
                    AND i.CONTENT LIKE @CONTENT_DATE ");
            }
            if (!String.IsNullOrEmpty(searchCondition.START_DATE))
            {
                sql.Append(@"
                    AND i.PUBLISH_DATE_START >= @START_DATE ");
            }
            if (!String.IsNullOrEmpty(searchCondition.END_DATE))
            {
                sql.Append(@"
                    AND i.PUBLISH_DATE_END <= @END_DATE ");
            }

            sql.Append(@"
                    AND i.COMPANY_CD = '     ' ");

            sql.Append(@"
                    ORDER BY i.PUBLISH_DATE_START DESC,
                             i.DSP_PRIORITY DESC");

            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;

            var dataList = base.Query<AllInformationListModel>(sqlpage,
                new
                {
                    COMPANY_CD = "     ",
                    CONTENT_DATE = '%' + searchCondition.CONTENT_DATE + '%',
                    START_DATE = searchCondition.START_DATE,
                    END_DATE = searchCondition.END_DATE,
                    INCLUDE_DELETED = Constants.DeleteFlag.NON_DELETE,
                    pageindex = lower,
                    pagesize = upper
                }).ToList();

            totalrow = base.Query<int>(sqlcount,
                new
                {
                    COMPANY_CD = "     ",
                    CONTENT_DATE = '%' + searchCondition.CONTENT_DATE + '%',
                    START_DATE = searchCondition.START_DATE,
                    END_DATE = searchCondition.END_DATE,
                    INCLUDE_DELETED = Constants.DeleteFlag.NON_DELETE,
                    pageindex = lower,
                    pagesize = upper
                }).FirstOrDefault();

            return dataList;
        }

        /// <summary>
        /// Search payment/Not payment
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<InformationEntityPlus> PostInformationSearch(DataTablesModel dt, ref PostInformationListModel searchCondition, out int totalrow)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT i.INFO_SEQ_NO, i.CONTENT, i. PUBLISH_DATE_START, i.PUBLISH_DATE_END, i.DSP_PRIORITY, i.UPD_DATE, u.CONTRACT_USER_NAME as UPD_USERNAME
                FROM
                    Tbl_Information i
                LEFT JOIN
                    Mst_ContractFirmUser u
                    ON i.UPD_USER_ID = u.CONTRACT_USER_SEQ_CD
                    AND i.COMPANY_CD = u.COMPANY_CD
                WHERE
                    i.COMPANY_CD = @COMPANY_CD ");
            if (!String.IsNullOrEmpty(searchCondition.CONTENT))
            {
                sql.Append(@"
                    AND i.CONTENT LIKE @CONTENT ");
            }
            if (searchCondition.START_DATE.HasValue)
            {
                sql.Append(@"
                    AND i.PUBLISH_DATE_END >= @START_DATE ");
            }
            if (searchCondition.END_DATE.HasValue)
            {
                sql.Append(@"
                    AND i.PUBLISH_DATE_START <= @END_DATE ");
            }
            if (true.Equals(searchCondition.INCLUDE_DELETED))
            {
                sql.Append(@"
                    AND i.DEL_FLG = @DEL_FLG ");
            }
            sql.Append(@"
                ORDER BY i.INFO_SEQ_NO ASC");

            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;

            var dataList = base.Query<InformationEntityPlus>(sqlpage,
                new
                {
                    COMPANY_CD = base.CmnEntityModel.CompanyCd,
                    CONTENT = '%' + searchCondition.CONTENT + '%',
                    START_DATE = searchCondition.START_DATE,
                    END_DATE = searchCondition.END_DATE,
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    pageindex = lower,
                    pagesize = upper
                }).ToList();

            totalrow = base.Query<int>(sqlcount,
                new
                {
                    COMPANY_CD = base.CmnEntityModel.CompanyCd,
                    CONTENT = '%' + searchCondition.CONTENT + '%',
                    START_DATE = searchCondition.START_DATE,
                    END_DATE = searchCondition.END_DATE,
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    pageindex = lower,
                    pagesize = upper
                }).FirstOrDefault();

            return dataList;
        }
        #endregion

        #region UPDATE
        public long UpdateInformation(InformationEntity information)
        {
            StringBuilder sql = new StringBuilder();
            information.COMPANY_CD = base.CmnEntityModel.CompanyCd;
            information.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            sql.Append(@"
             UPDATE [dbo].[Tbl_Information] 
             SET [PUBLISH_DATE_START] = @PUBLISH_DATE_START
                ,[PUBLISH_DATE_END] = @PUBLISH_DATE_END
                ,[COMPANY_CD] = @COMPANY_CD
                ,[CONTENT] = @CONTENT
                ,[DSP_PRIORITY] = @DSP_PRIORITY
                ,[DEL_FLG] = @DEL_FLG
                ,[UPD_DATE] = @UPD_DATE
                ,[UPD_USER_ID] = @UPD_USER_ID
                ,[UPD_PROG_ID] = @UPD_PROG_ID
                WHERE INFO_SEQ_NO = @ORG_INFO_SEQ_NO ");
            return base.DbUpdate(sql.ToString(), information, new { INFO_SEQ_NO = information.INFO_SEQ_NO });
        }
        #endregion

        #region UPDATE
        public long UpdateAllInformationModel(AllInformationEntity UPD)
        {
            StringBuilder sql = new StringBuilder();
            UPD.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            sql.Append(@"
             UPDATE [dbo].[Tbl_Information] 
             SET [PUBLISH_DATE_START] = @PUBLISH_DATE_START
                ,[PUBLISH_DATE_END] = @PUBLISH_DATE_END
                ,[COMPANY_CD] = @COMPANY_CD
                ,[CONTENT] = @CONTENT
                ,[CONTENT_TYPE] = @CONTENT_TYPE
                ,[TITLE] = @TITLE
                ,[DSP_PRIORITY] = @DSP_PRIORITY
                ,[DEL_FLG] = @DEL_FLG
                WHERE INFO_SEQ_NO = @INFO_SEQ_NO ");
            return base.DbUpdateByOutside(sql.ToString(), UPD, new
            {
                PUBLISH_DATE_START = UPD.PUBLISH_DATE_START,
                PUBLISH_DATE_END = UPD.PUBLISH_DATE_END,
                COMPANY_CD = UPD.COMPANY_CD,
                CONTENT = UPD.CONTENT,
                DSP_PRIORITY = UPD.DSP_PRIORITY,
                DEL_FLG = UPD.DEL_FLG
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
            sqlupdate2.Append(" UPDATE Tbl_Information ");
            sqlupdate2.Append(" SET ");
            sqlupdate2.Append("    DEL_FLG = @DEL_FLG,");
            sqlupdate2.Append("    UPD_DATE = @UPD_DATE,");
            sqlupdate2.Append("    UPD_USER_ID = @UPD_USER_ID");
            sqlupdate2.Append(" WHERE INFO_SEQ_NO = @INFO_SEQ_NO");

            base.Execute(sqlupdate2.ToString(), new
            {
                INFO_SEQ_NO = infoSeqNo,
                DEL_FLG = Constants.DeleteFlag.DELETE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = base.CmnEntityModel.UserSegNo
            });


            StringBuilder sqlupdate = new StringBuilder();
            sqlupdate.Append(" UPDATE Tbl_Information ");
            sqlupdate.Append(" SET ");
            sqlupdate.Append("    DEL_FLG = @DEL_FLG,");
            sqlupdate.Append("    UPD_DATE = @UPD_DATE,");
            sqlupdate.Append("    UPD_USER_ID = @UPD_USER_ID");
            sqlupdate.Append(" WHERE  INFO_SEQ_NO = @INFO_SEQ_NO");

            return base.Execute(sqlupdate.ToString(), new
            {
                INFO_SEQ_NO = infoSeqNo,
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
        public IList<InformationModel> GetAuxiliaryCode(string infoseqno)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            SELECT
                *
            FROM
                Mst_Tbl_Information
                WHERE
                INFO_SEQ_NO = @INFO_SEQ_NO");
            return base.Query<InformationModel>(sql.ToString(), new
            {
                INFO_SEQ_NO = infoseqno
            }).ToList();
        }

        #region Check exist INFO_SEQ_NO
        /// <summary>
        /// Check exist INFO_SEQ_NO
        /// </summary>
        /// <param name="Modele"></param>
        /// <returns></returns>
        public bool CheckExist(AllInformationEntity model)
        {
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT INFO_SEQ_NO FROM Tbl_Information ");
            sql.Append(" WHERE INFO_SEQ_NO = @INFO_SEQ_NO");

            var cus = base.Query<string>(sql.ToString(), new
            {
                INFO_SEQ_NO = model.INFO_SEQ_NO
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

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
    public class LunchDa : BaseDa
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
        public LunchInformationEntity GetLunchInformation(long infoSeqNo)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT *
                FROM LUNCH_INFORMATION 
                WHERE LUNCH_SEQ_CD = @LUNCH_SEQ_CD
                   ");
            return base.Query<LunchInformationEntity>(sql.ToString(), new
            {
                LUNCH_SEQ_CD = infoSeqNo,
                COMPANY_CD = base.CmnEntityModel.CompanyCd
            }).FirstOrDefault();
        }

        public IList<LunchMenuEntity> GetMealListToday()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
	                *
                FROM
	                LUNCH_MENU 
                WHERE 
	                DISABLE_FLG = @DISABLE_FLG
                    ");
            return base.Query<LunchMenuEntity>(sql.ToString(), new
            {                
                DISABLE_FLG = Constants.DeleteFlag.NON_DELETE                
            }).ToList();
        }

        public long RegisterMeal(LunchInformationEntity meal)
        {
            long result = 0;
            if(meal.LUNCH_SEQ_CD >0)
            {
                StringBuilder sql = new StringBuilder();
                meal.LUNCH_DATE = DateTime.Now;
                meal.SETUP_USER_ACCOUNT = CmnEntityModel.UserSegNo;
                sql.Append(@"
             UPDATE [dbo].[LUNCH_INFORMATION] 
             SET [SETUP_USER_ACCOUNT] = @SETUP_USER_ACCOUNT
                ,[LUNCH_DATE] = @LUNCH_DATE
                ,[LUNCH_MENU_TYPE] = @LUNCH_MENU_TYPE                
                WHERE LUNCH_SEQ_CD = @LUNCH_SEQ_CD ");
                result = base.DbUpdate(sql.ToString(), meal, new
                {
                    SETUP_USER_ACCOUNT = CmnEntityModel.UserSegNo,
                    LUNCH_DATE = DateTime.Now,
                    LUNCH_MENU_TYPE = meal.LUNCH_MENU_TYPE,                    
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE  
                });
            }
            else
            {
                meal.LUNCH_DATE = DateTime.Now;
                meal.SETUP_USER_ACCOUNT = CmnEntityModel.UserSegNo;
                meal.STATUS = Constants.StatusMeal.CREATED;
                meal.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
                StringBuilder sqlinsert = new StringBuilder();
                sqlinsert.Append(@"
                INSERT INTO LUNCH_INFORMATION
                (
                    SETUP_USER_ACCOUNT,
                    LUNCH_DATE,
                    LUNCH_MENU_TYPE,
                    STATUS,
                    DEL_FLG
                )
                VALUES
                (
                    @SETUP_USER_ACCOUNT,
                    @LUNCH_DATE,
                    @LUNCH_MENU_TYPE,
                    @STATUS,
                    @DEL_FLG
                ) ");
                var res = base.DbAdd(sqlinsert.ToString(), meal);
                if (res > 0)
                {
                    // Get Balance now 
                    StringBuilder sql = new StringBuilder();
                    long balance;
                    sql.Append(@"
                        SELECT
					        BALANCE
                        FROM
	                        USERS_PAYMENT                                              
                        WHERE
                            DEL_FLG = 0 AND SETUP_USER_SEQ_CD = @SETUP_USER_SEQ_CD
                    ");

                    balance = base.Query<long>(sql.ToString(),
                        new
                        {
                            SETUP_USER_SEQ_CD = CmnEntityModel.UserSegNo
                        }).FirstOrDefault();

                    // Update balance now
                    StringBuilder sqlupdate = new StringBuilder();
                    UserPaymentEntity userPayment = new UserPaymentEntity();                    
                    userPayment.SETUP_USER_SEQ_CD = CmnEntityModel.UserSegNo;
                    userPayment.BALANCE = balance - Constants.UnitMeal.NORMAL;
                    sqlupdate.Append(@"
                     UPDATE [dbo].[USERS_PAYMENT] 
                     SET [BALANCE] = @BALANCE                                        
                        WHERE DEL_FLG = '0' AND SETUP_USER_SEQ_CD = @SETUP_USER_SEQ_CD ");
                    var resupd = base.DbUpdate(sqlupdate.ToString(), userPayment, new {
                        SETUP_USER_SEQ_CD = CmnEntityModel.UserSegNo,                                    
                        DEL_FLG = Constants.DeleteFlag.NON_DELETE
                    });
                    if (resupd > 0)
                    {
                        var query = "SELECT ident_current('LUNCH_INFORMATION')";
                        return base.ExecuteScalar<long>(query);
                    }
                }
            }

            return result;
        }

        public long GetBalance()
        {
            StringBuilder sql = new StringBuilder();
            long balance;
            sql.Append(@"
                SELECT
					BALANCE
                FROM
	                USERS_PAYMENT                                              
                WHERE
                    DEL_FLG = 0 AND SETUP_USER_SEQ_CD = @SETUP_USER_SEQ_CD
            ");

            balance = base.Query<long>(sql.ToString(),
                new
                {                    
                    SETUP_USER_SEQ_CD = CmnEntityModel.UserSegNo                   
                }).FirstOrDefault();

            return balance;
        }

        public string GetMailBody()
        {
            StringBuilder sql = new StringBuilder();
            string mailbody= string.Empty;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            int year = DateTime.Now.Year;
            System.DateTime from_date = new System.DateTime(year, month, day, 0, 0, 0, 0);
            System.DateTime to_date = new System.DateTime(year, month, day, 11, 30, 0, 0);
            //long balance;
            sql.Append(@"
                select TB.LUNCH_MENU_TYPE,LM.LUNCH_MENU_NAME,TB.COUNT_MENU from (select LUNCH_MENU_TYPE, count(LUNCH_MENU_TYPE) as COUNT_MENU
                  from 
                  LUNCH_INFORMATION
                  WHERE DEL_FLG = '0' AND LUNCH_DATE <= @TO_DATE AND LUNCH_DATE>= @FROM_DATE AND STATUS = @STATUS
                  GROUP BY LUNCH_MENU_TYPE) AS TB

                  INNER JOIN LUNCH_MENU LM ON 
	                TB.LUNCH_MENU_TYPE = LM.LUNCH_MENU_TYPE
            ");

            var result = base.Query<LunchMailEntity>(sql.ToString(),
                new
                {
                    STATUS = Constants.StatusMeal.BOOKED,
                    FROM_DATE = from_date,
                    TO_DATE = to_date
                }).ToList();
            StringBuilder content = new StringBuilder();
            content.Append("Hôm nay cho anh như sau nhé : ");
            content.AppendLine();
            foreach (var menu in result)
            {
                content.Append(String.Format("{0} {1}", menu.COUNT_MENU, menu.LUNCH_MENU_NAME));
                content.AppendLine();
            }
            mailbody = content.ToString();
            return mailbody;
        }

        public IEnumerable<LunchListModel> LunchSearch(DataTablesModel dt, ref LunchListModel searchCondition, out int totalrow)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT
					LI.LUNCH_SEQ_CD,
                    LI.SETUP_USER_ACCOUNT,
                    LI.LUNCH_DATE,
                    LI.DEL_FLG,
                    LI.LUNCH_MENU_TYPE,
                    LI.STATUS,
                    LM.LUNCH_MENU_NAME,
                    US.SETUP_USER_NAME
                FROM
	                LUNCH_INFORMATION   LI  
                        INNER JOIN LUNCH_MENU LM ON
                            LI.LUNCH_MENU_TYPE = LM.LUNCH_MENU_TYPE
                        INNER JOIN USERS US ON
                            LI.SETUP_USER_ACCOUNT = US.SETUP_USER_SEQ_CD                                              
                WHERE
                    LI.DEL_FLG = 0 
            ");

            if (CmnEntityModel.UserType != 0)
            {
                sql.Append(@"
                    AND LI.SETUP_USER_ACCOUNT = @SETUP_USER_ACCOUNT  ");
            }

            if (!String.IsNullOrEmpty(searchCondition.SEARCH_APPLY_DATE_FROM))
            {
                sql.Append(@"
                    AND LUNCH_DATE >= @SEARCH_APPLY_DATE_FROM  ");
            }

            if (!String.IsNullOrEmpty(searchCondition.SEARCH_APPLY_DATE_TO))
            {
                sql.Append(@"
                    AND LUNCH_DATE <= @SEARCH_APPLY_DATE_TO  ");
            }           

            sql.Append(@"
                ORDER BY LUNCH_DATE DESC");

            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;

            var dataList = base.Query<LunchListModel>(sqlpage,
                new
                {
                    SEARCH_APPLY_DATE_FROM = searchCondition.SEARCH_APPLY_DATE_FROM,
                    SEARCH_APPLY_DATE_TO = searchCondition.SEARCH_APPLY_DATE_TO,     
                    SETUP_USER_ACCOUNT = CmnEntityModel.UserSegNo,
                    pageindex = lower,
                    pagesize = upper
                }).ToList();

            totalrow = base.Query<int>(sqlcount,
                new
                {
                    SEARCH_APPLY_DATE_FROM = searchCondition.SEARCH_APPLY_DATE_FROM,
                    SEARCH_APPLY_DATE_TO = searchCondition.SEARCH_APPLY_DATE_TO,
                    SETUP_USER_ACCOUNT = CmnEntityModel.UserSegNo,
                    pageindex = lower,
                    pagesize = upper
                }).FirstOrDefault();

            return dataList;
        }

        public int UpdatePayment(PaymentEntity billingHeader)
        {
            StringBuilder sqlupdate = new StringBuilder();
            sqlupdate.Append(@"
                UPDATE [dbo].[USERS_PAYMENT]
                   SET [BALANCE] = @BALANCE                     
                 WHERE [PAYMENT_CD] = @PAYMENT_CD
                        ");

            return base.DbUpdate(sqlupdate.ToString(), billingHeader, new { PAYMENT_CD = billingHeader.PAYMENT_CD });
        }

        public IEnumerable<PaymentListModel> PaymentSearch(DataTablesModel dt, ref PaymentListModel searchCondition, out int totalrow)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT
					UP.PAYMENT_CD,
                    UP.SETUP_USER_SEQ_CD,
                    UP.BALANCE,
                    UP.DEL_FLG,                   
                    U.SETUP_USER_NAME
                FROM
	                USERS_PAYMENT   UP  
                        INNER JOIN USERS U ON
                            U.SETUP_USER_SEQ_CD = UP.SETUP_USER_SEQ_CD                                                                      
                WHERE
                    UP.DEL_FLG = 0 AND U.DEL_FLG = 0
                ORDER BY BALANCE ASC
            ");

//            if (CmnEntityModel.UserType != 0)
//            {
//                sql.Append(@"
//                    AND LI.SETUP_USER_ACCOUNT = @SETUP_USER_ACCOUNT  ");
//            }

            if (!String.IsNullOrEmpty(searchCondition.NAME_SEARCH))
            {
                sql.Append(@"
                    AND U.SETUP_USER_NAME LIKE @SETUP_USER_NAME  ");
            }

//            if (!String.IsNullOrEmpty(searchCondition.SEARCH_APPLY_DATE_TO))
//            {
//                sql.Append(@"
//                    AND LUNCH_DATE <= @SEARCH_APPLY_DATE_TO  ");
//            }

//            sql.Append(@"
//                ORDER BY SETUP_USER_SEQ_CD ASC");

            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;

            var dataList = base.Query<PaymentListModel>(sqlpage,
                new
                {
                    SETUP_USER_NAME = '%'+searchCondition.NAME_SEARCH+'%',
                    pageindex = lower,
                    pagesize = upper
                }).ToList();

            totalrow = base.Query<int>(sqlcount,
                new
                {
                    SETUP_USER_NAME = '%' + searchCondition.NAME_SEARCH + '%',
                    pageindex = lower,
                    pagesize = upper
                }).FirstOrDefault();

            return dataList;
        }

        public int DeleteLunchDeal(long LUNCH_SEQ_CD = 0)
        {

            // Get Balance now 
            StringBuilder sql = new StringBuilder();
            long balance;
            sql.Append(@"
                        SELECT
					        BALANCE
                        FROM
	                        USERS_PAYMENT                                              
                        WHERE
                            DEL_FLG = 0 AND SETUP_USER_SEQ_CD = @SETUP_USER_SEQ_CD
                    ");

            balance = base.Query<long>(sql.ToString(),
                new
                {
                    SETUP_USER_SEQ_CD = CmnEntityModel.UserSegNo
                }).FirstOrDefault();

            // Update balance now
            StringBuilder sqlupdate = new StringBuilder();
            UserPaymentEntity userPayment = new UserPaymentEntity();
            userPayment.SETUP_USER_SEQ_CD = CmnEntityModel.UserSegNo;
            userPayment.BALANCE = balance + Constants.UnitMeal.NORMAL;
            sqlupdate.Append(@"
                     UPDATE [dbo].[USERS_PAYMENT] 
                     SET [BALANCE] = @BALANCE                                        
                        WHERE DEL_FLG = '0' AND SETUP_USER_SEQ_CD = @SETUP_USER_SEQ_CD ");
            var resupd = base.DbUpdate(sqlupdate.ToString(), userPayment, new
            {
                SETUP_USER_SEQ_CD = CmnEntityModel.UserSegNo,
                DEL_FLG = Constants.DeleteFlag.NON_DELETE
            });

            if(resupd ==0)
            {
                return resupd;
            }

            #region Delete Mst_TaxRate
            StringBuilder sqlupdateLunch = new StringBuilder();
            sqlupdateLunch.Append(" UPDATE LUNCH_INFORMATION");
            sqlupdateLunch.Append(" SET ");
            sqlupdateLunch.Append(" DEL_FLG = @DEL_FLG");

            sqlupdateLunch.Append(" WHERE LUNCH_SEQ_CD = @LUNCH_SEQ_CD");

            return base.Execute(sqlupdateLunch.ToString(), new
            {
                LUNCH_SEQ_CD = LUNCH_SEQ_CD,
                DEL_FLG = Constants.DeleteFlag.DELETE
            });
            #endregion

        }

        public int ApproveLunchDeal()
        {

            #region Approve Meal
            StringBuilder sqlupdate = new StringBuilder();
            sqlupdate.Append(" UPDATE LUNCH_INFORMATION");
            sqlupdate.Append(" SET ");
            sqlupdate.Append(" STATUS = @STATUS");

            sqlupdate.Append(" WHERE DEL_FLG = @DEL_FLG");

            return base.Execute(sqlupdate.ToString(), new
            {
                STATUS = Constants.StatusMeal.BOOKED,
                DEL_FLG = Constants.DeleteFlag.NON_DELETE
            });
            #endregion

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

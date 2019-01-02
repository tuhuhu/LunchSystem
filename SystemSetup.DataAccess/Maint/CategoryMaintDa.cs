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
    public class CategoryMaintDa : BaseDa
    {
        #region READ

        /// <summary>
        /// Search payment/Not payment
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<CategoryMaintModel> CategoryMaintSearch(DataTablesModel dt, ref CategoryMaintModel searchCondition, out int totalrow)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT
					mfcc.CONTRACT_TYPE,
	                mfcc.COMPANY_CD,
	                mfcc.CONTRACT_TYPE_DISP_NAME,
	                mfcc.DSP_PRIORITY,
                    mfcc.EST_NO_PREFIX,
                    mfcc.DELIVERY_NO_PREFIX,
                    mfcc.PLC_ORDER_NO_PREFIX,
                    mfcc.EST_EFFECTIVE_TYPE,
                    mfcc.EST_EFFECTIVE_INTERVAL,
	                mfcc.UPD_DATE,
	                mcfu.CONTRACT_USER_NAME AS UPD_USERNAME,
                    mcf.CONTRACT_LEVEL_TYPE
                FROM
	                Mst_FirmContractConfig mfcc
                        LEFT JOIN Mst_ContractFirmUser mcfu ON
                            mfcc.COMPANY_CD = mcfu.COMPANY_CD AND
                            mfcc.UPD_USER_ID = mcfu.CONTRACT_USER_SEQ_CD
                        LEFT JOIN Mst_ContractFirm mcf ON
                            mfcc.COMPANY_CD = mcf.COMPANY_CD
                WHERE
                    mfcc.DEL_FLG = 0
            ");

            if (!String.IsNullOrEmpty(searchCondition.COMPANY_CD))
            {
                sql.Append(@"
                    AND mfcc.COMPANY_CD = @COMPANY_CD  ");
            }
            string contract_type = "";
            if (!String.IsNullOrEmpty(searchCondition.CONTRACT_TYPE_EDIT))
            {
                sql.Append(@"
                    AND mfcc.CONTRACT_TYPE = @CONTRACT_TYPE");

                contract_type = searchCondition.CONTRACT_TYPE_EDIT.Split('_')[1];
            }

            if (!String.IsNullOrEmpty(searchCondition.EST_NO_PREFIX))
            {
                sql.Append(@"
                    AND mfcc.EST_NO_PREFIX LIKE @EST_NO_PREFIX  ");
            }

            if (searchCondition.EST_EFFECTIVE_INTERVAL != 0 && searchCondition.EST_EFFECTIVE_INTERVAL != null)
            {
                sql.Append(@"
                    AND mfcc.EST_EFFECTIVE_INTERVAL = @EST_EFFECTIVE_INTERVAL  ");
            }

            if (searchCondition.EST_EFFECTIVE_TYPE != null)
            {
                sql.Append(@"
                    AND mfcc.EST_EFFECTIVE_TYPE = @EST_EFFECTIVE_TYPE  ");
            }

            sql.Append(@"
                ORDER BY mfcc.DSP_PRIORITY DESC");


            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;


            var dataList = base.Query<CategoryMaintModel>(sqlpage,
                new
                {
                    COMPANY_CD = searchCondition.COMPANY_CD,
                    CONTRACT_TYPE = contract_type,
                    EST_NO_PREFIX = '%' + searchCondition.EST_NO_PREFIX + '%',
                    EST_EFFECTIVE_TYPE = searchCondition.EST_EFFECTIVE_TYPE,
                    EST_EFFECTIVE_INTERVAL = searchCondition.EST_EFFECTIVE_INTERVAL,
                    pageindex = lower,
                    pagesize = upper
                }).ToList();

            totalrow = base.Query<int>(sqlcount,
                new
                {
                    COMPANY_CD = searchCondition.COMPANY_CD,
                    CONTRACT_TYPE = contract_type,
                    EST_NO_PREFIX = searchCondition.EST_NO_PREFIX,
                    EST_EFFECTIVE_TYPE = searchCondition.EST_EFFECTIVE_TYPE,
                    EST_EFFECTIVE_INTERVAL = searchCondition.EST_EFFECTIVE_INTERVAL,
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
        public CategoryMaintModel GetInformation(string companyCd, int contractType)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
					mfcc.*,
	                mcfu_upd.CONTRACT_USER_NAME AS UPD_USERNAME,
	                mcfu_ins.CONTRACT_USER_NAME AS INS_USERNAME,
                    mcf.CONTRACT_LEVEL_TYPE
                FROM
	                Mst_FirmContractConfig mfcc
                        LEFT JOIN Mst_ContractFirmUser mcfu_upd ON
                            mfcc.COMPANY_CD = mcfu_upd.COMPANY_CD AND
                            mfcc.UPD_USER_ID = mcfu_upd.CONTRACT_USER_SEQ_CD
                        LEFT JOIN Mst_ContractFirmUser mcfu_ins ON
                            mfcc.COMPANY_CD = mcfu_ins.COMPANY_CD AND
                            mfcc.INS_USER_ID = mcfu_ins.CONTRACT_USER_SEQ_CD
                        LEFT JOIN Mst_ContractFirm mcf ON
                            mfcc.COMPANY_CD = mcf.COMPANY_CD
                WHERE
                    mfcc.COMPANY_CD = @COMPANY_CD AND
					mfcc.CONTRACT_TYPE = @CONTRACT_TYPE
            ");

            return base.Query<CategoryMaintModel>(sql.ToString(), new
            {
                COMPANY_CD = companyCd,
                CONTRACT_TYPE = contractType
            }).FirstOrDefault();
        }

        public CategoryMaintModel GetContractLevelType(string companyCd)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
                    mcf.CONTRACT_LEVEL_TYPE
                FROM
                    Mst_ContractFirm mcf
                WHERE
                    mcf.COMPANY_CD = @COMPANY_CD
            ");

            return base.Query<CategoryMaintModel>(sql.ToString(), new
            {
                COMPANY_CD = companyCd
            }).FirstOrDefault();
        }

        #endregion

        #region UPDATE
        public long UpdateCategoryMaintModel(CategoryMaintModel model)
        {
            StringBuilder sql = new StringBuilder();
            int result = 0;
            sql.Append(@"
                UPDATE [dbo].[Mst_FirmContractConfig] 
                    SET [DSP_PRIORITY] = @DSP_PRIORITY
                    ,[CONTRACT_TYPE_DISP_NAME] = @CONTRACT_TYPE_DISP_NAME
                    ,[EST_NO_PREFIX] = @EST_NO_PREFIX
                    ,[DELIVERY_NO_PREFIX] = @DELIVERY_NO_PREFIX
                    ,[PLC_ORDER_NO_PREFIX] = @PLC_ORDER_NO_PREFIX
                    ,[EST_EFFECTIVE_TYPE] = @EST_EFFECTIVE_TYPE
                    ,[EST_EFFECTIVE_INTERVAL] = @EST_EFFECTIVE_INTERVAL
                    ,[UPD_DATE] = @UPD_DATE
                    ,[UPD_USER_ID] = @UPD_USER_ID
                    ,[UPD_PROG_ID] = @UPD_PROG_ID
                WHERE COMPANY_CD = @COMPANY_CD AND
                    CONTRACT_TYPE = @CONTRACT_TYPE
            ");

            FirmContractConfigEntity configEntity = new FirmContractConfigEntity();

            configEntity.DSP_PRIORITY = model.DSP_PRIORITY;
            configEntity.CONTRACT_TYPE_DISP_NAME = model.CONTRACT_TYPE_DISP_NAME;
            configEntity.EST_NO_PREFIX = model.EST_NO_PREFIX;
            configEntity.DELIVERY_NO_PREFIX = model.DELIVERY_NO_PREFIX;
            configEntity.PLC_ORDER_NO_PREFIX = model.PLC_ORDER_NO_PREFIX;
            configEntity.EST_EFFECTIVE_TYPE = model.EST_EFFECTIVE_TYPE;
            configEntity.EST_EFFECTIVE_INTERVAL = model.EST_EFFECTIVE_INTERVAL;
            configEntity.UPD_DATE = Utility.GetCurrentDateTime();
            configEntity.UPD_USER_ID = 0;
            configEntity.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;

            configEntity.COMPANY_CD = model.COMPANY_CD;
            configEntity.CONTRACT_TYPE = model.CONTRACT_TYPE;

            result = base.DbUpdateByOutside(sql.ToString(), configEntity, new
            {
                COMPANY_CD = model.COMPANY_CD,
                CONTRACT_TYPE = model.CONTRACT_TYPE
            });

            if (result != 0 && model.CONTRACT_LEVEL_TYPE.Equals("0"))
            {
                sql.Clear();
                sql.Append(@"
                    SELECT
                        *
                    FROM
                        Mst_FirmContractClass mfcc
                    WHERE
                        mfcc.COMPANY_CD = @COMPANY_CD
                        AND mfcc.CONTRACT_TYPE = @CONTRACT_TYPE
                        AND mfcc.CONTRACT_TYPE_CLASS = ISNULL((SELECT MIN(CONTRACT_TYPE_CLASS) FROM Mst_FirmContractClass WHERE COMPANY_CD = @COMPANY_CD AND CONTRACT_TYPE = @CONTRACT_TYPE), 0)
                ");

                var wrk = base.Query<FirmContractClassEntityPlus>(sql.ToString(), new
                {
                    COMPANY_CD = model.COMPANY_CD,
                    CONTRACT_TYPE = model.CONTRACT_TYPE
                }).FirstOrDefault();

                FirmContractClassEntity classEntity = new FirmContractClassEntity();

                classEntity.COMPANY_CD = model.COMPANY_CD;
                classEntity.CONTRACT_TYPE = model.CONTRACT_TYPE;
                classEntity.DSP_PRIORITY = model.DSP_PRIORITY;
                classEntity.CONTRACT_TYPE_CLASS_DISP_NAME = model.CONTRACT_TYPE_DISP_NAME;
                classEntity.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
                classEntity.UPD_DATE = Utility.GetCurrentDateTime();
                classEntity.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
                classEntity.INS_USER_ID = 0;
                classEntity.INS_DATE = Utility.GetCurrentDateTime();
                classEntity.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
                classEntity.UPD_USER_ID = 0;

                if (wrk == null)
                {
                    sql.Clear();
                    sql.Append(@"
                        INSERT INTO Mst_FirmContractClass
                        (
                            COMPANY_CD,
                            CONTRACT_TYPE,
                            CONTRACT_TYPE_CLASS,
                            DSP_PRIORITY,
                            CONTRACT_TYPE_CLASS_DISP_NAME,
                            DEL_FLG,
                            INS_DATE,
                            INS_USER_ID,
                            UPD_DATE,
                            UPD_USER_ID,
                            UPD_PROG_ID
                        )
                        VALUES
                        (
                            @COMPANY_CD,
                            @CONTRACT_TYPE,
                            ISNULL((SELECT MAX(CONTRACT_TYPE_CLASS) FROM [Mst_FirmContractClass] WHERE [COMPANY_CD] = @COMPANY_CD AND [CONTRACT_TYPE] = @CONTRACT_TYPE), 0) + 1,
                            @DSP_PRIORITY,
                            @CONTRACT_TYPE_CLASS_DISP_NAME,
                            @DEL_FLG,
                            @INS_DATE,
                            @INS_USER_ID,
                            @UPD_DATE,
                            @UPD_USER_ID,
                            @UPD_PROG_ID
                        )
                    ");

                    result = base.DbAddByOutside(sql.ToString(), classEntity);
                }
                else
                {
                    sql.Clear();
                    sql.Append(@"
                        UPDATE [dbo].[Mst_FirmContractClass] 
                            SET [DSP_PRIORITY] = @DSP_PRIORITY
                            ,[CONTRACT_TYPE_CLASS_DISP_NAME] = @CONTRACT_TYPE_CLASS_DISP_NAME
                            ,[UPD_DATE] = @UPD_DATE
                            ,[UPD_USER_ID] = @UPD_USER_ID
                            ,[UPD_PROG_ID] = @UPD_PROG_ID
                        WHERE COMPANY_CD = @COMPANY_CD AND
                            CONTRACT_TYPE = @CONTRACT_TYPE AND
                            CONTRACT_TYPE_CLASS = ISNULL((SELECT MIN(CONTRACT_TYPE_CLASS) FROM Mst_FirmContractClass WHERE COMPANY_CD = @COMPANY_CD AND CONTRACT_TYPE = @CONTRACT_TYPE), 0)
                    ");

                    result = base.DbUpdateByOutside(sql.ToString(), classEntity, new
                    {
                        COMPANY_CD = model.COMPANY_CD,
                        CONTRACT_TYPE = model.CONTRACT_TYPE
                    });
                }
            }
            return result;
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Delete Mst_FirmContractClass
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int DeleteCategoryMaint(string COMPANY_CD, int CONTRACT_TYPE, string CONTRACT_LEVEL_TYPE)
        {
            int result = 0;

            #region Delete Mst_FirmContractClass
            result = Delete(COMPANY_CD, CONTRACT_TYPE, CONTRACT_LEVEL_TYPE);
            #endregion

            return result;
        }

        /// <summary>
        /// Delete Mst_FirmContractClass
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public int Delete(string COMPANY_CD, int CONTRACT_TYPE, string CONTRACT_LEVEL_TYPE)
        {
            int result = 0;

            StringBuilder sql = new StringBuilder();
            sql.Append(" UPDATE Mst_FirmContractConfig");
            sql.Append(" SET ");
            sql.Append("    DEL_FLG = @DEL_FLG,");
            sql.Append("    UPD_DATE = @UPD_DATE,");
            sql.Append("    UPD_USER_ID = @UPD_USER_ID");

            sql.Append(" WHERE    COMPANY_CD = @COMPANY_CD");
            sql.Append("  AND CONTRACT_TYPE = @CONTRACT_TYPE");

            result = base.Execute(sql.ToString(), new
            {
                COMPANY_CD = COMPANY_CD,
                CONTRACT_TYPE = CONTRACT_TYPE,

                DEL_FLG = Constants.DeleteFlag.DELETE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = 0
            });

            if (result != 0)
            {
                sql.Clear();

                sql.Append(" UPDATE Mst_FirmContractClass");
                sql.Append(" SET ");
                sql.Append("    DEL_FLG = @DEL_FLG,");
                sql.Append("    UPD_DATE = @UPD_DATE,");
                sql.Append("    UPD_USER_ID = @UPD_USER_ID");

                sql.Append(" WHERE    COMPANY_CD = @COMPANY_CD");
                sql.Append("  AND CONTRACT_TYPE = @CONTRACT_TYPE");

                result += base.Execute(sql.ToString(), new
                {
                    COMPANY_CD = COMPANY_CD,
                    CONTRACT_TYPE = CONTRACT_TYPE,

                    DEL_FLG = Constants.DeleteFlag.DELETE,
                    UPD_DATE = Utility.GetCurrentDateTime(),
                    UPD_USER_ID = 0
                });

                Common.CommonDa cDa = new Common.CommonDa();
                result += cDa.deleteMstProductByOutside(COMPANY_CD, CONTRACT_TYPE);
                result += cDa.deleteMstItemByOutside(COMPANY_CD, CONTRACT_TYPE);
            }

            return result;
        }

        #endregion


        #region CREATE
        /// <summary>
        /// Insert into Mst_FirmContractClass
        /// </summary>
        /// <param name="Maint"></param>
        /// <returns></returns>
        public long InsertCategoryMaint(CategoryMaintModel model)
        {
            int result = 0;

            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                INSERT INTO Mst_FirmContractConfig
                (
                    COMPANY_CD,
                    CONTRACT_TYPE,
                    DSP_PRIORITY,
                    CONTRACT_TYPE_DISP_NAME,
                    EST_NO_PREFIX,
                    DELIVERY_NO_PREFIX,
                    PLC_ORDER_NO_PREFIX,
                    EST_EFFECTIVE_TYPE,
                    EST_EFFECTIVE_INTERVAL,
                    DEL_FLG,
                    INS_DATE,
                    INS_USER_ID,
                    UPD_DATE,
                    UPD_USER_ID,
                    UPD_PROG_ID
                )
                VALUES
                (
                    @COMPANY_CD,
                    ISNULL((SELECT MAX(CONTRACT_TYPE) FROM [Mst_FirmContractConfig] WHERE [COMPANY_CD] = @COMPANY_CD), 0) + 1,
                    @DSP_PRIORITY,
                    @CONTRACT_TYPE_DISP_NAME,
                    @EST_NO_PREFIX,
                    @DELIVERY_NO_PREFIX,
                    @PLC_ORDER_NO_PREFIX,
                    @EST_EFFECTIVE_TYPE,
                    @EST_EFFECTIVE_INTERVAL,
                    @DEL_FLG,
                    @INS_DATE,
                    @INS_USER_ID,
                    @UPD_DATE,
                    @UPD_USER_ID,
                    @UPD_PROG_ID
                ) 
            ");

            FirmContractConfigEntity configEntity = new FirmContractConfigEntity();

            configEntity.COMPANY_CD = model.COMPANY_CD;
            configEntity.DSP_PRIORITY = model.DSP_PRIORITY;
            configEntity.CONTRACT_TYPE_DISP_NAME = model.CONTRACT_TYPE_DISP_NAME;
            configEntity.EST_NO_PREFIX = model.EST_NO_PREFIX;
            configEntity.DELIVERY_NO_PREFIX = model.DELIVERY_NO_PREFIX;
            configEntity.PLC_ORDER_NO_PREFIX = model.PLC_ORDER_NO_PREFIX;
            configEntity.EST_EFFECTIVE_TYPE = model.EST_EFFECTIVE_TYPE;
            configEntity.EST_EFFECTIVE_INTERVAL = model.EST_EFFECTIVE_INTERVAL;
            configEntity.UPD_DATE = Utility.GetCurrentDateTime();
            configEntity.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
            configEntity.INS_USER_ID = 0;
            configEntity.INS_DATE = Utility.GetCurrentDateTime();
            configEntity.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
            configEntity.UPD_USER_ID = 0;

            result = base.DbAddByOutside(sql.ToString(), configEntity);

            if (result != 0 && model.CONTRACT_LEVEL_TYPE.Equals("0"))
            {
                FirmContractClassEntity classEntity = new FirmContractClassEntity();

                classEntity.COMPANY_CD = model.COMPANY_CD;
                classEntity.CONTRACT_TYPE = model.CONTRACT_TYPE;
                classEntity.DSP_PRIORITY = model.DSP_PRIORITY;
                classEntity.CONTRACT_TYPE_CLASS_DISP_NAME = model.CONTRACT_TYPE_DISP_NAME;
                classEntity.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
                classEntity.UPD_DATE = Utility.GetCurrentDateTime();
                classEntity.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
                classEntity.INS_USER_ID = 0;
                classEntity.INS_DATE = Utility.GetCurrentDateTime();
                classEntity.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
                classEntity.UPD_USER_ID = 0;

                sql.Clear();
                sql.Append(@"
                        INSERT INTO Mst_FirmContractClass
                        (
                            COMPANY_CD,
                            CONTRACT_TYPE,
                            CONTRACT_TYPE_CLASS,
                            DSP_PRIORITY,
                            CONTRACT_TYPE_CLASS_DISP_NAME,
                            DEL_FLG,
                            INS_DATE,
                            INS_USER_ID,
                            UPD_DATE,
                            UPD_USER_ID,
                            UPD_PROG_ID
                        )
                        VALUES
                        (
                            @COMPANY_CD,
                            ISNULL((SELECT MAX(CONTRACT_TYPE) FROM [Mst_FirmContractConfig] WHERE [COMPANY_CD] = @COMPANY_CD), 0),
                            ISNULL((SELECT MAX(CONTRACT_TYPE_CLASS) FROM [Mst_FirmContractClass] WHERE [COMPANY_CD] = @COMPANY_CD AND [CONTRACT_TYPE] = @CONTRACT_TYPE), 0) + 1,
                            @DSP_PRIORITY,
                            @CONTRACT_TYPE_CLASS_DISP_NAME,
                            @DEL_FLG,
                            @INS_DATE,
                            @INS_USER_ID,
                            @UPD_DATE,
                            @UPD_USER_ID,
                            @UPD_PROG_ID
                        )
                ");

                result = base.DbAddByOutside(sql.ToString(), classEntity);
            }

            return result;
        }
        #endregion
    }
}

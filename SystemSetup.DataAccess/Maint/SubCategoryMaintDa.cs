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
    public class SubCategoryMaintDa : BaseDa
    {
        #region READ

        /// <summary>
        /// Search payment/Not payment
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<SubCategoryMaintModel> SubCategoryMaintSearch(DataTablesModel dt, ref SubCategoryMaintModel searchCondition, out int totalrow)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT
					mfcc.CONTRACT_TYPE,
	                mfcc.CONTRACT_TYPE_CLASS,
	                mfcc.COMPANY_CD,
	                mfcc.CONTRACT_TYPE_CLASS_DISP_NAME,
	                mfcc.DSP_PRIORITY,
	                mfcc.UPD_DATE,
	                mcfu.CONTRACT_USER_NAME AS UPD_USERNAME,
                    mfco.CONTRACT_TYPE_DISP_NAME AS CONTRACT_TYPE_DISP_NAME
                FROM
	                Mst_FirmContractClass mfcc
                        LEFT JOIN Mst_ContractFirmUser mcfu ON
                            mfcc.COMPANY_CD = mcfu.COMPANY_CD AND
                            mfcc.UPD_USER_ID = mcfu.CONTRACT_USER_SEQ_CD
                        LEFT JOIN Mst_FirmContractConfig mfco ON
                            mfcc.COMPANY_CD = mfco.COMPANY_CD AND
                            mfcc.CONTRACT_TYPE = mfco.CONTRACT_TYPE AND
                            mfco.DEL_FLG = 0
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
            string contract_type_class = "";
            if (!String.IsNullOrEmpty(searchCondition.CONTRACT_TYPE_CLASS_EDIT))
            {
                sql.Append(@"
                    AND mfcc.CONTRACT_TYPE_CLASS = @CONTRACT_TYPE_CLASS");

                contract_type_class = searchCondition.CONTRACT_TYPE_CLASS_EDIT.Split('_')[2];
            }

            sql.Append(@"
                ORDER BY 
                    mfcc.CONTRACT_TYPE,
                    mfcc.DSP_PRIORITY DESC,
                    mfcc.CONTRACT_TYPE_CLASS");


            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;


            var dataList = base.Query<SubCategoryMaintModel>(sqlpage,
                new
                {
                    COMPANY_CD = searchCondition.COMPANY_CD,
                    CONTRACT_TYPE = contract_type,
                    CONTRACT_TYPE_CLASS = contract_type_class,
                    pageindex = lower,
                    pagesize = upper
                }).ToList();

            totalrow = base.Query<int>(sqlcount,
                new
                {
                    COMPANY_CD = searchCondition.COMPANY_CD,
                    CONTRACT_TYPE = contract_type,
                    CONTRACT_TYPE_CLASS = contract_type_class,
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
        public SubCategoryMaintModel GetInformation(string companyCd, int contractType, int contractTypeClass)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
					mfcc.*,
	                mcfu_upd.CONTRACT_USER_NAME AS UPD_USERNAME,
	                mcfu_ins.CONTRACT_USER_NAME AS INS_USERNAME,
                    mfco.CONTRACT_TYPE_DISP_NAME AS CONTRACT_TYPE_DISP_NAME
                FROM
	                Mst_FirmContractClass mfcc
                        LEFT JOIN Mst_ContractFirmUser mcfu_upd ON
                            mfcc.COMPANY_CD = mcfu_upd.COMPANY_CD AND
                            mfcc.UPD_USER_ID = mcfu_upd.CONTRACT_USER_SEQ_CD
                        LEFT JOIN Mst_ContractFirmUser mcfu_ins ON
                            mfcc.COMPANY_CD = mcfu_ins.COMPANY_CD AND
                            mfcc.INS_USER_ID = mcfu_ins.CONTRACT_USER_SEQ_CD
                        LEFT JOIN Mst_FirmContractConfig mfco ON
                            mfcc.COMPANY_CD = mfco.COMPANY_CD AND
                            mfcc.CONTRACT_TYPE = mfco.CONTRACT_TYPE AND
                            mfco.DEL_FLG = 0
                WHERE
                    mfcc.COMPANY_CD = @COMPANY_CD AND
					mfcc.CONTRACT_TYPE = @CONTRACT_TYPE AND
	                mfcc.CONTRACT_TYPE_CLASS = @CONTRACT_TYPE_CLASS
            ");

            return base.Query<SubCategoryMaintModel>(sql.ToString(), new
            {
                COMPANY_CD = companyCd,
                CONTRACT_TYPE = contractType,
                CONTRACT_TYPE_CLASS = contractTypeClass
            }).FirstOrDefault();
        }

        /// <summary>
        /// GetSubCategory
        /// </summary>
        /// <returns></returns>
        public IList<SubCategoryMaintModel> GetSubCategory(string companyCd, int contractType, int contractTypeClass)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
                    *
                FROM
                    Mst_FirmContractClass
                WHERE
                    COMPANY_CD = @COMPANY_CD AND
                    CONTRACT_TYPE = @CONTRACT_TYPE AND
                    CONTRACT_TYPE_CLASS = @CONTRACT_TYPE_CLASS
            ");
            return base.Query<SubCategoryMaintModel>(sql.ToString(), new
            {
                COMPANY_CD = companyCd,
                CONTRACT_TYPE = contractType,
                CONTRACT_TYPE_CLASS = contractTypeClass
            }).ToList();
        }

        /// <summary>
        /// Get contract firm master
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractFirmEntity> GetContractFirmByContractTypeLevelExceptZero()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT * FROM Mst_ContractFirm WHERE DEL_FLG = @DEL_FLG AND CONTRACT_LEVEL_TYPE <> 0 ");
            sql.Append(" ORDER BY COMPANY_CD");

            var data = base.Query<ContractFirmEntity>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).ToList();

            return data;
        }

        /// <summary>
        /// Get contract firm master
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractFirmEntity> GetContractFirmMasterByOutside()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT * FROM Mst_ContractFirm WHERE DEL_FLG = @DEL_FLG ");
            sql.Append(" ORDER BY COMPANY_CD");

            var data = base.Query<ContractFirmEntity>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).ToList();

            return data;
        }
        #endregion

        #region UPDATE
        public long UpdateSubCategoryMaintModel(FirmContractClassEntity maint)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                UPDATE [dbo].[Mst_FirmContractClass] 
                    SET [DSP_PRIORITY] = @DSP_PRIORITY
                    ,[CONTRACT_TYPE_CLASS_DISP_NAME] = @CONTRACT_TYPE_CLASS_DISP_NAME
                    ,[UPD_DATE] = @UPD_DATE
                    ,[UPD_USER_ID] = @UPD_USER_ID
                    ,[UPD_PROG_ID] = @UPD_PROG_ID
                WHERE COMPANY_CD = @COMPANY_CD AND
                    CONTRACT_TYPE = @CONTRACT_TYPE AND
                    CONTRACT_TYPE_CLASS = @CONTRACT_TYPE_CLASS
            ");

            return base.DbUpdateByOutside(sql.ToString(), maint, new
            {
                COMPANY_CD = maint.COMPANY_CD,
                CONTRACT_TYPE = maint.CONTRACT_TYPE,
                CONTRACT_TYPE_CLASS = maint.CONTRACT_TYPE_CLASS
            });

            //return base.Execute(sql.ToString(), new
            //{
            //    DSP_PRIORITY = maint.DSP_PRIORITY,
            //    CONTRACT_TYPE_CLASS_DISP_NAME = maint.DSP_PRIORITY,
            //    UPD_DATE = maint.UPD_DATE,
            //    UPD_USER_ID = 0,
            //    UPD_PROG_ID = maint.UPD_PROG_ID,
            //    COMPANY_CD = maint.COMPANY_CD,
            //    CONTRACT_TYPE = maint.CONTRACT_TYPE,
            //    CONTRACT_TYPE_CLASS = maint.CONTRACT_TYPE_CLASS
            //});
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Delete Mst_FirmContractClass
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int DeleteSubCategoryMaint(string COMPANY_CD, int CONTRACT_TYPE, int CONTRACT_TYPE_CLASS)
        {
            int result = 0;

            #region Delete Mst_FirmContractClass
            result = Delete(COMPANY_CD, CONTRACT_TYPE, CONTRACT_TYPE_CLASS);
            #endregion

            return result;
        }

        /// <summary>
        /// Delete Mst_FirmContractClass
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public int Delete(string COMPANY_CD, int CONTRACT_TYPE, int CONTRACT_TYPE_CLASS)
        {
            StringBuilder sqlupdate = new StringBuilder();
            sqlupdate.Append(" UPDATE Mst_FirmContractClass");
            sqlupdate.Append(" SET ");
            sqlupdate.Append("    DEL_FLG = @DEL_FLG,");
            sqlupdate.Append("    UPD_DATE = @UPD_DATE,");
            sqlupdate.Append("    UPD_USER_ID = @UPD_USER_ID");

            sqlupdate.Append(" WHERE    COMPANY_CD = @COMPANY_CD");
            sqlupdate.Append("  AND CONTRACT_TYPE = @CONTRACT_TYPE");
            sqlupdate.Append("  AND CONTRACT_TYPE_CLASS = @CONTRACT_TYPE_CLASS");

            var res = base.Execute(sqlupdate.ToString(), new
            {
                COMPANY_CD = COMPANY_CD,
                CONTRACT_TYPE = CONTRACT_TYPE,
                CONTRACT_TYPE_CLASS = CONTRACT_TYPE_CLASS,

                DEL_FLG = Constants.DeleteFlag.DELETE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = 0
            });

            if (res > 0)
            {
                Common.CommonDa cDa = new Common.CommonDa();
                res += cDa.deleteMstProductByOutside(COMPANY_CD, CONTRACT_TYPE, CONTRACT_TYPE_CLASS);
                res += cDa.deleteMstItemByOutside(COMPANY_CD, CONTRACT_TYPE, CONTRACT_TYPE_CLASS);
            }
            return res;
        }

        #endregion


        #region CREATE
        /// <summary>
        /// Insert into Mst_FirmContractClass
        /// </summary>
        /// <param name="Maint"></param>
        /// <returns></returns>
        public long InsertSubCategoryMaint(FirmContractClassEntity Maint)
        {
            int result = 0;

            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append(@"
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
                ) ");
            result = base.DbAddByOutside(sqlinsert.ToString(), Maint);

            return result;
        }
        #endregion
    }
}

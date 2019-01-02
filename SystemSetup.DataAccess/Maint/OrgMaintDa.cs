//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/03
// Comment		: Create new
//------------------------------------------------------------------------
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;


namespace SystemSetup.DataAccess.Common
{
    #region using

    using SystemSetup.Models;
    using SystemSetup.Constants;
    using SystemSetup.UtilityServices.PagingHelper;
    using Dapper;
    using System.Text;
    using System.Data.Common;
    using SystemSetup.UtilityServices.LogService;
    using SystemSetup.UtilityServices;

	#endregion using

    /// <summary>
    /// Get List Tax rate
    /// </summary>
    public class OrgMaintDa : BaseDa
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public IList<OrgMaintModel> GetGroupByParentID(long parentID, string companyCd)
        {
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(@"
                SELECT 
                    *
                FROM [Mst_Group]
                WHERE COMPANY_CD = @COMPANY_CD
                    AND UP_GROUP_ID = @UP_GROUP_ID
                    AND DEL_FLG = @DEL_FLG
                ORDER BY GROUP_TYPE
            ");

            return base.Query<OrgMaintModel>(sql.ToString(), new
            {
                COMPANY_CD = companyCd,
                UP_GROUP_ID = parentID,
                DEL_FLG = Constants.DeleteFlag.NON_DELETE
            }).ToList();
        }

        /// <summary>
        /// Get list staff wwith email by group id
        /// </summary>
        /// <param name="GROUP_ID"></param>
        /// <returns></returns>
        public OrgMaintModel GetListByGroupId(long GROUP_ID)
        {
            string COMPANY_CD = base.CmnEntityModel.CompanyCd;
            var listOrgMaintInfo = new List<OrgMaintModel>();
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(@"
            SELECT
	            mg.GROUP_ID
	            ,mg.COMPANY_CD
	            ,mg.GROUP_TYPE
	            ,mg.GROUP_NAME
	            ,mg.GROUP_NAME_KANA
                ,mg.ANY_GROUP_CD
	            ,mg.DSP_PRIORITY
                ,mg.UPD_DATE
                ,mg.INS_DATE
            FROM
	            Mst_Group mg 
            WHERE
                mg.COMPANY_CD = @COMPANY_CD
                AND mg.GROUP_ID = @GROUP_ID
                AND mg.DEL_FLG = @DEL_FLG
            ORDER BY mg.GROUP_ID
            ");


            return base.Query<OrgMaintModel>(sql.ToString(), new
            {
                COMPANY_CD = COMPANY_CD,
                GROUP_ID = GROUP_ID,
                DEL_FLG = Constants.DeleteFlag.NON_DELETE
            }).FirstOrDefault();

        }

        /// <summary>
        /// Get list staff wwith email by group id
        /// </summary>
        /// <param name="GROUP_ID"></param>
        /// <returns></returns>
        public IList<OrgMaintInfo> GetListByName(string NAME)
        {
            string COMPANY_CD = base.CmnEntityModel.CompanyCd;
            var listOrgMaintInfo = new List<OrgMaintInfo>();
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(@"
            SELECT
	            mg.GROUP_ID
	            ,mg.COMPANY_CD
	            ,mg.GROUP_TYPE
	            ,mg.GROUP_NAME
	            ,mg.GROUP_NAME_KANA
                ,mg.ANY_GROUP_CD
	            ,mg.DSP_PRIORITY
            FROM
	            Mst_Group mg
            WHERE
                mg.COMPANY_CD = @COMPANY_CD
                AND mg.GROUP_NAME like '%' + @GROUP_NAME + '%'
                ORDER BY mg.GROUP_ID ");

            listOrgMaintInfo = base.Query<OrgMaintInfo>(sql.ToString(), new
            {
                COMPANY_CD = COMPANY_CD,
                GROUP_NAME = NAME
            }).ToList();

            return listOrgMaintInfo.ToList();
        }

        public int Update(OrgMaintRegistModel　update)
        {
            StringBuilder sql = new StringBuilder();

            //組織マスタ削除
            sql.Append(@"
             UPDATE dbo.Mst_Group 
             SET    GROUP_TYPE = @GROUP_TYPE
                    ,GROUP_NAME = @GROUP_NAME
                    ,GROUP_NAME_KANA = @GROUP_NAME_KANA
                    ,ANY_GROUP_CD = @ANY_GROUP_CD
                    ,UP_GROUP_ID = @UP_GROUP_ID
                    ,DEL_FLG = @DEL_FLG
                    ,UPD_DATE = @UPD_DATE
                    ,UPD_USER_ID = @UPD_USER_ID
                    ,UPD_PROG_ID = @UPD_PROG_ID
             WHERE COMPANY_CD = @COMPANY_CD AND GROUP_ID = @GROUP_ID
             ");

            int groupcnt = base.Execute(sql.ToString(), new
            {
                //更新
                GROUP_TYPE = update.GROUP_TYPE_NEW,
                GROUP_NAME = update.GROUP_NAME,
                GROUP_NAME_KANA = update.GROUP_NAME_KANA,
                ANY_GROUP_CD = update.ANY_GROUP_CD,
                UP_GROUP_ID = update.UP_GROUP_ID,
                DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = 0,
                UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE,
                //条件
                COMPANY_CD = update.COMPANY_CD,
                GROUP_ID = update.GROUP_ID,
            });
            return 0;
        }
        public int UpdateDelete(string groupidList, string companyCd)
        {
            StringBuilder sql = new StringBuilder();


            //組織社員関係テーブル削除
            sql.Append(@"
             UPDATE dbo.Rel_GroupStaff 
             SET    DEL_FLG = @DEL_FLG
                    ,UPD_DATE = @UPD_DATE
                    ,UPD_USER_ID = @UPD_USER_ID
                    ,UPD_PROG_ID = @UPD_PROG_ID
             FROM dbo.Rel_GroupStaff
             INNER JOIN Mst_Group gp ON 
                        dbo.Rel_GroupStaff.COMPANY_CD = gp.COMPANY_CD AND dbo.Rel_GroupStaff.GROUP_ID = gp.GROUP_ID

             WHERE gp.COMPANY_CD = @COMPANY_CD AND gp.GROUP_ID NOT IN ( " + groupidList + @")
             ");

            int relstaffcnt = base.Execute(sql.ToString(), new
            {
                //更新
                DEL_FLG = Constants.DeleteFlag.DELETE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = 0,
                UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE,
                //条件
                COMPANY_CD = companyCd,
                GROUP_ID = groupidList
            });

            //組織非属人関係マスタ削除
            sql.Append(@"
             UPDATE dbo.Rel_GroupNonPersonal 
             SET    DEL_FLG = @DEL_FLG
                    ,UPD_DATE = @UPD_DATE
                    ,UPD_USER_ID = @UPD_USER_ID
                    ,UPD_PROG_ID = @UPD_PROG_ID
             FROM dbo.Rel_GroupNonPersonal
             INNER JOIN Mst_Group gp ON 
                        dbo.Rel_GroupNonPersonal.COMPANY_CD = gp.COMPANY_CD AND dbo.Rel_GroupNonPersonal.GROUP_ID = gp.GROUP_ID
             WHERE gp.COMPANY_CD = @COMPANY_CD AND gp.GROUP_ID NOT IN ( " + groupidList + @")
             ");

            int relnonpersonalcnt = base.Execute(sql.ToString(), new
            {
                //更新
                DEL_FLG = Constants.DeleteFlag.DELETE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = 0,
                UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE,
                //条件
                COMPANY_CD = companyCd,
                GROUP_ID = groupidList
            });


            //組織マスタ削除
            sql.Append(@"
             UPDATE dbo.Mst_Group 
             SET    DEL_FLG = @DEL_FLG
                    ,UPD_DATE = @UPD_DATE
                    ,UPD_USER_ID = @UPD_USER_ID
                    ,UPD_PROG_ID = @UPD_PROG_ID
             WHERE COMPANY_CD = @COMPANY_CD AND GROUP_ID NOT IN ( " + groupidList + @")
             ");

            int groupcnt = base.Execute(sql.ToString(), new
            {
                //更新
                DEL_FLG = Constants.DeleteFlag.DELETE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = 0,
                UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE,
                //条件
                COMPANY_CD = companyCd,
                GROUP_ID = groupidList
            });
            return 0;
        }
        public long Insert(OrgMaintRegistModel insert)
        {
            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append(@"
                INSERT INTO Mst_Group
                (
                    COMPANY_CD,
                    GROUP_TYPE,
                    GROUP_NAME,
                    GROUP_NAME_KANA,
                    ANY_GROUP_CD,
                    UP_GROUP_ID,
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
                    @COMPANY_CD,
                    @GROUP_TYPE,
                    @GROUP_NAME,
                    @GROUP_NAME_KANA,
                    @ANY_GROUP_CD,
                    @UP_GROUP_ID,
                    @DSP_PRIORITY,
                    @DEL_FLG,
                    @INS_DATE,
                    @INS_USER_ID,
                    @UPD_DATE,
                    @UPD_USER_ID,
                    @UPD_PROG_ID
                ) ");
            var res = base.DbAddByOutside(sqlinsert.ToString(), insert);
            if (res > 0)
            {
                var query = "SELECT ident_current('Mst_Group')";
                return base.ExecuteScalar<long>(query);
            }
            return 0;
        }

        /// <summary>
        /// Get contract firm master
        /// </summary>
        /// <returns></returns>
        public ContractFirmEntity GetContractFirmMaster(string companyCd)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT * FROM Mst_ContractFirm WHERE COMPANY_CD = @COMPANY_CD AND DEL_FLG = @DEL_FLG ");
            return base.SingleOrDefault<ContractFirmEntity>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    COMPANY_CD = companyCd
                });
        }  
    }
}
//---------------------------------------------------------------------------
// Version Id		: 001
// Designer			: GiangVT
// Programmer		: GiangVT
// Date				: 2014/10/17
// Comment			: Create new
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.DataAccess
{
    #region using

    //using Framework;
    using SystemSetup.Models;
    using SystemSetup.Constants;
    using Dapper;
    using DapperExtensions;
    using SystemSetup.UtilityServices.LogService;

    #endregion using

    /// <summary>
    /// Data access class for Login screen
    /// </summary>
    public class PasswordReissueDa : BaseDa
    {
        /// <summary>
        ///  Update password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int UpdatePassword(SystemSetupUserEntity user)
        {
            // Create connection 
            StringBuilder sqlupdate = new StringBuilder();
            // UPDATE文生成
            sqlupdate.Append(@"
            UPDATE USERS
            SET 
                SETUP_USER_PASSWORD = @SETUP_USER_PASSWORD               
            WHERE SETUP_USER_ACCOUNT = @ORG_SETUP_USER_ACCOUNT");

            return base.DbUpdate(sqlupdate.ToString(), user, new
            {
                SETUP_USER_ACCOUNT = user.SETUP_USER_ACCOUNT
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsSamePassword(ContractFirmUserEntity user)
        {
            StringBuilder sql = new StringBuilder();
            // UPDATE文生成
            sql.Append(@"
                SELECT
                    Count(1) AS CNT
                FROM Mst_SystemSetupUser
                WHERE SETUP_USER_ACCOUNT = @SETUP_USER_ACCOUNT
                    AND SETUP_USER_PASSWORD = @SETUP_USER_PASSWORD COLLATE Japanese_CS_AS ");

            var result = base.SingleOrDefault<CountResult>(sql.ToString(), user);
            if(result != null)
            {
                return (result.CNT > 0);
            }
            return false;
        }

        /// <summary>
        /// Get current account's password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetCurrentPassword(ChangePasswordModel user)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
                    SETUP_USER_PASSWORD 
                FROM USERS
                WHERE SETUP_USER_ACCOUNT = @SETUP_USER_ACCOUNT");
            var result = base.SingleOrDefault<SystemSetupUserEntity>(sql.ToString(), new
            {
                SETUP_USER_ACCOUNT = user.SETUP_USER_ACCOUNT
            });
            return result.SETUP_USER_PASSWORD;
        }
    }
}

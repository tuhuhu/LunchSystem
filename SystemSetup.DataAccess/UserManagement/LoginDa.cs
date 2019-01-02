//---------------------------------------------------------------------------
// Version Id		: 001
// Designer			: GiangVT
// Programmer		: GiangVT
// Date				: 2014/09/17
// Comment			: Create new
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Text;

namespace SystemSetup.DataAccess
{
    #region using
    
    //using Framework;
    using SystemSetup.Models;
    using SystemSetup.Constants;        
    using Dapper;    
    using SystemSetup.UtilityServices;
    using SystemSetup.UtilityServices.SafePassword;
    using SystemSetup.UtilityServices.LogService;
    using System.Net;
    using System.Net.Sockets;
    
    #endregion using

    /// <summary>
    /// Data access class for Login screen
    /// </summary>
    public class LoginDa : BaseDa
    {
        /// <summary>
        /// Get Authority List
        /// </summary>
        /// <param name="CompanyCd"></param>
        /// <param name="userSegNo"></param>
        /// <returns></returns>
        public List<string> getAuthorityList(string CompanyCd, long userSegNo)
        {
            string sql = @"
                SELECT
                    AUTHORITY_KEY
                FROM
                    Mst_Authority
                WHERE
                    COMPANY_CD = @COMPANY_CD
                AND DEL_FLG = @DEL_FLG
                AND AUTHORITY_GROUP_CD = (
                    SELECT AUTHORITY_GROUP_CD
                    FROM Mst_ContractFirmUser
                    WHERE CONTRACT_USER_SEQ_CD = @CONTRACT_USER_SEQ_CD
                ) ";

            return base.Query<string>(sql,
                new
                {
                    COMPANY_CD = CompanyCd,
                    DEL_FLG = DeleteFlag.NON_DELETE,
                    CONTRACT_USER_SEQ_CD = userSegNo
                }).ToList();
        }

        /// <summary>
        ///	 Login User authentication
        /// </summary>
        /// <param name="loginInfoInput">Login Info Input Model</param>
        /// <returns>Return User Authentication Model</returns>
        public bool LoginUserAuthentication(ref LoginAuthenticationModel loginInfoInput)
        {
            // Declare database connection      
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT ");
            sql.Append(" A.SETUP_USER_SEQ_CD ");            
            sql.Append(" ,A.SETUP_USER_ACCOUNT ");
            sql.Append(" ,A.SETUP_USER_NAME ");
            sql.Append(" ,A.SETUP_USER_TYPE ");
            //sql.Append(" ,A.LOGIN_LOCK_FLG ");
            sql.Append(" FROM USERS A ");

            sql.Append(" WHERE A.SETUP_USER_ACCOUNT = @SETUP_USER_ACCOUNT");
            sql.Append(" AND A.SETUP_USER_PASSWORD = @SETUP_USER_PASSWORD");
            //sql.Append(" AND A.DISABLE_FLG = @DISABLE_FLG");
            sql.Append(" AND A.DEL_FLG = @DEL_FLG");                   

            var user = base.SingleOrDefaultLogIn<LoginAuthenticationModel>(sql.ToString(),
                new
                {                    
                    DISABLE_FLG = Constants.DisableFlag.Enable,
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                    SETUP_USER_ACCOUNT = loginInfoInput.SETUP_USER_ACCOUNT,
                    SETUP_USER_PASSWORD = SafePassword.GetSaltedPassword(loginInfoInput.SETUP_USER_PASSWORD)
                }, loginInfoInput.SETUP_USER_ACCOUNT,
                loginInfoInput.IpAddress, loginInfoInput.UserAgent, loginInfoInput.BrowserType, loginInfoInput.BrowserVersion);                      

            // Create variable ouput   
            if (user != null)
            {
                //var authorityList = getAuthorityList(user.COMPANY_CD, user.CONTRACT_USER_SEQ_CD);

                loginInfoInput = new LoginAuthenticationModel
                {
                    SETUP_USER_SEQ_CD = user.SETUP_USER_SEQ_CD,
                    //COMPANY_CD = user.COMPANY_CD,
                    SETUP_USER_ACCOUNT = user.SETUP_USER_ACCOUNT,
                    SETUP_USER_NAME = user.SETUP_USER_NAME,
                    SETUP_USER_TYPE = user.SETUP_USER_TYPE,
                    //LOGIN_LOCK_FLG = user.LOGIN_LOCK_FLG
                    //AUTHORITY_GROUP_CD = user.AUTHORITY_GROUP_CD,
                    //AUTHORITY_LIST = authorityList,
                    //ROUNDING_TYPE = user.ROUNDING_TYPE,
                    //CONTRACT_LEVEL_TYPE = user.CONTRACT_LEVEL_TYPE,
                    //DOC_COVER_LETTER_FORMAT_PATH = user.DOC_COVER_LETTER_FORMAT_PATH,
                    //FAX_COVER_LETTER_FORMAT_PATH = user.FAX_COVER_LETTER_FORMAT_PATH,
                    //ENVELOPE_DESTINATION_FORMAT_PATH = user.ENVELOPE_DESTINATION_FORMAT_PATH
                };
                return true;
            }
            return false;
        }

        /// <summary>
        /// Lock out an user
        /// </summary>
        /// <param name="contractUserSeqCd"></param>
        public void LockOutUser(long contractUserSeqCd)
        {
            // Declare database connection      
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" UPDATE Mst_SystemSetupUser");
            sql.Append(" SET LOGIN_LOCK_FLG = 1");
            sql.Append(" WHERE SETUP_USER_SEQ_CD = @SETUP_USER_SEQ_CD");
            base.Execute(sql.ToString(), new { SETUP_USER_SEQ_CD = contractUserSeqCd });
        }

        /// <summary>
        /// Get SETUP_USER_SEQ_CD by company code and user account
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public long GetUserId(string userAccount)
        {
            // Declare database connection      
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT SETUP_USER_SEQ_CD");
            sql.Append(" FROM Mst_SystemSetupUser");
            sql.Append(" WHERE SETUP_USER_ACCOUNT = @SETUP_USER_ACCOUNT");
            long userId = base.Query<long>(sql.ToString(), new { SETUP_USER_ACCOUNT = userAccount }).FirstOrDefault();
            return userId;
        }

        /// <summary>
        /// Get LOGIN_LOCK_FLG by company code and user account 
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public string GetLockOutFlag(string userAccount)
        {
            // Declare database connection      
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT LOGIN_LOCK_FLG");
            sql.Append(" FROM Mst_SystemSetupUser");
            sql.Append(" WHERE SETUP_USER_ACCOUNT = @SETUP_USER_ACCOUNT");
            string isLockOutFlag = base.Query<string>(sql.ToString(), new { SETUP_USER_ACCOUNT = userAccount }).FirstOrDefault();
            return isLockOutFlag;
        }
    }
}

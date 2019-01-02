//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/17
// Comment		: Create new
//------------------------------------------------------------------------

namespace SystemSetup.BusinessServices
{
    #region using

    using DataAccess;
    using Models;
    using System.Collections.Generic;

    #endregion using

    /// <summary>
    /// Business Service for Login screen
    /// </summary>
    public class LoginServices : BaseServices
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public List<string> getAuthorityList(string CompanyCd, long user_id)
        {
            // Declare new DataAccess object
            LoginDa dataAccess = new LoginDa();

            // Get authority List    
            var authorityList = dataAccess.getAuthorityList(CompanyCd, user_id);
            if (authorityList == null)
            {
                base.CmnEntityModel.ErrorMsgCd = Constants.MessageCd.W0015;
                //return null;
            }
            else
            {
                base.CmnEntityModel.ErrorMsgCd = string.Empty;
            }

            return authorityList;
        }

        /// <summary>
        /// Login User Authentication
        /// </summary>
        /// <param name="loginInfoInput">Login Info Input Model</param>
        /// <returns>Return User Authentication Model</returns>
        public bool LoginUserAuthentication(ref LoginAuthenticationModel loginInfoInput)
        {
            // Declare new DataAccess object
            LoginDa dataAccess = new LoginDa();

            // Assign result variable for get authentication user
            //LoginAuthenticationModel result = dataAccess.LoginUserAuthentication(loginInfoInput);
            bool result = dataAccess.LoginUserAuthentication(ref loginInfoInput);
            if (loginInfoInput == null)
            {
                base.CmnEntityModel.ErrorMsgCd = Constants.MessageCd.W0015;
                //return null;
            }
            else
            {
                base.CmnEntityModel.ErrorMsgCd = string.Empty;
            }
            return result;
        }

        /// <summary>
        /// Lock out an user
        /// </summary>
        /// <param name="contractUserSeqCd"></param>
        public void LockOutUser(long contractUserSeqCd)
        {
            // Declare new DataAccess object
            LoginDa dataAccess = new LoginDa();
            dataAccess.LockOutUser(contractUserSeqCd);
        }

        /// <summary>
        /// Get SETUP_USER_SEQ_CD by company code and user account
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public long GetUserId(string userAccount)
        {
            LoginDa dataAccess = new LoginDa();
            return dataAccess.GetUserId(userAccount);
        }

        /// <summary>
        /// Get LOGIN_LOCK_FLG by company code and user account
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public string GetLockOutFlag(string userAccount)
        {
            LoginDa dataAccess = new LoginDa();
            return dataAccess.GetLockOutFlag(userAccount);
        }
    }
}

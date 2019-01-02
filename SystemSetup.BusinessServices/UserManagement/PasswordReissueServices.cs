//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/10/15
// Comment		: Create new
//------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.BusinessServices
{
    #region using

    using DataAccess;
    using Models;
    using System.Collections.Generic;

    #endregion using

    /// <summary>
    /// Business Service for Password Reissue screen
    /// </summary>
    public class PasswordReissueServices : BaseServices
    {
        /// <summary>
        /// Get email to reissue password
        /// </summary>
        /// <param name="reissueinfo">Password Reissue Model</param>
        /// <returns>Return PasswordReissue Model</returns>
        public ChangePasswordModel GetEmailReissue(ChangePasswordModel reissueinfo)
        {
            // Declaration and init a list of select list item
            ChangePasswordModel results = new ChangePasswordModel();

            // Declare new DataAccess object
            PasswordReissueDa dataAccess = new PasswordReissueDa();
            //results = dataAccess.GetUserReissueByMail(reissueinfo.MAIL_ADDRESS);

            if (results == null)
            {
                base.CmnEntityModel.ErrorMsgCd = Constants.MessageCd.W0015;
                return null;
            }
            base.CmnEntityModel.ErrorMsgCd = string.Empty;

            return results;
        }

        /// <summary>
        /// Update Password mail reissue
        /// </summary>
        /// <param name="loginInfoInput">Password Reissue Model</param>
        /// <returns>Return Password Reissue Model</returns>
        public int UpdatePassword(SystemSetupUserEntity user)
        {
            // Declaration and init return value
            int result = 0;

            // Declare new DataAccess object
            PasswordReissueDa dataAccess = new PasswordReissueDa();
            result = dataAccess.UpdatePassword(user);

            if (result == -1)
            {
                base.CmnEntityModel.ErrorMsgCd = Constants.MessageCd.W0015;
                return result;
            }
            base.CmnEntityModel.ErrorMsgCd = string.Empty;

            return result;
        }

        /// <summary>
        /// Update Password mail reissue
        /// </summary>
        /// <param name="loginInfoInput">Password Reissue Model</param>
        /// <returns>Return Password Reissue Model</returns>
        public bool IsSamePassword(ContractFirmUserEntity user)
        {
            // Declare new DataAccess object
            PasswordReissueDa dataAccess = new PasswordReissueDa();
            return dataAccess.IsSamePassword(user);
        }

        /// <summary>
        /// Get current account's password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetCurrentPassword(ChangePasswordModel user)
        {
            PasswordReissueDa dataAccess = new PasswordReissueDa();
            return dataAccess.GetCurrentPassword(user);
        }
    }
}

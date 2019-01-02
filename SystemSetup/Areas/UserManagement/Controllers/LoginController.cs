using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SystemSetup.Constants.Resources;
using SystemSetup.UtilityServices.LoginLogService;
using iEnterAsia.iseiQ.Models;

namespace SystemSetup.Areas.UserManagement.Controllers
{
    using BusinessServices;
    using SystemSetup.Models;
    using SystemSetup.Constants;
    using SystemSetup.ReportServices;
    using SystemSetup.Controllers;
    using SystemSetup.UtilityServices;

    public class LoginController : BaseController
    {        
        //
        // GET: /UserManagement/Login/
        /// <summary>
        /// LogIn 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(bool timeout = false)
        {
            if (base.GetCache<CmnEntityModel>("CmnEntityModel") != null 
                && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                base.RemoveCache("CmnEntityModel");
                Session.Clear();
                return this.RedirectToAction("Login", "Login", new { area = "UserManagement" }); 
            }

            ViewBag.TimeOut = timeout;
            return View();
        }

        /// <summary>
        /// LogOut
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logout()
        {            
            TempData.Clear();
            FormsAuthentication.SignOut();
            base.RemoveCache("CmnEntityModel");
            Session.Clear();

            return this.RedirectToAction("Login", "Login", new { area = "UserManagement" });
        }

        /// <summary>
        /// Check Login Input
        /// </summary>
        /// <param name="loginInfoInput"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginAuthenticationModel loginInfoInput, string btnLogin, string btnChangePass)
        {
            if (ModelState.IsValid)
            {
                if (btnLogin != null)
                {
                    using (LoginServices service = new LoginServices())
                    {
                        string reg = Constant.REGEX_PASSWORD;

                        string IpAddress = GetIpAddress();
                        string UserAgent = Request.UserAgent;
                        string BrowserType = Request.Browser.Type;
                        string BrowserVersion = Request.Browser.Version;
                        string InputedCompanyCd = loginInfoInput.COMPANY_CD;
                        string InputedUserAccount = loginInfoInput.SETUP_USER_ACCOUNT;


                        //Check min length user acount and password
                        if (loginInfoInput.SETUP_USER_ACCOUNT.Length < Constant.MIN_INPUT_ACCOUNT 
                            || loginInfoInput.SETUP_USER_ACCOUNT.Length > Constant.MAX_INPUT_ACCOUNT 
                            || loginInfoInput.SETUP_USER_PASSWORD.Length < Constant.MIN_INPUT_PASS 
                            || loginInfoInput.SETUP_USER_PASSWORD.Length > Constant.MAX_INPUT_PASS
                            || !Regex.IsMatch(loginInfoInput.SETUP_USER_ACCOUNT, reg) 
                            || !Regex.IsMatch(loginInfoInput.SETUP_USER_PASSWORD, reg))
                        {
                            LoginLogService.LoginFailLog("NG: min length user acount and password", IpAddress, UserAgent, BrowserType, BrowserVersion, InputedCompanyCd, InputedUserAccount);
                            ModelState.AddModelError("", string.Format(Constants.Resources.Messages.AccountFailed));
                            return this.View();
                        }

                        loginInfoInput.IpAddress = IpAddress;
                        loginInfoInput.UserAgent = UserAgent;
                        loginInfoInput.BrowserType = BrowserType;
                        loginInfoInput.BrowserVersion = BrowserVersion;

                        // Check user account is locked out or not
                        bool isAuthenticated = service.LoginUserAuthentication(ref loginInfoInput);
                        if (!isAuthenticated)
                        {
                            string message = Constants.Resources.Messages.AccountFailed;
                            LoginLogService.LoginFailLog(message, IpAddress, UserAgent, BrowserType, BrowserVersion,
                                InputedCompanyCd, InputedUserAccount);
                            ModelState.AddModelError("", message);
                            return this.View();
                        }

                        // Update Command Entity Model in session
                        UpdateCmnEntityModel(loginInfoInput);
                        SetScrollTop(0);

                        loginInfoInput.IpAddress = IpAddress;
                        loginInfoInput.UserAgent = UserAgent;
                        loginInfoInput.BrowserType = BrowserType;
                        loginInfoInput.BrowserVersion = BrowserVersion;

                        // Save authentication
                        FormsAuthentication.SetAuthCookie(loginInfoInput.SETUP_USER_ACCOUNT, false);

                        LoginLogService.LoginSuccessLog("ログイン成功", loginInfoInput);
                        return this.RedirectToAction("Index", "Information", new { area = "Information" });
                    }
                }
                if (btnChangePass != null)
                {
                    using (LoginServices service = new LoginServices())
                    {
                        string IpAddress = GetIpAddress();
                        string UserAgent = Request.UserAgent;
                        string BrowserType = Request.Browser.Type;
                        string BrowserVersion = Request.Browser.Version;
                        string InputedCompanyCd = loginInfoInput.COMPANY_CD;
                        string InputedUserAccount = loginInfoInput.SETUP_USER_ACCOUNT;

                        loginInfoInput.IpAddress = IpAddress;
                        loginInfoInput.UserAgent = UserAgent;
                        loginInfoInput.BrowserType = BrowserType;
                        loginInfoInput.BrowserVersion = BrowserVersion;

                        // Check user account is locked out or not
                        bool isAuthenticated = service.LoginUserAuthentication(ref loginInfoInput);
                        if (!isAuthenticated)
                        {
                            string message = Constants.Resources.Messages.AccountFailed;
                            LoginLogService.LoginFailLog(message, IpAddress, UserAgent, BrowserType, BrowserVersion,
                                InputedCompanyCd, InputedUserAccount);
                            ModelState.AddModelError("", message);
                            return this.View();
                        }

                        var changePassModel = new ChangePasswordModel();
                        //changePassModel.COMPANY_CD = loginInfoInput.COMPANY_CD;
                        changePassModel.SETUP_USER_ACCOUNT = loginInfoInput.SETUP_USER_ACCOUNT;
                        TempData["UserInfo"] = changePassModel;

                        return this.RedirectToAction("ChangePassword", "ChangePassword", new { area = "UserManagement" });
                    }
                }
            }
            return this.View();
        }

        /// <summary>
        /// Update Command Entity Model in session
        /// </summary>
        /// <param name="loginInfoInput"></param>
        private void UpdateCmnEntityModel(LoginAuthenticationModel loginInfoInput)
        {
            //CmnEntityModel.CompanyCd = loginInfoInput.COMPANY_CD;
            CmnEntityModel.UserID = loginInfoInput.SETUP_USER_ACCOUNT;
            CmnEntityModel.UserName = loginInfoInput.SETUP_USER_NAME;
            CmnEntityModel.UserSegNo = loginInfoInput.SETUP_USER_SEQ_CD;
            CmnEntityModel.UserType = loginInfoInput.SETUP_USER_TYPE;
            Session["AdminType"] = loginInfoInput.SETUP_USER_TYPE; 
            //CmnEntityModel.AuthorityGroupCd = loginInfoInput.AUTHORITY_GROUP_CD;
            //CmnEntityModel.AuthorityList = loginInfoInput.AUTHORITY_LIST;
            CmnEntityModel.MenuStatus = "sidebar-collapse";
            //CmnEntityModel.RoundingType = loginInfoInput.ROUNDING_TYPE;
            //CmnEntityModel.isDisplaySubContractType = ContractLevelType.Level3.Equals(loginInfoInput.CONTRACT_LEVEL_TYPE);
            //CmnEntityModel.formatPathCoverLetterByDocument = loginInfoInput.DOC_COVER_LETTER_FORMAT_PATH;
            //CmnEntityModel.formatPathCoverLetterByFax = loginInfoInput.FAX_COVER_LETTER_FORMAT_PATH;
            //CmnEntityModel.formatPathCoverLetterByEnvelopeDestination = loginInfoInput.ENVELOPE_DESTINATION_FORMAT_PATH;
        }

        private string GetIpAddress()
        {
            string IpAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(IpAddress))
            {
                IpAddress = Request.UserHostAddress;
            }
            return IpAddress ?? string.Empty;
        }

        #region Lock out account
        /// <summary>
        /// Check account status to lock out
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool CheckAccountToLockOut(string companyCode, long userId)
        {
            // Get list of invalid user from session
            var listInvalidUser = Session[Constant.SESSION_INVALID_LOGIN_USER] as List<InvalidLoginUser>;

            if (listInvalidUser == null)
            {
                listInvalidUser = new List<InvalidLoginUser>();
            }

            // get the limit of input password times
            int limitInputTimes = int.Parse(ConfigurationManager.AppSettings[ConfigurationKeys.LIMITED_INPUT_PASSWORD_TIMES]);
            // Get info of user Account if is an invalid User or wrong password 
            InvalidLoginUser invalidUser = listInvalidUser.FirstOrDefault(i => i.UserId == userId);
            if (invalidUser != null)
            {
                invalidUser.InvalidCount++;

                if (invalidUser.InvalidCount >= limitInputTimes)
                {
                    LoginServices loginService = new LoginServices();
                    // reach the limit input times, lock password
                    loginService.LockOutUser(userId);
                    listInvalidUser.Remove(invalidUser);
                    return true;
                }
            }
            else
            {
                listInvalidUser.Add(new InvalidLoginUser(userId));
            }

            // save list of invalid user to session
            Session[Constant.SESSION_INVALID_LOGIN_USER] = listInvalidUser;
            return false;
        }

        #endregion
    }
}

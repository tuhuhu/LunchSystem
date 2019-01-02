using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;

namespace SystemSetup.Areas.UserManagement.Controllers
{
    using BusinessServices;
    using Models;
    using SystemSetup.Constants;
    using SystemSetup.UtilityServices;
    using SystemSetup.UtilityServices.SafePassword;
    using SystemSetup.Controllers;

    public class ChangePasswordController : BaseController
    {        
        //
        // GET: /UserManagement/ChangePassword/
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            ChangePasswordModel loginModel = (ChangePasswordModel)TempData["UserInfo"];
            TempData.Keep("UserInfo");

            if (loginModel != null)
            {
                return View("ChangePassword", loginModel);
            }
            else
            {
                return this.RedirectToAction("Logout", "Login", new { area = "UserManagement" });
            }
        }

         /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                using (PasswordReissueServices service = new PasswordReissueServices())
                {
                    // get current user's password
                    var currentpass = service.GetCurrentPassword(model);
                    var user = new SystemSetupUserEntity();
                    user.SETUP_USER_ACCOUNT = model.SETUP_USER_ACCOUNT;
                    user.SETUP_USER_PASSWORD = SafePassword.GetSaltedPassword(model.NEW_PASSWORD);
                    if (currentpass.Equals(user.SETUP_USER_PASSWORD))
                    {
                        ModelState.AddModelError("", "New password is same old password ! ");
                        return View(model);
                    }
                    user.PASSWORD_LAST_UPDATE_DATE = Utility.GetCurrentDateTime();
                    var res = service.UpdatePassword(user);
                    if (res > 0)
                    {
                        ViewBag.ChangePasswordSuccess = true;
                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError("", Constants.Resources.Messages.PasswordChangeFailed);
                    }
                }
            }
            return View(model);
        }
    }
}

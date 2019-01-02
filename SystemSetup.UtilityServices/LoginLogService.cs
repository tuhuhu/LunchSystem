//------------------------------------------------------------------------
// Version		: 001
// Designer		: h-yamauchi
// Programmer	: h-yamauchi
// Date			: 2015/12/02
// Comment		: Create new
//------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemSetup.Constants;
using System.Diagnostics;
using System.Web;
using SystemSetup.Models;

namespace SystemSetup.UtilityServices.LoginLogService
{
    using System.IO;
    using System.Configuration;
    public class LoginLogService
    {
        private static log4net.ILog logger;

        /// <summary>
        /// Initial Log4net setting
        /// </summary>
        public static void InitialLog(string companyCd)
        {
            log4net.GlobalContext.Properties["LogFileName"] = string.Format("{0}_{1}", companyCd, ConfigurationManager.AppSettings[ConfigurationKeys.LOG_FILE_LOGIN]);
            log4net.Config.XmlConfigurator.Configure(new FileInfo(HttpContext.Current.Server.MapPath("~/Configs/log.config")));
            logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        public static void LoginSuccessLog(string message, LoginAuthenticationModel loginInfoInput)
        {
            StringBuilder strlog = new StringBuilder();

            strlog.AppendLine("\n ");
            strlog.AppendLine(message);
            strlog.AppendLine("\n 時刻: " + Utility.GetCurrentDateTime().ToString("yyyy/MM/dd HH:mm:ss.fff") + " \n ");
            strlog.AppendLine("\n " + "接続元IPアドレス: " + loginInfoInput.IpAddress);
            strlog.AppendLine(string.Format(" UserAgent: {0}", loginInfoInput.UserAgent));
            strlog.AppendLine(string.Format(" Browser: {0} {1}", loginInfoInput.BrowserType, loginInfoInput.BrowserVersion));
            strlog.AppendLine(" 企業コード: " + loginInfoInput.COMPANY_CD);
            strlog.AppendLine(" ユーザーアカウント: " + string.Format("{0} {1}", loginInfoInput.CONTRACT_USER_ACCOUNT, loginInfoInput.CONTRACT_USER_NAME));
            strlog.AppendLine("\n ");

            InitialLog(loginInfoInput.COMPANY_CD);
            Debug(logger, strlog.ToString());
        }

        public static void LoginFailLog(string message, string IpAddress, string UserAgent, string BrowserType, string BrowserVersion, string InputedCompanyCd, string InputedUserAccount)
        {
            StringBuilder strlog = new StringBuilder();

            strlog.AppendLine("\n ");
            strlog.AppendLine(message);
            strlog.AppendLine("\n 時刻: " + Utility.GetCurrentDateTime().ToString("yyyy/MM/dd HH:mm:ss.fff") + " \n ");
            strlog.AppendLine("\n " + "接続元IPアドレス: " + IpAddress);
            strlog.AppendLine(string.Format(" UserAgent: {0}", UserAgent));
            strlog.AppendLine(string.Format(" Browser: {0} {1}", BrowserType, BrowserVersion));
            strlog.AppendLine(" 入力された企業コード: " + InputedCompanyCd);
            strlog.AppendLine(" 入力されたユーザーアカウント: " + InputedUserAccount);
            strlog.AppendLine("\n ");

            InitialLog(InputedCompanyCd);
            Debug(logger, strlog.ToString());
        }

        /// <summary>
        /// log DEBUG
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sqllog"></param>
        public static void Debug(log4net.ILog logger, string sqllog)
        {
            // Use logger to log into file
            logger.Debug(sqllog);
        }

        public static void Fatal(log4net.ILog logger, string errorMess, Exception ex = null)
        {
            // Use logger to log into file
            if (ex == null)
            {
                logger.Fatal(errorMess);
            }
            else
            {
                logger.Fatal(errorMess, ex);
            }
        }
    }
}

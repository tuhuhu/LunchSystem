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

namespace SystemSetup.UtilityServices.ActionLogService
{
    using System.IO;
    using System.Configuration;
    public class ActionLogService
    {
        private static log4net.ILog logger;

        public static void ActionLog(string actionKey, CmnEntityModel cmnEntityModel, string userAgent, string browserType, string browserVersion, string requestedUrl)
        {
            StringBuilder strlog = new StringBuilder();

            strlog.AppendLine("\n ");
            strlog.AppendLine(string.Format(" userAgent: {0}", userAgent));
            strlog.AppendLine(string.Format(" Browser: {0} {1}", browserType, browserVersion));
            strlog.AppendLine(" 企業コード: " + cmnEntityModel.CompanyCd);
            strlog.AppendLine(" ユーザーアカウント: " + string.Format("{0} {1} {2}", cmnEntityModel.UserSegNo, cmnEntityModel.UserID, cmnEntityModel.UserName));
            strlog.AppendLine("\n 時刻: " + Utility.GetCurrentDateTime().ToString("yyyy/MM/dd HH:mm:ss.fff") + " \n ");
            strlog.AppendLine(" 実行アクション: " + actionKey + " \n\n ");

            log4net.GlobalContext.Properties["LogFileName"] = string.Format("{0}_{1}_{2}", cmnEntityModel.CompanyCd, cmnEntityModel.UserSegNo, 
                ConfigurationManager.AppSettings[ConfigurationKeys.LOG_FILE_ACTION]);

            log4net.Config.XmlConfigurator.Configure(new FileInfo(HttpContext.Current.Server.MapPath("~/Configs/log.config")));
            logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            Debug(logger, strlog.ToString());

            CallActionLogApi(actionKey, cmnEntityModel, userAgent, browserType, browserVersion, requestedUrl);
        }

        private static void CallActionLogApi(string actionKey, CmnEntityModel cmnEntityModel, string userAgent, string browserType, string browserVersion, string requestedUrl)
        {
            string url = string.Empty;
            try
            {
                url = string.Format(
                    @"{0}?actionKey={1}&requestedUrl={2}&browserType={3}&browserVersion={4}&companyCd={5}&userSegNo={6}&userId={7}&userName={8}&time={9}"
                    , @"https://720teo2s3e.execute-api.ap-northeast-1.amazonaws.com/prod/iSeiQActionLogApi"
                    , HttpUtility.UrlPathEncode(actionKey)
                    , HttpUtility.UrlPathEncode(requestedUrl)
                    , HttpUtility.UrlPathEncode(browserType)
                    , HttpUtility.UrlPathEncode(browserVersion)
                    , HttpUtility.UrlPathEncode(cmnEntityModel.CompanyCd)
                    , HttpUtility.UrlPathEncode(cmnEntityModel.UserSegNo.ToString())
                    , HttpUtility.UrlPathEncode(cmnEntityModel.UserID)
                    , HttpUtility.UrlPathEncode(cmnEntityModel.UserName)
                    , HttpUtility.UrlPathEncode(Utility.GetCurrentDateTime().ToString("yyyy/MM/dd HH:mm:ss.fff")));

                url = new string(url.Take(2000).ToArray());

                var request = System.Net.WebRequest.Create(url) as System.Net.HttpWebRequest;
                if (request != null)
                {
                    using (var response = request.GetResponse())
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
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

//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/17
// Comment		: Create new
//------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemSetup.Constants;
//using SystemSetup.DataAccess;
using SystemSetup.Models;
using System.Diagnostics;
using SystemSetup.UtilityServices;
using System.Web;

namespace SystemSetup.UtilityServices.LogService
{
    using Dapper;
    using System.IO;
    using System.Configuration;
    public class LogService
    {
		private static string errorLogFolderPath = string.Empty;
		private static string errorFilePath = string.Empty;
        //public static log4net.ILog logger = log4net.LogManager.GetLogger("LoggerAzureBlob");        
        private static log4net.ILog logger;
        private static Stopwatch watch = new Stopwatch();
        private static DateTime startTime;
        private static string companyCd;
        private static string userAccount;
        private static string ipAddress;
        private static string userAgent;
        private static string browserType;
        private static string browserVersion;
        private static DateTime endTime;

        /// <summary>
        /// Initial Log4net setting
        /// </summary>
        public static void InitialLog()
        {
            log4net.GlobalContext.Properties["LogFileName"] = companyCd + "_" + ConfigurationManager.AppSettings[ConfigurationKeys.LOG_FILE_SQL];
            log4net.Config.XmlConfigurator.Configure(new FileInfo(HttpContext.Current.Server.MapPath("~/Configs/log.config")));
            logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <summary>
        /// Save sql query with params
        /// </summary>
        /// <param name="sqlQuerry"></param>
        /// <param name="parameters"></param>
        /// <param name="startindex"></param>
        /// <param name="endindex"></param>
        /// <param name="totalrow"></param>
        private static void SaveLog(string sqlQuerry, string parameters, int startindex = 0, int endindex = 0, int totalrow =0)
        {
            StringBuilder strlog = new StringBuilder();

            strlog.AppendLine("\n " + "接続元IPアドレス: " + ipAddress);
            strlog.AppendLine(string.Format(" UserAgent: {0}", userAgent));
            strlog.AppendLine(string.Format(" Browser: {0} {1}", browserType, browserVersion));
            strlog.AppendLine(" 企業コード: " + companyCd);
            strlog.AppendLine(" ユーザーアカウント: " + userAccount);
            strlog.AppendLine("\n クエリー開始時刻: " + startTime.ToString("yyyy/MM/dd HH:mm:ss.fff") + " \n ");
            strlog.AppendLine(sqlQuerry + " \n ");
            strlog.AppendLine(" "+ parameters);
            strlog.AppendLine(" 経過時間: " + watch.ElapsedMilliseconds + " ms " + " \n クエリー終了時刻: " + endTime.ToString("yyyy/MM/dd HH:mm:ss.fff"));
            if (totalrow > 0)
            {
                strlog.AppendLine(string.Format(" 全{0}件中{1}件～{2}件を表示", totalrow, startindex,endindex));
            }
            InitialLog();
            Debug(logger, strlog.ToString());            
        }

        /// <summary>
        /// Start log timer
        /// </summary>
        /// <param name="exeTime"></param>
        public static void StartLog(DateTime exeTime, string COMPANY_CD, string CONTRACT_USER_ACCOUNT, string IpAddress, string UserAgent, string BrowserType, string BrowserVersion)
        {
            watch.Start();
            startTime = exeTime;
            companyCd = COMPANY_CD;
            userAccount = CONTRACT_USER_ACCOUNT;
            ipAddress = IpAddress;
            userAgent = UserAgent;
            browserType = BrowserType;
            browserVersion = BrowserVersion;
        }

        /// <summary>
        /// Save log and reset timer
        /// </summary>
        /// <param name="sql"></param>
        public static void EndLog(string sql)
        {
            // Save log process sql
            watch.Stop();
            endTime = Utility.GetCurrentDateTime();
            SaveLog(sql, Dapper.SqlMapper.parameterInfo);
            watch.Reset();
        }

        /// <summary>
        /// log DEBUG
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sqllog"></param>
        public static void Debug(log4net.ILog logger,string sqllog)
        {
            // Use logger to log into file
            logger.Debug(sqllog);            
        }

        public static void Fatal(log4net.ILog logger, string errorMess, Exception ex = null)
        {
            // Use logger to log into file
            if(ex == null)
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

//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/03
// Comment		: Create new
//------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Xml;
using System.Linq;
using System.Text;
using SystemSetup.Constants;
//using SystemSetup.DataAccess.Framework;
using SystemSetup.Models;
using System.Data.Common;
using SystemSetup.UtilityServices;
using System.Web.Routing;
using SystemSetup.UtilityServices.LogService;

namespace SystemSetup.DataAccess
{
    using Dapper;
    using DapperExtensions;
    public class BaseDa
    {
        private CmnEntityModel cmnEntityModel = null;
        
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDbConnection conn;
        private IDbCommand cmd;
        private static string companyCd;
        private static string userAccount;
        private static string ipAddress;
        private static string userAgent;
        private static string browserType;
        private static string browserVersion;

        public CmnEntityModel CmnEntityModel
        {
            get
            {
                if ( cmnEntityModel == null )
                {
                    if ( HttpContext.Current.Session[ "CmnEntityModel" ] == null)
                    {
                        HttpContext.Current.Session[ "CmnEntityModel" ] = new CmnEntityModel();
                    }
                    cmnEntityModel = (CmnEntityModel)HttpContext.Current.Session[ "CmnEntityModel" ];
                }
                return cmnEntityModel;
            }
        }

        /// <summary>
        /// Init SqlClient Connection
        /// </summary>
        private void InitConnection()
        {
            var factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            this.conn = factory.CreateConnection();
            this.conn.ConnectionString = ConnectionString;
            this.conn.Open();
        }

        /// <summary>
        /// create IDBDataParameter
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private IDbDataParameter createParam(string name, object value)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            return param;
        }


        public int DbAdd(string sql, BaseEntity entity)
        {
            return this.ExecuteNonQuery(true, sql, entity);
        }

        public int DbUpdate(string sql, object entity, object condition)
        {
            return this.ExecuteNonQuery(false, sql, entity, condition);
        }


        /// <summary>
        /// Executes an SQL statement against the Connection object of a .NET Framework
        /// data provider, and returns the number of rows affected.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private int ExecuteNonQuery(bool isInsert, string sql, object entity, object condition = null)
        {
            int result = 0;

            DateTime startTime = Utility.GetCurrentDateTime();
            LogService.StartLog(startTime, companyCd, userAccount, ipAddress, userAgent, browserType, browserVersion);

            this.InitConnection();
            this.cmd = conn.CreateCommand();
            this.cmd.CommandText = sql;

            // Add sql param from entity
            foreach (var prop in entity.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                var key = prop.Name;
                bool blUseParam = true;
                var value = prop.GetValue(entity, null);
                if (key == "INS_DATE" && isInsert)
                    value = startTime;
                if (key == "UPD_DATE")
                    value = startTime;
                if (key == "UPD_USER_ID")
                    value = CmnEntityModel.UserSegNo;
                if (key == "INS_DATE" && !isInsert)
                    blUseParam = false;
                if (key == "INS_USER_ID" && !isInsert)
                    blUseParam = false;
                if (key == "TAX_RATE" && (decimal)value > 1)
                    value = (decimal)value / 100;

                if (blUseParam)
                    this.cmd.Parameters.Add(createParam("@" + key, value));
            }

            // Add sql condition params
            if (condition != null)
            {
                foreach (var prop in condition.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    this.cmd.Parameters.Add(createParam("@ORG_" + prop.Name, prop.GetValue(condition, null)));
                }
            }

            // Execute command
            try
            {
                result = this.cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.conn.Dispose();
                this.conn = null;
                this.cmd.Dispose();
                this.cmd = null;
                LogService.EndLog(sql);
            }
            return result;
        }

        public int DbAddByOutside(string sql, BaseEntity entity)
        {
            return this.ExecuteNonQueryByOutside(true, sql, entity);
        }

        public int DbUpdateByOutside(string sql, object entity, object condition)
        {
            return this.ExecuteNonQueryByOutside(false, sql, entity, condition);
        }


        /// <summary>
        /// Executes an SQL statement against the Connection object of a .NET Framework
        /// data provider, and returns the number of rows affected.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private int ExecuteNonQueryByOutside(bool isInsert, string sql, object entity, object condition = null)
        {
            int result = 0;

            DateTime startTime = Utility.GetCurrentDateTime();
            LogService.StartLog(startTime, companyCd, userAccount, ipAddress, userAgent, browserType, browserVersion);

            this.InitConnection();
            this.cmd = conn.CreateCommand();
            this.cmd.CommandText = sql;

            // Add sql param from entity
            foreach (var prop in entity.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                var key = prop.Name;
                bool blUseParam = true;
                var value = prop.GetValue(entity, null);
                if (key == "INS_DATE" && isInsert)
                    value = startTime;
                if (key == "UPD_DATE")
                    value = startTime;
                if (key == "UPD_USER_ID")
                    value = 0;
                if (key == "INS_DATE" && !isInsert)
                    blUseParam = false;
                if (key == "INS_USER_ID" && !isInsert)
                    blUseParam = true;
                if (key == "TAX_RATE" && (decimal)value > 1)
                    value = (decimal)value / 100;

                if (blUseParam)
                    this.cmd.Parameters.Add(createParam("@" + key, value));
            }

            // Add sql condition params
            if (condition != null)
            {
                foreach (var prop in condition.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    this.cmd.Parameters.Add(createParam("@ORG_" + prop.Name, prop.GetValue(condition, null)));
                }
            }

            // Execute command
            try
            {
                result = this.cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.conn.Dispose();
                this.conn = null;
                this.cmd.Dispose();
                this.cmd = null;
                LogService.EndLog(sql);
            }
            return result;
        }

        /// <summary>
        /// Execute parameterized SQL that selects a single value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, object param = null)
        {
            dynamic result = null;
            this.InitConnection();

            try
            {
                DateTime startTime = Utility.GetCurrentDateTime();
                LogService.StartLog(startTime, companyCd, userAccount, ipAddress, userAgent, browserType, browserVersion);
                result = this.conn.ExecuteScalar<T>(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.conn.Dispose();
                this.conn = null;
                LogService.EndLog(sql);
            }
            return result;
        }

        /// <summary>
        /// Executes a query, returning the data typed as per T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, object param = null)
        {
            dynamic result = null;
            this.InitConnection();

            try
            {
                DateTime startTime = Utility.GetCurrentDateTime();
                LogService.StartLog(startTime, companyCd, userAccount, ipAddress, userAgent, browserType, browserVersion);
                result = conn.Query<T>(sql, param);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.conn.Dispose();
                this.conn = null;
                LogService.EndLog(sql);
            }
            return result;
        }

        /// <summary>
        /// Executes a query
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Execute(string sql, object param = null)
        {
            int result = 0;
            this.InitConnection();

            try
            {
                DateTime startTime = Utility.GetCurrentDateTime();
                LogService.StartLog(startTime, companyCd, userAccount, ipAddress, userAgent, browserType, browserVersion);
                IDbTransaction transaction = conn.BeginTransaction();
                result = conn.Execute(sql, param,transaction);
                transaction.Commit();                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                
            }
            finally
            {
                this.conn.Dispose();
                this.conn = null;
                LogService.EndLog(sql);
            }
            return result;
        }

        /// <summary>
        /// Use only for login
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T SingleOrDefaultLogIn<T>(string sql, object param = null, string COMPANY_CD = null, string CONTRACT_USER_ACCOUNT = null,
            string IpAddress = null, string UserAgent = null, string BrowserType = null, string BrowserVersion = null)
        {
            dynamic result = null;
            this.InitConnection();

            try
            {
                DateTime startTime = Utility.GetCurrentDateTime();
                companyCd = COMPANY_CD;
                userAccount = CONTRACT_USER_ACCOUNT;
                ipAddress = IpAddress;
                userAgent = UserAgent;
                browserType = BrowserType;
                browserVersion = BrowserVersion;
                LogService.StartLog(startTime, companyCd, userAccount, ipAddress, userAgent, browserType, browserVersion);                
                result = conn.Query<T>(sql, param).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.conn.Dispose();
                this.conn = null;
                LogService.EndLog(sql);
            }
            return result;
        }

        /// <summary>
        /// Executes a query, returning the data typed as per T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T SingleOrDefault<T>(string sql, object param = null)
        {
            dynamic result = null;
            this.InitConnection();

            try
            {
                DateTime startTime = Utility.GetCurrentDateTime();
                LogService.StartLog(startTime, companyCd, userAccount, ipAddress, userAgent, browserType, browserVersion);
                result = conn.Query<T>(sql, param).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.conn.Dispose();
                this.conn = null;
                LogService.EndLog(sql);
            }
            return result;
        }
    }
}

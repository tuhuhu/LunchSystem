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
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemSetup.Models
{
    /// <summary>
    /// Login User Authentication Model
    /// </summary>
    public class LoginAuthenticationModel
    {
        /// <summary>
        /// contract user seg code
        /// </summary>
        public long SETUP_USER_SEQ_CD { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SETUP_USER_TYPE { get; set; }

        /// <summary>
        /// Get or set user account
        /// </summary>
        public string SETUP_USER_ACCOUNT { get; set; }

        /// <summary>
        /// Get or set Password
        /// </summary>
        public string SETUP_USER_PASSWORD { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string SETUP_USER_NAME { get; set; }

        /// <summary>
        /// contract user seg code
        /// </summary>
        public long CONTRACT_USER_SEQ_CD { get; set; }

        /// <summary>
        /// Get or set CompanyCode
        /// </summary>
        public string COMPANY_CD { get; set; }

        /// <summary>
        /// Company Name
        /// </summary>
        public string COMPANY_NAME { get; set; }

        /// <summary>
        /// Get or set user account
        /// </summary>
        public string CONTRACT_USER_ACCOUNT { get; set; }

        /// <summary>
        /// Get or set Password
        /// </summary>
        public string CONTRACT_USER_PASSWORD { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string CONTRACT_USER_NAME { get; set; }

        /// <summary>
        /// rounding type
        /// </summary>
        public string ROUNDING_TYPE { get; set; }

        /// <summary>
        /// contract level type
        /// </summary>
        public string CONTRACT_LEVEL_TYPE { get; set; }

        /// <summary>
        /// Staff id
        /// </summary>
        public long STAFF_ID { get; set; }

        /// <summary>
        /// authority group id
        /// </summary>
        public int AUTHORITY_GROUP_CD { get; set; }

        /// <summary>
        /// authority list
        /// </summary>
        public List<string> AUTHORITY_LIST { get; set; }

        /// <summary>
        /// Last time update
        /// </summary>
        public DateTime PASSWORD_LAST_UPDATE_DATE { get; set; }

        /// <summary>
        /// effective account
        /// </summary>
        public string DISABLE_FLG { get; set; }

        /// <summary>
        /// delete flag
        /// </summary>
        public string DEL_FLG { get; set; }

        /// <summary>
        /// insert time
        /// </summary>
        public DateTime INS_DATE { get; set; }

        /// <summary>
        /// insert user id
        /// </summary>
        public int INS_USER_ID { get; set; }

        /// <summary>
        /// update time
        /// </summary>
        public DateTime UPD_DATE { get; set; }

        /// <summary>
        /// update user id
        /// </summary>
        public int UPD_USER_ID { get; set; }

        /// <summary>
        /// update program id
        /// </summary>
        public string UPD_PROG_ID { get; set; }

        /// <summary>
        /// IP address
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// UserAgent
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Request.Browser.Type
        /// </summary>
        public string BrowserType { get; set; }

        /// <summary>
        /// Request.Browser.Version
        /// </summary>
        public string BrowserVersion { get; set; }

        public string DOC_COVER_LETTER_FORMAT_PATH { get; set; }

        public string FAX_COVER_LETTER_FORMAT_PATH { get; set; }

        public string ENVELOPE_DESTINATION_FORMAT_PATH { get; set; }

        public string LOGIN_LOCK_FLG { get; set; }

        public LoginAuthenticationModel()
        {
            SETUP_USER_TYPE = 1;
            
        }
    }
}

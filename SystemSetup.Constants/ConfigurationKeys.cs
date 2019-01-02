//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/10/14
// Comment		: Create new
//------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemSetup.Constants
{
    /// <summary>
    /// A description of the outline of ConfigurationKeys
    /// </summary>
    public static class ConfigurationKeys
    {
        public static readonly string SMTP_SERVER = "smtpServer";

        public static readonly string ENABLE_SSL = "EnableSsl";

        public static readonly string SMTP_MAIL_TITLE_PREFIX = "smtpMailTitlePrefix";

        public static readonly string SMTP_PORT = "smtpPort";

        public static readonly string SMTP_USER = "smtpUser";

        public static readonly string SMTP_PASS = "smtpPass";

        public static readonly string SMTP_SUPPORT = "smtpSupport";

        public static readonly string SAVE_BASE_FILE_PATH = "saveBaseFilePath";

        // USER
        public static readonly string TEMP_USER_PATH = "temp_user";

        public static readonly string USER_PATH = "m_user";

        //CUSTOMER
        public static readonly string TEMP_CUSTOMER_PATH = "temp_customer";

        public static readonly string CUSTOMER_PATH = "m_customer";

        //CUSTOMER
        public static readonly string TEMP_COMPANY_PATH = "temp_company";

        public static readonly string COMPANY_PATH = "m_company";

        //NAME IMAGE
        public static readonly string LOGO_IMAGE = "logo_image";

        public static readonly string PROFILE_IMAGE = "profile_image";

        public static readonly string LIST_ITEMS_PER_PAGE = "list_items_per_page";

        public static readonly string ENCRYTION_STRING = "encryption_string";

        public static readonly string VERSION_SYSTEM = "version_system";

        public static readonly string ZIP_CODE_CSV = "zip_code_csv";

        public static readonly string TEMPLATE = "template";

        public static readonly string LOG_FILE_SQL = "log_file_sql";

        public static readonly string LOG_FILE_ACTION = "log_file_action";

        public static readonly string LOG_FILE_LOGIN = "log_file_login";

        public static readonly string LOG_FILE_ERROR = "log_file_error";

        public static readonly string MENU_STATE_COOKIE_LIFETIME_DAYS = "menu_state_cookie_lifetime_days";

        public static readonly string MAX_MONTH_PASSWORD_EXPIRED = "max_month_password_expired";

        public static readonly string LIMITED_INPUT_PASSWORD_TIMES = "limited_input_password_times";
    }
}

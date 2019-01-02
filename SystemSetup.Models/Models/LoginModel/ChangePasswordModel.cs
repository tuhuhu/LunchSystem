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

namespace SystemSetup.Models
{
    using Constants.Resources;    
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// PasswordReissue Model
    /// </summary>
    public class ChangePasswordModel
    {
        /// <summary>
        /// New password for input
        /// </summary>
        public string NEW_PASSWORD { get; set; }

        public string NEW_PASSWORD_REPEAT { get; set; }

        public string COMPANY_CD { get; set; }

        public string CONTRACT_USER_ACCOUNT { get; set; }

        public string SETUP_USER_ACCOUNT { get; set; }
    }
}

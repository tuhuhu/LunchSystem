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

namespace iEnterAsia.iseiQ.Entities
{
    using Constants.Resources;
    using iEnterAsia.iseiQ.DataValidation;
    using System.ComponentModel.DataAnnotations;

    public class LoginInputModel
    {
        /// <summary>
        /// Companycode
        /// </summary>
        [Display(Name = "CompanyCode", ResourceType = typeof(Login))]
        //[EXRequired(Constants.MessageCd.W0001, typeof(Login), "UserAccount")]
        //[EXHalfSize(Constants.MessageCd.W0002, typeof(MCA0010), "UserIDHalfSize")]
        public string CompanyCode { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [Display(Name = "UserAccount", ResourceType = typeof(Login))]
        //[EXRequired(Constants.MessageCd.W0001, typeof(Login), "UserAccount")]
        //[EXHalfSize(Constants.MessageCd.W0002, typeof(MCA0010), "UserIDHalfSize")]
        public string UserAccount { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Display(Name = "Password", ResourceType = typeof(Login))]
        //[EXRequired(Constants.MessageCd.W0001, typeof(Login), "Password")]
        //[EXHalfSize(Constants.MessageCd.W0002, typeof(MCA0010), "PasswordHaftSize")]
        public string Password { get; set; }       
    }
}

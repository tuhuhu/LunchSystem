using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.Models
{
    public class AuthorityEntity : BaseEntity
    {
        public string COMPANY_CD{ get; set; }
        public int? AUTHORITY_GROUP_CD{ get; set; }
        public string AUTHORITY_KEY { get; set; }
        public string DISABLE_FLG { get; set; }
    }
}

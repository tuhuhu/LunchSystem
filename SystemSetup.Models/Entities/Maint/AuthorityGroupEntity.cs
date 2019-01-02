using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.Models
{
    public class AuthorityGroupEntity : BaseEntity
    {
            public string COMPANY_CD { get; set; }
            public int? AUTHORITY_GROUP_CD { get; set; }
            public string AUTHORITY_GROUP_NAME { get; set; }
            public string DISABLE_FLG { get; set; }
    }
    public class AuthorityGroupEntityPlus : AuthorityGroupEntity
    {
        new public int? INS_USER_ID { get; set; }
        new public int? UPD_USER_ID { get; set; }
        new public DateTime? INS_DATE { get; set; }
        new public DateTime? UPD_DATE { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SystemSetup.Constants;
using SystemSetup.Models;

namespace SystemSetup.Models
{
    public class OrgMaintRegistModel : GroupEntity
    {
        public string SEARCH_COMPANY_CD { get; set; }
        //新組織区分
        public int GROUP_TYPE_NEW { get; set; }
    
    }
}

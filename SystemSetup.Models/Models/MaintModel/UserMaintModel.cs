using System;
using System.Web.Mvc;
using System.Collections.Generic;
using SystemSetup.Constants;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SystemSetup.Models
{
    public class UserMaintModel : UserMaintEntityPlus
    {
        public IList<UserMaintEntity> INFO_LIST { get; set; }
        public IList<SelectListItem> AUTHORITY_GROUP_LIST { get; set; }

        public IList<SelectListItem> CONTRACT_FIRM_LIST { get; set; }

        public UserMaintEntity SEARCH_PARAM { get; set; }
        public String DISPLAY_START { get; set; }
        public String DISPLAY_LENGTH { get; set; }

        //権限グループコード
        public string AUTHORITY_GROUP_CD_STRING { get; set; }

        public UserMaintModel()
        {
            INFO_LIST = new List<UserMaintEntity>();
            AUTHORITY_GROUP_LIST = new List<SelectListItem>();

        }
    }
}

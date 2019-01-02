using System;
using System.Web.Mvc;
using System.Collections.Generic;

namespace SystemSetup.Models
{
    public class UserMaintListModel : UserMaintEntityPlus
    {
        public IList<SelectListItem> CONTRACT_FIRM_LIST { get; set; }

        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public bool INCLUDE_DISABLE { get; set; }
        public bool INCLUDE_DELETED { get; set; }

        public UserMaintEntity SEARCH_PARAM { get; set; }
        public String DISPLAY_START { get; set; }
        public String DISPLAY_LENGTH { get; set; }

        public String AUTHORITY_GROUP_CD_EDIT { get; set; }
        public IList<SelectListItem> AUTHORITY_GROUP_LIST { get; set; }


        public UserMaintListModel()
        {
            INCLUDE_DISABLE = false;
            INCLUDE_DELETED = false;

            AUTHORITY_GROUP_LIST = new List<SelectListItem>();


        }
    }
}

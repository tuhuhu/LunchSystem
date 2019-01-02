using System;
using System.Web.Mvc;
using System.Collections.Generic;

namespace SystemSetup.Models
{
    public class SetUpUserMaintListModel : SetUpUserMaintEntityPlus
    {
        public IList<SelectListItem> SETUP_FIRM_LIST { get; set; }

        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public bool INCLUDE_DISABLE { get; set; }
        public bool INCLUDE_DELETED { get; set; }

        public SetUpUserMaintEntity SEARCH_PARAM { get; set; }
        public String DISPLAY_START { get; set; }
        public String DISPLAY_LENGTH { get; set; }



        public SetUpUserMaintListModel()
        {
            INCLUDE_DISABLE = false;
            INCLUDE_DELETED = false;



        }
    }
}

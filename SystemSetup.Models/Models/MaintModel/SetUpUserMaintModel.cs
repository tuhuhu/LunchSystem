using System;
using System.Web.Mvc;
using System.Collections.Generic;
using SystemSetup.Constants;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemSetup.Models
{
    public class SetUpUserMaintModel : SetUpUserMaintEntityPlus
    {

        public IList<SetUpUserMaintEntity> INFO_LIST { get; set; }

        public IList<SelectListItem> SETUP_FIRM_LIST { get; set; }

        public SetUpUserMaintEntity SEARCH_PARAM { get; set; }
        public String DISPLAY_START { get; set; }
        public String DISPLAY_LENGTH { get; set; }

        public SetUpUserMaintModel()
        {
            INFO_LIST = new List<SetUpUserMaintEntity>();

        }

    }
}

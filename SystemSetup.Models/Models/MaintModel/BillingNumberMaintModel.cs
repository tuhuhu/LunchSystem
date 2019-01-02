using System;
using System.Web.Mvc;
using System.Collections.Generic;
using SystemSetup.Constants;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SystemSetup.Models
{
    public class BillingNumberMaintModel : BillingNumberMaintEntityPlus
    {
        public String SEARCH_COMPANY_NAME { get; set; }
        public String DISPLAY_START { get; set; }
        public String DISPLAY_LENGTH { get; set; }
    }
}

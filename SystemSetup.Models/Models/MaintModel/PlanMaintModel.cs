using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.Models
{
    public class PlanMaintModel : PlanMaintEntityPlus
    {
        public PlanMaintEntityPlus SEARCH_PARAM { get; set; }

        public IList<PlanMaintEntityPlus> INFO_LIST { get; set; }
        public bool INCLUDE_DISABLE { get; set; }

        public string SEARCH_START { get; set; }
        public string SEARCH_LENGTH { get; set; }

        public PlanMaintModel()
        {
            //FirmContractClass = new FirmContractClassEntity();
        }
    }
}

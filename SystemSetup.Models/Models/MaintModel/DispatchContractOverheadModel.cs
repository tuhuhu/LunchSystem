using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.Models
{
    public class DispatchContractOverheadModel : DispatchContractOverheadEntity
    {
        public IList<SelectListItem> CONTRACT_FIRM_LIST { get; set; }

        public IList<DispatchContractOverheadEntity> DISPATCH_CONTRACT_OVERHEAD_LIST { get; set; }

        public string SEARCH_COMPANY_CD { get; set; }

        public DispatchContractOverheadModel()
        {
            DISPATCH_CONTRACT_OVERHEAD_LIST = new List<DispatchContractOverheadEntity>();
        }
    }
}

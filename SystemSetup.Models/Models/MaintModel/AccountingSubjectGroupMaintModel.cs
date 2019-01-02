using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.Models
{
    public class AccountingSubjectGroupMaintModel : AccountGroupEntityPlus
    {

        public AccountGroupEntity SEARCH_PARAM { get; set; }

        public IList<AccountGroupEntity> INFO_LIST { get; set; }
        public bool INCLUDE_DISABLE { get; set; }

        public string SEARCH_START { get; set; }
        public string SEARCH_LENGTH { get; set; }

        public AccountingSubjectGroupMaintModel()
        {
            //FirmContractClass = new FirmContractClassEntity();
        }
    }
}

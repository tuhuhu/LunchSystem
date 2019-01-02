using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.Models
{
    public class SubCategoryMaintModel : FirmContractClassEntityPlus
    {
        public FirmContractClassEntityPlus REGIST_PARAM { get; set; }

        public IList<SelectListItem> CONTRACT_FIRM_LIST { get; set; }
        public IList<SelectListItem> CONTRACT_TYPE_LIST { get; set; }
        public IList<SelectListItem> CONTRACT_SUB_TYPE_LIST { get; set; }

        public string CONTRACT_TYPE_DISP_NAME { get; set; }

        public string CONTRACT_TYPE_EDIT { get; set; }
        public string CONTRACT_TYPE_CLASS_EDIT { get; set; }

        public string SEARCH_COMPANY_CD { get; set; }
        public string SEARCH_CONTRACT_TYPE_EDIT { get; set; }
        public string SEARCH_CONTRACT_TYPE_CLASS_EDIT { get; set; }

        public string SEARCH_START { get; set; }
        public string SEARCH_LENGTH { get; set; }

        public SubCategoryMaintModel()
        {
            //FirmContractClass = new FirmContractClassEntity();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.Models
{
    public class ProductMaintModel : ProductEntityPlus
    {
        public ProductEntityPlus REGIST_PARAM { get; set; }

        public IList<SelectListItem> CONTRACT_FIRM_LIST { get; set; }
        public IList<SelectListItem> CONTRACT_TYPE_LIST { get; set; }
        public IList<SelectListItem> CONTRACT_SUB_TYPE_LIST { get; set; }
        public IList<SelectListItem> PRODUCT_LIST { get; set; }
        public IList<SelectListItem> PRODUCT_NAME_LIST { get; set; }
        public IList<SelectListItem> PRODUCT_SEQ_NO_LIST { get; set; }

        public string CONTRACT_TYPE_EDIT { get; set; }
        public string CONTRACT_TYPE_CLASS_EDIT { get; set; }
        public string PRODUCT_SEQ_NO_EDIT { get; set; }

        public string CONTRACT_TYPE_DISP_NAME { get; set; }
        public string CONTRACT_TYPE_CLASS_DISP_NAME { get; set; }

        public string CONTRACT_INPUT_TYPE_NAME { get; set; }

        public string SEARCH_COMPANY_CD { get; set; }
        public string SEARCH_CONTRACT_TYPE_EDIT { get; set; }
        public string SEARCH_CONTRACT_TYPE_CLASS_EDIT { get; set; }
        public string SEARCH_PRODUCT_NAME_EDIT { get; set; }
        public string SEARCH_PRODUCT_SEQ_NO_EDIT { get; set; }

        public string SEARCH_START { get; set; }
        public string SEARCH_LENGTH { get; set; }
    }
}

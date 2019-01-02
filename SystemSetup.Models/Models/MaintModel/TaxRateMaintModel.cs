using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.Models
{
    public class TaxRateMaintModel : TaxRateEntity
    {
        public TaxRateEntity REGIST_PARAM { get; set; }

        //検索条件
        public string SEARCH_APPLY_DATE_FROM { get; set; }
        public string SEARCH_APPLY_DATE_TO { get; set; }
        public string SEARCH_TAX_RATE { get; set; }

        // 作成日
        public DateTime? INS_DATE { get; set; }
        // 更新日
        public DateTime? UPD_DATE { get; set; }

        public string UPD_USERNAME { get; set; }
        public string INS_USERNAME { get; set; }

        public string SEARCH_START { get; set; }
        public string SEARCH_LENGTH { get; set; }

        public TaxRateMaintModel()
        {
            //FirmContractClass = new FirmContractClassEntity();
        }
    }
}

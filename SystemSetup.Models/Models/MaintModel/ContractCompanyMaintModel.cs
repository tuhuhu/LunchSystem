using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.Models
{
    public class ContractCompanyMaintModel : ContractFirmEntityPlus
    {
        public ContractFirmEntityPlus REGIST_PARAM { get; set; }

        //端数処理区分セレクトボックスアイテムリスト
        public IList<SelectListItem> ROUNDING_TYPE_LIST { get; set; }
        //契約階層区分セレクトボックスアイテムリスト
        public IList<SelectListItem> CONTRACT_LEVEL_TYPE_LIST { get; set; }
        //無し・有りセレクトボックスアイテムリスト
        public IList<SelectListItem> EXISTENCE_LIST { get; set; }
        //契約状態セレクトボックスアイテムリスト
        public IList<SelectListItem> COMPANY_CONTRACT_STATUS_LIST { get; set; }
        //契約プラン区分セレクトボックスアイテムリスト
        public IList<SelectListItem> PLAN_LIST { get; set; }

        public IList<SelectListItem> MONTH_LIST { get; set; } 

        //契約企業コード
        public string SEARCH_COMPANY_CD { get; set; }
        //契約企業名
        public string SEARCH_COMPANY_NAME { get; set; }
        //契約担当部署
        public string SEARCH_COMPANY_GROUP_NAME { get; set; }
        //契約担当者名
        public string SEARCH_COMPANY_STAFF_NAME { get; set; }
        //契約状態
        public bool SEARCH_COMPANY_CONTRACT_STATUS { get; set; }

        //契約期間（開始）
        new public DateTime AVAILABLE_PERIOD_FROM { get; set; }
        //契約期間（終了）
        new public DateTime AVAILABLE_PERIOD_TO { get; set; }


        public string SEARCH_START { get; set; }
        public string SEARCH_LENGTH { get; set; }

        public ContractCompanyMaintModel()
        {
            //FirmContractClass = new FirmContractClassEntity();
            MONTH_LIST =  new List<SelectListItem>
            {
                new SelectListItem{ Value="1", Text="1月" },
                new SelectListItem{ Value="2", Text="2月" },
                new SelectListItem{ Value="3", Text="3月", Selected = true},
                new SelectListItem{ Value="4", Text="4月" },
                new SelectListItem{ Value="5", Text="5月" },
                new SelectListItem{ Value="6", Text="6月" },
                new SelectListItem{ Value="7", Text="7月" },
                new SelectListItem{ Value="8", Text="8月" },
                new SelectListItem{ Value="9", Text="9月" },
                new SelectListItem{ Value="10",Text="10月" },
                new SelectListItem{ Value="11",Text="11月" },
                new SelectListItem{ Value="12",Text="12月" }
            };
        }
    }
}

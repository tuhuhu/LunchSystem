using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.Models
{
    public class LunchListModel : LunchInformationEntity
    {
        public TaxRateEntity REGIST_PARAM { get; set; }

        //検索条件
        public string SEARCH_APPLY_DATE_FROM { get; set; }
        public string SEARCH_APPLY_DATE_TO { get; set; }
        public string SEARCH_TAX_RATE { get; set; }
        public string LUNCH_MENU_NAME { get; set; }
        public string SETUP_USER_NAME { get; set; }
        // 作成日
        public DateTime? INS_DATE { get; set; }
        // 更新日
        public DateTime? UPD_DATE { get; set; }

        public string UPD_USERNAME { get; set; }
        public string INS_USERNAME { get; set; }

        public string SEARCH_START { get; set; }
        public string SEARCH_LENGTH { get; set; }

        public LunchListModel()
        {
            //FirmContractClass = new FirmContractClassEntity();
        }
    }

    public class PaymentListModel : BaseEntity
    {
        //勘定科目コード
        public long PAYMENT_CD { get; set; }
        //勘定科目名称
        public long SETUP_USER_SEQ_CD { get; set; }
        public long BALANCE { get; set; }
        public long CHARGE { get; set; }
        public string SETUP_USER_NAME { get; set; }
        public string NAME_SEARCH { get; set; }
        public string SEARCH_START { get; set; }
        public string SEARCH_LENGTH { get; set; }

        public IList<PaymentEntity> PAYMENT_LIST { get; set; }

        public PaymentListModel()
        {
            //FirmContractClass = new FirmContractClassEntity();
        }
    }
    
}

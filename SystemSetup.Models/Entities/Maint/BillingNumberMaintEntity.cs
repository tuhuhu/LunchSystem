using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SystemSetup.Models
{
    public class BillingNumberMaintEntity : BaseEntity
    {
        public string COMPANY_CD { get; set; }
        //契約企業名
        public string COMPANY_NAME { get; set; }
        //ご契約プラン名
        public string PLAN_NAME { get; set; }
        //データ最大件数 
        public int MONTHLY_BILL_DATA_UPPER { get; set; }
        //請求データ件数（先月）
        public int BILLING_NUMBER_DATA_PREV_MONTH { get; set; }
        //請求データ件数（当月）
        public int BILLING_NUMBER_DATA_THIS_MONTH { get; set; }
        //請求データ件数（翌月）
        public int BILLING_NUMBER_DATA_NEXT_MONTH { get; set; }

        public decimal FILE_SIZE_TOTAL { get; set; }
        //無効フラグ
        public string DISABLE_FLG { get; set; }
        //削除フラグ
        public string DEL_FLG { get; set; }
    }

    public class BillingNumberMaintEntityPlus : BillingNumberMaintEntity
    {
        public string INS_USERNAME { get; set; }
        public string UPD_USERNAME { get; set; }
        new public DateTime? INS_DATE { get; set; }
        new public DateTime? UPD_DATE { get; set; }
    }

}


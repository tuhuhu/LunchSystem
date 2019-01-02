using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemSetup.Models
{
    public class Mst_PlanEntity : BaseEntity
    {
            //プラン通し番号
            public string PLAN_SEQ_NO { get; set; }
            //プランコード
            public string PLAN_CD { get; set; }
            //プラン名称
            public string PLAN_NAME { get; set; }
            //基本金額
            public string PLAN_BASE_PRICE { get; set; }
            //アカウント数上限値
            public string LOGIN_ACCOUNT_UPPER { get; set; }
            //月間請求データ数上限値
            public string MONTHLY_BILL_DATA_UPPER { get; set; }
            //無効フラグ
            public string DISABLE_FLG { get; set; }
        }

        public class Mst_PlanEntityPlus : Mst_PlanEntity
        {
            public string INS_USERNAME { get; set; }
            public string UPD_USERNAME { get; set; }
            new public DateTime? INS_DATE { get; set; }
            new public DateTime? UPD_DATE { get; set; }
        }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SystemSetup.Models
{
    public class PlanMaintEntity : BaseEntity
    {
        //プラン通し番号
        public string PLAN_SEQ_NO { get; set; }
        //プランコード
        public string PLAN_CD { get; set; }
        //プラン名称
        public string PLAN_NAME { get; set; }
        //基本金額
        public decimal? PLAN_BASE_PRICE { get; set; }
        //アカウント数上限値
        public int? LOGIN_ACCOUNT_UPPER { get; set; }
        //月間請求データ数上限値
        public int? MONTHLY_BILL_DATA_UPPER { get; set; }
        //無効フラグ
        public string DISABLE_FLG { get; set; }
        //編集
        public string EDIT { get; set; }
        //削除
        public string DELETE { get; set; }
    }

    public class PlanMaintEntityPlus : PlanMaintEntity
    {
        public string INS_USERNAME { get; set; }
        public string UPD_USERNAME { get; set; }
        new public DateTime? INS_DATE { get; set; }
        new public DateTime? UPD_DATE { get; set; }
    }
}
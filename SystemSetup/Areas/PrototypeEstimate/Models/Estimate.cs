using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace iEnterAsia.iseiQ.Areas.PrototypeEstimate.Models
{
    public class Estimate
    {
        [DisplayName("見積番号")]
        public int est_no { get; set; }

        public int est_no_sc { get; set; }

        [DisplayName("取引先企業")]
        public string bp_name_disp { get; set; }

        [DisplayName("案件名")]
        public string prj_name { get; set; }

        [DisplayName("案件表示名")]
        public string prj_dsp_name { get; set; }

        [DisplayName("案件詳細名")]
        public string prj_desc_name { get; set; }

        [DisplayName("契約期間開始日")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime prj_period_start { get; set; }

        [DisplayName("契約期間終了日")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime prj_period_end { get; set; }

        [DisplayName("納品日")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime delivery_date { get; set; }

        [DisplayName("検収日")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime acceptance_date { get; set; }

        [DisplayName("締めサイト")]
        public string closing_site_type { get; set; }

        [DisplayName("締日")]
        public int closing_day { get; set; }

        [DisplayName("支払サイト")]
        public string payment_site_type { get; set; }

        [DisplayName("支払日")]
        public int payment_day { get; set; }

        [DisplayName("見積区分")]
        public string quotation_type { get; set; }

        [DisplayName("見積金額合計")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, NullDisplayText = "")]
        public decimal est_amount_total { get; set; }

        [DisplayName("見積条件")]
        public string est_condition { get; set; }

        [DisplayName("共有メモ")]
        public string memo { get; set; }
    }
}
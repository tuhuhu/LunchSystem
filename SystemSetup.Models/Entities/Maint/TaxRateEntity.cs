using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemSetup.Models
{
    public class TaxRateEntity : BaseEntity
    {
        //消費税率ＩＤ
        public long TAX_RATE_ID { get; set; }
        //適用日付（開始）
        public DateTime APPLY_DATE_FROM { get; set; }
        //適用日付（終了）
        public DateTime APPLY_DATE_TO { get; set; }
        //消費税率
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal TAX_RATE { get; set; }

    }
}

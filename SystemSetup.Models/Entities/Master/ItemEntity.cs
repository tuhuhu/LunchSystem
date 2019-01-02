using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemSetup.Models
{
    public class ItemEntity : BaseEntity
    {
        //品目通し番号
        public long ITEM_SEQ_NO { get; set; }
        //契約企業コード
        public string COMPANY_CD { get; set; }
        //契約種別
        public int CONTRACT_TYPE { get; set; }
        //契約種別分類
        public int CONTRACT_TYPE_CLASS { get; set; }
        //製品番号
        public int PRODUCT_NO { get; set; }
        //品目番号
        public int ITEM_NO { get; set; }
        //料金種別
        public int CHARGE_TYPE { get; set; }
        //品名
        public string NOMEN { get; set; }
        //表示優先度
        public short DSP_PRIORITY { get; set; }
        //単価
        public decimal UNIT_PRICE { get; set; }
        //仕入単価
        public decimal PURCHASE_PRICE { get; set; }
        //単位
        public string UNIT_TYPE { get; set; }
        //課税フラグ
        public int TAXABLE_FLG { get; set; }
    }

    public class ItemEntityPlus : ItemEntity
    {
        public string INS_USERNAME { get; set; }
        public string UPD_USERNAME { get; set; }
        new public DateTime? INS_DATE { get; set; }
        new public DateTime? UPD_DATE { get; set; }

    }
}


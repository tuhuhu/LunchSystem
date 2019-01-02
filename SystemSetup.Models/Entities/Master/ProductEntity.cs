using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemSetup.Models
{
    public class ProductEntity : BaseEntity
    {
        //製品通し番号
        public long PRODUCT_SEQ_NO { get; set; }
        //契約企業コード
        public string COMPANY_CD { get; set; }
        //契約種別
        public int CONTRACT_TYPE { get; set; }
        //契約種別分類
        public int CONTRACT_TYPE_CLASS { get; set; }
        //製品番号
        public int PRODUCT_NO { get; set; }
        //製品コード
        public string PRODUCT_CD { get; set; }
        //製品名
        public string PRODUCT_NAME { get; set; }
        //表示優先度
        public short DSP_PRIORITY { get; set; }
        //契約入力区分
        public string CONTRACT_INPUT_TYPE { get; set; }
        //案件名初期値
        public string PJT_NAME_INIT_VALUE { get; set; }
        //作業内容初期値
        public string WORKING_CONTENT_INIT_VALUE { get; set; }
        //作業場所初期値
        public string WORKING_LOCATION_INIT_VALUE { get; set; }
        //納品物件初期値
        public string DELIVERABLES_INIT_VALUE { get; set; }
        //納品場所初期値
        public string DELIVERY_LOCATION_INIT_VALUE { get; set; }
        //見積書フォーマットパス
        public string EST_FORMAT_PATH { get; set; }
        //見積書明細行数
        public int EST_FORMAT_DETAIL_LINE { get; set; }
        //処理区分
        public int PROCESSING_TYPE { get; set; }
        //自動展開フラグ
        public string AUTO_EXPAND_FLG { get; set; }
        //見積書フォーマットパス
        public string ORD_FORMAT_PATH { get; set; }
        //見積書明細行数
        public int ORD_FORMAT_DETAIL_LINE { get; set; }
        //納品書フォーマットパス
        public string INV_FORMAT_PATH { get; set; }
        //納品書明細行数
        public int INV_FORMAT_DETAIL_LINE { get; set; }
    }

    public class ProductEntityPlus : ProductEntity
    {
        public string INS_USERNAME { get; set; }
        public string UPD_USERNAME { get; set; }
    }
}

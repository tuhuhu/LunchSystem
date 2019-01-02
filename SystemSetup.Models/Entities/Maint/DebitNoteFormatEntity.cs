using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemSetup.Models
{
    public class DebitNoteFormatEntity: BaseEntity
    {
        //請求書追加フォーマット通し番号
        public long BILLING_ADD_FORMAT_SEQ_NO { get; set; }
        //契約企業コード
        public string COMPANY_CD { get; set; }
        //請求書種別
        public string BILLING_TYPE { get; set; }
        //請求書フォーマット表示名
        public string BILLING_FORMAT_DISP_NAME { get; set; }
        //請求書フォーマットパス
        public string BILLING_FORMAT_PATH { get; set; }
        //請求書明細行数
        public int BILLING_FORMAT_DETAIL_LINE { get; set; }
        //無効フラグ
        public string DISABLE_FLG { get; set; }
    }
}

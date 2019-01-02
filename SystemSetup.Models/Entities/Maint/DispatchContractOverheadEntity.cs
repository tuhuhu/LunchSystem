using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemSetup.Models
{
    // Dispatch Contract Overhead Entity
    public class DispatchContractOverheadEntity : BaseEntity
    {
        // フォーマット通し番号
        public long FORMAT_SEQ_NO { get; set; }
        // 契約企業コード
        public string COMPANY_CD { get; set; }
        // フォーマット種別
        public string FORMAT_TYPE { get; set; }
        // フォーマット副種別
        public string FORMAT_SUB_TYPE { get; set; }
        // フォーマット表示名
        public string FORMAT_DISP_NAME { get; set; }
        // フォーマットパス
        public string FORMAT_PATH { get; set; }
        // 明細行数
        public int FORMAT_DETAIL_LINE { get; set; }
        // 無効フラグ
        public string DISABLE_FLG { get; set; }

    }
}

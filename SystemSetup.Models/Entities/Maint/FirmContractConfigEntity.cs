using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemSetup.Models
{
    public class FirmContractConfigEntity : BaseEntity
    {
        //契約企業コード
        public string COMPANY_CD { get; set; }
        //契約種別
        public int CONTRACT_TYPE { get; set; }
        //表示優先度
        public short DSP_PRIORITY { get; set; }
        //契約種別表示名称
        public string CONTRACT_TYPE_DISP_NAME { get; set; }
        //見積番号接頭辞
        public string EST_NO_PREFIX { get; set; }

        public string DELIVERY_NO_PREFIX { get; set; }

        public string PLC_ORDER_NO_PREFIX { get; set; }

        //見積有効期間単位
        public string EST_EFFECTIVE_TYPE { get; set; }
        //見積有効期間
        public int? EST_EFFECTIVE_INTERVAL { get; set; }
    }

    public class FirmContractConfigEntityPlus : FirmContractConfigEntity
    {
        public string INS_USERNAME { get; set; }
        public string UPD_USERNAME { get; set; }
        new public DateTime? INS_DATE { get; set; }
        new public DateTime? UPD_DATE { get; set; }
    }
}

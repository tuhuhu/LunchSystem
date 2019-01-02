using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemSetup.Models
{
    public class FirmContractClassEntity : BaseEntity
    {
        //契約企業コード
        public string COMPANY_CD { get; set; }
        //契約種別
        public int CONTRACT_TYPE { get; set; }
        //契約種別分類
        public int CONTRACT_TYPE_CLASS { get; set; }
        //表示優先度
        public int DSP_PRIORITY { get; set; }
        //契約種別分類表示名所
        public string CONTRACT_TYPE_CLASS_DISP_NAME { get; set; }
    }

    public class FirmContractClassEntityPlus : FirmContractClassEntity
    {
        public string INS_USERNAME { get; set; }
        public string UPD_USERNAME { get; set; }
        new public DateTime? INS_DATE { get; set; }
        new public DateTime? UPD_DATE { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemSetup.Models
{
    public class ContractTypeMasterModel
    {
        public string COMPANY_CD { get; set; }

        public int CONTRACT_TYPE { get; set; }

        public int DSP_PRIORITY { get; set; }

        public int CONTRACT_TYPE_CLASS { get; set; }

        public int EST_EFFECTIVE_INTERVAL { get; set; }

        public int EST_FORMAT_DETAIL_LINE { get; set; }

        public string CONTRACT_TYPE_DISP_NAME { get; set; }

        public string CONTRACT_TYPE_CLASS_DISP_NAME { get; set; }

        public string EST_NO_PREFIX { get; set; }

        public string EST_EFFECTIVE_TYPE { get; set; }

        public string EST_FORMAT_PATH { get; set; }

        public string DEL_FLG { get; set; }

        public DateTime INS_DATE { get; set; }

        public long INS_USER_ID { get; set; }

        public DateTime UPD_DATE { get; set; }

        public long UPD_USER_ID { get; set; }

        public string UPD_PROG_ID { get; set; }
    }
}

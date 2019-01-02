using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iEnterAsia.iseiQ.Constants.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace iEnterAsia.iseiQ.Models
{
    public class ContractCompanyMasterModel
    {
        public string COMPANY_CD { get; set; }

        public string COMPANY_NAME { get; set; }

        public string COMPANY_ZIP_CD { get; set; }

        public string COMPANY_ADDR_1 { get; set; }

        public string COMPANY_ADDR_2 { get; set; }

        public string COMPANY_GROUP_NAME { get; set; }

        public string COMPANY_STAFF_NAME { get; set; }

        public string COMPANY_STAFF_NAME_KANA { get; set; }

        public string COMPANY_STAFF_TEL { get; set; }

        public string COMPANY_STAFF_TEL_EXT { get; set; }

        public string COMPANY_STAFF_EMAIL_ADDR { get; set; }

        public string ROUNDING_TYPE { get; set; }

        public string CONTRACT_LEVEL_TYPE { get; set; }

        public string EST_NO_USE_PREFIX { get; set; }

        public string EST_NO_USE_YM { get; set; }

        public string COMPANY_CONTRACT_STATUS { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? AVAILABLE_PERIOD_FROM { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? AVAILABLE_PERIOD_TO { get; set; }

        public string DEL_FLG { get; set; }

        public DateTime INS_DATE { get; set; }

        public long INS_USER_ID { get; set; }

        public DateTime UPD_DATE { get; set; }

        public long UPD_USER_ID { get; set; }

        public string UPD_PROG_ID { get; set; }
    }
}

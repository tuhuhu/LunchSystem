using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemSetup.Models
{
    public class ContractFirmUserEntity : BaseEntity
    {
        public long CONTRACT_USER_SEQ_CD { get; set; }
        public string COMPANY_CD { get; set; }
        public string CONTRACT_USER_ACCOUNT { get; set; }
        public string CONTRACT_USER_PASSWORD { get; set; }
        public string CONTRACT_USER_NAME { get; set; }
        public long STAFF_ID { get; set; }
        public int AUTHORITY_GROUP_CD { get; set; }
        public DateTime PASSWORD_LAST_UPDATE_DATE { get; set; }
    }

    public class SystemSetupUserEntity : BaseEntity
    {
        public long SETUP_USER_SEQ_CD { get; set; }
        public string SETUP_USER_ACCOUNT { get; set; }
        public string SETUP_USER_PASSWORD { get; set; }
        public string SETUP_USER_NAME { get; set; }
        public string DISABLE_FLG { get; set; }       
        public DateTime PASSWORD_LAST_UPDATE_DATE { get; set; }
    }
}

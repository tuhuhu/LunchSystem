using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SystemSetup.Models
{
    public class ContractFirmPlanMaintEntity : BaseEntity
    {
        public long PLAN_REL_SEQ_NO { get; set; }
        public string COMPANY_CD { get; set; }
        public long PLAN_SEQ_NO { get; set; }
        public DateTime PLAN_START_DATE { get; set; }
    }
}


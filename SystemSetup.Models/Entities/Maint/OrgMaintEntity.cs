using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SystemSetup.Models
{
    public class OrgMaintEntity : BaseEntity
    {
        //組織ID
        public long GROUP_ID { get; set; }
        //契約企業コード
        public string COMPANY_CD { get; set; }
        //組織区分
        public int GROUP_TYPE { get; set; }
        //組織名
        public string GROUP_NAME { get; set; }
        //組織名(カナ)
        public string GROUP_NAME_KANA { get; set; }
        //上位組織ID
        public long UP_GROUP_ID { get; set; }
        //表示優先度
        public int DSP_PRIORITY { get; set; }

    }

    public class OrgMaintEntityPlus : OrgMaintEntity
    {
        public string INS_USERNAME { get; set; }
        public string UPD_USERNAME { get; set; }
        new public DateTime? INS_DATE { get; set; }
        new public DateTime? UPD_DATE { get; set; }

        public string RELFLAG { get; set; }
    }
}

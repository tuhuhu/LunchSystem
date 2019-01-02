using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SystemSetup.Models
{
    public class InformationEntity : BaseEntity
    {
        // 掲載ID(PK)
        public long INFO_SEQ_NO { get; set; }

        // 掲載開始日
        public DateTime? PUBLISH_DATE_START { get; set; }

        // 掲載終了日
        public DateTime? PUBLISH_DATE_END { get; set; }

        // 企業コード
        public string COMPANY_CD { get; set; }

        // 掲載内容
        public string CONTENT { get; set; }

        // 表示優先度
        public int DSP_PRIORITY { get; set; }

        // 削除
        public int DELETE { get; set; }
    }

    public class InformationEntityPlus : InformationEntity
    {
        public string INS_USERNAME { get; set; }
        public string UPD_USERNAME { get; set; }
        new public DateTime? INS_DATE { get; set; }
        new public DateTime? UPD_DATE { get; set; }
    }
}

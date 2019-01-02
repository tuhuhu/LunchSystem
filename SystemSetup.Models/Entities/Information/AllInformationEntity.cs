using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SystemSetup.Models
{
    public class AllInformationEntity : BaseEntity
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

        // 掲載区分
        public string CONTENT_TYPE { get; set; }

        // タイトル
        public string TITLE { get; set; }

        // 削除
        public int DELETE { get; set; }

        //削除済も含む
        public bool INCLUDE_DELETED { get; set; }

        //削除フラグ
        public string DEL_FLG { get; set; }
    }
}

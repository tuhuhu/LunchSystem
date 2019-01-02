using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SystemSetup.Models
{
    public class SystemStatusEntity : BaseEntity
    {
        //運転制御ID
        public long INFO_SEQ_NO { get; set; }

        // 運転モード
        public string SYSTEM_OPERATION_MODE { get; set; }

        // 停止時表示メッセージ
        public string SYSTEM_STOPPED_MESSAGE { get; set; }

        // 告知有無
        public int? NOTICE_FLG { get; set; }

        // 告知タイトル
        public string NOTICE_TITLE { get; set; }

        // 告知メッセージ
        public string NOTICE_MESSAGE { get; set; }
    }

    public class SystemStatusEntityPlus : SystemStatusEntity
    {
        public string INS_USERNAME { get; set; }
        public string UPD_USERNAME { get; set; }
        new public DateTime? INS_DATE { get; set; }
        new public DateTime? UPD_DATE { get; set; }
    }
}

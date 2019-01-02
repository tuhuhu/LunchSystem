using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemSetup.Models
{
    public class AccountGroupEntity : BaseEntity
    {
        //勘定科目グループ通し番号
        public string ACCOUNT_GROUP_SEQ_NO { get; set; }
        //勘定科目グループコード
        public string ACCOUNT_GROUP_CD { get; set; }
        //勘定科目グループ名称
        public string ACCOUNT_GROUP_NAME { get; set; }
        //無効フラグ
        public string DISABLE_FLG { get; set; }
    }

    public class AccountGroupEntityPlus : AccountGroupEntity
    {
        public string INS_USERNAME { get; set; }
        public string UPD_USERNAME { get; set; }
        new public DateTime? INS_DATE { get; set; }
        new public DateTime? UPD_DATE { get; set; }
    }
}

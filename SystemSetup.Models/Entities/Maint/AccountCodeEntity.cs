using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.Models
{
    //勘定科目マスタ - Mst_AccountCode
    public class AccountCodeEntity : BaseEntity
    {
        //勘定科目コード
        public string ACCOUNT_CD { get; set; }
        //勘定科目名称
        public string ACCOUNT_NAME { get; set; }
        //勘定科目表示名
        public string ACCOUNT_DISP_NAME { get; set; }
        //貸借区分
        public string ASSETS_LIABILITIES_TYPE { get; set; }
        //勘定科目グループ通し番号
        public long ACCOUNT_GROUP_SEQ_NO { get; set; }
        //勘定科目コード(JIS)
        public string JIS_ACCOUNT_CD { get; set; }
        //無効フラグ
        public string DISABLE_FLG { get; set; }
    }
    public class AccountCodeEntityPlus : AccountCodeEntity
    {
        public string INS_USERNAME { get; set; }
        public string UPD_USERNAME { get; set; }
        new public DateTime? INS_DATE { get; set; }
        new public DateTime? UPD_DATE { get; set; }
    }
}

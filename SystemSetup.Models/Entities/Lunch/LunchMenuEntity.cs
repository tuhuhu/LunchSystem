using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.Models
{
    //勘定科目マスタ - Mst_AccountCode
    public class LunchMenuEntity : BaseEntity
    {
        //勘定科目コード
        public long LUNCH_MENU_SEQ_CD { get; set; }
        //勘定科目名称
        public int LUNCH_MENU_TYPE { get; set; }
        //勘定科目表示名
        public string LUNCH_MENU_NAME { get; set; }        
        //無効フラグ
        public string DISABLE_FLG { get; set; }
    }
    //public class AccountCodeEntityPlus : AccountCodeEntity
    //{
    //    public string INS_USERNAME { get; set; }
    //    public string UPD_USERNAME { get; set; }
    //    new public DateTime? INS_DATE { get; set; }
    //    new public DateTime? UPD_DATE { get; set; }
    //}

    public class LunchMailEntity : BaseEntity
    {
        //勘定科目コード
        public int COUNT_MENU { get; set; }
        //勘定科目名称
        public int LUNCH_MENU_TYPE { get; set; }
        //勘定科目表示名
        public string LUNCH_MENU_NAME { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.Models
{
    //勘定科目マスタ - Mst_AccountCode
    public class LunchInformationEntity : BaseEntity
    {
        //勘定科目コード
        public long LUNCH_SEQ_CD { get; set; }
        //勘定科目名称
        public long SETUP_USER_ACCOUNT { get; set; }
        public long BALANCE { get; set; }
        public DateTime LUNCH_DATE { get; set; }
        public int LUNCH_MENU_TYPE { get; set; }
        //勘定科目表示名
        public int STATUS { get; set; }
        public int HAVE_MENU { get; set; }
        public LunchInformationEntity()
        {            
            LUNCH_MENU_TYPE = 1;
        }
        //無効フラグ
        //public string DISABLE_FLG { get; set; }
    }
    public class UserPaymentEntity : BaseEntity
    {
        public long PAYMENT_CD { get; set; }
        public long SETUP_USER_SEQ_CD { get; set; }
        public long BALANCE { get; set; }        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.Models
{
    //勘定科目マスタ - Mst_AccountCode
    public class PaymentEntity : BaseEntity
    {
        //勘定科目コード
        public long PAYMENT_CD { get; set; }
        //勘定科目名称
        public long SETUP_USER_SEQ_CD { get; set; }
        public long BALANCE { get; set; }     
        //無効フラグ
        //public string DISABLE_FLG { get; set; }
    }
    //public class AccountCodeEntityPlus : AccountCodeEntity
    //{
    //    public string INS_USERNAME { get; set; }
    //    public string UPD_USERNAME { get; set; }
    //    new public DateTime? INS_DATE { get; set; }
    //    new public DateTime? UPD_DATE { get; set; }
    //}
}

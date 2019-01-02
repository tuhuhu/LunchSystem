using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SystemSetup.Constants;
using SystemSetup.Models;

namespace SystemSetup.Models
{
    //public class OrgGroupWithChilds : GroupEntity
    //{
    //    public OrgGroupWithChilds()
    //    {
    //        ORG_LIST_CHILD_GROUP = new List<OrgGroupWithChilds>();
    //    }

    //    public IList<OrgGroupWithChilds> ORG_LIST_CHILD_GROUP { get; set; }

    //    public int LEVEL { get; set; }
    //    public int DSP_PRIORITY { get; set; }
    //    public string PATH { get; set; }
    //    public int ROW_NUMBER { get; set; }
    //    public string WORK { get; set; }
    //}

    public class OrgMaintInfo
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
        //削除フラグ
        public int DEL_FLG { get; set; }
    }

    public class OrgMaintModel : GroupEntity
    {
        public IList<SelectListItem> CONTRACT_FIRM_LIST { get; set; }

        public string INS_USERNAME { get; set; }
        public string UPD_USERNAME { get; set; }
        new public DateTime? INS_DATE { get; set; }
        new public DateTime? UPD_DATE { get; set; }

        public string FM_INS_DATE
        { 
            get {
                return INS_DATE.HasValue ? INS_DATE.Value.ToString("yyyy/MM/dd HH:mm") : String.Empty;
            }
        }
        public string FM_UPD_DATE
        {
            get
            {
                return UPD_DATE.HasValue ? UPD_DATE.Value.ToString("yyyy/MM/dd HH:mm") : String.Empty;
            }
        }
       
    }
}

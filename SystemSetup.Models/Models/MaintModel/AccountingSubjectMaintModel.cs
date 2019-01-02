using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.Models
{
    public class AccountingSubjectMaintModel : AccountCodeEntityPlus
    {
        public string REGIST_PARAM_ACCOUNT_CD { get; set; }
        public string REGIST_PARAM_ACCOUNT_GROUP_SEQ_NO { get; set; }

        //貸借区分セレクトボックスアイテムリスト
        public IList<SelectListItem> ASSETS_LIABILITIES_TYPE_LIST { get; set; }
        //勘定科目グループセレクトボックスアイテムリスト
        public IList<SelectListItem> ACCOUNT_GROUP_LIST { get; set; }
        //勘定科目コード（JIS）セレクトボックスアイテムリスト
        public IList<SelectListItem> JIS_ACCOUNT_CD_LIST { get; set; }

        //勘定科目コード
        public string SEARCH_ACCOUNT_CD { get; set; }
        //勘定科目名称
        public string SEARCH_ACCOUNT_NAME { get; set; }
        //勘定科目表示名
        public string SEARCH_ACCOUNT_DISP_NAME { get; set; }
        //貸借区分
        public string SEARCH_ASSETS_LIABILITIES_TYPE { get; set; }
        //勘定科目グループ通し番号
        public long? SEARCH_ACCOUNT_GROUP_SEQ_NO { get; set; }
        //無効フラグ
        public bool SEARCH_DISABLE_FLG { get; set; }

        public string SEARCH_START { get; set; }
        public string SEARCH_LENGTH { get; set; }

        public AccountingSubjectMaintModel()
        {
        }
    }
}

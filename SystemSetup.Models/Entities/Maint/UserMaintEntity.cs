using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SystemSetup.Constants;

namespace SystemSetup.Models
{
    public class UserMaintEntity : BaseEntity
    {
        //契約ユーザーコード
        public long CONTRACT_USER_SEQ_CD { get; set; }
        //契約企業コード
        public string COMPANY_CD { get; set; }
        //契約ユーザーアカウント
        public string CONTRACT_USER_ACCOUNT{ get; set; }
        //契約ユーザーログインパスワード
        public string CONTRACT_USER_PASSWORD { get; set; }
        //契約ユーザー名
        public string CONTRACT_USER_NAME { get; set; }
        //社員ID
        public string STAFF_ID { get; set; }
        //ログインパスワード変更日
        public DateTime? PASSWORD_LAST_UPDATE_DATE { get; set; }
        //権限グループコード
        public int? AUTHORITY_GROUP_CD { get; set; }
        public string AUTHORITY_GROUP_NAME { get; set; }
        //無効フラグ
        public string DISABLE_FLG { get; set; }
        //削除フラグ
        public string DEL_FLG { get; set; }

        //ログイン制限フラグ
        public string LOGIN_LOCK_FLG { get; set; }

        //検索権限区分
        public string SEARCH_AUTHORITY_TYPE { get; set; }

        //営業分析権限区分
        public string SALES_ANALYSIS_AUTHORITY_TYPE { get; set; }

        public UserMaintEntity()
        {
            SEARCH_AUTHORITY_TYPE = Authority_Type.All_company;
            SALES_ANALYSIS_AUTHORITY_TYPE = Authority_Type.All_company;
        }

    }

    public class UserMaintEntityPlus : UserMaintEntity
    {
        public string INS_USERNAME { get; set; }
        public string UPD_USERNAME { get; set; }
        new public DateTime? INS_DATE { get; set; }
        new public DateTime? UPD_DATE { get; set; }

        public string CONTRACT_USER_PASSWORD_REPEAT { get; set; }

    }

}


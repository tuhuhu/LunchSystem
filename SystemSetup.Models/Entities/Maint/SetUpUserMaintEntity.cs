using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SystemSetup.Models
{
    public class SetUpUserMaintEntity : BaseEntity
    {
        //設定ユーザコード
        public long SETUP_USER_SEQ_CD { get; set; }
        //設定ユーザーアカウント
        public string SETUP_USER_ACCOUNT { get; set; }
        //設定ユーザーログインパスワード
        public string SETUP_USER_PASSWORD { get; set; }
        //設定ユーザー名
        public string SETUP_USER_NAME { get; set; }
        //ログインパスワード変更日
        public DateTime? PASSWORD_LAST_UPDATE_DATE { get; set; }
        //無効フラグ
        public string DISABLE_FLG { get; set; }
        //ログイン制限フラグ
        public string LOGIN_LOCK_FLG { get; set; }
        //削除フラグ
        public string DEL_FLG { get; set; }
    }

    public class SetUpUserMaintEntityPlus : SetUpUserMaintEntity
    {
        public string INS_USERNAME { get; set; }
        public string UPD_USERNAME { get; set; }
        new public DateTime? INS_DATE { get; set; }
        new public DateTime? UPD_DATE { get; set; }

        public string UPD_PROG_ID { get; set; }

        public string SETUP_USER_PASSWORD_REPEAT { get; set; }

    }

}

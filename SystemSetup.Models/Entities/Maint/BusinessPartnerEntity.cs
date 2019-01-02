using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SystemSetup.Models
{
    public class BusinessPartnerEntity : BaseEntity
    {
        //取引先保守
        //取引先企業通し番号
        public long BP_SEQ_NO { get; set; }
        //契約企業コード
        public string COMPANY_CD { get; set; }
        //取引先企業コード
        public int BP_CD { get; set; }
        //取引先企業名
        public string BP_NAME { get; set; }
        //取引先企業名(カナ)
        public string BP_NAME_KANA { get; set; }
        //取引先企業名(表示名)
        public string BP_NAME_DISP { get; set; }
        //作成時取引先企業通し番号
        public long BASE_BP_SEQ_NO { get; set; }
        //特殊値区分
        public string SPECIAL_VALUE_TYPE { get; set; }
        //無効フラグ
        public string DISABLE_FLG { get; set; }
    }

    public class BusinessPartnerEntityPlus : BusinessPartnerEntity
    {
        //取引先企業住所マスタ
        //取引先企業住所通し番号
        public long BP_ADDR_SEQ_NO { get; set; }
        //取引先企業住所コード
        public int BP_ADDR_CD { get; set; }
        //取引先企業住所識別名
        public string BP_ADDR_NAME { get; set; }
        //取引先企業郵便番号
        public string BP_ZIP_CD { get; set; }
        //取引先企業住所１
        public string BP_ADDR_1 { get; set; }
        //取引先企業住所２
        public string BP_ADDR_2 { get; set; }
        //取引先代表電話番号
        public string BP_TEL { get; set; }
        //取引先代表FAX番号
        public string BP_FAX { get; set; }
        //取引先企業URL
        public string BP_HP_URL { get; set; }
        //社員数
        public int BP_NUM_EMPLOYEES { get; set; }
        //資本金
        public decimal BP_CAPITAL { get; set; }
        //業種
        public string BP_BIZ_CATEGORY { get; set; }
        //備考
        public string BP_NOTE { get; set; }

        //取引先企業取引情報マスタ
        //取引区分
        public string BP_TRADE_TYPE { get; set; }
        //支払いサイト
        public int BP_PAYMENT_SITE_TYPE { get; set; }
        //支払いサイト(支払日)
        public int BP_PAYMENT_DAY { get; set; }
        //基本契約締結フラグ
        public string BP_BASICCONTRACT_CONCLUSION_FLG { get; set; }
        //基本契約締結日
        public DateTime? BP_BASICCONTRACT_CONCLUSION_DATE { get; set; }
        //NDA締結フラグ
        public string BP_NDA_CONCLUSION_FLG { get; set; }
        //NDA締結日
        public DateTime? BP_NDA_CONCLUSION_DATE { get; set; }
        //その他締結内容
        public string BP_OTHER_CONCLUSION { get; set; }
        //支払方法区分
        public string BP_PAYMENT_TYPE { get; set; }
        //請求書日付発行区分
        public string BP_BILLING_ISSUE_DAY_TYPE { get; set; }

        //取引先企業取引窓口マスタ
        //金融機関コード
        public string FINANCIAL_INST_CD { get; set; }
        //金融機関名
        public string FINANCIAL_INST_NAME { get; set; }
        //取引口座支店コード
        public string BANK_ACCOUNT_BRANCH_CD { get; set; }
        //取引口座支店名
        public string BANK_ACCOUNT_BRANCH_NAME { get; set; }
        //取引口座種別
        public string BANK_ACCOUNT_TYPE { get; set; }
        //取引口座番号
        public string BANK_ACCOUNT_NO { get; set; }
        //取引口座名義
        public string BANK_ACCOUNT_HOLDER { get; set; }
        //銀行間通信網銀行カード
        public string SWIFT_BIC_CD { get; set; }
        //銀行用管理コード
        public string BANK_MANAGE_CD { get; set; }

        //企業情報共有マスタ
        //受注契約時共有情報
        public string BP_APT_PRIVATE_NOTE { get; set; }
        //発注契約時共有情報
        public string BP_PLC_PRIVATE_NOTE { get; set; }
        
    }

}

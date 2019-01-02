using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemSetup.Constants.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SystemSetup.Models
{
    public class DebitAccountEntity : BaseEntity
    {
        //契約請求通し番号
        public long CONT_BILLING_SEQ_NO { get; set; }
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
        //銀行間通信網銀行コード
        public string SWIFT_BIC_CD { get; set; }
        //銀行用管理コード
        public string BANK_MANAGE_CD { get; set; }               
    }

    public class BusinessPartnerBankAccountEntity : BaseEntity
    {
        public string COMPANY_CD { get; set; }
        public int BP_CD { get; set; }
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
        //銀行間通信網銀行コード
        public string SWIFT_BIC_CD { get; set; }
        //銀行用管理コード
        public string BANK_MANAGE_CD { get; set; }
    }
}

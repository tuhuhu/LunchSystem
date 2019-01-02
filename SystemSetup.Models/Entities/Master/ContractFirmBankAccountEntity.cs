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
    public class ContractFirmBankAccountInfoEntity : BaseEntity
    {
        //取引窓口通し番号
        public long BANK_ACCOUNT_SEQ_NO { get; set; }        
        //契約企業コード
        public string COMPANY_CD { get; set; }
        //表示順
        public int DSP_PRIORITY { get; set; }
        //口座表示名称
        public string BANK_ACCOUNT_DISP_NAME { get; set; }
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
        //締めサイト（締日）
        public string SWIFT_BIC_CD { get; set; }        
    }
}

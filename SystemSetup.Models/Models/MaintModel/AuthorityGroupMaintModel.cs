using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.Models
{
    public class AuthorityGroupMaintModel : AuthorityGroupEntityPlus
    {
        public IList<SelectListItem> CONTRACT_FIRM_LIST { get; set; }

        public string ACTION_TYPE { get; set; }
        public AuthorityGroupEntity REGIST_PARAM { get; set; }

        public string SEARCH_COMPANY_CD { get; set; }
        public string SEARCH_AUTHORITY_GROUP_CD { get; set; }
        public string SEARCH_AUTHORITY_GROUP_NAME { get; set; }
        public string SEARCH_DISABLE_FLG { get; set; }

        public string SEARCH_START { get; set; }
        public string SEARCH_LENGTH { get; set; }

        public string UPD_USERNAME { get; set; }

        public bool VIEW_DISABLE_FLG { get; set; }

        public bool ESTIMATE_VIEWESTIMATE { get; set; }
        public bool ESTIMATE_EDIT { get; set; }
        public bool ESTIMATE_ISSUEESTIMATEFILE { get; set; }
        public bool ESTIMATE_SENDESTIMATE { get; set; }
        public bool ESTIMATE_DELETE { get; set; }
        public bool ESTIMATE_COLLECTION { get; set; }
        public bool CONTRACT_VIEW { get; set; }
        public bool CONTRACT_EDIT { get; set; }
        public bool CONTRACT_FINISHCONTRACT { get; set; }
        public bool CONTRACT_FINISHCONTRACTDIRECT { get; set; }
        public bool CONTRACT_DELETE { get; set; }
        public bool CONTRACT_EXPORTCSVCONTRACTLIST { get; set; }
        public bool CONTRACT_RESEND { get; set; }
        public bool ESTIMATE_ESTIMATELISTCONTRACT { get; set; }
        public bool CONTRACT_CANCELCONTRACTEND { get; set; }
        public bool BILLING_CREATEBILLINGDATA { get; set; }
        public bool BILLING_EXPORTBILLINGDATACSV { get; set; }
        public bool BILLING_BILLINGINPUTOPERATETIME { get; set; }
        public bool BILLING_BILLING_ISSUE_DEBIT_NOTE { get; set; }
        public bool BILLING_EDITDEBITNOTE { get; set; }
        public bool BILLING_BILLINGINPUTDATE { get; set; }
        public bool BILLING_BILLINGSETTING { get; set; }
        public bool BILLING_INPUTBILLINGPREPARE { get; set; }
        public bool BILLING_DELETE_DEBIT_NOTE { get; set; }
        public bool PAYMENT_PAYMENTCLEARSETTING { get; set; }
        public bool PAYMENT_PACKAGE_CLEAR_DEPOSIT { get; set; }
        public bool PAYMENT_CREATEREDINVOICE { get; set; }
        public bool PAYMENT_PAYMENTINPUTPRIVATE { get; set; }
        public bool PAYMENT_PAYMENTEDITORDERSTATUS { get; set; }
        public bool BILLING_BILLINGEDITORDERSTATUS { get; set; }
        public bool PAYMENT_EXPORTSALESPAYMENTSDATACSV { get; set; }
        public bool CONTRACT_ISSUEDESTIMATE { get; set; }
        public bool CONTRACT_ISSUEPACKAGEDELIVERY { get; set; }
        public bool PARTNERSTAFF_COVERLETTER { get; set; }
        public bool PAYMENT_ISSUEORDERFORPAYMENT { get; set; }
        public bool ISSUE_DISPATCHCONTRACT { get; set; }
        public bool ISSUE_DISPATCH_PAYMENT { get; set; }
        public bool PAYMENT_PAYMENT_NOTICE_ISSUE { get; set; }
        public bool CUSTMAINT_CUSTMAINTLIST { get; set; }
        public bool CONTACTPERSMAINT_CONTACTPERSMAINTLIST { get; set; }
        public bool ORGMAINT_ORGMAINTLIST { get; set; }
        public bool PERSINCHARMAINT_PERSINCHARMAINTLIST { get; set; }
        public bool TRANSMAINT_TRANSMAINTLIST { get; set; }
        public bool ACCOUNTCODEMAINT_ACCOUNTCODEMAINTLIST { get; set; } 
        public bool SUBJECTMAINT_SUBJECTMAINTLIST { get; set; }
        public bool CATEGORYMAINT_CATEGORYMAINTLIST { get; set; }
        public bool USERMAINT_USERMAINTLIST { get; set; }
        public bool INFORMATION_POSTINFORMATIONLIST { get; set; }
        public bool ESTIMATE_VIEWATTACHEDFILE { get; set; }
        public bool SHOWLOG_SHOWLOG { get; set; }
        public bool PROCEEDS_PAYMENT_PLAN { get; set; }
        public bool PROCEEDS_PAYMENT_CONFIRM { get; set; }
        public bool CASH_FLOW { get; set; }
        public bool SALE_ACTUAL { get; set; }
        public bool SALE_ACTUAL_BY_CUSTOMER { get; set; }
        public bool SALE_ACTUAL_BY_CUSTOMER_GRAPH { get; set; }
        // 営業分析機能
        public bool DASHBOARD_DISPLAY { get; set; }

        public bool ACRECEIVABLE_CSV_OUTPUT { get; set; }
        public bool ACPAYABLE_CSV_OUTPUT { get; set; }
        public bool DEPOSIT_CSV_OUTPUT { get; set; }
        public bool SETTING_ACRECEIVABLE { get; set; }
        public bool SETTING_ACPAYABLE { get; set; }
        public bool SETTING_DEPOSIT { get; set; }        
        public bool NOMEN_LIST { get; set; }
        public bool PAYMENTSTATE_PAYMENTSTATEMAINTLIST { get; set; }
        public bool AUTHORITYGROUPMAINT_AUTHORITYGROUPMAINTLIST { get; set; }
        public bool CONTRACT_PLAN_CONFIRM { get; set; }
        public bool LOCK_OUT_DISABLE { get; set; }
        // 技術者管理機能
        public bool MANAGE_ENGINEER_LIST { get; set; }
        public bool MANAGE_ENGINEER_DELETE { get; set; }
        public bool MANAGE_ENGINEER_CATEGORY { get; set; }
        public bool BILLING_NOTE_ISSUE_CONTROL { get; set; }
        public bool MANAGE_ENGINEER_UPLOAD_FILE { get; set; }

        public AuthorityGroupMaintModel()
        {
        }
    }
}
                          
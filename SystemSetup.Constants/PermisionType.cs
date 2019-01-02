//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/03
// Comment		: Create new
//------------------------------------------------------------------------

namespace SystemSetup.Constants.Security
{
	/// <summary>
	/// Permision type
	/// </summary>
	public class PermisionType
	{
		/// <summary>
		/// 01 不可
		/// </summary>
		public const string ACCESS_DENY = "01";

		/// <summary>
		/// 02 可
		/// </summary>
		public const string ACCESS = "02";

		/// <summary>
        /// 見積情報一覧を表示する
		/// </summary>
        public const string ESTIMATE_LIST = "Estimate/Estimate/EstimateList";

		/// <summary>
        /// 既存の見積や新規の見積を表示する
		/// </summary>
        public const string ESTIMATE_NEW_OR_EDIT = "Estimate/Estimate/Edit";

		/// <summary>
        /// 見積を複写する
		/// </summary>
        public const string ESTIMATE_DUPLICATE = "Estimate/Estimate/Duplicate";

		/// <summary>
        /// 見積を閲覧する
		/// </summary>
        public const string ESTIMATE_VIEW = "Estimate/Estimate/ViewEstimate";

		/// <summary>
        /// 見積ファイルを作成する
		/// </summary>
        public const string ESTIMATE_ISSUE_FILE = "Estimate/Estimate/IssueEstimateFile";

		/// <summary>
        /// 見積をDBに更新する
		/// </summary>
        public const string ESTIMATE_UPDATE_DB = "Estimate/Estimate/EditEstimate";

        // setting display Attached file area or not
        public const string DISPLAY_ATTACHED_FILE = "Estimate/Estimate/ViewAttachedFile";

		/// <summary>
        /// 契約の見積情報一覧を表示する
		/// </summary>
        public const string ESTIMATE_LIST_CONTRACT = "Estimate/Estimate/EstimateListContract";

        /// <summary>
        /// 契約の見積情報一覧を表示する
        /// </summary>
        public const string ESTIMATE_ISSUE_LIST = "Estimate/Estimate/IssueEstimateFileFromContract";

        /// <summary>
        /// 見積をDBに更新する
        /// </summary>
        public const string ESTIMATE_DELETE = "Estimate/Estimate/Delete";

        /// <summary>
        /// 契約情報一覧を表示する
        /// </summary>
        public const string CONTRACT_LIST = "Contract/Contract/List";

        /// <summary>
        /// 既存の契約や新規の契約を表示する
        /// </summary>
        public const string CONTRACT_NEW_OR_EDIT = "Contract/Contract/Edit";

        public const string CONTRACT_DELETE = "Contract/Contract/Delete";

        /// <summary>
        /// 契約を閲覧する
        /// </summary>
        public const string CONTRACT_VIEW = "Contract/Contract/View";

        /// <summary>
        /// 契約をDBに更新する
        /// </summary>
        public const string CONTRACT_UPDATE_DB = "Contract/Contract/EditContract";

        /// <summary>
        /// ユーザーが契約を終了したい時、契約情報を表示する
        /// </summary>
        public const string CONTRACT_FINISH = "Contract/Contract/Finish";

        /// <summary>
        /// 契約を終了する時、一部の契約情報をDBに更新する
        /// </summary>
        public const string CONTRACT_FINISH_UPDATE_DB = "Contract/Contract/FinishContract";

        /// <summary>
        /// 契約を終了する時、一部の契約情報をDBに更新する
        /// </summary>
        public const string CONTRACT_ISSUE_ESTIMATE = "Contract/Contract/IssuedEstimate";

        /// <summary>
        /// 取引先情報一覧を表示する
        /// </summary>
        public const string CUSTOMER_LIST = "Customer/Customer/SearchCustomerIndex";

        /// <summary>
        /// 既存の取引先情報や新しい取引先情報を表示する
        /// </summary>
        public const string CUSTOMER_NEW_OR_EDIT = "Customer/Customer/Edit";

        /// <summary>
        /// 契約の見積情報一覧を表示する
        /// </summary>
        public const string CUSTOMER_UPDATE_DB = "Customer/Customer/EditCustomer";

        /// <summary>
        /// 担当者の情報一覧を表示する
        /// </summary>
        public const string PIC_LIST = "PartnerStaff/PartnerStaff/SearchPartnerStaff";

        /// <summary>
        /// 既存の担当者情報や新しい担当者情報を表示する
        /// </summary>
        public const string PIC_NEW_OR_EDIT = "PartnerStaff/PartnerStaff/Register";

        /// <summary>
        /// 担当者情報をDBに更新する
        /// </summary>
        public const string PIC_UPDATE_DB = "PartnerStaff/PartnerStaff/EditPartnerStaff";

        /// <summary>
        /// 既存の取引先の住所情報や新しい取引先の住所情報を表示する
        /// </summary>
        public const string BPADDRESS_NEW_OR_EDIT = "BPAddress/BPAddress/Register";

        /// <summary>
        /// 取引先の住所情報をDBに更新する
        /// </summary>
        public const string BPADDRESS_UPDATE_DB = "BPAddress/BPAddress/RegisterAddress";

        public const string PAYMENT_OR_NOT_PAYMENT = "Payment/Payment/PaymentNotPayment";

        public const string PACKAGE_CLEAR_DEPOSIT = "Payment/Payment/PackageClearDeposit";

        public const string PAYMENT_HISTORY = "Payment/Payment/PaymentHistory";

        public const string PAYMENT_HISTORY_EXPORTCSV = "Payment/Payment/PaymentHistoryExportCSV";

        public const string PAYMENT_LIST = "Payment/Payment/PaymentList";

        public const string PAYMENT_EDIT_ORDER_STATUS = "Payment/Payment/EditOrderStatus";

        public const string BILLING_EDIT_ORDER_STATUS = "Billing/Billing/EditOrderStatus";

        public const string ISSUE_ORDER_FOR_PAYMENT_SEARCH = "Payment/Payment/IssueOrderForPaymentSearch";

        public const string ISSUE_ORDER_FOR_PAYMENT = "Payment/Payment/IssueOrderForPayment";

        public const string ISSUE_DISPATCHCONTRACT = "Contract/Contract/IssueDispatchContract";

        public const string PAYMENT_NOTICE_ISSUE = "Payment/Payment/IssueNoticeForPayment";

        public const string POST_INFORMATION_LIST = "Information/Information/PostInformationList";

        public const string POST_INFORMATION_EDIT = "Information/Information/PostInformationEdit";

        public const string POST_INFORMATION_UPDATE_DB = "Information/Information/Edit";

        public const string SYSTEMSTATUS_UPDATE_DB = "Maint/SystemStatus/Edit";
        
        public const string ALL_INFORMATION_LIST = "Information/AllInformation/AllInformationList";

        public const string ALL_INFORMATION_REGISTER = "Information/AllInformation/AllInformationRegister";

        public const string ALL_INFORMATION_UPDATE_DB = "Information/AllInformation/Edit";

        public const string COVER_LETTER = "PartnerStaff/PartnerStaff/CoverLetter";

        public const string TRANS_MAINT_LIST = "Maint/TransMaint/TransMaintList";

        public const string BILLING_LIST = "Billing/Billing/BillingListIndex";
        public const string BILLING_INPUT_OPERATE_TIME = "Billing/Billing/BillingInputOperateTime";
        public const string BILLING_ISSUE_DEBIT_NOTE = "Billing/Billing/BillingIssueDebitNoteList";
        public const string BILLING_EDIT_DEBIT_NOTE = "Billing/Billing/EditDebitNote";
        public const string BILLING_INPUT_DATE = "Billing/Billing/BillingInputDate";
        public const string BILLING_SETTING = "Billing/Billing/BillingSetting";
        public const string BILLING_INPUT_PREPARE = "Billing/Billing/InputBillingPrepare";
        public const string BILLING_INPUT_PRIVATE = "Billing/Billing/BillingInputPrivate";

        public const string EXPORT_ACPAYABLE_DATACSV = "ExternalLink/ExternalLink/ExportAcPayableDataCsv";

        /// <summary>
        /// LOG 
        /// </summary>
        public const string SHOW_LOG = "Log/ShowLog/ShowLog";
	}
}
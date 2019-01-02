using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SystemSetup.Models;
using SystemSetup.UtilityServices;
using SystemSetup.UtilityServices.LogService;
using SystemSetup.UtilityServices.PagingHelper;
using SystemSetup.Constants.Resources;

namespace SystemSetup.DataAccess
{
    public class AuthorityGroupMaintDa : BaseDa
    {
        #region READ

        /// <summary>
        /// Search payment/Not payment
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<AuthorityGroupMaintModel> AuthorityGroupMaintSearch(DataTablesModel dt, ref AuthorityGroupMaintModel searchCondition, out int totalrow)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT
	                mfcc.COMPANY_CD,
                    mfcc.AUTHORITY_GROUP_CD,
                    mfcc.AUTHORITY_GROUP_NAME,
	                mfcc.UPD_DATE,
	                mcfu.CONTRACT_USER_NAME AS UPD_USERNAME
                FROM
	                Mst_AuthorityGroup mfcc
                        LEFT JOIN Mst_ContractFirmUser mcfu ON
                            mfcc.COMPANY_CD = mcfu.COMPANY_CD AND
                            mfcc.UPD_USER_ID = mcfu.CONTRACT_USER_SEQ_CD
                WHERE
                    mfcc.DEL_FLG = 0
            ");

            if (!String.IsNullOrEmpty(searchCondition.COMPANY_CD))
            {
                sql.Append(@"
                    AND mfcc.COMPANY_CD = @COMPANY_CD  ");
            }

            if (searchCondition.AUTHORITY_GROUP_CD != null)
            {
                sql.Append(@"
                    AND mfcc.AUTHORITY_GROUP_CD = @AUTHORITY_GROUP_CD  ");
            }

            if (!String.IsNullOrEmpty(searchCondition.AUTHORITY_GROUP_NAME))
            {
                sql.Append(@"
                    AND mfcc.AUTHORITY_GROUP_NAME LIKE @AUTHORITY_GROUP_NAME  ");
            }

            if (searchCondition.DISABLE_FLG == "false")
            {
                sql.Append(@"
                    AND mfcc.DISABLE_FLG = @DISABLE_FLG ");
            }

            int lower = dt.iDisplayStart + 1;
            int upper = dt.iDisplayStart + dt.iDisplayLength;

            PagingHelper.SQLParts parts;
            PagingHelper.SplitSQL(sql.ToString(), out parts);

            string sqlpage = PagingHelper.BuildPageQuery(lower, dt.iDisplayLength, parts);
            string sqlcount = parts.sqlCount;


            var dataList = base.Query<AuthorityGroupMaintModel>(sqlpage,
                new
                {
                    COMPANY_CD = searchCondition.COMPANY_CD,
                    AUTHORITY_GROUP_CD = searchCondition.AUTHORITY_GROUP_CD,
                    AUTHORITY_GROUP_NAME = '%' + searchCondition.AUTHORITY_GROUP_NAME + '%',
                    DISABLE_FLG = "0",
                    pageindex = lower,
                    pagesize = upper
                }).ToList();

            totalrow = base.Query<int>(sqlcount,
                new
                {
                    COMPANY_CD = searchCondition.COMPANY_CD,
                    AUTHORITY_GROUP_CD = searchCondition.AUTHORITY_GROUP_CD,
                    AUTHORITY_GROUP_NAME = '%' + searchCondition.AUTHORITY_GROUP_NAME + '%',
                    DISABLE_FLG = "0",
                    pageindex = lower,
                    pagesize = upper
                }).FirstOrDefault();

            return dataList;
        }

        /// <summary>
        /// Get a row from Tbl_Information based on INFO_SEQ_NO
        /// </summary>
        /// <param name="infoSeqNo"></param>
        /// <returns></returns>
        public AuthorityGroupMaintModel GetInformation(string companyCd, int AuthorityGroupCd)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
					AUTHORITY_GROUP_NAME,
                    DISABLE_FLG
                FROM
	                Mst_AuthorityGroup
                WHERE
                    COMPANY_CD = @COMPANY_CD AND
					AUTHORITY_GROUP_CD = @AUTHORITY_GROUP_CD
            ");

            AuthorityGroupMaintModel results = base.Query<AuthorityGroupMaintModel>(sql.ToString(), new
            {
                COMPANY_CD = companyCd,
                AUTHORITY_GROUP_CD = AuthorityGroupCd
            }).FirstOrDefault();

            StringBuilder sqlkey = new StringBuilder();
            sqlkey.Append(@"
                SELECT
					AUTHORITY_KEY
                FROM
	                Mst_Authority
                WHERE
                    COMPANY_CD = @COMPANY_CD AND
					AUTHORITY_GROUP_CD = @AUTHORITY_GROUP_CD AND
                    DEL_FLG = @DEL_FLG
            ");
            IEnumerable<String> keyresults = base.Query<String>(sqlkey.ToString(), new
            {
                COMPANY_CD = companyCd,
                AUTHORITY_GROUP_CD = AuthorityGroupCd,
                DEL_FLG = Constants.DeleteFlag.NON_DELETE
            });

            foreach(string key in keyresults){
                if (key == Maint.KEY_Estimate_ViewEstimate) { results.ESTIMATE_VIEWESTIMATE = true; };
                if (key == Maint.KEY_Estimate_Edit) { results.ESTIMATE_EDIT = true; };
                if (key == Maint.KEY_Estimate_IssueEstimateFile) { results.ESTIMATE_ISSUEESTIMATEFILE = true; };
                if (key == Maint.KEY_Estimate_SendEstimate) { results.ESTIMATE_SENDESTIMATE = true; };
                if (key == Maint.KEY_Estimate_Delete) { results.ESTIMATE_DELETE = true; };
                if (key == Maint.KEY_Estimate_Collection) { results.ESTIMATE_COLLECTION = true; };
                if (key == Maint.KEY_Contract_View) { results.CONTRACT_VIEW = true; };
                if (key == Maint.KEY_Contract_Edit) { results.CONTRACT_EDIT = true; };
                if (key == Maint.KEY_Contract_FinishContract) { results.CONTRACT_FINISHCONTRACT = true; };
                if (key == Maint.KEY_Contract_FinishContractDirect){ results.CONTRACT_FINISHCONTRACTDIRECT = true;};
                if (key == Maint.KEY_Contract_Delete) { results.CONTRACT_DELETE = true; };
                if (key == Maint.KEY_Contract_ExportCsvContractList) { results.CONTRACT_EXPORTCSVCONTRACTLIST = true; };
                if (key == Maint.KEY_Contract_Resend) { results.CONTRACT_RESEND = true; };

                if (key == Maint.KEY_Estimate_EstimateListContract) { results.ESTIMATE_ESTIMATELISTCONTRACT = true; };
                if (key == Maint.KEY_Contract_CancelContractEnd) { results.CONTRACT_CANCELCONTRACTEND = true; };
                if (key == Maint.KEY_Billing_CreateBillingData) { results.BILLING_CREATEBILLINGDATA = true; };
                if (key == Maint.KEY_Billing_ExportBillingDataCsv) { results.BILLING_EXPORTBILLINGDATACSV = true; };
                if (key == Maint.KEY_Billing_BillingInputOperateTime) { results.BILLING_BILLINGINPUTOPERATETIME = true; };
                if (key == Maint.KEY_Billing_BillingIssueDebitNoteList) { results.BILLING_BILLING_ISSUE_DEBIT_NOTE = true; };
                if (key == Maint.KEY_Billing_EditDebitNote) { results.BILLING_EDITDEBITNOTE = true; };
                if (key == Maint.KEY_Billing_BillingInputDate) { results.BILLING_BILLINGINPUTDATE = true; };
                if (key == Maint.KEY_Billing_BillingSetting) { results.BILLING_BILLINGSETTING = true; };
                if (key == Maint.KEY_Billing_InputBillingPrepare) { results.BILLING_INPUTBILLINGPREPARE = true; };
                if (key == Maint.KEY_Billing_DeleteDebitNote) { results.BILLING_DELETE_DEBIT_NOTE = true; };
                if (key == Maint.KEY_Billing_NoteIssueControl) { results.BILLING_NOTE_ISSUE_CONTROL = true; };

                if (key == Maint.KEY_Payment_PaymentClearSetting) { results.PAYMENT_PAYMENTCLEARSETTING = true; };
                if (key == Maint.KEY_Payment_PackageClearDeposit) { results.PAYMENT_PACKAGE_CLEAR_DEPOSIT = true; };
                if (key == Maint.KEY_Payment_CreateRedInvoice) { results.PAYMENT_CREATEREDINVOICE = true; };
                if (key == Maint.KEY_Payment_PaymentInputPrivate) { results.PAYMENT_PAYMENTINPUTPRIVATE = true; };
                if (key == Maint.KEY_Payment_EditOrderStatus) { results.PAYMENT_PAYMENTEDITORDERSTATUS = true; };
                if (key == Maint.KEY_Billing_EditOrderStatus) { results.BILLING_BILLINGEDITORDERSTATUS = true; };
                if (key == Maint.KEY_Payment_ExportSalesPaymentsDataCsv) { results.PAYMENT_EXPORTSALESPAYMENTSDATACSV = true; };
                if (key == Maint.KEY_Contract_IssuedEstimate) { results.CONTRACT_ISSUEDESTIMATE = true; };
                if (key == Maint.KEY_Contract_IssuePackageDelivery) { results.CONTRACT_ISSUEPACKAGEDELIVERY = true; };
                if (key == Maint.KEY_PartnerStaff_CoverLetter) { results.PARTNERSTAFF_COVERLETTER = true; };
                if (key == Maint.KEY_Payment_IssueOrderForPayment) { results.PAYMENT_ISSUEORDERFORPAYMENT = true; };
                if (key == Maint.KEY_Issue_Dispatch_Contract) { results.ISSUE_DISPATCHCONTRACT = true; };
                if (key == Maint.KEY_Contract_PaymentIssueDispatch) { results.ISSUE_DISPATCH_PAYMENT = true; };
                if (key == Maint.KEY_Payment_Notice_Issue) { results.PAYMENT_PAYMENT_NOTICE_ISSUE = true; };
                if (key == Maint.KEY_CustMaint_CustMaintList) { results.CUSTMAINT_CUSTMAINTLIST = true; };
                if (key == Maint.KEY_ContactPersMaint_ContactPersMaintList) { results.CONTACTPERSMAINT_CONTACTPERSMAINTLIST = true; };
                if (key == Maint.KEY_OrgMaint_OrgMaintList) { results.ORGMAINT_ORGMAINTLIST = true; };
                if (key == Maint.KEY_PersInCharMaint_PersInCharMaintList) { results.PERSINCHARMAINT_PERSINCHARMAINTLIST = true; };
                if (key == Maint.KEY_TransMaint_TransMaintList) { results.TRANSMAINT_TRANSMAINTLIST = true; };
                if (key == Maint.KEY_AccountCodeMaint_AccountCodeMaintList) { results.ACCOUNTCODEMAINT_ACCOUNTCODEMAINTLIST = true; };
                if (key == Maint.KEY_SubjectMaint_SubjectMaintList) { results.SUBJECTMAINT_SUBJECTMAINTLIST = true; };
                if (key == Maint.KEY_CategoryMaint_CategoryMaintList) { results.CATEGORYMAINT_CATEGORYMAINTLIST = true; };
                if (key == Maint.KEY_UserMaint_UserMaintList) { results.USERMAINT_USERMAINTLIST = true; };
                if (key == Maint.KEY_Information_PostInformationList) { results.INFORMATION_POSTINFORMATIONLIST = true; };
                if (key == Maint.KEY_Estimate_ViewAttachedFile) { results.ESTIMATE_VIEWATTACHEDFILE = true; };
                if (key == Maint.KEY_ShowLog_ShowLog) { results.SHOWLOG_SHOWLOG = true; };
                if (key == Maint.KEY_Proceeds_Payment_Plan) { results.PROCEEDS_PAYMENT_PLAN = true; };
                if (key == Maint.KEY_Proceeds_Payment_Confirm) { results.PROCEEDS_PAYMENT_CONFIRM = true; };
                if (key == Maint.KEY_Cash_Flow) { results.CASH_FLOW = true; };
                if (key == Maint.KEY_Sale_Actual) { results.SALE_ACTUAL = true; };
                if (key == Maint.KEY_Sale_Actual_By_Customer) { results.SALE_ACTUAL_BY_CUSTOMER = true; };
                if (key == Maint.KEY_Sale_Actual_By_Customer_Graph) { results.SALE_ACTUAL_BY_CUSTOMER_GRAPH = true; };
                if (key == Maint.KEY_Dashboard_Display) { results.DASHBOARD_DISPLAY = true; };
                if (key == Maint.KEY_AcReceivable_CsvOutput) { results.ACRECEIVABLE_CSV_OUTPUT = true; };
                if (key == Maint.KEY_AcPayable_CsvOutput) { results.ACPAYABLE_CSV_OUTPUT = true; };
                if (key == Maint.KEY_Deposit_CsvOutput) { results.DEPOSIT_CSV_OUTPUT = true; };
                if (key == Maint.KEY_Setting_AcReceivable) { results.SETTING_ACRECEIVABLE = true; };
                if (key == Maint.KEY_Setting_AcPayable) { results.SETTING_ACPAYABLE = true; };
                if (key == Maint.KEY_Setting_Deposit) { results.SETTING_DEPOSIT = true; };
                if (key == Maint.KEY_AcPayable_CsvOutput) { results.ACPAYABLE_CSV_OUTPUT = true; };
                if (key == Maint.KEY_ExternalLinkSettingAcPayable) { results.SETTING_ACPAYABLE = true; };
                if (key == Maint.KEY_Nomen_List) { results.NOMEN_LIST = true; };
                if (key == Maint.KEY_PaymentState_PaymentStateMaintList){ results.PAYMENTSTATE_PAYMENTSTATEMAINTLIST = true; }
                if (key == Maint.KEY_AuthorityGroupMaint_AuthorityGroupMaintList) { results.AUTHORITYGROUPMAINT_AUTHORITYGROUPMAINTLIST = true; }
                if (key == Maint.KEY_Contract_Plan_Confirm) { results.CONTRACT_PLAN_CONFIRM = true; };
                if (key == Maint.KEY_Lock_Out_Disable) { results.LOCK_OUT_DISABLE = true; };
                //技術者管理機能
                if (key == Maint.KEY_EngineerList) { results.MANAGE_ENGINEER_LIST = true; };
                if (key == Maint.KEY_EngineerDelete) { results.MANAGE_ENGINEER_DELETE = true; };
                if (key == Maint.KEY_EngineerCategory) { results.MANAGE_ENGINEER_CATEGORY = true; };
                if (key == Maint.KEY_EngineerUploadFile) { results.MANAGE_ENGINEER_UPLOAD_FILE = true; };

            }

            return results;
        }

        /// <summary>
        /// Get contract firm master
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractFirmEntity> GetContractFirmByContractTypeLevelExceptZero()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT * FROM Mst_ContractFirm WHERE DEL_FLG = @DEL_FLG AND CONTRACT_LEVEL_TYPE <> 0 ");
            sql.Append(" ORDER BY COMPANY_CD");

            var data = base.Query<ContractFirmEntity>(sql.ToString(),
                new
                {
                    DEL_FLG = Constants.DeleteFlag.NON_DELETE
                }).ToList();

            return data;
        }

        #endregion

        #region UPDATE
        public long UpdateAuthorityGroupMaintModel(AuthorityGroupMaintModel model)
        {
            #region update Mst_AuthorityGroup
            StringBuilder sqlupd = new StringBuilder();
            AuthorityGroupEntity entity = new AuthorityGroupEntity();

            sqlupd.Append(@"
                UPDATE [dbo].[Mst_AuthorityGroup] 
                    SET [AUTHORITY_GROUP_NAME] = @AUTHORITY_GROUP_NAME
                    ,[DISABLE_FLG] = @DISABLE_FLG
                    ,[UPD_DATE] = @UPD_DATE
                    ,[UPD_USER_ID] = @UPD_USER_ID
                    ,[UPD_PROG_ID] = @UPD_PROG_ID
                WHERE COMPANY_CD = @COMPANY_CD AND
                    AUTHORITY_GROUP_CD = @AUTHORITY_GROUP_CD
            ");

            entity.COMPANY_CD = model.COMPANY_CD;
            entity.AUTHORITY_GROUP_CD = model.AUTHORITY_GROUP_CD;
            entity.AUTHORITY_GROUP_NAME = model.AUTHORITY_GROUP_NAME;
            entity.DISABLE_FLG = model.VIEW_DISABLE_FLG ? "1" : "0";
            entity.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;

            var results = base.DbUpdateByOutside(sqlupd.ToString(), entity, new { });

            #endregion
            if (results > 0)
            {
                #region Mst_Authority
                List<string> reqkeylist = getKeyList(model);

                if (reqkeylist.Count() > 0)
                {
                    StringBuilder sqlgetkey = new StringBuilder();
                    sqlgetkey.Append(@"
                    SELECT
					    AUTHORITY_KEY
                    FROM
	                    Mst_Authority
                    WHERE
                        COMPANY_CD = @COMPANY_CD AND
					    AUTHORITY_GROUP_CD = @AUTHORITY_GROUP_CD
                ");

                    IEnumerable<String> dbkeylist = base.Query<String>(sqlgetkey.ToString(), new
                    {
                        COMPANY_CD = model.COMPANY_CD,
                        AUTHORITY_GROUP_CD = model.AUTHORITY_GROUP_CD,
                    });

                    IEnumerable<String> updatekeylist = reqkeylist.Intersect(dbkeylist);
                    IEnumerable<String> insertkeylist = reqkeylist.Except(dbkeylist);

                    if (updatekeylist.Count() > 0)
                    {
                        #region update Mst_Authority
                        StringBuilder inwhere = new StringBuilder();

                        foreach (string updkey in updatekeylist)
                        {
                            inwhere.Append("'" + updkey + "',");
                        }
                        inwhere = inwhere.Remove(inwhere.ToString().LastIndexOf(","), 1);

                        StringBuilder sqlkeyupdate = new StringBuilder();
                        sqlkeyupdate.Append(@"
                            UPDATE [dbo].[Mst_Authority] 
                                SET [DEL_FLG] = @DEL_FLG
                                ,[UPD_DATE] = @UPD_DATE
                                ,[UPD_USER_ID] = @UPD_USER_ID
                                ,[UPD_PROG_ID] = @UPD_PROG_ID
                            WHERE COMPANY_CD = @COMPANY_CD AND
                                AUTHORITY_GROUP_CD = @AUTHORITY_GROUP_CD AND
                        ");

                        string nondel_update = sqlkeyupdate.ToString() + "AUTHORITY_KEY IN(" + inwhere.ToString() + ")";

                        AuthorityEntity KeyUpdateEntity = new AuthorityEntity();
                        KeyUpdateEntity.COMPANY_CD = model.COMPANY_CD;
                        KeyUpdateEntity.AUTHORITY_GROUP_CD = model.AUTHORITY_GROUP_CD;
                        KeyUpdateEntity.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
                        KeyUpdateEntity.DEL_FLG = Constants.DeleteFlag.NON_DELETE;

                        base.DbUpdateByOutside(nondel_update, KeyUpdateEntity, new { });


                        string del_update = sqlkeyupdate.ToString() + "AUTHORITY_KEY NOT IN(" + inwhere.ToString() + ")";

                        KeyUpdateEntity.DEL_FLG = Constants.DeleteFlag.DELETE;

                        base.DbUpdateByOutside(del_update, KeyUpdateEntity, new { });
                        #endregion
                    }
                    if (insertkeylist.Count() > 0)
                    {
                        #region insert Mst_Authority
                        AuthorityEntity KeyInsertEntity = new AuthorityEntity();
                        KeyInsertEntity.COMPANY_CD = model.COMPANY_CD;
                        KeyInsertEntity.AUTHORITY_GROUP_CD = model.AUTHORITY_GROUP_CD;
                        KeyInsertEntity.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
                        KeyInsertEntity.DEL_FLG = Constants.DeleteFlag.NON_DELETE;

                        StringBuilder sqlkeyinsert = new StringBuilder();
                        sqlkeyinsert.Append(@"
                            INSERT INTO Mst_Authority
                            (
                                COMPANY_CD,
                                AUTHORITY_GROUP_CD,
                                AUTHORITY_KEY,
                                DEL_FLG,
                                INS_DATE,
                                INS_USER_ID,
                                UPD_DATE,
                                UPD_USER_ID,
                                UPD_PROG_ID
                                )VALUES
                        ");
                        foreach (string key in insertkeylist)
                        {
                            sqlkeyinsert.Append(@"
                                (                  
                                @COMPANY_CD,
                                @AUTHORITY_GROUP_CD,
                            ");
                            sqlkeyinsert.Append("'" + key + "',");
                            sqlkeyinsert.Append(@"
                                @DEL_FLG,
                                @INS_DATE,
                                @INS_USER_ID,
                                @UPD_DATE,
                                @UPD_USER_ID,
                                @UPD_PROG_ID),
                            ");
                        }
                        string sqlkeyinsertstr = sqlkeyinsert.ToString();
                        sqlkeyinsertstr = sqlkeyinsertstr.Remove(sqlkeyinsertstr.LastIndexOf(","), 1);
                        var keyresult = base.DbAddByOutside(sqlkeyinsertstr, KeyInsertEntity);

                        return keyresult;
                        #endregion
                    }
                }


                #endregion
            }

            return results;
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Delete Mst_AuthorityGroup and Mst_Authority
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int DeleteAuthorityGroupMaint(string COMPANY_CD, int AUTHORITY_GROUP_CD)
        {
            int result = 0;

            #region Delete Mst_FirmContractClass
            result = Delete(COMPANY_CD, AUTHORITY_GROUP_CD);
            #endregion

            return result;
        }

        /// <summary>
        /// Delete Mst_AuthorityGroup and Mst_Authority
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public int Delete(string COMPANY_CD, int AUTHORITY_GROUP_CD)
        {
            #region Delete Mst_AuthorityGroup
            StringBuilder sqlupdate = new StringBuilder();
            sqlupdate.Append(" UPDATE Mst_AuthorityGroup");
            sqlupdate.Append(" SET ");
            sqlupdate.Append("    DEL_FLG = @DEL_FLG,");
            sqlupdate.Append("    UPD_DATE = @UPD_DATE,");
            sqlupdate.Append("    UPD_USER_ID = @UPD_USER_ID");

            sqlupdate.Append(" WHERE    COMPANY_CD = @COMPANY_CD");
            sqlupdate.Append("  AND AUTHORITY_GROUP_CD = @AUTHORITY_GROUP_CD");

            var results = base.Execute(sqlupdate.ToString(), new
            {
                COMPANY_CD = COMPANY_CD,
                AUTHORITY_GROUP_CD = AUTHORITY_GROUP_CD,
                DEL_FLG = Constants.DeleteFlag.DELETE,
                UPD_DATE = Utility.GetCurrentDateTime(),
                UPD_USER_ID = 0
            });
            #endregion

            if (results > 0)
            {
                #region Delete Mst_Authority
                StringBuilder sqlkeyupdate = new StringBuilder();
                sqlupdate.Append(" UPDATE Mst_Authority");
                sqlupdate.Append(" SET ");
                sqlupdate.Append("    DEL_FLG = @DEL_FLG,");
                sqlupdate.Append("    UPD_DATE = @UPD_DATE,");
                sqlupdate.Append("    UPD_USER_ID = @UPD_USER_ID");

                sqlupdate.Append(" WHERE    COMPANY_CD = @COMPANY_CD");
                sqlupdate.Append("  AND AUTHORITY_GROUP_CD = @AUTHORITY_GROUP_CD");

                return base.Execute(sqlupdate.ToString(), new
                {
                    COMPANY_CD = COMPANY_CD,
                    AUTHORITY_GROUP_CD = AUTHORITY_GROUP_CD,
                    DEL_FLG = Constants.DeleteFlag.DELETE,
                    UPD_DATE = Utility.GetCurrentDateTime(),
                    UPD_USER_ID = 0
                });
                #endregion
            }
            return results;

        }

        #endregion

        #region CREATE
        /// <summary>
        /// Insert into Mst_AuthorityGroup and Mst_Authority
        /// </summary>
        /// <param name="Maint"></param>
        /// <returns></returns>
        public long InsertAuthorityGroupMaint(AuthorityGroupMaintModel model)
        {
            int result = 0;

            #region Insert into Mst_AuthorityGroup

            model.DISABLE_FLG = model.VIEW_DISABLE_FLG ? "1":"0";
            model.INS_DATE = Utility.GetCurrentDateTime();
            model.UPD_DATE = Utility.GetCurrentDateTime();

            List<String> keylist = getKeyList(model);

            string groupCdSql = "SELECT MAX(AUTHORITY_GROUP_CD) FROM [Mst_AuthorityGroup] WHERE [COMPANY_CD] = '" + model.COMPANY_CD + "'";
            var groupCd = base.ExecuteScalar<long>(groupCdSql) + 1;


            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append(@"
                INSERT INTO Mst_AuthorityGroup
                (
                    COMPANY_CD,
                    AUTHORITY_GROUP_CD,
                    AUTHORITY_GROUP_NAME,
                    DISABLE_FLG,
                    DEL_FLG,
                    INS_DATE,
                    INS_USER_ID,
                    UPD_DATE,
                    UPD_USER_ID,
                    UPD_PROG_ID
                )
                VALUES
                (
                    @COMPANY_CD,
                    @AUTHORITY_GROUP_CD,
                    @AUTHORITY_GROUP_NAME,
                    @DISABLE_FLG,
                    @DEL_FLG,
                    @INS_DATE,
                    @INS_USER_ID,
                    @UPD_DATE,
                    @UPD_USER_ID,
                    @UPD_PROG_ID
                ) ");
            //result = base.DbAddByOutside(sqlinsert.ToString(), model);
            result = base.Execute(sqlinsert.ToString(), new
            {
                COMPANY_CD = model.COMPANY_CD,
                AUTHORITY_GROUP_CD = groupCd,
                AUTHORITY_GROUP_NAME = model.AUTHORITY_GROUP_NAME,
                DISABLE_FLG = model.DISABLE_FLG,
                DEL_FLG = Constants.DeleteFlag.NON_DELETE,
                INS_DATE = model.INS_DATE,
                INS_USER_ID = 0,
                UPD_DATE = model.UPD_DATE,
                UPD_USER_ID = 0,
                UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE
            });

            #endregion

            if (result > 0)
            {
                #region Insert into Mst_Authority
                AuthorityEntity keyentity = new AuthorityEntity();
                keyentity.COMPANY_CD = model.COMPANY_CD;
                keyentity.AUTHORITY_GROUP_CD = (int)groupCd;
                keyentity.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
                keyentity.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;

                StringBuilder sqlkeyinsert = new StringBuilder();
//                sqlkeyinsert.Append(@"
//                INSERT INTO Mst_Authority
//                (
//                    COMPANY_CD,
//                    AUTHORITY_GROUP_CD,
//                    AUTHORITY_KEY,
//                    DEL_FLG,
//                    INS_DATE,
//                    INS_USER_ID,
//                    UPD_DATE,
//                    UPD_USER_ID,
//                    UPD_PROG_ID
//                )
//                VALUES
//                (
//                    @COMPANY_CD,
//                    @AUTHORITY_GROUP_CD,
//                    @AUTHORITY_KEY,
//                    @DEL_FLG,
//                    @INS_DATE,
//                    @INS_USER_ID,
//                    @UPD_DATE,
//                    @UPD_USER_ID,
//                    @UPD_PROG_ID
//                ) ");
//                foreach(string key in keylist){
//                    keyentity.AUTHORITY_KEY = key;
//                    base.DbAddByOutside(sqlkeyinsert.ToString(), keyentity);
                //}
                sqlkeyinsert.Append(@"
                INSERT INTO Mst_Authority
                (
                    COMPANY_CD,
                    AUTHORITY_GROUP_CD,
                    AUTHORITY_KEY,
                    DEL_FLG,
                    INS_DATE,
                    INS_USER_ID,
                    UPD_DATE,
                    UPD_USER_ID,
                    UPD_PROG_ID
                    )VALUES
                    ");
                foreach (string key in keylist)
                {
                    sqlkeyinsert.Append(@"
                    (                  
                    @COMPANY_CD,
                    @AUTHORITY_GROUP_CD,
                    ");
                    sqlkeyinsert.Append("'"+key+"',");
                    sqlkeyinsert.Append(@"
                    @DEL_FLG,
                    @INS_DATE,
                    @INS_USER_ID,
                    @UPD_DATE,
                    @UPD_USER_ID,
                    @UPD_PROG_ID),
                    ");
                }
                string sqlkeyinsertstr = sqlkeyinsert.ToString();
                sqlkeyinsertstr = sqlkeyinsertstr.Remove(sqlkeyinsertstr.LastIndexOf(","), 1);
                var keyresult = base.DbAddByOutside(sqlkeyinsertstr, keyentity);

                return keyresult;
                #endregion 

            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region KEYLIST
        /// <summary>
        /// KEYLIST
        /// </summary>
        /// <param name="Maint"></param>
        /// <returns></returns>
        /// 
        public List<String> getKeyList(AuthorityGroupMaintModel model)
        {
            List<String> keylist = new List<String>();

            //見積機能
            if (model.ESTIMATE_VIEWESTIMATE ||
                model.ESTIMATE_EDIT ||
                model.ESTIMATE_ISSUEESTIMATEFILE ||
                model.ESTIMATE_SENDESTIMATE ||
                model.ESTIMATE_DELETE || 
                model.ESTIMATE_COLLECTION)
            {
                keylist.Add(Maint.KEY_Estimate_EstimateList);

                if (model.ESTIMATE_VIEWESTIMATE)
                {
                    keylist.Add(Maint.KEY_Estimate_ViewEstimate);
                    keylist.Add(Maint.KEY_Estimate_ViewEstimateHistory);
                }
                if (model.ESTIMATE_EDIT)
                {
                    keylist.Add(Maint.KEY_Estimate_Edit);
                    keylist.Add(Maint.KEY_Estimate_Duplicate);
                    keylist.Add(Maint.KEY_Estimate_EditEstimate);
                }
                if (model.ESTIMATE_ISSUEESTIMATEFILE) { keylist.Add(Maint.KEY_Estimate_IssueEstimateFile); }
                if (model.ESTIMATE_SENDESTIMATE) { keylist.Add(Maint.KEY_Estimate_SendEstimate); }
                if (model.ESTIMATE_DELETE) { keylist.Add(Maint.KEY_Estimate_Delete); }
                if (model.ESTIMATE_COLLECTION)
                {
                    keylist.Add(Maint.KEY_Estimate_Collection);
                    keylist.Add(Maint.KEY_Regist_Estimate_Collection);
                    keylist.Add(Maint.KEY_Release_Estimate_Collection);

                    keylist.Add(Maint.KEY_Issue_Estimate_Collection);
                    keylist.Add(Maint.KEY_Send_Estimate_Collection);
                    keylist.Add(Maint.KEY_Issue_Order_Collection);
                    keylist.Add(Maint.KEY_Send_Order_Collection);
                    keylist.Add(Maint.KEY_Receive_Order_Collection);
                }
            }
            //契約機能
            if (model.CONTRACT_VIEW ||
                model.CONTRACT_EDIT ||
                model.CONTRACT_FINISHCONTRACT ||
                model.CONTRACT_DELETE ||
                model.CONTRACT_EXPORTCSVCONTRACTLIST ||
                model.CONTRACT_RESEND ||
                model.ESTIMATE_ESTIMATELISTCONTRACT ||
                model.CONTRACT_CANCELCONTRACTEND)
            {
                keylist.Add(Maint.KEY_Contract_List);

                if (model.CONTRACT_VIEW) { keylist.Add(Maint.KEY_Contract_View); }
                if (model.CONTRACT_EDIT)
                {
                    keylist.Add(Maint.KEY_Contract_Edit);
                    keylist.Add(Maint.KEY_Contract_EditContract);
                }
                if (model.CONTRACT_FINISHCONTRACT)
                {
                    keylist.Add(Maint.KEY_Contract_FinishContract);
                    keylist.Add(Maint.KEY_SelectMail_Index);
                    keylist.Add(Maint.KEY_SendMail_InputDestination);
                    keylist.Add(Maint.KEY_SendMail_SendMail);
                    keylist.Add(Maint.KEY_Contract_Finish);
                }
                if (model.CONTRACT_FINISHCONTRACTDIRECT)
                {
                    keylist.Add(Maint.KEY_Contract_FinishContractDirect);
                }
                if (model.CONTRACT_DELETE) { keylist.Add(Maint.KEY_Contract_Delete); }
                if (model.CONTRACT_EXPORTCSVCONTRACTLIST) 
                { 
                    keylist.Add(Maint.KEY_Contract_ExportCsvContractList);
                    keylist.Add(Maint.KEY_Contract_ExportCsvContractListData);
                }
                if (model.CONTRACT_RESEND)
                {
                    keylist.Add(Maint.KEY_Contract_Resend);
                }
                if (model.ESTIMATE_ESTIMATELISTCONTRACT)
                {
                    keylist.Add(Maint.KEY_Estimate_EstimateListContract);
                    keylist.Add(Maint.KEY_Contract_EditContract);
                }
                if (model.CONTRACT_CANCELCONTRACTEND)
                {
                    keylist.Add(Maint.KEY_Contract_CancelContractEnd);
                }
            }
            //請求機能
            if (model.BILLING_CREATEBILLINGDATA ||
                model.BILLING_EXPORTBILLINGDATACSV ||
                model.BILLING_BILLINGINPUTOPERATETIME ||
                model.BILLING_BILLING_ISSUE_DEBIT_NOTE ||
                model.BILLING_EDITDEBITNOTE ||
                model.BILLING_BILLINGINPUTDATE ||
                model.BILLING_BILLINGSETTING || model.BILLING_DELETE_DEBIT_NOTE || model.BILLING_NOTE_ISSUE_CONTROL ||
                model.BILLING_INPUTBILLINGPREPARE)
            {
                keylist.Add(Maint.KEY_Billing_BillingListIndex);

                if (model.BILLING_CREATEBILLINGDATA)
                {
                    keylist.Add(Maint.KEY_Billing_CreateBillingData);
                    keylist.Add(Maint.KEY_Billing_DownloadBillingData);
                }
                if (model.BILLING_EXPORTBILLINGDATACSV)
                {
                    keylist.Add(Maint.KEY_Billing_ExportBillingDataCsv);
                    keylist.Add(Maint.KEY_Billing_ExportBillingInfo);
                }
                if (model.BILLING_BILLINGINPUTOPERATETIME)
                {
                    keylist.Add(Maint.KEY_Billing_BillingInputOperateTime);
                    keylist.Add(Maint.KEY_Billing_BillingInputPrivate);
                    keylist.Add(Maint.KEY_Billing_BillingInputPrivateDispatch);

                }
                if (model.BILLING_BILLING_ISSUE_DEBIT_NOTE)
                {
                    keylist.Add(Maint.KEY_Billing_BillingIssueDebitNoteList);
                    keylist.Add(Maint.KEY_Billing_BillingIssueDebitNote_Download);
                }
                if (model.BILLING_EDITDEBITNOTE)
                {
                    keylist.Add(Maint.KEY_Billing_EditDebitNote);
                    keylist.Add(Maint.KEY_Billing_ChangeDebitNote);
                }
                if (model.BILLING_BILLINGINPUTDATE)
                {
                    keylist.Add(Maint.KEY_Billing_BillingInputDate);
                    //keylist.Add(Maint.KEY_Billing_BillingInputPrivate);
                }
                if (model.BILLING_BILLINGSETTING) { keylist.Add(Maint.KEY_Billing_BillingSetting); }
                if (model.BILLING_INPUTBILLINGPREPARE) { keylist.Add(Maint.KEY_Billing_InputBillingPrepare); }
                if (model.BILLING_DELETE_DEBIT_NOTE)
                {
                    keylist.Add(Maint.KEY_Billing_DeleteDebitNote);
                }
                if (model.BILLING_NOTE_ISSUE_CONTROL)
                {
                    keylist.Add(Maint.KEY_Billing_NoteIssueControl);
                }

            }
            //入金機能
            if (model.PAYMENT_PAYMENTCLEARSETTING ||
                model.PAYMENT_PACKAGE_CLEAR_DEPOSIT ||
                model.PAYMENT_CREATEREDINVOICE)
            {
                keylist.Add(Maint.KEY_Payment_PaymentNotPayment);
                keylist.Add(Maint.KEY_Payment_PaymentHistory);
                keylist.Add(Maint.KEY_Payment_PaymentHistory_ExportCSV);

                if (model.PAYMENT_PAYMENTCLEARSETTING) { keylist.Add(Maint.KEY_Payment_PaymentClearSetting); }
                if (model.PAYMENT_PACKAGE_CLEAR_DEPOSIT) { keylist.Add(Maint.KEY_Payment_PackageClearDeposit); }
                if (model.PAYMENT_CREATEREDINVOICE) { keylist.Add(Maint.KEY_Payment_CreateRedInvoice); }
            }
            //出金機能
            if (model.PAYMENT_PAYMENTINPUTPRIVATE ||
                model.PAYMENT_EXPORTSALESPAYMENTSDATACSV)
            {
                keylist.Add(Maint.KEY_Payment_PaymentList);

                if (model.PAYMENT_PAYMENTINPUTPRIVATE) { keylist.Add(Maint.KEY_Payment_PaymentInputPrivate); }
                if (model.PAYMENT_EXPORTSALESPAYMENTSDATACSV)
                {
                    keylist.Add(Maint.KEY_Payment_ExportSalesPaymentsDataCsv);
                    keylist.Add(Maint.KEY_Payment_ExportSalesPaymentsInfo);
                }
            }
            if (model.PAYMENT_PAYMENTEDITORDERSTATUS)
            {
                keylist.Add(Maint.KEY_Payment_EditOrderStatus);
            }
            if (model.BILLING_BILLINGEDITORDERSTATUS)
            {
                keylist.Add(Maint.KEY_Billing_EditOrderStatus);
            }
            //帳票出力機能
            if (model.CONTRACT_ISSUEDESTIMATE) 
            {
                keylist.Add(Maint.KEY_Contract_IssuedEstimate); 
                keylist.Add(Maint.KEY_Estimate_IssueEstimateFileFromContract); 
            }
            if (model.CONTRACT_ISSUEPACKAGEDELIVERY)
            {
                keylist.Add(Maint.KEY_Contract_IssuePackageDelivery);
                keylist.Add(Maint.KEY_Contract_IssueSlipFileFromContract);
            }
            if (model.PARTNERSTAFF_COVERLETTER) 
            {
                keylist.Add(Maint.KEY_PartnerStaff_CoverLetter);
                keylist.Add(Maint.KEY_PartnerStaff_CreateCoverLetter);
            }
            if (model.PAYMENT_ISSUEORDERFORPAYMENT) 
            {
                keylist.Add(Maint.KEY_Payment_IssueOrderForPayment); 
                keylist.Add(Maint.KEY_Payment_ExportFilePaymentOrders); 
            }

            if (model.ISSUE_DISPATCHCONTRACT)
            {
                keylist.Add(Maint.KEY_Issue_Dispatch_Contract);
                keylist.Add(Maint.KEY_Issue_Dispatch_Contract_Download);
            }

            if (model.ISSUE_DISPATCH_PAYMENT)
            {
                keylist.Add(Maint.KEY_Contract_PaymentIssueDispatch);
                keylist.Add(Maint.KEY_Contract_PaymentIssueDispatchDownload);
            }

            if (model.PAYMENT_PAYMENT_NOTICE_ISSUE)
            {
                keylist.Add(Maint.KEY_Payment_Notice_Issue);
                keylist.Add(Maint.KEY_Payment_Notice_Issue_Download); 
            }

            //マスタメンテナンス機能
            if (model.CUSTMAINT_CUSTMAINTLIST) 
            {
                keylist.Add(Maint.KEY_CustMaint_CustMaintList); 
                keylist.Add(Maint.KEY_CustMaint_CustMaintSearch);
                keylist.Add(Maint.KEY_CustMaint_Edit);
                keylist.Add(Maint.KEY_CustMaint_CustMaintEdit);
                keylist.Add(Maint.KEY_CustMaint_ImportCSV);
                keylist.Add(Maint.KEY_CustMaint_DELETE);
                keylist.Add(Maint.KEY_CustMaint_ExportCSV);
            }
            if (model.CONTACTPERSMAINT_CONTACTPERSMAINTLIST) 
            {
                keylist.Add(Maint.KEY_ContactPersMaint_ContactPersMaintList); 
                keylist.Add(Maint.KEY_ContactPersMaint_ContactPersMaintSearch);
                keylist.Add(Maint.KEY_ContactPersMaint_Edit);
                keylist.Add(Maint.KEY_ContactPersMaint_ContactPersMaintEdit);
                keylist.Add(Maint.KEY_ContactPersMaint_DELETE);
                keylist.Add(Maint.KEY_ContactPersMaint_ExportCSV);
            }
            if (model.ORGMAINT_ORGMAINTLIST) 
            {
                keylist.Add(Maint.KEY_OrgMaint_OrgMaintList); 
                keylist.Add(Maint.KEY_OrgMaint_Regist); 
            }
            if (model.PERSINCHARMAINT_PERSINCHARMAINTLIST) 
            {
                keylist.Add(Maint.KEY_PersInCharMaint_PersInCharMaintList); 
                keylist.Add(Maint.KEY_PersInCharMaint_SearchList);
                keylist.Add(Maint.KEY_PersInCharMaint_PersInCharMaintEdit);
                //keylist.Add(Maint.KEY_PersInCharMaint_PersInCharMaintListReturn);
                keylist.Add(Maint.KEY_PersInCharMaint_PersInCharMaintRegister); 
                keylist.Add(Maint.KEY_PersInCharMaint_DELETE); 
            }
            if (model.TRANSMAINT_TRANSMAINTLIST) 
            {
                keylist.Add(Maint.KEY_TransMaint_TransMaintList); 
                keylist.Add(Maint.KEY_TransMaint_TransMaintSearch);
                keylist.Add(Maint.KEY_TransMaint_TransMaintEdit);
                keylist.Add(Maint.KEY_TransMaint_Edit);
                keylist.Add(Maint.KEY_TransMaint_DELETE); 
            }
            if (model.ACCOUNTCODEMAINT_ACCOUNTCODEMAINTLIST)
            {
                keylist.Add(Maint.KEY_AccountCodeMaint_AccountCodeMaintList);
                keylist.Add(Maint.KEY_AccountCodeMaint_AccountCodeMaintEdit);
                keylist.Add(Maint.KEY_AccountCodeMaint_Edit);
                keylist.Add(Maint.KEY_AccountCodeMaint_DELETE);
            }
            if (model.SUBJECTMAINT_SUBJECTMAINTLIST) 
            {
                keylist.Add(Maint.KEY_SubjectMaint_SubjectMaintList); 
                keylist.Add(Maint.KEY_SubjectMaint_SubjectMaintSearch);
                keylist.Add(Maint.KEY_SubjectMaint_SubjectMaintEdit);
                keylist.Add(Maint.KEY_SubjectMaint_Edit);
                keylist.Add(Maint.KEY_SubjectMaint_DELETE); 
            }
            if (model.CATEGORYMAINT_CATEGORYMAINTLIST) 
            {
                keylist.Add(Maint.KEY_CategoryMaint_CategoryMaintList); 
                keylist.Add(Maint.KEY_CategoryMaint_CategoryMaintSearch);
                keylist.Add(Maint.KEY_CategoryMaint_CategoryMaintEdit);
                keylist.Add(Maint.KEY_CategoryMaint_DELETE);
                keylist.Add(Maint.KEY_CategoryMaint_EditProduct);
            }
            if (model.USERMAINT_USERMAINTLIST) 
            {
                keylist.Add(Maint.KEY_UserMaint_UserMaintList); 
                keylist.Add(Maint.KEY_UserMaint_UserMaintSearch);
                keylist.Add(Maint.KEY_UserMaint_UserMaintEdit);
                keylist.Add(Maint.KEY_UserMaint_Edit);
                keylist.Add(Maint.KEY_UserMaint_DELETE); 
            }
            if (model.INFORMATION_POSTINFORMATIONLIST) 
            {
                keylist.Add(Maint.KEY_Information_PostInformationList); 
                keylist.Add(Maint.KEY_Information_PostInformationSearch); 
                keylist.Add(Maint.KEY_Information_PostInformationEdit);
                keylist.Add(Maint.KEY_Information_Edit);
            }

            //その他
            if (model.ESTIMATE_VIEWATTACHEDFILE) { keylist.Add(Maint.KEY_Estimate_ViewAttachedFile); }
            if (model.SHOWLOG_SHOWLOG) { keylist.Add(Maint.KEY_ShowLog_ShowLog); }

            if (model.PROCEEDS_PAYMENT_PLAN)
            {
                keylist.Add(Maint.KEY_Proceeds_Payment_Plan);
                keylist.Add(Maint.KEY_Proceeds_Payment_Plan_Graph);
                keylist.Add(Maint.KEY_Proceeds_Payment_Plan_ExportCSV);

            }

            if (model.PROCEEDS_PAYMENT_CONFIRM)
            {
                keylist.Add(Maint.KEY_Proceeds_Payment_Confirm);
                keylist.Add(Maint.KEY_Proceed_Payment_Confirm_Graph);
                keylist.Add(Maint.KEY_Proceeds_Payment_Confirm_ExportCSV);
            }

            if (model.CASH_FLOW)
            {
                keylist.Add(Maint.KEY_Cash_Flow);
                keylist.Add(Maint.KEY_Cash_Flow_Graph);
                keylist.Add(Maint.KEY__CashFlow_ExportCSV);
            }

            if (model.SALE_ACTUAL)
            {
                keylist.Add(Maint.KEY_Sale_Actual);
                keylist.Add(Maint.KEY_Sale_Actual_Graph);
                keylist.Add(Maint.KEY_Sale_Actual_ExportCSV);
            }

            if (model.SALE_ACTUAL_BY_CUSTOMER)
            {
                keylist.Add(Maint.KEY_Sale_Actual_By_Customer);
                keylist.Add(Maint.KEY_Sale_Actual_By_Customer_Graph);
                keylist.Add(Maint.KEY_Sale_Actual_By_Customer_ExportCSV);
            }

            if (model.DASHBOARD_DISPLAY)
            {
                keylist.Add(Maint.KEY_Dashboard_Display);
            }

            if (model.ACRECEIVABLE_CSV_OUTPUT)
            {
                keylist.Add(Maint.KEY_AcReceivable_CsvOutput);
                keylist.Add(Maint.KEY_ExportAcReceivable_Csv);
            }

            if (model.ACPAYABLE_CSV_OUTPUT)
            {
                keylist.Add(Maint.KEY_AcPayable_CsvOutput);
                keylist.Add(Maint.KEY_ExportAcPayable_Csv);
            }

            if (model.DEPOSIT_CSV_OUTPUT)
            {
                keylist.Add(Maint.KEY_Deposit_CsvOutput);
                keylist.Add(Maint.KEY_ExportDeposit_Csv);
            }

            if (model.SETTING_ACRECEIVABLE)
            {
                keylist.Add(Maint.KEY_Setting_AcReceivable);
            }

            if (model.SETTING_ACPAYABLE)
            {
                keylist.Add(Maint.KEY_Setting_AcPayable);
            }

            if (model.SETTING_DEPOSIT)
            {
                keylist.Add(Maint.KEY_Setting_Deposit);
            }

            if (model.ACPAYABLE_CSV_OUTPUT)
            {
                keylist.Add(Maint.KEY_AcPayable_CsvOutput);
                keylist.Add(Maint.KEY_ExternalLink_ExportAcPayableDataCsv);
            }

            if (model.SETTING_ACPAYABLE)
            {
                keylist.Add(Maint.KEY_ExternalLinkSettingAcPayable);
            }

            if (model.NOMEN_LIST)
            {
                keylist.Add(Maint.KEY_Nomen_List);
                keylist.Add(Maint.KEY_Nomen_Edit);
                keylist.Add(Maint.KEY_Nomen_Delete);
                keylist.Add(Maint.KEY_Nomen_Edit_Nomen);
            }

            if (model.PAYMENTSTATE_PAYMENTSTATEMAINTLIST)
            {
                keylist.Add(Maint.KEY_PaymentState_PaymentStateMaintList);
                keylist.Add(Maint.KEY_PaymentState_EditPaymentStateList);
            }

            if (model.AUTHORITYGROUPMAINT_AUTHORITYGROUPMAINTLIST)
            {
                keylist.Add(Maint.KEY_AuthorityGroupMaint_AuthorityGroupMaintList);
                keylist.Add(Maint.KEY_AuthorityGroupMaint_AuthorityGroupMaintEdit);
                keylist.Add(Maint.KEY_AuthorityGroupMaint_Edit);
                keylist.Add(Maint.KEY_AuthorityGroupMaint_Delete);
            }

            if (model.CONTRACT_PLAN_CONFIRM)
            {
                keylist.Add(Maint.KEY_Contract_Plan_Confirm);
                keylist.Add(Maint.KEY_Send_Mail_Contact);
            }

            if (model.LOCK_OUT_DISABLE)
            {
                keylist.Add(Maint.KEY_Lock_Out_Disable);
            }
            ///技術者管理機能
            if (model.MANAGE_ENGINEER_LIST)
            {
                keylist.Add(Maint.KEY_EngineerList);
                keylist.Add(Maint.KEY_EngineerEdit);
            }
            if (model.MANAGE_ENGINEER_DELETE)
            {
                keylist.Add(Maint.KEY_EngineerDelete);
            }
            if (model.MANAGE_ENGINEER_CATEGORY)
            {
                keylist.Add(Maint.KEY_EngineerCategory);
                keylist.Add(Maint.KEY_EngineerCategoryEdit);
            }
            if (model.MANAGE_ENGINEER_UPLOAD_FILE)
            {
                keylist.Add(Maint.KEY_EngineerUploadFile);
                keylist.Add(Maint.KEY_EngineerDownloadFile);
            }

            //必須
            keylist.Add(Maint.KEY_BPAddress_Register);
            keylist.Add(Maint.KEY_BPAddress_RegisterAddress);
            keylist.Add(Maint.KEY_Information_Index);
            keylist.Add(Maint.KEY_Customer_Edit);
            keylist.Add(Maint.KEY_Customer_EditCustomer);
            keylist.Add(Maint.KEY_Customer_SearchCustomerIndex);
            keylist.Add(Maint.KEY_Customer_Index);
            keylist.Add(Maint.KEY_PartnerStaff_EditPartnerStaff);
            keylist.Add(Maint.KEY_PartnerStaff_Register);
            keylist.Add(Maint.KEY_PartnerStaff_SearchPartnerStaff);

            keylist = keylist.Distinct().ToList();
            return keylist;
        }
        #endregion

        #region DeleteBeforeCheck
        /// <summary>
        /// DeleteBeforeCheck
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool DeleteBeforeCheck(string COMPANY_CD, int AUTHORITY_GROUP_CD)
        {
            StringBuilder sql = new StringBuilder();
            // SQL発行  
            sql.Append(" SELECT CONTRACT_USER_SEQ_CD FROM Mst_ContractFirmUser ");
            sql.Append(" WHERE COMPANY_CD = @COMPANY_CD");
            sql.Append(" AND AUTHORITY_GROUP_CD = @AUTHORITY_GROUP_CD");
            sql.Append(" AND DEL_FLG = @DEL_FLG");

            var cus = base.Query<string>(sql.ToString(), new
            {
                COMPANY_CD = COMPANY_CD,
                AUTHORITY_GROUP_CD = AUTHORITY_GROUP_CD,
                DEL_FLG = Constants.DeleteFlag.NON_DELETE
            });

            if (cus.Count() != 0)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SystemSetup.DataAccess;
using SystemSetup.Models;

namespace SystemSetup.BusinessServices
{
    public class DebitNoteMaintService: BaseServices
    {
        /// <summary>
        /// Search debit note format
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<DebitNoteFormatModel> DebitNoteFormatSearch(DataTablesModel dt, ref DebitNoteFormatModel searchCondition, out int totalrow)
        {
            // Declare new DataAccess object
            DebitNoteMaintDa dataAccess = new DebitNoteMaintDa();
            IEnumerable<DebitNoteFormatModel> results = dataAccess.DebitNoteFormatSearch(dt, ref searchCondition, out totalrow);

            if (results == null)
            {
                base.CmnEntityModel.ErrorMsgCd = Constants.MessageCd.W0015;
            }
            else
            {
                base.CmnEntityModel.ErrorMsgCd = string.Empty;
            }

            return results;
        }

        public int UpdateDebitNote(DebitNoteFormatModel model)
        {
            // Declare new DataAccess object
            DebitNoteMaintDa dataAccess = new DebitNoteMaintDa();
            int result = 1;
            using (var transaction = new TransactionScope())
            {
                try
                {
                    foreach (var debitNote in model.DEBIT_NOTE_FORMAT_LIST)
                    {
                        if (debitNote.BILLING_ADD_FORMAT_SEQ_NO > 0)
                        {
                            DebitNoteFormatEntity debit = new DebitNoteFormatEntity();
                            debit.BILLING_ADD_FORMAT_SEQ_NO = debitNote.BILLING_ADD_FORMAT_SEQ_NO;
                            debit.BILLING_FORMAT_DISP_NAME = debitNote.BILLING_FORMAT_DISP_NAME;
                            debit.BILLING_FORMAT_PATH = debitNote.BILLING_FORMAT_PATH !=null?  debitNote.BILLING_FORMAT_PATH.Trim():"";
                            debit.BILLING_FORMAT_DETAIL_LINE = debitNote.BILLING_FORMAT_DETAIL_LINE;
                            debit.DEL_FLG = debitNote.DEL_FLG == null ? Constants.DeleteFlag.NON_DELETE : Constants.DeleteFlag.DELETE;
                            debit.DISABLE_FLG = debitNote.DISABLE_FLG == null ? Constants.DisableFlag.Enable : Constants.DisableFlag.Disable;
                            debit.UPD_PROG_ID = "0";
                            debit.UPD_USER_ID = base.CmnEntityModel.UserSegNo;
                            result = dataAccess.UpdateDebitNote(debit);
                            if (result <= 0)
                                return result;
                        }
                        else if (debitNote.BILLING_ADD_FORMAT_SEQ_NO == 0 && debitNote.DEL_FLG == null)
                        {
                            DebitNoteFormatEntity debit = new DebitNoteFormatEntity();
                            debit.COMPANY_CD = model.SEARCH_COMPANY_CD;
                            debit.BILLING_TYPE = debitNote.BILLING_TYPE;
                            debit.BILLING_FORMAT_DISP_NAME = debitNote.BILLING_FORMAT_DISP_NAME;
                            debit.BILLING_FORMAT_PATH = debitNote.BILLING_FORMAT_PATH != null ? debitNote.BILLING_FORMAT_PATH.Trim() : "";
                            debit.BILLING_FORMAT_DETAIL_LINE = debitNote.BILLING_FORMAT_DETAIL_LINE;
                            debit.DEL_FLG = debitNote.DEL_FLG == null ? Constants.DeleteFlag.NON_DELETE : Constants.DeleteFlag.DELETE;
                            debit.DISABLE_FLG = debitNote.DISABLE_FLG == null ? Constants.DisableFlag.Enable : Constants.DisableFlag.Disable;
                            debit.INS_USER_ID = base.CmnEntityModel.UserSegNo;

                            result = dataAccess.InsertDebitNote(debit);
                            if (result <= 0)
                                return result;
                        }
                    }
                    if (result > 0)
                        transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    result = -1;
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    transaction.Dispose();
                }
            }
            return result;
        }
    }
}

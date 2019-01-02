using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SystemSetup.Models;
using SystemSetup.Constants;
using SystemSetup.DataAccess;
namespace SystemSetup.BusinessServices
{
    public class DispatchContractOverheadServices : BaseServices
    {
        #region Dispatch Contract Overhead
        /// <summary>
        /// Search Dispatch Contract Overhead
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<DispatchContractOverheadModel> DispatchContractOverheadSearch(DataTablesModel dt, ref DispatchContractOverheadModel searchCondition, out int totalrow)
        {
            var dataAccess = new DispatchContractOverheadDa();
            IEnumerable<DispatchContractOverheadModel> result = dataAccess.DispatchContractOverheadSearch(dt, ref searchCondition, out totalrow);

            if (result == null)
            {
                base.CmnEntityModel.ErrorMsgCd = MessageCd.W0015;
            }
            else
            {
                base.CmnEntityModel.ErrorMsgCd = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// Update Dispatch Contract Overhead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long UpdateDispatchContractOverhead(DispatchContractOverheadModel model)
        {
            DispatchContractOverheadDa dataAccess = new DispatchContractOverheadDa();
            long result = 1;
            using (var transaction = new TransactionScope())
            {
                try
                {
                    foreach (var Overhead in model.DISPATCH_CONTRACT_OVERHEAD_LIST)
                    {
                        if (Overhead.FORMAT_SEQ_NO > 0)
                        {
                            DispatchContractOverheadEntity dispatchContract = new DispatchContractOverheadEntity();
                            dispatchContract.FORMAT_SEQ_NO = Overhead.FORMAT_SEQ_NO;
                            dispatchContract.FORMAT_DISP_NAME = Overhead.FORMAT_DISP_NAME;
                            dispatchContract.FORMAT_PATH = string.IsNullOrEmpty(Overhead.FORMAT_PATH) ? "" : Overhead.FORMAT_PATH.Trim();
                            dispatchContract.DEL_FLG = Overhead.DEL_FLG == null ? DeleteFlag.NON_DELETE : DeleteFlag.DELETE ;
                            dispatchContract.DISABLE_FLG = Overhead.DISABLE_FLG == null ? DisableFlag.Enable : DisableFlag.Disable;
                            dispatchContract.UPD_PROG_ID = "0";
                            dispatchContract.UPD_USER_ID = base.CmnEntityModel.UserSegNo;

                            result = dataAccess.UpdateDispatchContractOverhead(dispatchContract);

                            if (result <= 0)
                            {
                                return result;
                            }
                        }
                        else if (Overhead.FORMAT_SEQ_NO == 0 && Overhead.DEL_FLG != "on")
                        {
                            DispatchContractOverheadEntity dispatchContract = new DispatchContractOverheadEntity();
                            dispatchContract.COMPANY_CD = model.SEARCH_COMPANY_CD;
                            dispatchContract.FORMAT_SEQ_NO = Overhead.FORMAT_SEQ_NO;
                            dispatchContract.FORMAT_DISP_NAME = Overhead.FORMAT_DISP_NAME;
                            dispatchContract.FORMAT_PATH = string.IsNullOrEmpty(Overhead.FORMAT_PATH) ? "" : Overhead.FORMAT_PATH.Trim();
                            dispatchContract.FORMAT_TYPE = Constants.FormatType.DispatchType;
                            dispatchContract.FORMAT_SUB_TYPE = Constants.FormatSubType.Billing;
                            dispatchContract.FORMAT_DETAIL_LINE = FormatType.FORMAT_DETAIL_LINE;
                            dispatchContract.DEL_FLG = Overhead.DEL_FLG == null ? DeleteFlag.NON_DELETE : DeleteFlag.DELETE;
                            dispatchContract.DISABLE_FLG = Overhead.DISABLE_FLG == null ? DisableFlag.Enable : DisableFlag.Disable;
                            dispatchContract.INS_USER_ID = base.CmnEntityModel.UserSegNo;

                            result = dataAccess.InsertDispatchContractOverhead(dispatchContract);

                            if (result <= 0)
                            {
                                return result;
                            }
                        }
                    }
                    if (result > 0)
                    {
                        transaction.Complete();
                    }
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
        #endregion

        #region Payment Dispatch Contract Overhead
        /// <summary>
        /// Search Payment Dispatch Contract Overhead
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<DispatchContractOverheadModel> PaymentDispatchContractOverheadSearch(DataTablesModel dt, ref DispatchContractOverheadModel searchCondition, out int totalrow)
        {
            var dataAccess = new DispatchContractOverheadDa();
            IEnumerable<DispatchContractOverheadModel> result = dataAccess.PaymentDispatchContractOverheadSearch(dt, ref searchCondition, out totalrow);

            if (result == null)
            {
                base.CmnEntityModel.ErrorMsgCd = MessageCd.W0015;
            }
            else
            {
                base.CmnEntityModel.ErrorMsgCd = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// Update Payment Dispatch Contract Overhead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long UpdatePaymentDispatchContractOverhead(DispatchContractOverheadModel model)
        {
            DispatchContractOverheadDa dataAccess = new DispatchContractOverheadDa();
            long result = 1;
            using (var transaction = new TransactionScope())
            {
                try
                {
                    foreach (var Overhead in model.DISPATCH_CONTRACT_OVERHEAD_LIST)
                    {
                        if (Overhead.FORMAT_SEQ_NO > 0)
                        {
                            DispatchContractOverheadEntity dispatchContract = new DispatchContractOverheadEntity();
                            dispatchContract.FORMAT_SEQ_NO = Overhead.FORMAT_SEQ_NO;
                            dispatchContract.FORMAT_DISP_NAME = Overhead.FORMAT_DISP_NAME;
                            dispatchContract.FORMAT_PATH = string.IsNullOrEmpty(Overhead.FORMAT_PATH) ? "" : Overhead.FORMAT_PATH.Trim();
                            dispatchContract.DEL_FLG = Overhead.DEL_FLG == null ? DeleteFlag.NON_DELETE : DeleteFlag.DELETE;
                            dispatchContract.DISABLE_FLG = Overhead.DISABLE_FLG == null ? DisableFlag.Enable : DisableFlag.Disable;
                            dispatchContract.UPD_PROG_ID = "0";
                            dispatchContract.UPD_USER_ID = base.CmnEntityModel.UserSegNo;

                            result = dataAccess.UpdatePaymentDispatchContractOverhead(dispatchContract);

                            if (result <= 0)
                            {
                                return result;
                            }
                        }
                        else if (Overhead.FORMAT_SEQ_NO == 0 && Overhead.DEL_FLG != "on")
                        {
                            DispatchContractOverheadEntity dispatchContract = new DispatchContractOverheadEntity();
                            dispatchContract.COMPANY_CD = model.SEARCH_COMPANY_CD;
                            dispatchContract.FORMAT_SEQ_NO = Overhead.FORMAT_SEQ_NO;
                            dispatchContract.FORMAT_DISP_NAME = Overhead.FORMAT_DISP_NAME;
                            dispatchContract.FORMAT_PATH = string.IsNullOrEmpty(Overhead.FORMAT_PATH) ? "" : Overhead.FORMAT_PATH.Trim();
                            dispatchContract.FORMAT_TYPE = Constants.FormatType.DispatchType;
                            dispatchContract.FORMAT_SUB_TYPE = Constants.FormatSubType.Payment;
                            dispatchContract.FORMAT_DETAIL_LINE = FormatType.FORMAT_DETAIL_LINE;
                            dispatchContract.DEL_FLG = Overhead.DEL_FLG == null ? DeleteFlag.NON_DELETE : DeleteFlag.DELETE;
                            dispatchContract.DISABLE_FLG = Overhead.DISABLE_FLG == null ? DisableFlag.Enable : DisableFlag.Disable;
                            dispatchContract.INS_USER_ID = base.CmnEntityModel.UserSegNo;

                            result = dataAccess.InsertPaymentDispatchContractOverhead(dispatchContract);

                            if (result <= 0)
                            {
                                return result;
                            }
                        }
                    }
                    if (result > 0)
                    {
                        transaction.Complete();
                    }
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
        #endregion
    }
}

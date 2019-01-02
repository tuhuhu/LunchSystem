using SystemSetup.DataAccess;
using SystemSetup.DataAccess.Common;
using SystemSetup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SystemSetup.UtilityServices;

namespace SystemSetup.BusinessServices
{
    public class LunchServices : BaseServices
    {
        #region CREATE
        /// <summary>
        /// Insert into Information
        /// </summary>
        /// <param name="informationModel"></param>
        /// <returns></returns>
        public long InsertInformation(InformationEntity informationModel)
        {
            var informationDa = new InformationDa();

            return informationDa.InsertInformation(informationModel);
        }
        #endregion

        #region READ
        /// <summary>
        /// Get a row from Information based on INFO_SEQ_NO
        /// </summary>
        /// <param name="infoSeqNo"></param>
        /// <returns></returns>
        public LunchInformationEntity GetLunchInformation(long infoSeqNo)
        {
            // Declare new DataAccess object
            LunchDa dataAccess = new LunchDa();
            LunchInformationEntity result = dataAccess.GetLunchInformation(infoSeqNo);
            base.CmnEntityModel.ErrorMsgCd = (result == null) ? Constants.MessageCd.W0015 : String.Empty;
            return result;
        }

        public IList<LunchMenuEntity> GetMealListToday()
        {
            // Declare new DataAccess object
            LunchDa dataAccess = new LunchDa();
            IList<LunchMenuEntity> results = dataAccess.GetMealListToday();
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

        public long RegisterMeal(LunchInformationEntity meal)
        {
            // Declare new DataAccess object
            LunchDa dataAccess = new LunchDa();
            long result = 0;

            using (var transaction = new TransactionScope())
            {
                // Get customer info by Customer code
                result = dataAccess.RegisterMeal(meal);

                if (result > 0)
                    transaction.Complete();
            }

            if (result == 0)
            {
                base.CmnEntityModel.ErrorMsgCd = Constants.MessageCd.W0015;
            }
            else
            {
                base.CmnEntityModel.ErrorMsgCd = string.Empty;
            }

            return result;
        }

        public long GetBalance()
        {
            // Declare new DataAccess object
            LunchDa dataAccess = new LunchDa();
            return dataAccess.GetBalance();
        }

        public void SendMail()
        {
            // Declare new DataAccess object
            LunchDa dataAccess = new LunchDa();
            string mailbody = string.Empty;
            mailbody = dataAccess.GetMailBody();
            int result;
            result = SystemSetup.UtilityServices.MailUtility.SendMail(
                            "nguyendoan200887@gmail.com",
                            "votruonggiang2401@gmail.com",
                            "",
                            "Công ty i-enter Asia Tầng 9 IPH đặt cơm trưa",
                            mailbody
                        );
        }

        public IEnumerable<LunchListModel> LunchSearch(DataTablesModel dt, ref LunchListModel searchCondition, out int totalrow)
        {
            // Declare new DataAccess object
            LunchDa dataAccess = new LunchDa();
            IEnumerable<LunchListModel> results = dataAccess.LunchSearch(dt, ref searchCondition, out totalrow);

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

        public IEnumerable<PaymentListModel> PaymentSearch(DataTablesModel dt, ref PaymentListModel searchCondition, out int totalrow)
        {
            // Declare new DataAccess object
            LunchDa dataAccess = new LunchDa();
            IEnumerable<PaymentListModel> results = dataAccess.PaymentSearch(dt, ref searchCondition, out totalrow);

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


        public long UpdatePaymentClear(PaymentListModel paymentClear)
        {
            LunchDa dataAccess = new LunchDa();
            long result = 0;

            if(CmnEntityModel.UserType == 0)
            {
                using (var transaction = new TransactionScope())
                {
                    try
                    {
                        //Update TblBillingDeposit
                        foreach (var paymentItem in paymentClear.PAYMENT_LIST)
                        {
                            result = dataAccess.UpdatePayment(paymentItem);
                            if (result <= 0)
                            {
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
            }            

            return result;
        }    

        public int DeleteLunchDeal(long LUNCH_SEQ_CD = 0)
        {
            int result = 0;
            // Declare new DataAccess object
            LunchDa dataAccess = new LunchDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.DeleteLunchDeal(LUNCH_SEQ_CD);

                if (result > 0)
                    transaction.Complete();
            }

            if (result == 0)
            {
                base.CmnEntityModel.ErrorMsgCd = Constants.MessageCd.W0015;
            }
            else
            {
                base.CmnEntityModel.ErrorMsgCd = string.Empty;
            }

            return result;
        }

        public int ApproveLunchDeal()
        {
            int result = 0;
            // Declare new DataAccess object
            LunchDa dataAccess = new LunchDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.ApproveLunchDeal();

                if (result > 0)
                    transaction.Complete();
            }

            if (result == 0)
            {
                base.CmnEntityModel.ErrorMsgCd = Constants.MessageCd.W0015;
            }
            else
            {
                base.CmnEntityModel.ErrorMsgCd = string.Empty;
            }

            return result;
        }

        /// <summary>
        ///  Get Group Master
        /// </summary>
        /// <returns></returns>
        public IList<InformationEntity> GetInformation(string companyCd)
        {
            // Declare new DataAccess object
            InformationDa dataAccess = new InformationDa();
            IList<InformationEntity> results = dataAccess.GetInformation(companyCd);
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

        /// <summary>
        /// Post Information Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<InformationEntityPlus> PostInformationSearch(DataTablesModel dt, ref PostInformationListModel searchCondition, out int totalrow)
        {
            // Declare new DataAccess object
            InformationDa dataAccess = new InformationDa();
            IEnumerable<InformationEntityPlus> results = dataAccess.PostInformationSearch(dt, ref searchCondition, out totalrow);

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
        #endregion

        #region UPDATE
        public long UpdateInformation(InformationEntity informationModel)
        {
            var informationDa = new InformationDa();

            return informationDa.UpdateInformation(informationModel);
        }
        #endregion

        #region DELETE
        #endregion
    }
}

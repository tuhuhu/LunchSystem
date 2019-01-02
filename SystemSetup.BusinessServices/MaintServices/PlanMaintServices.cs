using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemSetup.Models;
using SystemSetup.DataAccess;
using System.Transactions;

namespace SystemSetup.BusinessServices
{
    public class PlanMaintServices : BaseServices
    {
        #region READ
        /// <summary>
        /// Post Information Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<PlanMaintModel> PlanMaintSearch(DataTablesModel dt, ref PlanMaintModel searchCondition, out int totalrow)
        {
            // Declare new DataAccess object
            PlanMaintDa dataAccess = new PlanMaintDa();
            IEnumerable<PlanMaintModel> results = dataAccess.PlanMaintSearch(dt, ref searchCondition, out totalrow);

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
        /// Get a row from Tbl_Information based on INFO_SEQ_NO
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public PlanMaintModel GetPlanMaint(String infoSeqNo)
        {
            //Declare new DataAccess object
            PlanMaintDa dataAccess = new PlanMaintDa();
            PlanMaintModel result = dataAccess.GetInformation(infoSeqNo);
            base.CmnEntityModel.ErrorMsgCd = (result == null) ? Constants.MessageCd.W0015 : String.Empty;
            return result;
        }

        #endregion

        #region UPDATE
        public long UpdatePlanMaint(PlanMaintEntity model)
        {
            long result = 0;
            // Declare new DataAccess object
            PlanMaintDa dataAccess = new PlanMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.UpdatePlanMaintModel(model);

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
        #endregion

        #region DELETE
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SEQNO"></param>
        /// <returns></returns>
        public int DeletePlanMaint(String infoSeqNo)
        {
            int result = 0;
            // Declare new DataAccess object
            PlanMaintDa dataAccess = new PlanMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.Delete(infoSeqNo);

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
        #endregion

        #region CREATE
        /// <summary>
        /// Insert into Tbl_Information
        /// </summary>
        /// <param name="Maint"></param>
        /// <returns></returns>
        public long InsertPlanMaint(PlanMaintEntity Maint)
        {
            long result = 0;
            // Declare new DataAccess object
            PlanMaintDa dataAccess = new PlanMaintDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.InsertPlanMaint(Maint);

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
        #endregion

        /// <summary>
        ///   GetAuxiliaryCode
        /// </summary>
        /// <returns></returns>
        public IList<PlanMaintModel> GetAuxiliaryCode(string mpPlanCd, string mpPlanName)
        {
            // Declare new DataAccess object
            PlanMaintDa dataAccess = new PlanMaintDa();
            IList<PlanMaintModel> results = dataAccess.GetAuxiliaryCode(mpPlanCd, mpPlanName);
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

        #region Check exist PLAN_CD
        /// <summary>
        /// Check exist PLAN_CD
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public bool CheckExist(PlanMaintEntity model)
        {
            PlanMaintDa dataAccess = new PlanMaintDa();
            return dataAccess.CheckExist(model);
        }
        #endregion

        #region DeleteBeforeCheck
        /// <summary>
        /// DeleteBeforeCheck
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public bool DeleteBeforeCheck(String PLAN_SEQ_NO)
        {
            PlanMaintDa dataAccess = new PlanMaintDa();
            return dataAccess.DeleteBeforeCheck(PLAN_SEQ_NO);
        }
        #endregion

    }
}

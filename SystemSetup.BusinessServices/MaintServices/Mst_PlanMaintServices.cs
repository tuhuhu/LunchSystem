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
    public class Mst_PlanMaintServices : BaseServices
    {
        #region READ
        /// <summary>
        /// Post Information Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<Mst_PlanModel> Mst_PlanMaintSearch(DataTablesModel dt, ref Mst_PlanModel searchCondition, out int totalrow)
        {
            // Declare new DataAccess object
            Mst_PlanDa dataAccess = new Mst_PlanDa();
            IEnumerable<Mst_PlanModel> results = dataAccess.Mst_PlanMaintSearch(dt, ref searchCondition, out totalrow);

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
        public Mst_PlanModel GetMst_PlanMaint(String infoSeqNo)
        {
            //Declare new DataAccess object
            Mst_PlanDa dataAccess = new Mst_PlanDa();
            Mst_PlanModel result = dataAccess.GetInformation(infoSeqNo);
            base.CmnEntityModel.ErrorMsgCd = (result == null) ? Constants.MessageCd.W0015 : String.Empty;
            return result;
        }

        #endregion

        #region UPDATE
        public long UpdateMst_PlanMaint(Mst_PlanEntity model)
        {
            long result = 0;
            // Declare new DataAccess object
            Mst_PlanDa dataAccess = new Mst_PlanDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.UpdateMst_PlanModel(model);

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
        public int DeleteMst_PlanMaint(String infoSeqNo)
        {
            int result = 0;
            // Declare new DataAccess object
            Mst_PlanDa dataAccess = new Mst_PlanDa();

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
        public long InsertMst_PlanMaint(Mst_PlanEntity Maint)
        {
            long result = 0;
            // Declare new DataAccess object
            Mst_PlanDa dataAccess = new Mst_PlanDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.InsertMst_PlanMaint(Maint);

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
        public IList<Mst_PlanModel> GetAuxiliaryCode(string mstPlanCd, string mstPlanName)
        {
            // Declare new DataAccess object
            Mst_PlanDa dataAccess = new Mst_PlanDa();
            IList<Mst_PlanModel> results = dataAccess.GetAuxiliaryCode(mstPlanCd, mstPlanName);
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
        public bool CheckExist(Mst_PlanEntity model)
        {
            Mst_PlanDa dataAccess = new Mst_PlanDa();
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
            Mst_PlanDa dataAccess = new Mst_PlanDa();
            return dataAccess.DeleteBeforeCheck(PLAN_SEQ_NO);
        }
        #endregion

    }
}

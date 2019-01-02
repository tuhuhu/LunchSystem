using SystemSetup.DataAccess;
using SystemSetup.DataAccess.Common;
using SystemSetup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SystemSetup.BusinessServices
{
    public class AllInformationServices : BaseServices
    {
        #region CREATE
        /// <summary>
        /// Insert into AllInformation
        /// </summary>
        /// <param name="allinformationModel"></param>
        /// <returns></returns>
        public long InsertAllInformation(AllInformationEntity informationModel)
        {
            var informationDa = new InformationDa();

            return informationDa.InsertAllInformation(informationModel);
        }
        #endregion

        #region READ
        /// <summary>
        /// Get a row from AllInformation based on INFO_SEQ_NO
        /// </summary>
        /// <param name="infoSeqNo"></param>
        /// <returns></returns>
        public AllInformationListModel GetAllInformation(long infoSeqNo)
        {
            // Declare new DataAccess object
            InformationDa dataAccess = new InformationDa();
            AllInformationListModel result = dataAccess.GetAllInformation(infoSeqNo);
            base.CmnEntityModel.ErrorMsgCd = (result == null) ? Constants.MessageCd.W0015 : String.Empty;
            return result;
        }

        #region UPDATE
        public long UpdateAllInformation(AllInformationEntity model)
        {
            long result = 0;
            // Declare new DataAccess object
            InformationDa dataAccess = new InformationDa();

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.UpdateAllInformationModel(model);

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
        /// AllInformation Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="searchCondition"></param>
        /// <param name="totalrow"></param>
        /// <returns></returns>
        public IEnumerable<AllInformationListModel> AllInformationSearch(DataTablesModel dt, ref AllInformationListModel searchCondition, out int totalrow)
        {
            // Declare new DataAccess object
            InformationDa dataAccess = new InformationDa();
            IEnumerable<AllInformationListModel> results = dataAccess.AllInformationSearch(dt, ref searchCondition, out totalrow);

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

        #region Check exist INFO_SEQ_NO
        /// <summary>
        /// Check exist INFO_SEQ_NO
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public bool CheckExist(AllInformationEntity model)
        {
            InformationDa dataAccess = new InformationDa();
            return dataAccess.CheckExist(model);
        }
        #endregion

        #region DELETE
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SEQNO"></param>
        /// <returns></returns>
        public int DeleteAllInformation(String infoSeqNo)
        {
            int result = 0;
            // Declare new DataAccess object
            InformationDa dataAccess = new InformationDa();

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
    }
}

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
    public class InformationServices : BaseServices
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
        public InformationEntityPlus GetInformation(long infoSeqNo)
        {
            // Declare new DataAccess object
            InformationDa dataAccess = new InformationDa();
            InformationEntityPlus result = dataAccess.GetInformation(infoSeqNo);
            base.CmnEntityModel.ErrorMsgCd = (result == null) ? Constants.MessageCd.W0015 : String.Empty;
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

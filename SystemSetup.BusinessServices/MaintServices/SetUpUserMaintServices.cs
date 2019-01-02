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
    public class SetUpUserMaintServices : BaseServices
    {

        #region READ

        public SetUpUserMaintModel GetSetUpUserMaint(long infoSeqNo)
        {
            // Declare new DataAccess object
            SetUpUserMaintDa dataAccess = new SetUpUserMaintDa();
            SetUpUserMaintModel result = dataAccess.GetSetUpUserMaintInformation(infoSeqNo);
            base.CmnEntityModel.ErrorMsgCd = (result == null) ? Constants.MessageCd.W0015 : String.Empty;


            return result;
        }


        public IEnumerable<SetUpUserMaintEntityPlus> SetUpUserMaintSearch(DataTablesModel dt, ref SetUpUserMaintListModel searchCondition, out int totalrow)
        {
            // Declare new DataAccess object
            SetUpUserMaintDa dataAccess = new SetUpUserMaintDa();
            IEnumerable<SetUpUserMaintEntityPlus> results = dataAccess.SetUpUserMaintSearch(dt, ref searchCondition, out totalrow);

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

        #region CREATE

        /// <summary>
        /// インサート
        /// </summary>
        /// <param name="SetUpUserMaint"></param>
        /// <returns></returns>
        public long InsertSetUpUserMaint(SetUpUserMaintEntity SetUpUserMaint)
        {
            var SetUPUserMaintDa = new SetUpUserMaintDa();

            long result = 0;

            SetUpUserMaintDa dataAccess = new SetUpUserMaintDa();

            using (var transaction = new TransactionScope())
            {
                result = dataAccess.InsertSetUpUserMaint(SetUpUserMaint);

                if (result > 0)
                {
                    transaction.Complete();
                }
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

        #region UPDATE

        /// <summary>
        /// アップデート
        /// </summary>
        /// <param name="SetUpUserMaint"></param>
        /// <returns></returns>
        public long UpdateSetUpUserMaint(SetUpUserMaintEntity SetUpUserMaint)
        {
            var SetUpUserMaintDa = new SetUpUserMaintDa();

            long result = 0;

            SetUpUserMaintDa dataAccess = new SetUpUserMaintDa();

            using (var transaction = new TransactionScope())
            {
                result = dataAccess.UpdateSetUpUserMaint(SetUpUserMaint);

                if (result > 0)
                    transaction.Complete();
            }

            if(result == 0)
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
        /// パスワードアップロード
        /// </summary>
        /// <param name="SetUpUserMaint"></param>
        /// <returns></returns>
        public long UpdatePassword(SetUpUserMaintEntity SetUpUserMaint)
        {
            var UserMaintDa = new SetUpUserMaintDa();

            long result = 0;

            SetUpUserMaintDa dateAccess = new SetUpUserMaintDa();

            using (var transaction = new TransactionScope())
            {
                result = dateAccess.UpdatePassword(SetUpUserMaint);

                if (result > 0)
                    transaction.Complete();
            }

            if(result == 0)
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

        public int DeleteUserMaint(long SEQNO)
        {
            int result = 0;

            SetUpUserMaintDa dateAccess = new SetUpUserMaintDa();

            using(var transaction = new TransactionScope())
            {

                result = dateAccess.DeleteSetUpUserMaint(SEQNO);

                if(result > 0)
                {
                    transaction.Complete();
                }
            }

            if(result == 0)
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
        public IList<SetUpUserMaintModel> GetAuxiliaryCode(string companyCd, string SeqNo, string Account, string Del)
        {
            // Declare new DataAccess object
            SetUpUserMaintDa dataAccess = new SetUpUserMaintDa();
            IList<SetUpUserMaintModel> results = dataAccess.GetAuxiliaryCode(companyCd, SeqNo, Account, Del);
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

    }
}

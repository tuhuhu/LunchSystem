using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemSetup.Models;
using SystemSetup.DataAccess;
using System.Transactions;
using SystemSetup.UtilityServices;

namespace SystemSetup.BusinessServices
{
    public class SystemStatusServices : BaseServices
    {
        /// <summary>
        /// Get a row from Tbl_Information based on INFO_SEQ_NO
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public SystemStatusModel GetSystemStatus(string opertionMode)
        {
            // Declare new DataAccess object
            SystemStatusDa dataAccess = new SystemStatusDa();
            SystemStatusModel result = dataAccess.GetInformation(opertionMode);

            base.CmnEntityModel.ErrorMsgCd = (result == null) ? Constants.MessageCd.W0015 : String.Empty;
            return result;
        }
       
        #region CREATE
        /// <summary>
        /// Insert into SystemStatus
        /// </summary>
        /// <param name="Maint"></param>
        /// <returns></returns>
        public long InsertSystemStatus()
        {
            long result = 0;
            // Declare new DataAccess object
            SystemStatusDa dataAccess = new SystemStatusDa();

            SystemStatusEntity model = new SystemStatusEntity();

            model.SYSTEM_OPERATION_MODE = "0";
            model.SYSTEM_STOPPED_MESSAGE = "";
            model.NOTICE_FLG = 0;
            model.NOTICE_TITLE = "";
            model.NOTICE_MESSAGE = "";
            model.DEL_FLG = "0";
            model.INS_DATE = Utility.GetCurrentDateTime();
            model.INS_USER_ID = 0;
            model.UPD_DATE = Utility.GetCurrentDateTime();
            model.UPD_USER_ID = 0;
            model.UPD_PROG_ID = "0";

            using (var transaction = new TransactionScope())
            {
                // Update issue flag
                result = dataAccess.InsertSystemStatus(model);

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

        #region UPDATE
        public long UpdateSystemStatus(SystemStatusEntity model)
        {
            var systemstatusDa = new SystemStatusDa();
            
            long result = 0;
            // Declare new DataAccess object
            SystemStatusDa dataAccess = new SystemStatusDa();

            model.SYSTEM_STOPPED_MESSAGE = model.SYSTEM_STOPPED_MESSAGE ?? "";
            model.NOTICE_TITLE = model.NOTICE_TITLE ?? "";
            model.NOTICE_MESSAGE = model.NOTICE_MESSAGE ?? "";
            model.DEL_FLG = "0";

            using (var transaction = new TransactionScope())
            {
                // Update issue flag            
                result = dataAccess.UpdateSystemStatusModel(model);

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
        public IList<SystemStatusModel> GetAuxiliaryCode(int? notice, string noticeTitle)
        {
            // Declare new DataAccess object
            SystemStatusDa dataAccess = new SystemStatusDa();
            IList<SystemStatusModel> results = dataAccess.GetAuxiliaryCode(notice, noticeTitle);
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

        public SystemStatusModel GetSystemStatus()
        {
            SystemStatusDa dataAccess = new SystemStatusDa();
            return dataAccess.GetSystemStatus();
        }
    }
        
}

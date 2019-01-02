using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemSetup.BusinessServices
{
    #region using
    using SystemSetup.DataAccess;
    using SystemSetup.Models;
    using System.Collections.Generic;
    using System.Transactions;
    using SystemSetup.DataAccess.Common;
    using SystemSetup.UtilityServices;
    #endregion using

    public class OrgMaintServices : BaseServices
    {
        public IList<OrgMaintModel> GetGroupByParentID(long parentID, string companyCd)
        {
            var OrgMaintDa = new OrgMaintDa();
            return OrgMaintDa.GetGroupByParentID(parentID, companyCd);
        }


        public OrgMaintModel GetListByGroupId(long GROUP_ID)
        {
            CommonService comService = new CommonService();
            var OrgMaintDa = new OrgMaintDa();
            var model = OrgMaintDa.GetListByGroupId(GROUP_ID);
            model.INS_USERNAME = model.INS_USER_ID != 0 ? comService.GetContractUserName(model.INS_USER_ID) : String.Empty;
            model.UPD_USERNAME = model.UPD_USER_ID != 0 ? comService.GetContractUserName(model.UPD_USER_ID) : String.Empty;
            return model;
        }


        public IList<OrgMaintInfo> GetListByName(string GROUP_NAME)
        {
            var OrgMaintDa = new OrgMaintDa();
            return OrgMaintDa.GetListByName(GROUP_NAME);
        }

        public int Regist(IList<OrgMaintRegistModel> OrgList)
        {
            var OrgMaintDa = new OrgMaintDa();
            string groupidList = string.Empty;
            string tmp = string.Empty;
            string companyCd = string.Empty;

            using (var Transtaction = new TransactionScope())
            {
                foreach (var data in OrgList)
                {
                    companyCd = data.COMPANY_CD;
                    groupidList += tmp + data.GROUP_ID;
                    tmp = ",";
                }

                //更新処理で削除データ作成
                OrgMaintDa.UpdateDelete(groupidList, companyCd);

                //更新処理
                foreach (var data in OrgList)
                {
                    var strGROUP_TYPE_NEW = data.GROUP_TYPE_NEW.ToString();

                    data.UP_GROUP_ID = 0;

                    if (data.GROUP_TYPE_NEW.ToString().Length > 1)
                    {
                        var strGROUP_TYPE = strGROUP_TYPE_NEW.Substring(0, strGROUP_TYPE_NEW.Length - 1);

                        var Result = OrgList.Where(m => m.GROUP_TYPE_NEW == int.Parse(strGROUP_TYPE)).ToList();

                        //親IDを設定
                        data.UP_GROUP_ID = Result[0].GROUP_ID;
                    }
                    data.GROUP_TYPE = data.GROUP_TYPE_NEW;
                    data.UPD_USER_ID = 0;
                    data.UPD_DATE = Utility.GetCurrentDateTime();
                    data.UPD_PROG_ID = Constants.Constant.DEFAULT_VALUE;
                    data.DEL_FLG = Constants.DeleteFlag.NON_DELETE;
                    data.ANY_GROUP_CD = string.IsNullOrEmpty(data.ANY_GROUP_CD) ?
                    "" : data.ANY_GROUP_CD.Trim();

                    data.DSP_PRIORITY = 0;

                    if (data.GROUP_ID == 0)
                    {
                        //登録処理
                        data.INS_USER_ID = 0;
                        data.INS_DATE = Utility.GetCurrentDateTime();

                        long new_seq = OrgMaintDa.Insert(data);

                        //現在ループしているデータにSEQ番号を更新
                        data.GROUP_ID = new_seq;
                    }
                    else
                    {
                        long recount = OrgMaintDa.Update(data);
                    }

                }

                Transtaction.Complete();

            }
            
            return 0;
        }

        /// <summary>
        /// Get contract firm master service
        /// </summary>
        /// <returns></returns>
        public ContractFirmEntity GetContractFirmMaster(string companyCd)
        {
            OrgMaintDa dataAccess = new OrgMaintDa();
            ContractFirmEntity results = dataAccess.GetContractFirmMaster(companyCd);
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

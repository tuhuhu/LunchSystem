using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemSetup.Areas.Maint.Controllers
{
    using BusinessServices;
    using SystemSetup.Constants;
    using SystemSetup.Models;
    using SystemSetup.ReportServices;
    using System.Configuration;
    using System.IO;
    using System.Data;
    using System.Text.RegularExpressions;
    using System.Text;
    using SystemSetup.UtilityServices;
    using SystemSetup.Controllers;
    public class OrgMaintController : BaseController
    {

        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="currentMail"></param>
        /// <returns></returns>
        public ActionResult OrgMaintList()
        {
            var model = CreateEditViewModel();

                        //共通サービス初期化
            CommonService comService = new CommonService();

            //契約企業
            model.CONTRACT_FIRM_LIST = comService.GetContractFirmMasterByOutside().Select(
                f => new SelectListItem
                {
                    Value = f.COMPANY_CD.ToString(),
                    Text = f.COMPANY_CD.ToString() + ": " + f.COMPANY_NAME.ToString()
                }
                ).ToList();

            return View("OrgMaint", model);//, model);
        }
        /// <summary>
        /// Select Email
        /// </summary>
        /// <returns></returns>
        private OrgMaintModel CreateEditViewModel()
        {
            var model = new OrgMaintModel();
            var OrgMaintServices = new OrgMaintServices();
            return model;
        }
        /// <summary>
        /// Search Email by Group Name
        /// </summary>
        /// <param name="NAME"></param>
        /// <returns></returns>
        public ActionResult SearchByGroupName(string GROUP_NAME)
        {
            using (var service = new OrgMaintServices())
            {
                var listMail = service.GetListByName(GROUP_NAME);

                return Json(listMail, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Search Email by Group ID
        /// </summary>
        /// <param name="GROUP_ID"></param>
        /// <returns></returns>
        public ActionResult SearchByGroupId(long GROUP_ID = 0)
        {
            using (var service = new OrgMaintServices())
            {
                var orgModel = service.GetListByGroupId(GROUP_ID);

                return Json(orgModel, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Get group by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetGroup(string id = Constant.DEFAULT_VALUE, string companyCd = "")
        {
            using (var service = new OrgMaintServices())
            {
                var dataList = service.GetGroupByParentID(Convert.ToInt64(id.Split('-')[0]), companyCd);

                IList<dynamic> data = new List<dynamic>();
                CommonService comService = new CommonService();

                if (!companyCd.Equals("") && id.Equals("0") && dataList.Count == 0)
                {
                    OrgMaintRegistModel model = new OrgMaintRegistModel();
                    IList<OrgMaintRegistModel> OrgList = new List<OrgMaintRegistModel>();

                    ContractFirmEntity contractFirm = service.GetContractFirmMaster(companyCd);

                    model.COMPANY_CD = contractFirm.COMPANY_CD;
                    model.GROUP_NAME = contractFirm.COMPANY_NAME;
                    model.UP_GROUP_ID = 0;
                    model.GROUP_TYPE_NEW = 1;

                    OrgList.Add(model);

                    service.Regist(OrgList);

                    dataList = service.GetGroupByParentID(Convert.ToInt64(id.Split('-')[0]), companyCd);
                }

                foreach (var obj in dataList)
                {
                    var ins_username = obj.INS_USER_ID != 0 ? comService.GetContractUserName(obj.INS_USER_ID) : String.Empty;
                    var upd_username = obj.UPD_USER_ID != 0 ? comService.GetContractUserName(obj.UPD_USER_ID) : String.Empty;

                    var text = "<span class='node-name'";
                    text += " data-group-name='" + HttpUtility.HtmlEncode(obj.GROUP_NAME) + "'";
                    text += " data-group-name-kana='" + HttpUtility.HtmlEncode(obj.GROUP_NAME_KANA) + "'";
                    text += " data-any-group-code='" + HttpUtility.HtmlEncode(obj.ANY_GROUP_CD) + "'";
                    text += " data-group-id='" + obj.GROUP_ID + "'";
                    text += " data-group-type='" + obj.GROUP_TYPE + "'";
                    text += " data-up-group-id='" + obj.UP_GROUP_ID + "'";
                    text += " data-ins-date='" + obj.FM_INS_DATE + "'";
                    text += " data-ins-username='" + ins_username + "'";
                    text += " data-upd-date='" + obj.FM_UPD_DATE + "'";
                    text += " data-upd-username='" + upd_username + "'";
                    text += " data-new-group-type='" + obj.GROUP_TYPE + "'";
                    text += " >";
                    text += HttpUtility.HtmlEncode(obj.GROUP_NAME);
                    text += "</span>";
                    
                    data.Add(new
                    {
                        id = obj.GROUP_ID,
                        text = text,
                        icon = " ",
                        children = true
                    });
                }

                var result = Json(
                data.ToList(),
                JsonRequestBehavior.AllowGet);

                return result;
            }

        }

        public ActionResult Regist(IList<OrgMaintRegistModel> OrgList)
        {
            using (var service = new OrgMaintServices())
            {
                var ret = service.Regist(OrgList);

                var result = Json(
                "",
                JsonRequestBehavior.AllowGet);

                return result;
            }
        }
    }
}

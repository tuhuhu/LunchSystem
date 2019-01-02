using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SystemSetup.Models;
using SystemSetup.Controllers;
using SystemSetup.BusinessServices;
using SystemSetup.Constants;
using SystemSetup.Constants.Resources;
using SystemSetup.UtilityServices;
using SystemSetup.UtilityServices.SafePassword;

namespace SystemSetup.Areas.Maint.Controllers
{
    public class ContractCompanyMaintController : BaseController
    {
        //
        // GET: /Contract/ContractCompanyMaint/

        public ActionResult Index()
        {
            return View();
        }

        #region ContractCompanyMaintList

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ContractCompanyMaintList(ContractCompanyMaintModel model = null)
        {
            if (model == null)
            {
                model = new ContractCompanyMaintModel();
            }

            //共通サービス初期化
            CommonService service = new CommonService();
            
            return View("ContractCompanyMaintList", model);
        }

        /// <summary>
        /// Cust Maint Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ContractCompanyMaintSearch(DataTablesModel dt, ContractCompanyMaintModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    using (ContractCompanyMaintServices service = new ContractCompanyMaintServices())
                    {
                        int total_row;
                        var dataList = service.ContractCompanyMaintSearch(dt, ref model, out total_row);
                        int order = 1;

                        var result = Json(
                        new
                        {
                            sEcho = dt.sEcho,
                            iTotalRecords = total_row,
                            iTotalDisplayRecords = total_row,
                            aaData = (from i in dataList
                                      select new object[]
                                {
                                    order++,
                                    i.COMPANY_CD,
                                    i.COMPANY_NAME != null ? HttpUtility.HtmlEncode(i.COMPANY_NAME) : String.Empty,
                                    i.COMPANY_GROUP_NAME != null ? HttpUtility.HtmlEncode(i.COMPANY_GROUP_NAME) : String.Empty,
                                    i.COMPANY_STAFF_NAME != null ? HttpUtility.HtmlEncode(i.COMPANY_STAFF_NAME) : String.Empty,
                                    i.COMPANY_CONTRACT_STATUS == "0" ? "0:無効" : "1:有効",
                                    i.UPD_DATE.HasValue ? i.UPD_DATE.Value.ToString("yyyy/MM/dd HH:mm") : String.Empty,
                                    i.UPD_USERNAME != null ? HttpUtility.HtmlEncode(i.UPD_USERNAME) : String.Empty
                                })
                        });
                        return result;
                    }
                }
            }
            return new EmptyResult();
        }

        /// <summary>
        /// Delete estimation
        /// </summary>
        /// <param name="SEQNO"></param>
        /// <returns></returns>
        public ActionResult DELETE(string COMPANY_CD = "")
        {
            using (ContractCompanyMaintServices service = new ContractCompanyMaintServices())
            {
                int result = service.DeleteContractCompanyMaint(COMPANY_CD);
                if (result > 0)
                {
                    Dictionary<String, Object> obj = new Dictionary<String, Object>();
                    obj.Add("success", result);

                    if (obj.Count > 0)
                    {
                        return Json(obj, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return new EmptyResult();
                    }
                }

                return new EmptyResult();
            }
        }

        #endregion

        #region ContractCompanyMaintRegister

        /// <summary>
        /// Cust Maint Register
        /// </summary>
        /// <returns></returns>
        public ActionResult ContractCompanyMaintEdit(ContractCompanyMaintModel REGMODEL = null)
        {
            ContractCompanyMaintModel model = CreateEditViewModel(REGMODEL);
            model.SEARCH_START = REGMODEL.SEARCH_START;
            model.SEARCH_LENGTH = REGMODEL.SEARCH_LENGTH;
            return View("ContractCompanyMaintRegister", model);

        }

        /// <summary>
        /// Create edit view model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ContractCompanyMaintModel CreateEditViewModel(ContractCompanyMaintModel REGMODEL)
        {
            CommonService comService = new CommonService();
            ContractCompanyMaintModel model = new ContractCompanyMaintModel();

            string companyCd = REGMODEL.REGIST_PARAM.COMPANY_CD;

            using (ContractCompanyMaintServices service = new ContractCompanyMaintServices())
            {
                //更新のとき
                if (!companyCd.Equals("0"))
                {
                    model = service.GetContractCompanyMaint(companyCd);
                }

                //セレクトボックスの情報を取得
                //端数処理区分
                model.ROUNDING_TYPE_LIST = new List<SelectListItem>()
                {
	                new SelectListItem() { Value = Constants.RoundingType.RoundDown, Text = "0:切捨て" },
	                new SelectListItem() { Value = Constants.RoundingType.RoundUp,   Text = "1:切り上げ" },
	                new SelectListItem() { Value = Constants.RoundingType.RoundOff,  Text = "2:四捨五入" },
                };

                //契約階層区分
                model.CONTRACT_LEVEL_TYPE_LIST = new List<SelectListItem>()
                {
	                new SelectListItem() { Value = Constants.ContractLevelType.Level2, Text = "0:２階層" },
	                new SelectListItem() { Value = Constants.ContractLevelType.Level3, Text = "1:３階層", Selected = true},
                };

                //無し・有り
                model.EXISTENCE_LIST = new List<SelectListItem>()
                {
	                new SelectListItem() { Value = "0", Text = "0:無し" },
	                new SelectListItem() { Value = "1", Text = "1:有り" },
                };
                
                //契約状態
                model.COMPANY_CONTRACT_STATUS_LIST = new List<SelectListItem>()
                {
	                new SelectListItem() { Value = "0", Text = "0:無効" },
	                new SelectListItem() { Value = "1", Text = "1:有効" },
                };


                IEnumerable<PlanMaintEntity> planMaster = comService.GetPlanMaster();
                model.PLAN_LIST = new List<SelectListItem>();
                model.PLAN_LIST.Add(new SelectListItem(){ Value="0", Text=""});
                foreach (var plan in planMaster)
                {
                    model.PLAN_LIST.Add(new SelectListItem(){ Value = plan.PLAN_SEQ_NO.ToString(), Text = plan.PLAN_NAME});
                }

                IEnumerable<long> listPlanSeqNo = comService.GetContractFirmPlan(companyCd);
                if (listPlanSeqNo.ToList().Count > 0)
                {
                    model.PLAN_SEQ_NO = listPlanSeqNo.ElementAt(0);
                }
                else
                {
                    model.PLAN_SEQ_NO = 0;
                }
            }

            return model;
        }

        /// <summary>
        /// GetDefaultSubCategory
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDefaultSubCategory(string COMPANY_CD, int CONTRACT_TYPE, int CONTRACT_TYPE_CLASS)
        {
            ContractCompanyMaintServices service = new ContractCompanyMaintServices();
              var list = new ContractCompanyMaintModel();

            return Json(new
            {
                IsExited = list
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Create/Update Tbl_Information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(ContractFirmEntity Entity)
        {
            try
            {
                using (ContractCompanyMaintServices service = new ContractCompanyMaintServices())
                {
                    if (ModelState.IsValid && ValidateData(Entity))
                    {
                        bool isNew = false;
                        List<int> errNo = new List<int>();

                        //Add Value for contract duration(finish) if it is null
                        if (Entity.AVAILABLE_PERIOD_TO == null)
                        {
                            Entity.AVAILABLE_PERIOD_TO = new DateTime(2099,12,31);
                        }

                        //CheckEnvironmentDependentCharacters
                        Entity.BILL_FORMAT_TEMP_PATH = string.IsNullOrEmpty(Entity.BILL_FORMAT_TEMP_PATH) ?
                            "" : Entity.BILL_FORMAT_TEMP_PATH;
                        for (int i = 0; i < 12; i++)
                        {
                            string CHECK_PATH = null;
                            if (i == 0) { CHECK_PATH = Entity.BILL_FORMAT_PATH; };
                            if (i == 1) { CHECK_PATH = Entity.BILL_FORMAT_SES_PATH; };
                            if (i == 2) { CHECK_PATH = Entity.BILL_FORMAT_TEMP_PATH; };
                            if (i == 3) { CHECK_PATH = Entity.PLC_ORDER_FORMAT_PATH; };
                            if (i == 4) { CHECK_PATH = Entity.PLC_PAYMENT_NOTICE_FORMAT_PATH; };
                            if (i == 5) { CHECK_PATH = Entity.DOC_COVER_LETTER_FORMAT_PATH; };
                            if (i == 6) { CHECK_PATH = Entity.FAX_COVER_LETTER_FORMAT_PATH; };
                            if (i == 7) { CHECK_PATH = Entity.ENVELOPE_DESTINATION_FORMAT_PATH; };

                            if (i == 8) { CHECK_PATH = Entity.EST_COLLECTION_FORMAT_PATH; };
                            if (i == 9) { CHECK_PATH = Entity.EST_COLLECTION_FORMAT_SES_PATH; };
                            if (i == 10) { CHECK_PATH = Entity.ORD_COLLECTION_FORMAT_PATH; };
                            if (i == 11) { CHECK_PATH = Entity.ORD_COLLECTION_FORMAT_SES_PATH; };

                            if (!CheckEnvironmentDependentCharacters(CHECK_PATH))
                            {
                                errNo.Add(i);
                            }
                        }
                        //Check exist COMPANY_CD
                        if (service.CheckExistCustomer(Entity) && Entity.INS_DATE == DateTime.MinValue)
                        {
                            errNo.Add(-1);
                        }
                        if (errNo.Count() == 0)
                        {
                            var commonService = new CommonService();
                            ContractFirmPlanMaintEntity contractFirmPlanMaintEntity = new ContractFirmPlanMaintEntity()
                            {
                                COMPANY_CD = Entity.COMPANY_CD,
                                PLAN_REL_SEQ_NO = Entity.PLAN_REL_SEQ_NO,
                                PLAN_SEQ_NO = Entity.PLAN_SEQ_NO,
                                PLAN_START_DATE = Utility.GetCurrentDateTime(),
                                DEL_FLG = DeleteFlag.NON_DELETE
                            };

                            if (Entity.PLAN_REL_SEQ_NO > 0)
                            {
                                commonService.DeleteContractFirmPlan(contractFirmPlanMaintEntity);
                            }

                            if (contractFirmPlanMaintEntity.PLAN_SEQ_NO != 0)
                            {
                                Entity.PLAN_REL_SEQ_NO = commonService.InsertContractFirmPlan(contractFirmPlanMaintEntity);
                            }
                            else
                            {
                                Entity.PLAN_REL_SEQ_NO = 0;
                            }

                            if (Entity.COMPANY_LOGO_IMAGE_PATH == null)
                            {
                                Entity.COMPANY_LOGO_IMAGE_PATH = "";
                            }

                            if (Entity.UPLOAD_FILE_PASSWORD == null)
                            {
                                Entity.UPLOAD_FILE_PASSWORD = "";
                            }

                            if (Entity.UPLOAD_FILE_SIZE_LIMIT == null)
                            {
                                Entity.UPLOAD_FILE_SIZE_LIMIT = 0;
                            }

                            if (Entity.INS_DATE == DateTime.MinValue)
                            {
                                isNew = true;
                                Entity.INS_USER_ID = 0;
                                Entity.INS_DATE = Utility.GetCurrentDateTime();
                                Entity.UPD_DATE = Utility.GetCurrentDateTime();
                                Entity.DEL_FLG = DeleteFlag.NON_DELETE;
                                service.InsertContractCompanyMaint(Entity);
                                JsonResult result = Json(new
                                {
                                    statusCode = Constants.Constant.CREATED,
                                    isNew = isNew
                                },
                                JsonRequestBehavior.AllowGet);
                                return result;
                            }
                            else
                            {
                                Entity.UPD_DATE = Utility.GetCurrentDateTime();
                                Entity.UPD_USER_ID = 0;
                                Entity.DEL_FLG = DeleteFlag.NON_DELETE;
                                service.UpdateContractCompanyMaint(Entity);
                                //service.UpdateRelContractFirm(Entity);
                                JsonResult result = Json(new
                                {
                                    statusCode = Constants.Constant.CREATED,
                                    isNew = isNew
                                },
                                JsonRequestBehavior.AllowGet);
                                return result;
                            }

                           
                        }
                        else
                        {
                            //ModelState.AddModelError("", Constants.Resources.Messages.DuplicateName);
                            JsonResult duplicate = Json(
                                new
                                {
                                    statusCode = Constants.Constant.INTERNAL_SERVER_ERROR,
                                    errNo = errNo
                                },
                                JsonRequestBehavior.AllowGet);
                            return duplicate;
                        }
                    }
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                System.Web.HttpContext.Current.Session["ERROR"] = ex;
                return new EmptyResult();
            }
        }

        /// <summary>
        /// Validate data Contract Company Maint Register
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool ValidateData(ContractFirmEntity Entity)
        {
            bool isValid = true;

            ///EST_COLLECTION_FORMAT_PATH
            if (string.IsNullOrEmpty(Entity.EST_COLLECTION_FORMAT_PATH))
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.Required, Constants.Resources.Maint.EST_COLLECTION_FORMAT_PATH));
                isValid = false;
            }
            else if (System.Text.ASCIIEncoding.UTF8.GetByteCount(Entity.EST_COLLECTION_FORMAT_PATH) > FormatType.MAX_FORMAT_PATH)
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.MaxByte, Constants.Resources.Maint.EST_COLLECTION_FORMAT_PATH));
                isValid = false;
            }
            ///EST_COLLECTION_FORMAT_SES_PATH
            if (string.IsNullOrEmpty(Entity.EST_COLLECTION_FORMAT_SES_PATH))
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.Required, Constants.Resources.Maint.EST_COLLECTION_FORMAT_SES_PATH));
                isValid = false;
            }
            else if (System.Text.ASCIIEncoding.UTF8.GetByteCount(Entity.EST_COLLECTION_FORMAT_SES_PATH) > FormatType.MAX_FORMAT_PATH)
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.MaxByte, Constants.Resources.Maint.EST_COLLECTION_FORMAT_SES_PATH));
                isValid = false;
            }
            ///ORD_COLLECTION_FORMAT_PATH
            if (string.IsNullOrEmpty(Entity.ORD_COLLECTION_FORMAT_PATH))
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.Required, Constants.Resources.Maint.ORD_COLLECTION_FORMAT_PATH));
                isValid = false;
            }
            else if (System.Text.ASCIIEncoding.UTF8.GetByteCount(Entity.ORD_COLLECTION_FORMAT_PATH) > FormatType.MAX_FORMAT_PATH)
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.MaxByte, Constants.Resources.Maint.ORD_COLLECTION_FORMAT_PATH));
                isValid = false;
            }
            ///ORD_COLLECTION_FORMAT_SES_PATH
            if (string.IsNullOrEmpty(Entity.ORD_COLLECTION_FORMAT_SES_PATH))
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.Required, Constants.Resources.Maint.ORD_COLLECTION_FORMAT_SES_PATH));
                isValid = false;
            }
            else if (System.Text.ASCIIEncoding.UTF8.GetByteCount(Entity.ORD_COLLECTION_FORMAT_SES_PATH) > FormatType.MAX_FORMAT_PATH)
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.MaxByte, Constants.Resources.Maint.ORD_COLLECTION_FORMAT_SES_PATH));
                isValid = false;
            }

            // BILL_FORMAT_PATH
            if (string.IsNullOrEmpty(Entity.BILL_FORMAT_PATH))
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.Required, Constants.Resources.Maint.BILL_FORMAT_PATH));
                isValid = false;
            }
            else if (System.Text.ASCIIEncoding.UTF8.GetByteCount(Entity.BILL_FORMAT_PATH) > FormatType.MAX_FORMAT_PATH)
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.MaxByte, Constants.Resources.Maint.BILL_FORMAT_PATH));
                isValid = false;
            }

            // BILL_FORMAT_SES_PATH
            if (string.IsNullOrEmpty(Entity.BILL_FORMAT_SES_PATH))
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.Required, Constants.Resources.Maint.BILL_FORMAT_SES_PATH));
                isValid = false;
            }
            else if (System.Text.ASCIIEncoding.UTF8.GetByteCount(Entity.BILL_FORMAT_SES_PATH) > FormatType.MAX_FORMAT_PATH)
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.MaxByte, Constants.Resources.Maint.BILL_FORMAT_SES_PATH));
                isValid = false;
            }

            // BILL_FORMAT_TEMP_PATH
            if (!string.IsNullOrEmpty(Entity.BILL_FORMAT_TEMP_PATH) 
                &&System.Text.ASCIIEncoding.UTF8.GetByteCount(Entity.BILL_FORMAT_TEMP_PATH) > FormatType.MAX_FORMAT_PATH)
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.MaxByte, Constants.Resources.Maint.BILL_FORMAT_TEMP_PATH));
                isValid = false;
            }

            // PLC_ORDER_FORMAT_PATH
            if (string.IsNullOrEmpty(Entity.PLC_ORDER_FORMAT_PATH))
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.Required, Constants.Resources.Maint.PLC_ORDER_FORMAT_PATH));
                isValid = false;
            }
            else if (System.Text.ASCIIEncoding.UTF8.GetByteCount(Entity.PLC_ORDER_FORMAT_PATH) > FormatType.MAX_FORMAT_PATH)
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.MaxByte, Constants.Resources.Maint.PLC_ORDER_FORMAT_PATH));
                isValid = false;
            }

            // PLC_PAYMENT_NOTICE_FORMAT_PATH
            if (string.IsNullOrEmpty(Entity.PLC_PAYMENT_NOTICE_FORMAT_PATH))
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.Required, Constants.Resources.Maint.PLC_PAYMENT_NOTICE_FORMAT_PATH));
                isValid = false;
            }
            else if (System.Text.ASCIIEncoding.UTF8.GetByteCount(Entity.PLC_PAYMENT_NOTICE_FORMAT_PATH) > FormatType.MAX_FORMAT_PATH)
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.MaxByte, Constants.Resources.Maint.PLC_PAYMENT_NOTICE_FORMAT_PATH));
                isValid = false;
            }

            // DOC_COVER_LETTER_FORMAT_PATH
            if (string.IsNullOrEmpty(Entity.DOC_COVER_LETTER_FORMAT_PATH))
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.Required, Constants.Resources.Maint.DOC_COVER_LETTER_FORMAT_PATH));
                isValid = false;
            }
            else if (System.Text.ASCIIEncoding.UTF8.GetByteCount(Entity.DOC_COVER_LETTER_FORMAT_PATH) > FormatType.MAX_FORMAT_PATH)
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.MaxByte, Constants.Resources.Maint.DOC_COVER_LETTER_FORMAT_PATH));
                isValid = false;
            }

            // FAX_COVER_LETTER_FORMAT_PATH
            if (string.IsNullOrEmpty(Entity.FAX_COVER_LETTER_FORMAT_PATH))
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.Required, Constants.Resources.Maint.FAX_COVER_LETTER_FORMAT_PATH));
                isValid = false;
            }
            else if (System.Text.ASCIIEncoding.UTF8.GetByteCount(Entity.FAX_COVER_LETTER_FORMAT_PATH) > FormatType.MAX_FORMAT_PATH)
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.MaxByte, Constants.Resources.Maint.FAX_COVER_LETTER_FORMAT_PATH));
                isValid = false;
            }

            // ENVELOPE_DESTINATION_FORMAT_PATH
            if (string.IsNullOrEmpty(Entity.ENVELOPE_DESTINATION_FORMAT_PATH))
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.Required, Constants.Resources.Maint.ENVELOPE_DESTINATION_FORMAT_PATH));
                isValid = false;
            }
            else if (System.Text.ASCIIEncoding.UTF8.GetByteCount(Entity.ENVELOPE_DESTINATION_FORMAT_PATH) > FormatType.MAX_FORMAT_PATH)
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.MaxByte, Constants.Resources.Maint.ENVELOPE_DESTINATION_FORMAT_PATH));
                isValid = false;
            }

            // COMPANY_LOGO_IMAGE_PATH
           if (!string.IsNullOrEmpty(Entity.COMPANY_LOGO_IMAGE_PATH) 
               && System.Text.ASCIIEncoding.UTF8.GetByteCount(Entity.COMPANY_LOGO_IMAGE_PATH) > FormatType.MAX_FORMAT_PATH)
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.MaxByte, Constants.Resources.Maint.COMPANY_LOGO_IMAGE_PATH));
                isValid = false;
            }

           // UPLOAD_FILE_SIZE_LIMIT
           if (Entity.UPLOAD_FILE_SIZE_LIMIT.HasValue && Entity.UPLOAD_FILE_SIZE_LIMIT.Value > FormatType.UPLOAD_FILE_SIZE_LIMIT)
           {
               ModelState.AddModelError(string.Empty, string.Format(Messages.InputNumErr, Constants.Resources.Maint.lblUploadFileSizeMax));
               isValid = false;
           }

            return isValid;
        }

        /// <summary>
        /// Check ENVIRONMENT_DEPENDENT_CHARACTERS
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CheckEnvironmentDependentCharacters(string CHECK_CHAR)
        {

                for (int i = 0; i < CHECK_CHAR.Length; i++)
                {
                    if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS1.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                    if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS2.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                    if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS3.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                    if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS4.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                    if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS5.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                    if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS6.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                    if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS7.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                    if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS8.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                    if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS9.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                    if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS10.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                    if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS11.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                    if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS12.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                    if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS13.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                    if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS14.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                    if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS15.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                    if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS16.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                    if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS17.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                    if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS18.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                    if (Constants.Constant.ENVIRONMENT_DEPENDENT_CHARACTERS19.Contains(CHECK_CHAR.Substring(i, 1))) { return false; };
                };
            return true;
        }
        #endregion

    }
}

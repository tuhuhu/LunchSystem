namespace SystemSetup.Areas.Common.Controllers
{
    using BusinessServices;
    using SystemSetup.Constants;
    using SystemSetup.Controllers;
    using SystemSetup.Models;
    using SystemSetup.ReportServices;
    using SystemSetup.UtilityServices;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    public class CommonController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Clear Session and Authentication 
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            Exception ex = (Exception)System.Web.HttpContext.Current.Session["ERROR"];
            FormsAuthentication.SignOut();
            Session.Clear();
            if (ex != null)
            {
                ViewBag.Message = ex.Message;
                ViewBag.Data = ex.Data;
                ViewBag.HelpLink = ex.HelpLink;
                ViewBag.HResult = ex.HResult;
                ViewBag.InnerException = ex.InnerException;
                ViewBag.Message = ex.Message;
                ViewBag.Source = ex.Source;
                ViewBag.StackTrace = ex.StackTrace;
                ViewBag.TargetSite = ex.TargetSite;

                try
                {
                    var httpException = (System.Web.HttpException)ex;
                    var httpCode = httpException.GetHttpCode();
                    if (httpCode == Constant.NOT_FOUND)
                    {
                        return View("NotFound");
                    }
                }
                catch (Exception)
                {
                }

            }
            return View("Error");
        }

        /// <summary>
        /// Clost Iframe
        /// </summary>
        /// <returns></returns>
        public ActionResult CloseIframe()
        {
            //FormsAuthentication.SignOut();
            //Session.Clear();
            return View("CloseIframe");
        }

        /// <summary>
        /// Check timeout
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CheckTimeOut()
        {
            if (base.GetCache<CmnEntityModel>("CmnEntityModel") == null)
            {
                this.Response.StatusCode = Constant.TIME_OUT;
                //logger.Fatal(Messages.E044);
                return new EmptyResult();
            }

            JsonResult result = Json(
                "Success",
                JsonRequestBehavior.AllowGet
            );
            return result;
        }

        /// <summary>
        /// Authent Timeout
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult AuthentTimeout()
        {
            Session.Clear();
            if (Request.IsAjaxRequest())
            {
                this.Response.StatusCode = Constant.TIME_OUT;
                return new EmptyResult();
            }
            else
            {
                return this.RedirectToAction("Login", "Login", new { area = "UserManagement", timeout = true });
            }
        }           

        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult UploadFile()
        {
            HttpPostedFileBase myFile = Request.Files["MyFile"];
            //bool isUploaded = false;
            string message = "File upload failed";

            if (myFile != null && myFile.ContentLength != 0)
            {
                string pathForSaving = Server.MapPath("~/Uploads");
                if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        myFile.SaveAs(Path.Combine(pathForSaving, myFile.FileName));
                        //isUploaded = true;
                        message = "File uploaded successfully!";
                    }
                    catch (Exception ex)
                    {
                        message = string.Format("File upload failed: {0}", ex.Message);
                    }
                }
            }
            //return Json(new { isUploaded = isUploaded, message = message }, "text/html");
            // Returns json
            return Content("{\"name\":\"" + myFile.FileName + "\",\"type\":\"" + myFile.ContentType + "\",\"size\":\"" + string.Format("{0} bytes", myFile.ContentLength) + "\"}", "application/json");
        }

        /// <summary>
        /// Creates the folder if needed.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// Call search product screen
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="contractSubType"></param>
        /// <param name="productNo"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public ActionResult SearchProductIndex(string contractTypeName = "", string contractSubTypeName = "", string productName = "", int contractType = 0, int contractSubType = 0, int productNo = 0, string callback = "")
        {
            var model = new SearchProductModel
            {
                CallBack = callback,
                CONTRACT_TYPE = contractType,
                CONTRACT_TYPE_CLASS = contractSubType,
                PRODUCT_NO = productNo,
                CONTRACT_TYPE_DISP_NAME = contractTypeName,
                CONTRACT_TYPE_CLASS_DISP_NAME = contractSubTypeName,
                PRODUCT_NAME = productName
            };

            return this.View("ProductList", model);
        }

        /// <summary>
        /// Search list product
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult SearchItemProduct(DataTablesModel dt, SearchProductModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    using (CommonService service = new CommonService())
                    {
                        // Call service to get customer info               
                        int total_row;
                        var customerinfo = service.SearchItemProduct(dt, ref model, out total_row);
                        int index = 1;
                        // Update json data to send to view
                        var result = Json(
                        new
                        {
                            sEcho = dt.sEcho,
                            iTotalRecords = total_row,
                            iTotalDisplayRecords = total_row,
                            aaData = (from t in customerinfo
                                      select
                                        new object[]
                                                  {
                                                      t.ITEM_SEQ_NO,
                                                      index++,
                                                      t.ITEM_NO,
                                                      t.NOMEN,
                                                      t.UNIT_PRICE,
                                                      t.UNIT_TYPE
                                                  }).ToList()
                        });

                        return result;
                    }
                }
            }
            return new EmptyResult();
        }

        /// <summary>
        /// Bank Account List
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public ActionResult BankAccountList(string callback = "")
        {
            var model = new SearchBankAccountModel
            {
                CallBack = callback
            };

            return this.View("BankAccountList", model);
        }        

        /// <summary>
        /// Get Item Bank Account
        /// </summary>
        /// <param name="accSegNo"></param>
        /// <returns></returns>
        public ActionResult GetItemBankAccountBySegNo(long accSegNo)
        {
            //long segno = 0;
            //bool isValid = Int64.TryParse(productsegno, out segno);

            using (CommonService service = new CommonService())
            {
                var bankAccount = service.GetItemBankAccountBySegNo(accSegNo);
                Dictionary<String, Object> obj = new Dictionary<String, Object>();
                if (bankAccount != null)
                {
                    obj.Add("itemsegno", bankAccount.BANK_ACCOUNT_SEQ_NO);
                    obj.Add("itemname", bankAccount.BANK_ACCOUNT_DISP_NAME);
                    obj.Add("itemfinancialcd", bankAccount.FINANCIAL_INST_CD);
                    obj.Add("itemfinancialname", bankAccount.FINANCIAL_INST_NAME);
                    obj.Add("itembankaccountcd", bankAccount.BANK_ACCOUNT_BRANCH_CD);
                    obj.Add("itembankaccountname", bankAccount.BANK_ACCOUNT_BRANCH_NAME);
                    obj.Add("itembankaccounttype", bankAccount.BANK_ACCOUNT_TYPE);
                    obj.Add("itembankaccountno", bankAccount.BANK_ACCOUNT_NO);
                    obj.Add("itembankaccountholder", bankAccount.BANK_ACCOUNT_HOLDER);
                    obj.Add("itemswiftbiccd", bankAccount.SWIFT_BIC_CD);
                }

                if (obj.Count > 0)
                {
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(string.Empty, JsonRequestBehavior.AllowGet);
                }
            }
        }
        
        /// <summary>
        /// Get Product by SegNo
        /// </summary>
        /// <param name="itemSegNo"></param>
        /// <returns></returns>
        public ActionResult GetItemProductByItemSegNo(long itemSegNo = 0)
        {
            using (CommonService service = new CommonService())
            {
                var product = service.GetItemProductByItemSegNo(itemSegNo);
                Dictionary<String, Object> obj = new Dictionary<String, Object>();
                if (product != null)
                {
                    obj.Add("itemsegno", product.ITEM_SEQ_NO);
                    obj.Add("itemname", product.NOMEN);
                    obj.Add("itemprice", product.UNIT_PRICE);
                    obj.Add("itemunit", product.UNIT_TYPE);
                    obj.Add("itemtax", product.TAXABLE_FLG);
                }

                if (obj.Count > 0)
                {
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(string.Empty, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// Check limit data when update by liscense of current user's company
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public ActionResult SetMenu(string dataType)
        {
            //Write menu state to session
            var sessionLogin = Session["CmnEntityModel"] as CmnEntityModel;
            sessionLogin.MenuStatus = dataType;
            //Write menu state to cookie
            HttpCookie menuStateCookie = new HttpCookie(Constant.MENU_STATE_COOKIE, dataType);
            menuStateCookie.Expires = Utility.GetCurrentDateTime().AddDays(Convert.ToDouble(ConfigurationManager.AppSettings[ConfigurationKeys.MENU_STATE_COOKIE_LIFETIME_DAYS]));
            Response.Cookies.Add(menuStateCookie);
            return Json(dataType, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update message
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateSuccess()
        {
            ulong id = Convert.ToUInt64(TempData["UpdateSuccessID"]);
            string screen = TempData["UpdateSuccessScreen"] as string;
            string action = TempData["Action"] as string;

            TempData.Keep("UpdateSuccessID");
            TempData.Keep("UpdateSuccessScreen");
            TempData.Keep("Action");

            if (id > 0 && !string.IsNullOrEmpty(screen))
            {
                ViewBag.Title = screen;
                ViewBag.Action = action;
                ViewBag.ID = id;

                return View("UpdateSuccess");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }       

        /// <summary>
        /// Get product data for select list
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="subContractType"></param>
        /// <returns></returns>
        public ActionResult GetProductSelectList(int contractType, int subContractType)
        {
            using (CommonService service = new CommonService())
            {
                var subcontractType = service.GetProductByContractType_ContractTypeClass(contractType, subContractType);
                var result = (from s in subcontractType
                              select new
                              {
                                  key = s.PRODUCT_SEQ_NO,
                                  value = s.PRODUCT_NAME
                              }).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get product data for select list
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="subContractType"></param>
        /// <returns></returns>
        public ActionResult GetProductSelectListByOutside(string contractFirm, int contractType, int subContractType)
        {
            using (CommonService service = new CommonService())
            {
                var subcontractType = service.GetProductByContractType_ContractTypeClass_Outside(contractFirm, contractType, subContractType);
                var result = (from s in subcontractType
                              select new
                              {
                                  key = s.PRODUCT_SEQ_NO,
                                  value = s.PRODUCT_NAME
                              }).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get sub contract type data for select list
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public ActionResult GetSubContractTypeSelectList(int contractType)
        {
            using (CommonService service = new CommonService())
            {
                var subcontractType = service.GetSubContractTypeByContractType(contractType);
                var result = (from s in subcontractType
                              select new
                              {
                                  key = s.CONTRACT_TYPE_CLASS,
                                  value = s.CONTRACT_TYPE_CLASS_DISP_NAME
                              }).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get sub contract type data for select list
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public ActionResult GetContractTypeSelectListByOutside(string contractFirm)
        {
            using (CommonService service = new CommonService())
            {
                var contractType = service.GetContractTypeSelectListByOutside(contractFirm);
                var result = (from s in contractType
                              select new
                              {
                                  key = s.CONTRACT_TYPE,
                                  value = s.CONTRACT_TYPE_DISP_NAME
                              }).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get sub contract type data for select list
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public ActionResult GetSubContractTypeSelectListByOutside(string contractFirm, int contractType)
        {
            using (CommonService service = new CommonService())
            {
                var subcontractType = service.GetSubContractTypeSelectListByOutside(contractFirm, contractType);
                var result = (from s in subcontractType
                              select new
                              {
                                  key = s.CONTRACT_TYPE_CLASS,
                                  value = s.CONTRACT_TYPE_CLASS_DISP_NAME
                              }).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

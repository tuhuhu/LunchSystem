using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemSetup.Models;
using SystemSetup.Controllers;
using SystemSetup.BusinessServices;
using SystemSetup.UtilityServices;

namespace SystemSetup.Areas.LunchRegister.Controllers
{
    public class LunchRegisterController : BaseController
    {
        //
        // GET: /LunchRegister/LunchRegister/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LunchRegisterEdit(long LUNCH_SEQ_CD = 0)
        {
            LunchInformationEntity model = CreateEditViewModel(LUNCH_SEQ_CD);
            List<SelectListItem> items = new List<SelectListItem>();
            
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            LunchServices lunchServices = new LunchServices();
            var list = lunchServices.GetMealListToday();

            foreach (var data in list)
            {
                dictionary.Add(data.LUNCH_MENU_NAME, data.LUNCH_MENU_TYPE.ToString());
            }            

            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                items.Add(new SelectListItem() { Text = pair.Key, Value = pair.Value, Selected = false });
            }
            if(list.Count > 0)
            {
                model.HAVE_MENU = 1;
            }
            else
            {
                model.HAVE_MENU = 0;
            }
            ViewBag.DurationType = new SelectList(items, "Value", "Text");
            model.BALANCE = lunchServices.GetBalance();
            model.LUNCH_DATE = DateTime.Now;
            return View("LunchRegisterEdit", model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditMeal(LunchInformationEntity model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string action = model.LUNCH_SEQ_CD > 0 ? "Update" : "Register";
                    LunchServices lunchServices = new LunchServices();
                    long lunchCd = lunchServices.RegisterMeal(model);

                    if (lunchCd > 0)
                    {
                        JsonResult result = Json(
                            new
                            {
                                message = "Update success",
                                action = action,
                                LUNCH_SEQ_CD = lunchCd
                            },
                            JsonRequestBehavior.AllowGet);
                        return result;
                    }
                }

                return new EmptyResult();
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                System.Web.HttpContext.Current.Session["ERROR"] = ex;
                return new EmptyResult();
            }
        }

        public ActionResult GetBalance()
        {
            if (Request.IsAjaxRequest())
            {
                using (var service = new LunchServices())
                {
                    var balance = service.GetBalance();
                    JsonResult result = Json(
                    balance,
                    JsonRequestBehavior.AllowGet
                );
                    result.MaxJsonLength = Int32.MaxValue;
                    return result;
                }
            }
            return new EmptyResult();
        }

        public ActionResult APPROVE()
        {
            if (Request.IsAjaxRequest())
            {
                using (LunchServices service = new LunchServices())
                {
                    int result = service.ApproveLunchDeal();
                    JsonResult res = Json(new
                    {
                        statusCode = result > 0 ? 201 : 200
                    }, JsonRequestBehavior.AllowGet);
                    
                    // Send mail
                    service.SendMail();
                    return res;
                }
            }
            return new EmptyResult();
        }

        public ActionResult DELETE(int LUNCH_SEQ_CD = 0)
        {
            if (Request.IsAjaxRequest())
            {
                using (LunchServices service = new LunchServices())
                {
                    int result = service.DeleteLunchDeal(LUNCH_SEQ_CD);
                    JsonResult res = Json(new
                    {
                        statusCode = result > 0 ? 201 : 200
                    }, JsonRequestBehavior.AllowGet);

                    //Dictionary<String, Object> obj = new Dictionary<String, Object>();
                    //obj.Add("success", result);

                    //if (obj.Count > 0)
                    //{
                    //    return Json(obj, JsonRequestBehavior.AllowGet);
                    //}
                    //else
                    //{
                    //    return new EmptyResult();
                    //}
                    return res;

                    
                }
            }
            return new EmptyResult();
        }

        /// <summary>
        /// Create edit view model
        /// </summary>
        /// <param name="infoSeqNo"></param>
        /// <returns></returns>
        public LunchInformationEntity CreateEditViewModel(long infoSeqNo)
        {
            CommonService comService = new CommonService();
            LunchInformationEntity model = new LunchInformationEntity();
            using (LunchServices service = new LunchServices())
            {
                if (infoSeqNo > 0)
                {
                    model = service.GetLunchInformation(infoSeqNo);                    
                }               
            }
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult LunchList(LunchListModel model = null)
        {
            if (model == null)
            {
                model = new LunchListModel();
            }

            return View("LunchList", model);
        }

        /// <summary>
        /// TaxRate Maint Search
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult LunchSearch(DataTablesModel dt, LunchListModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    using (LunchServices service = new LunchServices())
                    {
                        int total_row;
                        var dataList = service.LunchSearch(dt, ref model, out total_row);
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
                                    i.LUNCH_SEQ_CD,
                                    order++,
                                    i.SETUP_USER_NAME,
                                    i.LUNCH_DATE.ToString("yyyy/MM/dd hh:mm"),                                    
                                    i.LUNCH_MENU_NAME,
                                    i.STATUS ==1 ? i.STATUS : 0
                                })
                        });
                        return result;
                    }
                }
            }
            return new EmptyResult();
        }
    }
}

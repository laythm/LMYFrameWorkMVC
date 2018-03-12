using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Globalization;
using LMYFrameWorkMVC.Common.Models;
using LMYFrameWorkMVC.Web.CommonCode;
using System.Threading; 
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.Common.Models.Common;
using LMYFrameWorkMVC.Common.Models.Common;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Truck.Load;
using LMYFrameWorkMVC.BAL.Modules.Admin.Load;
using LMYFrameWorkMVC.Web.Areas.Common.Controllers;

namespace LMYFrameWorkMVC.Web.Areas.Admin.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class TruckLoadController : BaseController
    {
        // GET: TruckLoads 
        public ActionResult Index(TruckLoadSearchModel truckLoadSearchModel)
        {
            using (TruckLoadBAL truckLoadBAL = new TruckLoadBAL(ContextInfo))
            {
                  truckLoadBAL.PrepareTruckLoadSearchModel(truckLoadSearchModel);
            }
            return View(truckLoadSearchModel);
        }

        public JsonResult GetTruckLoads(DataTableSearchParameters<TruckLoadSearchModel> dataTableSearchParameters)
        {
            GenericListModel<TruckLoadModel> baseListModel = new GenericListModel<TruckLoadModel>();

            using (TruckLoadBAL truckLoadBAL = new TruckLoadBAL(ContextInfo))
            {
                baseListModel = truckLoadBAL.GetSearchTrucksLoadsList(dataTableSearchParameters);
            }

            return Json(new
            {
                baseModel = baseListModel
            }, JsonRequestBehavior.AllowGet);
        }

        [ActionNames("Create", "TruckLoadsList-CreateLoad")]
        public ActionResult Create(BaseModel baseModel)
        {
            TruckLoadModel truckLoadModel = new TruckLoadModel();
            truckLoadModel.CopyBaseModel(baseModel);

            using (TruckLoadBAL truckLoadBAL = new TruckLoadBAL(ContextInfo))
            {
                truckLoadBAL.PrepareTruckLoadModel(truckLoadModel);
            }

            return View("Create", truckLoadModel);
        }

        [ActionNames("Create", "TruckLoadsList-CreateLoad")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TruckLoadModel truckLoadModel)
        {
            ModelState.Clear<TruckLoadModel>(x => x.TruckModel);
            ModelState.Clear<TruckLoadModel>(x => x.DriverModel);
            ModelState.Clear<TruckLoadModel>(x => x.CompanyModel);

            using (TruckLoadBAL truckLoadBAL = new TruckLoadBAL(ContextInfo))
            {
                if (ModelState.IsValid)
                {
                    truckLoadBAL.Create(truckLoadModel);
                }

                if (truckLoadModel.HasErrorByType(LMYFrameWorkMVC.Common.LookUps.ErrorType.Critical) || truckLoadModel.HasSuccess(LMYFrameWorkMVC.Common.LookUps.SuccessType.Full))
                {
                    return base.RedirectToActionWithData(new Dictionary<string, object> { { "baseModel", truckLoadModel } });
                }

                truckLoadBAL.PrepareTruckLoadModel(truckLoadModel);
            }

            return View("Create", truckLoadModel);
        }

        // GET: TruckLoad/Edit/5
        public ActionResult Edit(string id, BaseModel baseModel)
        {
            TruckLoadModel truckLoadModel = new TruckLoadModel();
            truckLoadModel.CopyBaseModel(baseModel);
            truckLoadModel.Id = id;

            using (TruckLoadBAL truckLoadBAL = new TruckLoadBAL(ContextInfo))
            {
                truckLoadBAL.GetTruckLoadModel(truckLoadModel);
                truckLoadBAL.PrepareTruckLoadModel(truckLoadModel);
            }

            return View(truckLoadModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TruckLoadModel truckLoadModel)
        {
            ModelState.Clear<TruckLoadModel>(x => x.TruckModel);
            ModelState.Clear<TruckLoadModel>(x => x.DriverModel);
            ModelState.Clear<TruckLoadModel>(x => x.CompanyModel);

            using (TruckLoadBAL truckLoadBAL = new TruckLoadBAL(ContextInfo))
            {
                if (ModelState.IsValid)
                {
                    truckLoadBAL.Edit(truckLoadModel);
                }

                if (truckLoadModel.HasErrorByType(LMYFrameWorkMVC.Common.LookUps.ErrorType.Critical) || truckLoadModel.HasSuccess(LMYFrameWorkMVC.Common.LookUps.SuccessType.Full))
                {
                    return base.RedirectToActionWithData(new Dictionary<string, object> { { "baseModel", truckLoadModel } }, new { id = truckLoadModel.Id });
                }

                truckLoadBAL.PrepareTruckLoadModel(truckLoadModel);
            }

            return View(truckLoadModel);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            TruckLoadModel truckLoadModel = new TruckLoadModel();
            truckLoadModel.Id = id;

            using (TruckLoadBAL truckLoadBAL = new TruckLoadBAL(ContextInfo))
            {
                truckLoadBAL.GetTruckLoadModel(truckLoadModel);
            }

            return View(truckLoadModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(TruckLoadModel truckLoadModel)
        {
            using (TruckLoadBAL truckLoadBAL = new TruckLoadBAL(ContextInfo))
            {
                truckLoadBAL.Delete(truckLoadModel);

                if (truckLoadModel.HasErrorByType())
                {
                    truckLoadBAL.GetTruckLoadModel(truckLoadModel);
                }
            }

            return View(truckLoadModel);
        }

        [HttpGet]
        public ActionResult View(string id)
        {
            TruckLoadModel truckLoadModel = new TruckLoadModel();
            truckLoadModel.Id = id;

            using (TruckLoadBAL truckLoadBAL = new TruckLoadBAL(ContextInfo))
            {
                truckLoadBAL.GetTruckLoadModel(truckLoadModel);
                // truckLoadBAL.PrepareTruckLoadModel(truckLoadModel);
            }

            return View(truckLoadModel);
        }
    }
}

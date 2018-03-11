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
using LMYFrameWorkMVC.BAL.Modules.Admin.Truck;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Truck;
using LMYFrameWorkMVC.Common.Models.Common;
using LMYFrameWorkMVC.Web.Areas.Common.Controllers;

namespace LMYFrameWorkMVC.Web.Areas.Admin.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class TruckController : BaseController
    {
        // GET: Truck
        public ActionResult Index(TruckModel returnedModel)
        {
            return View(returnedModel);
        }

        public JsonResult GetTrucks(DataTableSearchParameters<Nullable<bool>> dataTableSearchParameters)
        {
            GenericListModel<TruckModel> baseListModel = new GenericListModel<TruckModel>();

            using (TruckBAL truckBAL = new TruckBAL(ContextInfo))
            {
                baseListModel = truckBAL.GetSearchTrucksList(dataTableSearchParameters);
            }

            return Json(new
            {
                baseModel = baseListModel
            }, JsonRequestBehavior.AllowGet);
        }

        [ActionNames("Create", "TrucksList-Create")]
        public ActionResult Create(BaseModel baseModel)
        {
            TruckModel truckModel = new TruckModel();
            truckModel.CopyBaseModel(baseModel);

            using (TruckBAL truckBAL = new TruckBAL(ContextInfo))
            {
                truckBAL.PrepareTruckModel(truckModel);
            }

            return View("Create", truckModel);
        }

        [ActionNames("Create", "TrucksList-Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TruckModel truckModel)
        {
            ModelState.Clear<TruckModel>(x => x.DriverModel);

            using (TruckBAL truckBAL = new TruckBAL(ContextInfo))
            {
                if (ModelState.IsValid)
                {
                    truckBAL.Create(truckModel);
                }

                if (truckModel.HasErrorByType(LMYFrameWorkMVC.Common.LookUps.ErrorType.Critical) || truckModel.HasSuccess(LMYFrameWorkMVC.Common.LookUps.SuccessType.Full))
                {
                    return base.RedirectToActionWithData(new Dictionary<string, object> { { "baseModel", truckModel } });
                }

                truckBAL.PrepareTruckModel(truckModel);
            }

            return View("Create", truckModel);
        }

        // GET: Truck/Edit/5
        public ActionResult Edit(string id, BaseModel baseModel)
        {
            TruckModel truckModel = new TruckModel();
            truckModel.CopyBaseModel(baseModel);
            truckModel.Id = id;

            using (TruckBAL truckBAL = new TruckBAL(ContextInfo))
            {
                truckBAL.GetTruckModel(truckModel);
                truckBAL.PrepareTruckModel(truckModel);
            }

            return View(truckModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TruckModel truckModel)
        {
            ModelState.Clear<TruckModel>(x => x.DriverModel);

            using (TruckBAL truckBAL = new TruckBAL(ContextInfo))
            {
                if (ModelState.IsValid)
                {
                    truckBAL.Edit(truckModel);
                }

                if (truckModel.HasErrorByType(LMYFrameWorkMVC.Common.LookUps.ErrorType.Critical) || truckModel.HasSuccess(LMYFrameWorkMVC.Common.LookUps.SuccessType.Full))
                {
                    return base.RedirectToActionWithData(new Dictionary<string, object> { { "baseModel", truckModel } }, new { id = truckModel.Id });
                }

                truckBAL.PrepareTruckModel(truckModel);
            }

            return View(truckModel);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            TruckModel truckModel = new TruckModel();
            truckModel.Id = id;

            using (TruckBAL truckBAL = new TruckBAL(ContextInfo))
            {
                truckBAL.GetTruckModel(truckModel);
                // truckBAL.PrepareTruckModel(truckModel);
            }

            return View(truckModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(TruckModel truckModel)
        {
            using (TruckBAL truckBAL = new TruckBAL(ContextInfo))
            {
                truckBAL.Delete(truckModel);

                if (truckModel.HasErrorByType())
                {
                    truckBAL.GetTruckModel(truckModel);
                    //     truckBAL.PrepareTruckModel(truckModel);
                }
            }

            return View(truckModel);
        }

        [HttpGet]
        public ActionResult View(string id)
        {
            TruckModel truckModel = new TruckModel();
            truckModel.Id = id;

            using (TruckBAL truckBAL = new TruckBAL(ContextInfo))
            {
                truckBAL.GetTruckModel(truckModel);
                // truckBAL.PrepareTruckModel(truckModel);
            }

            return View(truckModel);
        }

        [ActionNames(
                  "TruckLoadCreate-GetTrucksBySelect2Parameters",
                  "TruckLoadsList-GetTrucksBySelect2Parameters",
                  "TruckExpenseCreate-GetTrucksBySelect2Parameters",
                  "TruckExpensesList-GetTrucksBySelect2Parameters"
                  )]
        public JsonResult GetTrucksBySelect2Parameters(Select2Parameters<bool> select2Parameters)
        {
            GenericListModel<TruckModel> baseListModel = new GenericListModel<TruckModel>();

            using (TruckBAL truckBAL = new TruckBAL(ContextInfo))
            {
                baseListModel = truckBAL.GetTrucks(select2Parameters);
            }

            return Json(new
            {
                baseModel = baseListModel
            }, JsonRequestBehavior.AllowGet);
        }
    }
}

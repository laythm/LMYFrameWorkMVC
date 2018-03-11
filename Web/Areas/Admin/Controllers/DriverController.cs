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
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Driver;
using LMYFrameWorkMVC.BAL.Modules.Admin.Administration;
using LMYFrameWorkMVC.Common.Models.Common;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Truck;
using LMYFrameWorkMVC.BAL.Modules.Admin.Truck;
using LMYFrameWorkMVC.Web.Areas.Common.Controllers;

namespace LMYFrameWorkMVC.Web.Areas.Admin.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class DriverController : BaseController
    {
        // GET: Driver
        public ActionResult Index(DriverModel returnedModel)
        {
            return View(returnedModel);
        }

        public JsonResult GetDrivers(DataTableSearchParameters<Nullable<bool>> dataTableSearchParameters)
        {
            GenericListModel<DriverModel> baseListModel = new GenericListModel<DriverModel>();

            using (DriverBAL driverBAL = new DriverBAL(ContextInfo))
            {
                baseListModel = driverBAL.GetSearchDriversList(dataTableSearchParameters);
            }

            return Json(new
            {
                baseModel = baseListModel
            }, JsonRequestBehavior.AllowGet);
        }

        [ActionNames("Create", "DriversList-Create")]
        public ActionResult Create(BaseModel baseModel)
        {
            DriverModel driverModel = new DriverModel();
            driverModel.CopyBaseModel(baseModel);

            using (DriverBAL driverBAL = new DriverBAL(ContextInfo))
            {
                driverBAL.PrepareDriverModel(driverModel);
            }

            return View("Create", driverModel);
        }

        [ActionNames("Create", "DriversList-Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DriverModel driverModel)
        {
            using (DriverBAL driverBAL = new DriverBAL(ContextInfo))
            {
                if (ModelState.IsValid)
                {
                    driverBAL.Create(driverModel);
                }

                if (driverModel.HasErrorByType(LMYFrameWorkMVC.Common.LookUps.ErrorType.Critical) || driverModel.HasSuccess(LMYFrameWorkMVC.Common.LookUps.SuccessType.Full))
                {
                    return base.RedirectToActionWithData(new Dictionary<string, object> { { "baseModel", driverModel } });
                }

                driverBAL.PrepareDriverModel(driverModel);
            }

            return View("Create", driverModel);
        }

        // GET: Driver/Edit/5
        public ActionResult Edit(string id, BaseModel baseModel)
        {
            DriverModel driverModel = new DriverModel();
            driverModel.CopyBaseModel(baseModel);
            driverModel.Id = id;

            using (DriverBAL driverBAL = new DriverBAL(ContextInfo))
            {
                driverBAL.GetDriverModel(driverModel);
                driverBAL.PrepareDriverModel(driverModel);
            }

            return View(driverModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DriverModel driverModel)
        {
            using (DriverBAL driverBAL = new DriverBAL(ContextInfo))
            {
                if (ModelState.IsValid)
                {
                    driverBAL.Edit(driverModel);
                }

                if (driverModel.HasErrorByType(LMYFrameWorkMVC.Common.LookUps.ErrorType.Critical) || driverModel.HasSuccess(LMYFrameWorkMVC.Common.LookUps.SuccessType.Full))
                {
                    return base.RedirectToActionWithData(new Dictionary<string, object> { { "baseModel", driverModel } }, new { id = driverModel.Id });
                }

                driverBAL.PrepareDriverModel(driverModel);
            }

            return View(driverModel);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            DriverModel driverModel = new DriverModel();
            driverModel.Id = id;

            using (DriverBAL driverBAL = new DriverBAL(ContextInfo))
            {
                driverBAL.GetDriverModel(driverModel);
                // driverBAL.PrepareDriverModel(driverModel);
            }

            return View(driverModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DriverModel driverModel)
        {
            using (DriverBAL driverBAL = new DriverBAL(ContextInfo))
            {
                driverBAL.Delete(driverModel);

                if (driverModel.HasErrorByType())
                {
                    driverBAL.GetDriverModel(driverModel);
                    //     driverBAL.PrepareDriverModel(driverModel);
                }
            }

            return View(driverModel);
        }

        [ActionNames("View", "Driver-ViewDriver", "Load-ViewDriver")]
        [HttpGet]
        public ActionResult View(string id)
        {
            DriverModel driverModel = new DriverModel();
            driverModel.Id = id;

            using (DriverBAL driverBAL = new DriverBAL(ContextInfo))
            {
                driverBAL.GetDriverModel(driverModel);
                // driverBAL.PrepareDriverModel(driverModel);
            }

            return View(driverModel);
        }

        
        [HttpGet]
        public ActionResult ViewTruck(string id)
        {
            TruckModel truckModel = new TruckModel();
            truckModel.Id = id;

            using (TruckBAL truckBAL = new TruckBAL(ContextInfo))
            {
                truckBAL.GetTruckModel(truckModel);
            }

            return View(truckModel);
        }

        [ActionNames(
            "TruckCreate-GetDriversBySelect2Parameters",
            "TruckEdit-GetDriversBySelect2Parameters",
            "TruckLoadCreate-GetDriversBySelect2Parameters",
            "TruckLoadEdit-GetDriversBySelect2Parameters",
            "TruckLoadsList-GetDriversBySelect2Parameters",
            "TruckExpensesList-GetDriversBySelect2Parameters"
            )]
        public JsonResult GetDriversBySelect2Parameters(Select2Parameters<bool> select2Parameters)
        {
            GenericListModel<DriverModel> baseListModel = new GenericListModel<DriverModel>();

            using (DriverBAL driverBAL = new DriverBAL(ContextInfo))
            {
                baseListModel = driverBAL.GetDrivers(select2Parameters);
            }

            return Json(new
            {
                baseModel = baseListModel
            }, JsonRequestBehavior.AllowGet);
        }
    }
}

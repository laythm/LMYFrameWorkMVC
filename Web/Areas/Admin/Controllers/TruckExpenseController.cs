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
using LMYFrameWorkMVC.BAL.Modules.Admin.TruckExpenses;
using LMYFrameWorkMVC.Common.Models.Common;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Truck.TruckExpense;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Truck;
using LMYFrameWorkMVC.Web.Areas.Common.Controllers;

namespace LMYFrameWorkMVC.Web.Areas.Admin.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class TruckExpenseController : BaseController
    {
        // GET: TruckExpense
        public ActionResult Index(TruckExpenseSearchModel truckExpenseSearchModel)
        {
            using (TruckExpenseBAL truckExpenseBAL = new TruckExpenseBAL(ContextInfo))
            {
                truckExpenseBAL.PrepareTruckExpenseSearchModel(truckExpenseSearchModel);
            }

            return View(truckExpenseSearchModel);
        }

        public JsonResult GetTruckExpenses(DataTableSearchParameters<TruckExpenseSearchModel> dataTableSearchParameters)
        {
            GenericListModel<TruckExpenseModel> baseListModel = new GenericListModel<TruckExpenseModel>();

            using (TruckExpenseBAL truckExpenseBAL = new TruckExpenseBAL(ContextInfo))
            {
                baseListModel = truckExpenseBAL.GetSearchTruckExpensessList(dataTableSearchParameters);
            }

            return Json(new
            {
                baseModel = baseListModel
            }, JsonRequestBehavior.AllowGet);
        }

        [ActionNames("Create", "TruckExpensesList-CreateExpense")]
        public ActionResult Create(BaseModel baseModel)
        {
            TruckExpenseModel truckExpenseModel = new TruckExpenseModel();
            truckExpenseModel.CopyBaseModel(baseModel);

            using (TruckExpenseBAL truckExpenseBAL = new TruckExpenseBAL(ContextInfo))
            {
                truckExpenseBAL.PrepareTruckExpenseModel(truckExpenseModel);
            }

            return View("Create", truckExpenseModel);
        }

        [ActionNames("Create", "TruckExpensesList-CreateExpense")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TruckExpenseModel truckExpenseModel)
        {
            ModelState.Clear<TruckExpenseModel>(x => x.TruckModel);

            using (TruckExpenseBAL truckExpenseBAL = new TruckExpenseBAL(ContextInfo))
            {
                if (ModelState.IsValid)
                {
                    truckExpenseBAL.Create(truckExpenseModel);
                }

                if (truckExpenseModel.HasErrorByType(LMYFrameWorkMVC.Common.LookUps.ErrorType.Critical) || truckExpenseModel.HasSuccess(LMYFrameWorkMVC.Common.LookUps.SuccessType.Full))
                {
                    return base.RedirectToActionWithData(new Dictionary<string, object> { { "baseModel", truckExpenseModel } });
                }

                truckExpenseBAL.PrepareTruckExpenseModel(truckExpenseModel);
            }

            return View("Create", truckExpenseModel);
        }

        // GET: TruckExpense/Edit/5
        public ActionResult Edit(string id, BaseModel baseModel)
        {
            TruckExpenseModel truckExpenseModel = new TruckExpenseModel();
            truckExpenseModel.CopyBaseModel(baseModel);
            truckExpenseModel.Id = id;

            using (TruckExpenseBAL truckExpenseBAL = new TruckExpenseBAL(ContextInfo))
            {
                truckExpenseBAL.GetTruckExpenseModel(truckExpenseModel);
                truckExpenseBAL.PrepareTruckExpenseModel(truckExpenseModel);
            }

            return View(truckExpenseModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TruckExpenseModel truckExpenseModel)
        {
            ModelState.Clear<TruckExpenseModel>(x => x.TruckModel);

            using (TruckExpenseBAL truckExpenseBAL = new TruckExpenseBAL(ContextInfo))
            {
                if (ModelState.IsValid)
                {
                    truckExpenseBAL.Edit(truckExpenseModel);
                }

                if (truckExpenseModel.HasErrorByType(LMYFrameWorkMVC.Common.LookUps.ErrorType.Critical) || truckExpenseModel.HasSuccess(LMYFrameWorkMVC.Common.LookUps.SuccessType.Full))
                {
                    return base.RedirectToActionWithData(new Dictionary<string, object> { { "baseModel", truckExpenseModel } }, new { id = truckExpenseModel.Id });
                }

                truckExpenseBAL.PrepareTruckExpenseModel(truckExpenseModel);
            }

            return View(truckExpenseModel);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            TruckExpenseModel truckExpenseModel = new TruckExpenseModel();
            truckExpenseModel.Id = id;

            using (TruckExpenseBAL truckExpenseBAL = new TruckExpenseBAL(ContextInfo))
            {
                truckExpenseBAL.GetTruckExpenseModel(truckExpenseModel);
            }

            return View(truckExpenseModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(TruckExpenseModel truckExpenseModel)
        {
            using (TruckExpenseBAL truckExpenseBAL = new TruckExpenseBAL(ContextInfo))
            {
                truckExpenseBAL.Delete(truckExpenseModel);

                if (truckExpenseModel.HasErrorByType())
                {
                    truckExpenseBAL.GetTruckExpenseModel(truckExpenseModel);
                }
            }

            return View(truckExpenseModel);
        }
 
        [HttpGet]
        public ActionResult View(string id)
        {
            TruckExpenseModel truckExpenseModel = new TruckExpenseModel();
            truckExpenseModel.Id = id;

            using (TruckExpenseBAL truckExpenseBAL = new TruckExpenseBAL(ContextInfo))
            {
                truckExpenseBAL.GetTruckExpenseModel(truckExpenseModel);
                // truckExpenseBAL.PrepareTruckExpenseModel(truckExpenseModel);
            }

            return View(truckExpenseModel);
        }
    }
}

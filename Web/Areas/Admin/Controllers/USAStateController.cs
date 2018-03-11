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
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.USAState;
using LMYFrameWorkMVC.BAL.Modules.Admin.Administration;
using LMYFrameWorkMVC.Web.Areas.Common.Controllers;

namespace LMYFrameWorkMVC.Web.Areas.Admin.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class USAStateController : BaseController
    {
        // GET: USAState
        public ActionResult Index(USAStateModel returnedModel)
        {
            return View(returnedModel);
        }

        public JsonResult GetUSAStates(DataTableSearchParameters<Nullable<bool>> dataTableSearchParameters)
        {
            GenericListModel<USAStateModel> baseListModel = new GenericListModel<USAStateModel>();

            using (USAStateBAL uSAStateBAL = new USAStateBAL(ContextInfo))
            {
                baseListModel = uSAStateBAL.GetSearchUSAStatesList(dataTableSearchParameters);
            }

            return Json(new
            {
                baseModel = baseListModel
            }, JsonRequestBehavior.AllowGet);
        }

        [ActionNames("View", "USAStatesList-Create")]
        public ActionResult Create(BaseModel baseModel)
        {
            USAStateModel uSAStateModel = new USAStateModel();
            uSAStateModel.CopyBaseModel(baseModel);

            using (USAStateBAL uSAStateBAL = new USAStateBAL(ContextInfo))
            {
                uSAStateBAL.PrepareUSAStateModel(uSAStateModel);
            }

            return View("Create", uSAStateModel);
        }

        [ActionNames("View", "USAStatesList-Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(USAStateModel uSAStateModel)
        {
            using (USAStateBAL uSAStateBAL = new USAStateBAL(ContextInfo))
            {
                if (ModelState.IsValid)
                {
                    uSAStateBAL.Create(uSAStateModel);
                }

                if (uSAStateModel.HasErrorByType(LMYFrameWorkMVC.Common.LookUps.ErrorType.Critical) || uSAStateModel.HasSuccess(LMYFrameWorkMVC.Common.LookUps.SuccessType.Full))
                {
                    return base.RedirectToActionWithData(new Dictionary<string, object> { { "baseModel", uSAStateModel } });
                }

                uSAStateBAL.PrepareUSAStateModel(uSAStateModel);
            }

            return View("Create", uSAStateModel);
        }

        // GET: USAState/Edit/5
        public ActionResult Edit(string id, BaseModel baseModel)
        {
            USAStateModel uSAStateModel = new USAStateModel();
            uSAStateModel.CopyBaseModel(baseModel);
            uSAStateModel.Id = id;

            using (USAStateBAL uSAStateBAL = new USAStateBAL(ContextInfo))
            {
                uSAStateBAL.GetUSAStateModel(uSAStateModel);
                uSAStateBAL.PrepareUSAStateModel(uSAStateModel);
            }

            return View(uSAStateModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(USAStateModel uSAStateModel)
        {
            using (USAStateBAL uSAStateBAL = new USAStateBAL(ContextInfo))
            {
                if (ModelState.IsValid)
                {
                    uSAStateBAL.Edit(uSAStateModel);
                }

                if (uSAStateModel.HasErrorByType(LMYFrameWorkMVC.Common.LookUps.ErrorType.Critical) || uSAStateModel.HasSuccess(LMYFrameWorkMVC.Common.LookUps.SuccessType.Full))
                {
                    return base.RedirectToActionWithData(new Dictionary<string, object> { { "baseModel", uSAStateModel } }, new { id = uSAStateModel.Id });
                }

                uSAStateBAL.PrepareUSAStateModel(uSAStateModel);
            }

            return View(uSAStateModel);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            USAStateModel uSAStateModel = new USAStateModel();
            uSAStateModel.Id = id;

            using (USAStateBAL uSAStateBAL = new USAStateBAL(ContextInfo))
            {
                uSAStateBAL.GetUSAStateModel(uSAStateModel);
                // uSAStateBAL.PrepareUSAStateModel(uSAStateModel);
            }

            return View(uSAStateModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(USAStateModel uSAStateModel)
        {
            using (USAStateBAL uSAStateBAL = new USAStateBAL(ContextInfo))
            {
                uSAStateBAL.Delete(uSAStateModel);

                if (uSAStateModel.HasErrorByType())
                {
                    uSAStateBAL.GetUSAStateModel(uSAStateModel);
                    //     uSAStateBAL.PrepareUSAStateModel(uSAStateModel);
                }
            }

            return View(uSAStateModel);
        }

        [ActionNames("View", "Truck-USAState-View", "Load-USAState-View")]
        [HttpGet]
        public ActionResult View(string id)
        {
            USAStateModel uSAStateModel = new USAStateModel();
            uSAStateModel.Id = id;

            using (USAStateBAL uSAStateBAL = new USAStateBAL(ContextInfo))
            {
                uSAStateBAL.GetUSAStateModel(uSAStateModel);
                // uSAStateBAL.PrepareUSAStateModel(uSAStateModel);
            }

            return View(uSAStateModel);
        }
    }
}

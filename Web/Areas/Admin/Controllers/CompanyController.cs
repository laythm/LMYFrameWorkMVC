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
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Company;
using LMYFrameWorkMVC.BAL.Modules.Admin.Administration;
using LMYFrameWorkMVC.Common.Models.Common;
using LMYFrameWorkMVC.Web.Areas.Common.Controllers;

namespace LMYFrameWorkMVC.Web.Areas.Admin.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class CompanyController : BaseController
    {
        // GET: Company
        public ActionResult Index(CompanyModel returnedModel)
        {
            return View(returnedModel);
        }

        public JsonResult GetCompanies(DataTableSearchParameters<Nullable<bool>> dataTableSearchParameters)
        {
            GenericListModel<CompanyModel> baseListModel = new GenericListModel<CompanyModel>();

            using (CompanyBAL companyBAL = new CompanyBAL(ContextInfo))
            {
                baseListModel = companyBAL.GetSearchCompaniesList(dataTableSearchParameters);
            }

            return Json(new
            {
                baseModel = baseListModel
            }, JsonRequestBehavior.AllowGet);
        }

        [ActionNames("View", "CompaniesList-Create")]
        public ActionResult Create(BaseModel baseModel)
        {
            CompanyModel companyModel = new CompanyModel();
            companyModel.CopyBaseModel(baseModel);

            using (CompanyBAL companyBAL = new CompanyBAL(ContextInfo))
            {
                companyBAL.PrepareCompanyModel(companyModel);
            }

            return View("Create", companyModel);
        }

        [ActionNames("View", "CompaniesList-Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyModel companyModel)
        {
            using (CompanyBAL companyBAL = new CompanyBAL(ContextInfo))
            {
                if (ModelState.IsValid)
                {
                    companyBAL.Create(companyModel);
                }

                if (companyModel.HasErrorByType(LMYFrameWorkMVC.Common.LookUps.ErrorType.Critical) || companyModel.HasSuccess(LMYFrameWorkMVC.Common.LookUps.SuccessType.Full))
                {
                    return base.RedirectToActionWithData(new Dictionary<string, object> { { "baseModel", companyModel } });
                }

                companyBAL.PrepareCompanyModel(companyModel);
            }

            return View("Create", companyModel);
        }

        // GET: Company/Edit/5
        public ActionResult Edit(string id, BaseModel baseModel)
        {
            CompanyModel companyModel = new CompanyModel();
            companyModel.CopyBaseModel(baseModel);
            companyModel.Id = id;

            using (CompanyBAL companyBAL = new CompanyBAL(ContextInfo))
            {
                companyBAL.GetCompanyModel(companyModel);
                companyBAL.PrepareCompanyModel(companyModel);
            }

            return View(companyModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyModel companyModel)
        {
            using (CompanyBAL companyBAL = new CompanyBAL(ContextInfo))
            {
                if (ModelState.IsValid)
                {
                    companyBAL.Edit(companyModel);
                }

                if (companyModel.HasErrorByType(LMYFrameWorkMVC.Common.LookUps.ErrorType.Critical) || companyModel.HasSuccess(LMYFrameWorkMVC.Common.LookUps.SuccessType.Full))
                {
                    return base.RedirectToActionWithData(new Dictionary<string, object> { { "baseModel", companyModel } }, new { id = companyModel.Id });
                }

                companyBAL.PrepareCompanyModel(companyModel);
            }

            return View(companyModel);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            CompanyModel companyModel = new CompanyModel();
            companyModel.Id = id;

            using (CompanyBAL companyBAL = new CompanyBAL(ContextInfo))
            {
                companyBAL.GetCompanyModel(companyModel);
                // companyBAL.PrepareCompanyModel(companyModel);
            }

            return View(companyModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CompanyModel companyModel)
        {
            using (CompanyBAL companyBAL = new CompanyBAL(ContextInfo))
            {
                companyBAL.Delete(companyModel);

                if (companyModel.HasErrorByType())
                {
                    companyBAL.GetCompanyModel(companyModel);
                    //     companyBAL.PrepareCompanyModel(companyModel);
                }
            }

            return View(companyModel);
        }

        [ActionNames("View", "Load-Company-View")]
        [HttpGet]
        public ActionResult View(string id)
        {
            CompanyModel companyModel = new CompanyModel();
            companyModel.Id = id;

            using (CompanyBAL companyBAL = new CompanyBAL(ContextInfo))
            {
                companyBAL.GetCompanyModel(companyModel);
                // companyBAL.PrepareCompanyModel(companyModel);
            }

            return View(companyModel);
        }

        [ActionNames(
         "TruckLoadCreate-GetCompaniesBySelect2Parameters",
         "TruckLoadEdit-GetCompaniesBySelect2Parameters",
         "TruckLoadsList-GetCompaniesBySelect2Parameters"
         )]
        public JsonResult GetCompaniesBySelect2Parameters(Select2Parameters<bool> select2Parameters)
        {
            GenericListModel<CompanyModel> baseListModel = new GenericListModel<CompanyModel>();

            using (CompanyBAL companyBAL = new CompanyBAL(ContextInfo))
            {
                baseListModel = companyBAL.GetCompanies(select2Parameters);
            }

            return Json(new
            {
                baseModel = baseListModel
            }, JsonRequestBehavior.AllowGet);
        }
    }
}

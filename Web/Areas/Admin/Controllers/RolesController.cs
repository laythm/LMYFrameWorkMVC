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
using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Models.Common;
using LMYFrameWorkMVC.BAL.Modules.Admin.Administration;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Role;
using MvcSiteMapProvider;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.Web.Areas.Admin.Controllers;
using LMYFrameWorkMVC.Web.Areas.Common.Controllers;

namespace LMYFrameWorkMVC.Web.Areas.Admin.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class RolesController : BaseController
    {
        public ActionResult Index(RoleModel returnedModel)
        {
            return View(returnedModel);
        }

        public JsonResult GetRoles(DataTableSearchParameters<Nullable<bool>> dataTableSearchParameters)
        {
            GenericListModel<RoleModel> baseListModel = new GenericListModel<RoleModel>();

            using (RoleBAL roleBAL = new RoleBAL(ContextInfo))
            {
                baseListModel = roleBAL.GetSearchRolesList(dataTableSearchParameters);
            }

            return Json(new
            {
                baseModel = baseListModel
            }, JsonRequestBehavior.AllowGet);
        }

        [ActionNames("Create", "RolesList-Create")]
        public ActionResult Create(BaseModel baseModel)
        {
            RoleModel roleModel = new RoleModel();
            roleModel.CopyBaseModel(baseModel);

            using (RoleBAL roleBAL = new RoleBAL(ContextInfo))
            {
                roleBAL.PrepareRoleModel(roleModel);
            }

            SiteMapHelper.PrepareSiteMapModel(roleModel.SiteMapModel);

            return View("Create", roleModel);
        }

        [ActionNames("Create", "RolesList-Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoleModel roleModel)
        {
            using (RoleBAL roleBAL = new RoleBAL(ContextInfo))
            {
                if (ModelState.IsValid)
                {
                    roleBAL.Create(roleModel);
                }

                if (roleModel.HasErrorByType(LMYFrameWorkMVC.Common.LookUps.ErrorType.Critical) || roleModel.HasSuccess(LMYFrameWorkMVC.Common.LookUps.SuccessType.Full))
                {
                    return base.RedirectToActionWithData(new Dictionary<string, object> { { "baseModel", roleModel } } );
                }

                roleBAL.GetRoleModel(roleModel);
            }

            return View("Create", roleModel);
        }

        // GET: Role/Edit/5
        public ActionResult Edit(string id, BaseModel baseModel)
        {
            RoleModel roleModel = new RoleModel();
            roleModel.CopyBaseModel(baseModel);
            roleModel.Id = id;

            using (RoleBAL roleBAL = new RoleBAL(ContextInfo))
            {
                roleBAL.GetRoleModel(roleModel);
            }

            SiteMapHelper.PrepareSiteMapModel(roleModel.SiteMapModel, roleModel.Id);

            return View(roleModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoleModel roleModel)
        {
            using (RoleBAL roleBAL = new RoleBAL(ContextInfo))
            {
                if (ModelState.IsValid)
                {
                    roleBAL.Edit(roleModel);
                }

                if (roleModel.HasErrorByType(LMYFrameWorkMVC.Common.LookUps.ErrorType.Critical) || roleModel.HasSuccess(LMYFrameWorkMVC.Common.LookUps.SuccessType.Full))
                {
                    return base.RedirectToActionWithData(new Dictionary<string, object> { { "baseModel", roleModel } }, new { id = roleModel.Id });
                }

                roleBAL.GetRoleModel(roleModel);
            }

            return View(roleModel);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            RoleModel roleModel = new RoleModel();
            roleModel.Id = id;

            using (RoleBAL roleBAL = new RoleBAL(ContextInfo))
            {
                roleBAL.GetRoleModel(roleModel);
            }

            SiteMapHelper.PrepareSiteMapModel(roleModel.SiteMapModel, roleModel.Id);

            return View(roleModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(RoleModel roleModel)
        {
            using (RoleBAL roleBAL = new RoleBAL(ContextInfo))
            {
                roleBAL.Delete(roleModel);
                if (roleModel.HasErrorByType())
                    roleBAL.GetRoleModel(roleModel);
            }
 
            if (roleModel.HasErrorByType())
                SiteMapHelper.PrepareSiteMapModel(roleModel.SiteMapModel, roleModel.Id);

            return View(roleModel);
        }
    }
}

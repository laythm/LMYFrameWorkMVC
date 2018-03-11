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
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.User;
using LMYFrameWorkMVC.BAL.Modules.Admin.Administration;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.Web.Areas.Common.Controllers;
using System.Reflection;

namespace LMYFrameWorkMVC.Web.Areas.Admin.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index(UserModel returnedModel)
        {
            return View(returnedModel);
        }

        public JsonResult GetUsers(DataTableSearchParameters<Nullable<bool>> dataTableSearchParameters)
        {
            GenericListModel<UserModel> baseListModel = new GenericListModel<UserModel>();

            using (UserBAL userBAL = new UserBAL(ContextInfo))
            {
                baseListModel = userBAL.GetSearchUsersList(dataTableSearchParameters);
            }

            return Json(new
            {
                baseModel = baseListModel
            }, JsonRequestBehavior.AllowGet);
        }

        [ActionNames("Create", "UsersList-Create")]
        public ActionResult Create(BaseModel baseModel)
        {
            UserModel userModel = new UserModel();
            userModel.CopyBaseModel(baseModel);

            using (UserBAL userBAL = new UserBAL(ContextInfo))
            {
                userBAL.PrepareUserModel(userModel);
            }

            return View("Create", userModel);
        }

        [ActionNames("Create", "UsersList-Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserModel userModel)
        {
            using (UserBAL userBAL = new UserBAL(ContextInfo))
            {
                if (ModelState.IsValid)
                {
                    userBAL.Create(userModel, new Emails.User.RegestrationCompletedHelper(Url));
                }

                if (userModel.HasErrorByType(LMYFrameWorkMVC.Common.LookUps.ErrorType.Critical) || userModel.HasSuccess(LMYFrameWorkMVC.Common.LookUps.SuccessType.Full))
                {
                    return base.RedirectToActionWithData(new Dictionary<string, object> { { "baseModel", userModel } });
                }

                userBAL.PrepareUserModel(userModel);
            }

            return View("Create", userModel);
        }

        // GET: User/Edit/5
        public ActionResult Edit(string id, BaseModel baseModel)
        {
            UserModel userModel = new UserModel();
            userModel.CopyBaseModel(baseModel);
            userModel.Id = id;

            using (UserBAL userBAL = new UserBAL(ContextInfo))
            {
                userBAL.GetUserModel(userModel);
                userBAL.PrepareUserModel(userModel);
            }

            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserModel userModel)
        {
            ModelState.Clear<UserModel>(x => x.Password);

            using (UserBAL userBAL = new UserBAL(ContextInfo))
            {
                if (ModelState.IsValid)
                {
                    userBAL.Edit(userModel);
                }

                if (userModel.HasErrorByType(LMYFrameWorkMVC.Common.LookUps.ErrorType.Critical) || userModel.HasSuccess(LMYFrameWorkMVC.Common.LookUps.SuccessType.Full))
                {
                    return base.RedirectToActionWithData(new Dictionary<string, object> { { "baseModel", userModel } }, new { id = userModel.Id });
                }

                userBAL.PrepareUserModel(userModel);
            }

            return View(userModel);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            UserModel userModel = new UserModel();
            userModel.Id = id;

            using (UserBAL userBAL = new UserBAL(ContextInfo))
            {
                userBAL.GetUserModel(userModel);
                // userBAL.PrepareUserModel(userModel);
            }

            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(UserModel userModel)
        {
            using (UserBAL userBAL = new UserBAL(ContextInfo))
            {
                userBAL.Delete(userModel);

                if (userModel.HasErrorByType())
                {
                    userBAL.GetUserModel(userModel);
                    //     userBAL.PrepareUserModel(userModel);
                }
            }

            return View(userModel);
        }

        // GET: User/Edit/5
        public ActionResult ChangePassword(string id, BaseModel baseModel)
        {
            UserModel userModel = new UserModel();
            userModel.CopyBaseModel(baseModel);
            userModel.Id = id;

            using (UserBAL userBAL = new UserBAL(ContextInfo))
            {
                userBAL.GetUserModel(userModel);
                //userBAL.PrepareUserModel(userModel);
            }

            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(UserModel userModel)
        {
            //List<string> validateList = new List<string>() {
            //    userModel.nameof(x=>x.Password)
            //};
            //ModelState.ClearAllExcept<UserModel>(validateList);

            using (UserBAL userBAL = new UserBAL(ContextInfo))
            {
                if (ModelState.IsValidFor<UserModel>(x => x.Password))
                {
                    userBAL.ChangePassword(userModel);
                }

                if (userModel.HasErrorByType(LMYFrameWorkMVC.Common.LookUps.ErrorType.Critical) || userModel.HasSuccess(LMYFrameWorkMVC.Common.LookUps.SuccessType.Full))
                {
                    return base.RedirectToActionWithData(new Dictionary<string, object> { { "baseModel", userModel } }, "Edit");
                }

                //  userBAL.PrepareUserModel(userModel);
            }

            return View(userModel);
        }
    }
}

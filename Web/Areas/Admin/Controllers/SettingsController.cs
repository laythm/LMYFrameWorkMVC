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
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Setting;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.Web.Areas.Common.Controllers;

namespace LMYFrameWorkMVC.Web.Areas.Admin.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class SettingsController : BaseController
    {

        public ActionResult Index(SettingModel returnedModel)
        {
            return View(returnedModel);
        }

        public JsonResult GetSettings(DataTableSearchParameters<Nullable<bool>> dataTableSearchParameters)
        {
            GenericListModel<SettingModel> baseListModel = new GenericListModel<SettingModel>();

            using (SettingBAL settingBAL = new SettingBAL(ContextInfo))
            {
                baseListModel = settingBAL.GetSearchSettingsList(dataTableSearchParameters);
            }

            return Json(new
            {
                baseModel = baseListModel
            }, JsonRequestBehavior.AllowGet);
        }

        // GET: Setting/Edit/5
        public ActionResult Edit(string id, BaseModel baseModel)
        {
            SettingModel SettingModel = new SettingModel();
            SettingModel.CopyBaseModel(baseModel);
            SettingModel.Id = id;

            using (SettingBAL settingBAL = new SettingBAL(ContextInfo))
            {
                settingBAL.GetSettingModel(SettingModel);
            }

            return View(SettingModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SettingModel SettingModel)
        {
         //   List<SettingFileModel> lstOldSettingFileModel = new List<SettingFileModel>();
           // string oldImagePath = null;

            if (SettingModel.Type == LookUps.SettingsTypes.String)
            {
                ModelState.Remove(SettingModel.nameof(x => x.IntValue));
                ModelState.Remove(SettingModel.nameof(x => x.imageValue));
                ModelState.Remove(SettingModel.nameof(x => x.BoolValue));
            }
            else if (SettingModel.Type == LookUps.SettingsTypes.Bool)
            {
                ModelState.Remove(SettingModel.nameof(x => x.IntValue));
                ModelState.Remove(SettingModel.nameof(x => x.Value));
                ModelState.Remove(SettingModel.nameof(x => x.imageValue));
            }
            else if (SettingModel.Type == LookUps.SettingsTypes.Int)
            {
                ModelState.Remove(SettingModel.nameof(x => x.Value));
                ModelState.Remove(SettingModel.nameof(x => x.imageValue));
                ModelState.Remove(SettingModel.nameof(x => x.BoolValue));
            }
            else if (SettingModel.Type == LookUps.SettingsTypes.image)
            {
                ModelState.Remove(SettingModel.nameof(x => x.Value));
                ModelState.Remove(SettingModel.nameof(x => x.IntValue));
                ModelState.Remove(SettingModel.nameof(x => x.BoolValue));
            }

            using (SettingBAL settingBAL = new SettingBAL(ContextInfo))
            {
                if (ModelState.IsValid)
                {
                    settingBAL.Edit(SettingModel);
                }

                if (SettingModel.HasErrorByType(LMYFrameWorkMVC.Common.LookUps.ErrorType.Critical) || SettingModel.HasSuccess(LMYFrameWorkMVC.Common.LookUps.SuccessType.Full))
                {
                    return base.RedirectToActionWithData(new Dictionary<string, object> { { "baseModel", SettingModel } }, "Edit");
                }

                settingBAL.GetSettingModel(SettingModel);
            }

            return View(SettingModel);
        }
    }
}

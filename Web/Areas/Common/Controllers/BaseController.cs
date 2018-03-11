using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using LMYFrameWorkMVC.Common.Entities;
using LMYFrameWorkMVC.Web.CommonCode;
using LMYFrameWorkMVC.Common.Models;
using LMYFrameWorkMVC.Common.Models.Common;
using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Web.Areas.Admin.Controllers;

namespace LMYFrameWorkMVC.Web.Areas.Common.Controllers
{
    [SiteMapAuthorize]
    public class BaseController : Controller
    {
        protected ContextInfo ContextInfo { get; set; }

        protected RedirectToRouteResult RedirectToActionWithData(Dictionary<string, object> listOfValuse,  object routeValues = null,string view = null, string controller = null)
        {
            if (listOfValuse != null)
                foreach (string key in listOfValuse.Keys)
                {
                    TempData[key] = listOfValuse[key];
                }

            // TempData["ModelType"] = model.GetType();

            // controller= controller!=null? controller:HttpContext.cu

            return RedirectToAction(view, controller, routeValues);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (ContextInfo == null)
                ContextInfo = ContextInfoInitilizer.GetContextInfo(HttpContext, User);

            foreach (string key in TempData.Keys)
            {
                filterContext.ActionParameters[key] = TempData[key];
            }

            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var model = filterContext.Controller.ViewData.Model as BaseModel;

            if (model != null)
            {
                foreach (Error error in model.ErrorsList)
                {
                    ModelState.AddModelError(error.PropertyName, error.Message);
                }
            }

        }

        protected override void OnException(ExceptionContext filterContext)
        {
            //if (BAL.Common.Utilites.GetBoolSettingValue(LookUps.SettingsKeys.EnableErrorLogging))
            //{
            //    ContextInfo.ErrorHelper.LogError(filterContext.Exception);
            //}

            base.OnException(filterContext);
        }
    }
}
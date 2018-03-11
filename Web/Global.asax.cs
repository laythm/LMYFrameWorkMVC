using Elmah;
using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Web.Areas.Admin;
using LMYFrameWorkMVC.Web.Areas.Common;
using LMYFrameWorkMVC.Web.CommonCode;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LMYFrameWorkMVC.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            //register areas manually for the order 
            //always put the default as the last one 
            //there is a weired behavior for routs
            Utilities.RegisterArea<CommonAreaRegistration>(RouteTable.Routes, null);
            Utilities.RegisterArea<AdminAreaRegistration>(RouteTable.Routes, null);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //my initilizers
            ModelBinders.Binders.Add(typeof(string), new TrimModelBinder());
            var binder = new DateTimeModelBinder(
                LMYFrameWorkMVC.Common.Helpers.Utilites.GetSettingValue(LookUps.SettingsKeys.FullDateFormat),
                LMYFrameWorkMVC.Common.Helpers.Utilites.GetSettingValue(LookUps.SettingsKeys.DateFormat),
                LMYFrameWorkMVC.Common.Helpers.Utilites.GetSettingValue(LookUps.SettingsKeys.TimeFormat)
            );
            ModelBinders.Binders.Add(typeof(DateTime), binder);
            ModelBinders.Binders.Add(typeof(DateTime?), binder);
            LMYFrameWorkMVC.Web.CommonCode.SignalR.AppHub.Init();
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session == null) return;

            string culture = LMYFrameWorkMVC.Common.Helpers.Utilites.GetSettingValue(LMYFrameWorkMVC.Common.LookUps.SettingsKeys.DefaultCulture);

            if (Session["CurrentCulture"] != null)
            {
                culture = Session["CurrentCulture"].ToString();
            }

            if ((string)Session["CurrentCulture"] != culture)
            {
                Session["CurrentCulture"] = culture;
            }

            //set the current culture.
            var cultureInfo = new CultureInfo(culture);
            cultureInfo.DateTimeFormat.Calendar = new GregorianCalendar();
            cultureInfo.DateTimeFormat.ShortDatePattern = LMYFrameWorkMVC.Common.Helpers.Utilites.GetSettingValue(LookUps.SettingsKeys.DateFormat);
            cultureInfo.DateTimeFormat.LongDatePattern = LMYFrameWorkMVC.Common.Helpers.Utilites.GetSettingValue(LookUps.SettingsKeys.FullDateFormat);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                if (LMYFrameWorkMVC.Common.Helpers.Utilites.GetBoolSettingValue(LookUps.SettingsKeys.EnableErrorLogging))
                {
                    ErrorHelper.LogError(HttpContext.Current, Server.GetLastError());
                }
            }
            catch
            {

            }
        }
    }
}

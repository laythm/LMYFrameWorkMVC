using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace LMYFrameWorkMVC.Web.CommonCode
{
    public static class Utilities
    {
        public static string EncodeJson(object obj, bool encodeForhtml = true)
        {
            if (encodeForhtml)
                return Json.Encode(obj).Replace("\'", LMYFrameWorkMVC.Common.Helpers.Utilites.GetSettingValue(LMYFrameWorkMVC.Common.LookUps.SettingsKeys.SingleQuoteReplacement)).Replace("\"", LMYFrameWorkMVC.Common.Helpers.Utilites.GetSettingValue(LMYFrameWorkMVC.Common.LookUps.SettingsKeys.DoubleQuoteReplacement));

            return Json.Encode(obj);
        }

        //public static bool GetAppSettingsboolValue(Common.Lookups.AppSettingsKeys appSettingsKeys)
        //{
        //    return System.Configuration.ConfigurationManager.AppSettings[appSettingsKeys.ToString()] != null && System.Configuration.ConfigurationManager.AppSettings[appSettingsKeys.ToString()].ToLower() == "true" ? true : false;
        //}

        public static void RegisterArea<T>(RouteCollection routes, object state) where T : AreaRegistration
        {
            AreaRegistration registration = (AreaRegistration)Activator.CreateInstance(typeof(T));

            AreaRegistrationContext context = new AreaRegistrationContext(registration.AreaName, routes, state);

            string tNamespace = registration.GetType().Namespace;
            if (tNamespace != null)
            {
                context.Namespaces.Add(tNamespace + ".*");
            }

            registration.RegisterArea(context);

        }
    }
}
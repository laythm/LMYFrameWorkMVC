using Elmah;
using LMYFrameWorkMVC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMYFrameWorkMVC.Web.CommonCode
{
    public class ErrorHelper : LMYFrameWorkMVC.Common.Entities.IError
    {
        public void LogError(Exception ex)
        {
            LogError(HttpContext.Current, ex);
            //if (HttpContext.Current == null)
            //LogError(HttpContext.Current, ex);
            // else
            //  Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
        }

        public static void LogError(HttpContext context,Exception ex)
        {
            //if (HttpContext.Current == null)
             if (LMYFrameWorkMVC.Common.Helpers.Utilites.GetBoolSettingValue(LookUps.SettingsKeys.EnableErrorLogging))
                {
                    Elmah.ErrorLog.GetDefault(context).Log(new Error(ex));
                }
            
            // else
            //  Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
        }
    }
}
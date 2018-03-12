using LMYFrameWorkMVC.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;

namespace LMYFrameWorkMVC.Web.CommonCode
{
    public static class ContextInfoInitilizer
    {
        public static ContextInfo GetContextInfo(HttpContextBase httpContextBase, IPrincipal user, string SessionID = null)
        {
            ContextInfo contextInfo = new ContextInfo();

            contextInfo = new ContextInfo();
            if (user != null)
            {
                if (user != null)
                {
                    contextInfo.UserID = user.Identity.GetUserId();
                    contextInfo.UserName = user.Identity.Name;
                }

                if (httpContextBase.Session != null)
                    contextInfo.SessionID = httpContextBase.Session.SessionID;

                if(!string.IsNullOrEmpty(SessionID))
                    contextInfo.SessionID = SessionID;
            }

            contextInfo.HttpContextBase = httpContextBase;
            contextInfo.ErrorHelper = new ErrorHelper();

            return contextInfo;
        }
    }
}
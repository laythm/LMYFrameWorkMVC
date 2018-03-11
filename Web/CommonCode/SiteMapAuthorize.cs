using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMYFrameWorkMVC.Web.CommonCode;

namespace LMYFrameWorkMVC.Web.CommonCode
{
    public class SiteMapAuthorize : AuthorizeAttribute
    {
        // Custom property
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //var isAuthorized = base.AuthorizeCore(httpContext
            if(httpContext.User.HasAccessToNode(MvcSiteMapProvider.SiteMaps.Current.CurrentNode))
            {
                return true;
            }

            if(httpContext.Request.IsAjaxRequest())
                throw new Exception(Resources.Resources.NotAuthorized);

            return false;
        }

    }
}
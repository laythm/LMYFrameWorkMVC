using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LMYFrameWorkMVC.Web.Emails.User
{
    public class RegestrationCompletedHelper : LMYFrameWorkMVC.Common.Entities.ITemplate
    {
        public RegestrationCompletedHelper(UrlHelper url)
            : base(url)
        {
        }

        public override string PrepareTemplate(object datalist)
        {
            LMYFrameWorkMVC.Common.TemplateLists.Regestration regestration = datalist as LMYFrameWorkMVC.Common.TemplateLists.Regestration;

            return "html template: " + regestration.Email;
        }
    }

    public class ForegetPasswordHelper : LMYFrameWorkMVC.Common.Entities.ITemplate
    {
        public ForegetPasswordHelper(UrlHelper url)
            : base(url)
        {
        }

        public override string PrepareTemplate(object datalist)
        {
            return "html template";
        }
    }
}
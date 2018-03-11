using LMYFrameWorkMVC.Common.Models;
using LMYFrameWorkMVC.Common.Models.Common;
using LMYFrameWorkMVC.Web.CommonCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMYFrameWorkMVC.Web.Areas.Common.Controllers;

namespace LMYFrameWorkMVC.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(BaseModel returnedModel)
        {
            return View(returnedModel);
        }
    }
}
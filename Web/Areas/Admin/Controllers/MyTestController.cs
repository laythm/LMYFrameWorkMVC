using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMYFrameWorkMVC.Web.Areas.Admin.Controllers;

namespace LMYFrameWorkMVC.Web.Areas.Admin.Controllers
{
    public class MyTestController : Controller
    {
        // GET: MyTest
        public ActionResult Index()
        {
            return View();
        }
    }
}
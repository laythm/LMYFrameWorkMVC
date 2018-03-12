using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LMYFrameWorkMVC.Web.Areas.Admin.Controllers;

namespace LMYFrameWorkMVC.Web.Areas.Common.Controllers
{
    public class ErrorController : BaseController
    {
        // GET: Error
        public ViewResult NotFound()
        {
            Response.StatusCode = 404;  //you may want to set this to 200
            Response.ContentEncoding = Encoding.ASCII;
            Response.Charset = "utf-8";
            Response.StatusDescription = "PageNotFound";
            return View("NotFound");
        }

        public ViewResult ServerError()
        {
            Response.StatusCode = 500;  //you may want to set this to 200
            Response.Charset = "utf-8";
            Response.ContentEncoding = Encoding.ASCII;
            Response.StatusDescription = "InternalServerError";
            return View("ServerError");
        }
    }
}
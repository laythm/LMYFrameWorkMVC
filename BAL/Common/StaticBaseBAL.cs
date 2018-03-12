using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Security.Principal;
using LMYFrameWorkMVC.Common.Entities;
using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Models.Common;
using CommonLayer = LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.DAL;

namespace LMYFrameWorkMVC.Common.Helpers
{
    public static class StaticBaseBAL  
    {
        public static LMYFrameWorkMVCEntities dbContext;

        static StaticBaseBAL()
        {
            dbContext = new LMYFrameWorkMVCEntities(false);
        }
    }
}

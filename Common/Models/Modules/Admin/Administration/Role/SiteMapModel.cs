using LMYFrameWorkMVC.Common.Attributes;
using LMYFrameWorkMVC.Common.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Role
{
    public class SiteMapModel : BaseModel
    {
        public SiteMapModel()
        {
            this.SiteMapModels = new List<SiteMapModel>();
        }

        public SiteMapModel(bool init)
        {
        }

        public string Key { get; set; }
        public string Title { get; set; }
        public bool Selected { get; set; }

        public List<SiteMapModel> SiteMapModels { get; set; }
    }
}

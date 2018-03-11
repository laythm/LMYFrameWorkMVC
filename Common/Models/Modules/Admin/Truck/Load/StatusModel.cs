using LMYFrameWorkMVC.Common.Attributes;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Message;
using LMYFrameWorkMVC.Common.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Driver;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Company;

namespace LMYFrameWorkMVC.Common.Models.Modules.Admin.Truck.Load
{
    public class StatusModel : BaseModel
    {
        public StatusModel()
        {

        }

        public StatusModel(bool init)
        {
        }

        [Display(Name = "Id")]
        public string Id { get; set; }
 
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resources))]
        public new string Name { get; set; }
    }
}

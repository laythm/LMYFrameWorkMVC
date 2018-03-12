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

namespace LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Company
{
    public class CompanyModel : BaseModel
    {
        public CompanyModel()
        {

        }

        public CompanyModel(bool init)
        {
        }

        [Display(Name = "Id")]
        public string Id { get; set; }

        [MaxLength(300, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resources))]
        public new string Name { get; set; }
 
        [MaxLength(3000, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxLength")]
        [Display(Name = "Notes", ResourceType = typeof(Resources.Resources))]
        public string Notes { get; set; }
    }
}

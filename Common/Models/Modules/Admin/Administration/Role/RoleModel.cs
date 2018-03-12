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
    public class RoleModel : BaseModel
    {
        public RoleModel()
        {
            this.SiteMapModel = new SiteMapModel();
        }

        public RoleModel(bool init)
        {
        }

        [Display(Name = "Id")]
        public string Id { get; set; }

        [MaxLength(300, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resources))]
        public new string Name { get; set; }

        [Display(Name = "Permisions", ResourceType = typeof(Resources.Resources))]
        public SiteMapModel SiteMapModel { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        //[Display(Name = "Roles", ResourceType = typeof(Resources.Resources))]
        //public List<string> RolesIDs { get; set; }

        //  public List<LoginViewModel> Logins { get; set; }
    }
}

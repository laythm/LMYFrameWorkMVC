using LMYFrameWorkMVC.Common.Attributes;
using LMYFrameWorkMVC.Common.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.User
{
    public class ChangePasswordModel : BaseModel
    {
        [Display(Name = "Id")]
        public string Id { get; set; }
 
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Resources))]
        [Trim(false)]
        [MinLength(6, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMinimumLength")]
        public string Password { get; set; }
 
        public DateTime CreatedAt { get; set; }

      //  public List<LoginViewModel> Logins { get; set; }
    }
}

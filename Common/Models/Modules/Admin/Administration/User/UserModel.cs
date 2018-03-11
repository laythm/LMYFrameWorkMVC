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

namespace LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.User
{
    public class UserModel : BaseModel
    {
        public UserModel()
        {
            RolesIDs = new List<string>();
            LastMessageModel = new MessageModel(true);
            AspNetRolesListItems = new List<SelectListItem>();
            AspNetRolesListModel = new List<Role.RoleModel>();
            AspNetUserConnectionModelList = new List<AspNetUserConnectionModel>();
        }

        public UserModel(bool init)
        {
        }

        [Display(Name = "Id")]
        public string Id { get; set; }

        private string _Name;
        [Display(Name = "Name", ResourceType = typeof(Resources.Resources))]
        public new string Name
        {
            get
            {
                _Name = Helpers.Utilites.GetLocalizedName(EnglishName, ArabicName);
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        [MaxLength(300, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "UserName", ResourceType = typeof(Resources.Resources))]
        public string UserName { get; set; }

        [MaxLength(300, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resources))]
        public string EnglishName { get; set; }

        [MaxLength(300, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resources))]
        public string ArabicName { get; set; }

        [MaxLength(300, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired", ErrorMessage = null)]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationInvalidEmail", ErrorMessage = null)]
        [Display(Name = "Email", ResourceType = typeof(Resources.Resources))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Resources))]
        [Trim(false)]
        [MinLength(6, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMinimumLength")]
        public string Password { get; set; }

        [MaxLength(300, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "PhoneNumber", ResourceType = typeof(Resources.Resources))]
        [RegularExpression(@"^[0-9]*$", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationNumber")]
        public string PhoneNumber { get; set; }

        public List<SelectListItem> AspNetRolesListItems { get; set; }
        public List<Role.RoleModel> AspNetRolesListModel { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "Roles", ResourceType = typeof(Resources.Resources))]
        public List<string> RolesIDs { get; set; }

        public DateTime CreatedAt { get; set; }
        public MessageModel LastMessageModel { get; set; }
        public List<AspNetUserConnectionModel> AspNetUserConnectionModelList { get; set; }
        public bool IsOnline { get { return this.AspNetUserConnectionModelList.Count() > 0; } }
        //  public List<LoginViewModel> Logins { get; set; }
    }
}

using LMYFrameWorkMVC.Common.Attributes;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.User;
using LMYFrameWorkMVC.Common.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LMYFrameWorkMVC.Common.Models.Modules.Admin.Message
{
    public class MessageModel : BaseModel
    {
        public MessageModel()
        {
            FromUserModel = new UserModel();
            ToUserModel = new UserModel();
            ToUsersListModel = new List<UserModel>();
            ToUsersListItems = new List<SelectListItem>();
        }

        public MessageModel(bool init)
        {

        }

        public string ID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "FromUser", ResourceType = typeof(Resources.Resources))]
        public string FromUserID { get; set; }
        public string FromUser { get; set; }

        public List<SelectListItem> ToUsersListItems { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "ToUser", ResourceType = typeof(Resources.Resources))]
        public string ToUserID { get; set; }
        public string ToUser { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "MessageText", ResourceType = typeof(Resources.Resources))]
        public string MessageText { get; set; }



        [Display(Name = "Roles", ResourceType = typeof(Resources.Resources))]
        public List<SelectListItem> Roles { get; set; }
        [Display(Name = "Role", ResourceType = typeof(Resources.Resources))]
        public string RoleID { get; set; }

        public DateTime CreatedAt { get; set; }
        public UserModel FromUserModel { get; set; }
        public UserModel ToUserModel { get; set; }
        public bool Read { get; set; }
        public bool IsFromMe { get; set; }
        public bool NotFromMeAndNotRead { get; set; }

        public List<UserModel> ToUsersListModel { get; set; }

        //  public List<LoginViewModel> Logins { get; set; }
    }
}

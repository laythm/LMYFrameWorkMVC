using LMYFrameWorkMVC.Common.Attributes;
using LMYFrameWorkMVC.Common.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Setting
{
    public class SettingModel : BaseModel
    {
        public SettingModel()
        {
            SettingsFilesModel = new List<SettingFileModel>();
        }
        public SettingModel(bool init)
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

        //[Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        //[Display(Name = "Key", ResourceType = typeof(Resources.Resources))]
        public string Key { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        //[Display(Name = "ArabicName", ResourceType = typeof(Resources.Resources))]
        public string ArabicName { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        //[Display(Name = "EnglishName", ResourceType = typeof(Resources.Resources))]
        public string EnglishName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "Value", ResourceType = typeof(Resources.Resources))]
        //[RegularExpression(@"^(?!\s*$).+", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRegEx")]
        public string Value { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "Value", ResourceType = typeof(Resources.Resources))]
        //[RegularExpression(@"^(?!\s*$).+", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRegEx")]
        public bool BoolValue { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Value", ResourceType = typeof(Resources.Resources))]
        [RegularExpression(@"^[0-9]*$", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationNumber")]
        public int IntValue { get; set; }

        [MaxFileSize(4 * 1024 * 1024, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxFileSize")]
        [AllowedFileTypes("jpg,icon,png", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationAllowedFileTypes")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "Value", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase imageValue { get; set; }

        public string DisplayValue { get; set; }

        public LookUps.SettingsTypes Type { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<SettingFileModel> SettingsFilesModel { get; set; }
        //  public List<LoginViewModel> Logins { get; set; }
    }
    public class SettingFileModel
    {
        public string FileShortPath { get; set; }
        public string FileName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace LMYFrameWorkMVC.Common.Attributes
{
    //http://stackoverflow.com/questions/10445861/validating-for-large-files-upon-upload

    // [Required]
    //[MaxFileSize(8 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is {0} bytes")]
    //public HttpPostedFileBase File { get; set; }

    public class MaxFileSizeAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            if (file == null)
            {
                return true;
            }
            return file.ContentLength <= _maxFileSize;
        }

        public override string FormatErrorMessage(string name)
        {
            //if (ErrorMessageResourceType != null)
            //{
            //    ResourceManager rm =new ResourceManager(ErrorMessageResourceType);
     
            //    return rm.GetString(ErrorMessageString).Replace("{0}", name).Replace("{1}", _maxFileSize.ToString());
            //}

            return ErrorMessageString.Replace("{0}", name).Replace("{1}", _maxFileSize.ToString());
        }

        public IEnumerable<System.Web.Mvc.ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName.ToString());
            rule.ValidationParameters.Add("maxfilesize", _maxFileSize);
            rule.ValidationType = "sizelimitation";
            yield return rule;
        }
    }
}

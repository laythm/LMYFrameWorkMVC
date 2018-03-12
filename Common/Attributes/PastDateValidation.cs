using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LMYFrameWorkMVC.Common.Extensions;

namespace LMYFrameWorkMVC.Common.Attributes
{
    //example of using
    //[PastDateValidation(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationPastDate")]
    public class PastDateValidation : ValidationAttribute, IClientValidatable
    {
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return true;

            return value.ToString().ConvertToDateTime() <= DateTime.Now;
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessageString.Replace("{0}", name);
        }

        public IEnumerable<System.Web.Mvc.ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName.ToString());
            rule.ValidationParameters.Add("pastdateparameter", "");
            rule.ValidationType = "pastdatevalidation";
            yield return rule;
        }
    }
}

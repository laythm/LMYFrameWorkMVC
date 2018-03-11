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
    public class DateAttribute: ValidationAttribute, IClientValidatable
    {
        public DateAttribute()
        {

        }

        public override bool IsValid(object value)
        {
            string val = value as string;
            if (string.IsNullOrEmpty(val))
            {
                return true;
            }

            try
            {
                val.ConvertToDateTime();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessageString.Replace("{0}", name);
        }

        public IEnumerable<System.Web.Mvc.ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName.ToString());
            rule.ValidationParameters.Add("dateparameter", "");
            rule.ValidationType = "datevalidation";
            yield return rule;
        }
    }
}

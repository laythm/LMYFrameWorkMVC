using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMYFrameWorkMVC.Web.CommonCode
{
    public class DateTimeModelBinder : IModelBinder
    {
        private List<string> _formats = new List<string>();

        public DateTimeModelBinder(string fullDateFormat, string dateFormat, string timeFormat)
        {
            _formats.Add(fullDateFormat);
            _formats.Add(dateFormat);
            _formats.Add(timeFormat);
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (string.IsNullOrEmpty(value.AttemptedValue))
                return null;

            try
            {
                return DateTime.ParseExact(value.AttemptedValue, _formats.ToArray(), CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            catch
            {

            }

            return new DateTime();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMYFrameWorkMVC.Web.CommonCode
{
    public class TrimModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //if(propertyDescriptor.PropertyType==typeof(string))
            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueResult == null || valueResult.AttemptedValue == null)
                return null;
            else if (valueResult.AttemptedValue == string.Empty)
                return string.Empty;

            if (!string.IsNullOrEmpty(bindingContext.ModelMetadata.PropertyName))
            {
                var trimAttribute = bindingContext.ModelMetadata.Container.GetType().GetProperty(bindingContext.ModelMetadata.PropertyName).GetCustomAttributes(true).Where(x => x.GetType().IsEquivalentTo(typeof(LMYFrameWorkMVC.Common.Attributes.Trim))).FirstOrDefault();
                if (trimAttribute != null)
                {
                    if (((LMYFrameWorkMVC.Common.Attributes.Trim)trimAttribute).trim == false)
                        return valueResult.AttemptedValue;
                }

            }

            return valueResult.AttemptedValue.Trim();
        }
    }
}
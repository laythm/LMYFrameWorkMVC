using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace LMYFrameWorkMVC.Web.CommonCode
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ActionNamesAttribute : ActionNameSelectorAttribute
    {
        public ActionNamesAttribute(params string[] names)
        {
            if (names == null)
            {
                throw new ArgumentException("ActionNames cannot be empty or null", "names");
            }
            this.Names = new List<string>();
            foreach (string name in names)
            {
                if (String.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("ActionNames cannot be empty or null", "names");
                }
                this.Names.Add(name);
            }
        }

        private List<string> Names { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            return this.Names.Any(x => String.Equals(actionName, x, StringComparison.OrdinalIgnoreCase));
        }
    }
}
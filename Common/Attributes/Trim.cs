using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMYFrameWorkMVC.Common.Attributes
{
    public class Trim : System.Attribute
    {
        public readonly bool trim;
        public Trim(bool trim)
        {
            this.trim = trim;
        }
    }
}

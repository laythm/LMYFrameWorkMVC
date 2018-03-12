using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LMYFrameWorkMVC.Common.Entities
{
    public interface IError
    {
        void LogError(Exception ex);
    }
}

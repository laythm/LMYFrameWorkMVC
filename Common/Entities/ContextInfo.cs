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
    public class ContextInfo : IDisposable
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string SessionID { get; set; }

        public HttpContextBase HttpContextBase { get; set; }
        public IError ErrorHelper { get; set; }

        public CultureInfo cultureInfo { get; set; }

        public bool IsUserAuthenticated
        {
            get
            {
                return !string.IsNullOrEmpty(this.UserID);
            }
        }

        public void Dispose()
        {
            UserID = null;
            UserName = null;
            HttpContextBase = null;
            cultureInfo = null;
            ErrorHelper = null;
        }
    }
}

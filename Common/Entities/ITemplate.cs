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
    public abstract class ITemplate
    {
        protected UrlHelper url { get; set; }
        
        public ITemplate()
        {

        }

        public ITemplate(UrlHelper url)
        {
            this.url = url;
        }

        //public string getSmtp()
        //{
        //    return "return smtp settings if any";
        //}
        //here to add new class for email 
        public abstract string PrepareTemplate(object data);
    }
}

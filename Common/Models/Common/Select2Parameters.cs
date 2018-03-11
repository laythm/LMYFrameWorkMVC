using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMYFrameWorkMVC.Common.Models.Common
{
    public class Select2Parameters<T>
    {
        public string text { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public int start
        {
            get
            {
                return (page - 1) * pageSize;
            }
        }
        public T CustomObject { get; set; }
    }
}

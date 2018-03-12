using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMYFrameWorkMVC.Common.Models.Common
{
    public class GenericListModel<T> : BaseModel
    {
        public GenericListModel()
        {
            List = new List<T>();
        }
 
        public int Total { get; set; }
        public List<T> List { get; set; }
    }
}

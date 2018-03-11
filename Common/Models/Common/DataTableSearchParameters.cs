using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMYFrameWorkMVC.Common.Models.Common
{
    public class DataTableSearchParameters<T>
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }

        public List<JQDTColumn> columns { get; set; }
        public JQDTColumnSearch search { get; set; }
        public List<JQDTColumnOrder> order { get; set; }

        public T CustomSearchObject { get; set; }
    }


    public enum JQDTColumnOrderDirection { asc,desc}

    public class JQDTColumnSearch
    {
        public string value { get; set; }
        public string regex { get; set; }
    }
    public class JQDTColumnOrder
    {
        public int column { get; set; }
        public JQDTColumnOrderDirection dir { get; set; }
    }
    public class JQDTColumn
    {
        public string data { get; set; }
        public string name { get; set; }
        public Boolean searchable { get; set; }
        public Boolean orderable { get; set; }
        public JQDTColumnSearch search { get; set; }
    }
}

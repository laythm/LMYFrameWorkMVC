using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMYFrameWorkMVC.Common.Entities
{
    public class Notification
    {
        public Notification(string message, List<string> toUsers)
        {
            this.Message = message;
            this.ToUsers = toUsers;
        }

        public string Message { get; set; }
        public List<string> ToUsers { get; set; }
    }
}

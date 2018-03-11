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
    public class MailAddress
    {
        public MailAddress(string email, string name,string password=null)
        {
            this.Email = email;
            this.Name = name;
            this.Password = password;
        }

        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class SMTPInfo
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public MailAddress From { get; set; }
    }

    public class EmailInfo
    {
        public EmailInfo()
        {
            this.To = new List<MailAddress>();
            this.CC = new List<MailAddress>();
            this.SMTPInfo = new SMTPInfo();
        }

        public List<MailAddress> To { get; set; }
        public List<MailAddress> CC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public SMTPInfo SMTPInfo { get; set; }   
    }
}

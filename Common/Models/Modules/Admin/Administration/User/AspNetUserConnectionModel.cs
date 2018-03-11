using LMYFrameWorkMVC.Common.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.User
{
    public class AspNetUserConnectionModel : BaseModel
    {
        public string ID { get; set; }
        public string UserId { get; set; }
        public string ConnectionID { get; set; }
        public string IP { get; set; }
        public string Browser { get; set; }
        public string Version { get; set; }
        public string Type { get; set; }
        public string Platform { get; set; }
        public bool IsMobileDevice { get; set; }
        public string MobileDeviceModel { get; set; }
        public string MobileDeviceManufacturer { get; set; }
        public string SessionID { get; set; }
    }
}

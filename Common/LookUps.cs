using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMYFrameWorkMVC.Common
{
    public class LookUps
    {
        public enum ErrorType { Exception = 1, Critical = 2, Business = 3 }
        public enum SuccessType { Full = 1, Partial = 2 }
        public enum FolderName { Common = 1, User = 2 }
        public enum SettingsKeys { EnableAuditTrail, UploadFolder, EnableErrorLogging, FullDateFormat, TimeFormat, DateFormat, Logo, SingleQuoteReplacement, DoubleQuoteReplacement, FavIcon, AllowSimultaneousUserLogin, DefaultCulture }
        public enum SettingsTypes { Bool = 1, String = 2, select = 3, Int = 4, multiselect = 5, file = 6, image = 7 }
        public enum CacheKeys { SiteMap_ShowInSideBar, SiteMap_IsRolesEditable, SiteMap_Class, SiteMap_NodeRoles, SiteMap_AllowAnnonAccess, Settings, Roles, UsersRoles, Common }
        //public enum AspNetRoles { Admin = "B87182EE-EDB9-4DDE-B409-CEBAB54DC1AD", Employee = "1ADE9F79-E1CD-49C7-A5F0-024D06345CF4" }
    }
}

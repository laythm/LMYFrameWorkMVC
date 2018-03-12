using LMYFrameWorkMVC.Common.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMYFrameWorkMVC.BAL.Templates.Modules.Administration
{
    public static class ChangeEmailTemplate
    {
        public static string GetTemplate(string code)
        {
            string template = System.IO.File.ReadAllText(Path.Combine(Utilites.getBinFolderLocation(), @"EmailTemplates\Modules\Membership\ChangeEmailCodeEmailTemplate.html"));

            template = template.Replace("{Logo}", Utilites.GetSettingValue(Common.LookUps.SettingsKeys.Logo));

            template = template.Replace("{Code}", code);

            return template;
        }

        public static string GetSubject()
        {
            string subject = "تغيير البريد الالكتروني";

            return subject;
        }
    }
}

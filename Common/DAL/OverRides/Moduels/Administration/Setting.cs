using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMYFrameWorkMVC.Common;
using System.Data.Entity.Infrastructure;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.Common.Entities;
namespace LMYFrameWorkMVC.Common.DAL
{
    public partial class Setting : IAuditedDatesEntity, IAuditedUsersEntity, IAuditWrapper, ICacheClear
    {
          public string GetJSONData()
        {
            Setting tmpSetting = new Setting();
            tmpSetting.CopyPropertyValues(this);
            //to solve the issue of dynamic proxy which load huge data from database
            tmpSetting.SettingsType = new SettingsType();
            tmpSetting.SettingsType.CopyPropertyValues(this.SettingsType);

            tmpSetting.SettingsFiles = new List<SettingsFile>();

            foreach (SettingsFile settings_File in this.SettingsFiles)
            {
                SettingsFile tmpSettings_File = new SettingsFile();
                tmpSettings_File.CopyPropertyValues(settings_File, new List<string>() { tmpSettings_File.nameof(x => x.Setting) });
                tmpSetting.SettingsFiles.Add(tmpSettings_File);
            }

            //string json = JsonConvert.SerializeObject(tmpSetting);

            tmpSetting = null;

            return "";
        }

        public string GetPrimaryKey()
        {
            return this.Id;
        }

        public void ClearCache()
        {
            Common.Helpers.CacheHelper.RemoveCache(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.Settings, ObjectId = this.Key });
        }

    }
}

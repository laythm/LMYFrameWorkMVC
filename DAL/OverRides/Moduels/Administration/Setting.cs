using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMYFrameWorkMVC.Common;
using System.Data.Entity.Infrastructure;
using LMYFrameWorkMVC.Common.Helpers;
using LMYFrameWorkMVC.Common.Entities;
namespace LMYFrameWorkMVC.DAL
{
    public partial class Setting : IAuditedDatesEntity, IAuditedUsersEntity, IAuditWrapper, ICacheClear
    {
        public string GetJSONData()
        {
            Setting tmpSetting = new Setting();
            tmpSetting.CopyPropertyValues(this);
            //to solve the issue of dynamic proxy which load huge data from database
            tmpSetting.Settings_Types = new Settings_Types();
            tmpSetting.Settings_Types.CopyPropertyValues(this.Settings_Types);

            tmpSetting.Settings_Files = new List<Settings_Files>();

            foreach (Settings_Files settings_File in this.Settings_Files)
            {
                Settings_Files tmpSettings_File = new DAL.Settings_Files();
                tmpSettings_File.CopyPropertyValues(settings_File, new List<string>() { tmpSettings_File.nameof(x => x.Setting) });
                tmpSetting.Settings_Files.Add(tmpSettings_File);
            }

            string json = JsonConvert.SerializeObject(tmpSetting);

            tmpSetting = null;

            return json;
        }

        public string GetPrimaryKey()
        {
            return this.Id;
        }

        //public string Name
        //{
        //    get
        //    {
        //        return Utilites.GetLocalizedName(this,this.nameof(x => x.EnglishName), this.nameof(x => x.ArabicName));
        //    }
        //}

        public void ClearCache()
        {
            Common.Helpers.CacheHelper.RemoveCache(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.Settings, ObjectId = this.Key.ToString() });
        }
    }
}

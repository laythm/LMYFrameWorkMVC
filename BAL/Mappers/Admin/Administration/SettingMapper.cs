using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.DAL;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Setting;
using LMYFrameWorkMVC.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LMYFrameWorkMVC.BAL.Mappers.Admin.Administration
{
    public class SettingMapper
    {
        public static void Map(SettingModel src, Setting dest)
        {
            if (src == null || dest == null)
                return;

            dest.CopyPropertyValues(src, new List<string>() { src.nameof(x => x.CreatedAt) , src.nameof(x => x.EnglishName), src.nameof(x => x.ArabicName) });

            foreach (SettingFileModel settingFileModel in src.SettingsFilesModel)
            {
                dest.SettingsFiles.Add(new SettingsFile() { ID = Guid.NewGuid().ToString(), SettingID = src.Id, FileShortPath = settingFileModel.FileShortPath, FileName = settingFileModel.FileName });
            }
        }

        public static void Map(Setting src, SettingModel dest)
        {
            if (src == null || dest == null)
                return;

            //dest.Id = src.Id;
            //dest.Key = src.Key;
            //dest.Name = src.Name;
            dest.CopyPropertyValues(src);
            dest.Type = (LookUps.SettingsTypes)src.Type;

            dest.DisplayValue = src.Value;
            switch ((LookUps.SettingsTypes)src.Type)
            {
                case LookUps.SettingsTypes.Bool:dest.DisplayValue = src.Value.ToLower() == "true" ? Resources.Yes : Resources.No; 
                    dest.BoolValue=Convert.ToBoolean(src.Value) ;break;
                case LookUps.SettingsTypes.Int:  dest.IntValue = Convert.ToInt32(src.Value); break;
                //to get diplay from other tables if it was selet or multi selcet 
            }
        }

        public static void Map(List<Setting> src, List<SettingModel> dest)
        {
            foreach (Setting setting in src)
            {
                SettingModel settingModel = new SettingModel();
                Map(setting, settingModel);
                dest.Add(settingModel);
            }
        }

        public static void Map(List<SettingsFile> src, List<SettingFileModel> dest)
        {
            foreach (SettingsFile settings_File in src)
            {
                SettingFileModel tmpSettingFileModel = new SettingFileModel();
                tmpSettingFileModel.CopyPropertyValues(settings_File);
                dest.Add(tmpSettingFileModel);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using CommonLayer = LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Models.Common;
using LMYFrameWorkMVC.Common.Helpers;
using LMYFrameWorkMVC.BAL.Mappers.Admin.Administration;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Setting;
using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Helpers;
using System.Linq.Expressions;
using LMYFrameWorkMVC.Common.Entities;
using LMYFrameWorkMVC.Common.DAL;
using LMYFrameWorkMVC.Common.Resources;

namespace LMYFrameWorkMVC.BAL.Modules.Admin.Administration
{
    public class SettingBAL : BaseBAL
    {
        public SettingBAL(CommonLayer.Entities.ContextInfo contextInfo)
            : base(contextInfo)
        {
        }

        public void PrepareSettingModel(SettingModel settingModel)
        {
            try
            {
                //if (!base.UserHasPermision(settingModel))

            }
            catch (Exception ex)
            {
                base.HandleError(settingModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }
        }

        public void GetSettingModel(SettingModel settingModel)
        {
            try
            {
                Setting setting = dbContext.Settings.Where(x => x.Id == settingModel.Id).FirstOrDefault();

                if (setting == null)
                {
                    base.HandleError(settingModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                }
                else
                {
                    SettingMapper.Map(setting, settingModel);
                }
            }
            catch (Exception ex)
            {
                base.HandleError(settingModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }
        }

        public GenericListModel<SettingModel> GetSearchSettingsList(DataTableSearchParameters<Nullable<bool>> dataTableSearchParameters)
        {
            GenericListModel<SettingModel> baseListModel = new GenericListModel<SettingModel>();
            try
            {
                //if (!base.UserHasPermision(baseListModel))
                //    return baseListModel;

                IQueryable<Setting> settings = dbContext.Settings.OrderBy(x => x.CreatedAt);

                if (!string.IsNullOrEmpty(dataTableSearchParameters.search.value))
                {
                    settings = settings.Where(x => x.Key.ToLower().Contains(dataTableSearchParameters.search.value.ToLower()));
                }

                foreach (JQDTColumnOrder order in dataTableSearchParameters.order)
                {
                    switch (order.column)
                    {
                        case 0:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                settings = CommonLayer.Helpers.Utilites.GetLocalizedOrderBy(settings.OrderBy(c => c.EnglishName), settings.OrderBy(c => c.ArabicName));
                            else
                                settings = CommonLayer.Helpers.Utilites.GetLocalizedOrderBy(settings.OrderByDescending(c => c.EnglishName), settings.OrderByDescending(c => c.ArabicName));
                            break;
                    }
                }

                baseListModel.Total = settings.Count();
                settings = settings.Skip(dataTableSearchParameters.start);

                if (dataTableSearchParameters.length != -1)
                    settings = settings.Take(dataTableSearchParameters.length);


                SettingMapper.Map(settings.ToList(), baseListModel.List);
            }
            catch (Exception ex)
            {
                base.HandleError(baseListModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

            return baseListModel;
        }

        public void Edit(SettingModel settingModel)
        {
            List<SettingFileModel> lstOldSettingFileModel = new List<SettingFileModel>();
            string oldImageShortPath = null;

            try
            {
                Setting setting = dbContext.Settings.Where(x => x.Id == settingModel.Id).FirstOrDefault();

                if (settingModel.Type == LookUps.SettingsTypes.Int)
                {
                    settingModel.Value = settingModel.IntValue.ToString();
                }
                else if (settingModel.Type == LookUps.SettingsTypes.Bool)
                {
                    settingModel.Value = settingModel.BoolValue.ToString();
                }
                else if (settingModel.Type == LookUps.SettingsTypes.file)
                {
                    SettingMapper.Map(dbContext.Settings.Find(setting.Id).SettingsFiles.ToList(), lstOldSettingFileModel);
                }
                else if (settingModel.Type == LookUps.SettingsTypes.image)
                {
                    oldImageShortPath = setting.Value;
                    settingModel.Value = FileHelper.CreateShortFilePath(LookUps.FolderName.Common, settingModel.imageValue.FileName);
                }

                if (setting == null)
                {
                    base.HandleError(settingModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                    return;
                }

                dbContext.SettingsFiles.RemoveRange(setting.SettingsFiles);

                SettingMapper.Map(settingModel, setting);

                base.SaveChanges();
                ClearCache(setting.Id);


                if (settingModel.Type == LookUps.SettingsTypes.file)
                {
                    //save new 

                    //delete old 
                    foreach (var oldSettingFileModel in lstOldSettingFileModel)
                    {
                        FileHelper.DeleteFile(oldSettingFileModel.FileShortPath);
                    }
                }
                else if (settingModel.Type == LookUps.SettingsTypes.image)
                {
                    //save new image and delete old image
                    FileHelper.SaveFileByShortPath(settingModel.Value, settingModel.imageValue);
                    FileHelper.DeleteFileByShortPath(oldImageShortPath);
                }
 
                settingModel.AddSuccess(Resources.SettingUpdatedSuccessfully, LookUps.SuccessType.Full);
            }
            catch (Exception ex)
            {
                base.HandleError(settingModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        private void ClearCache(string Id)
        {
            LMYFrameWorkMVC.Common.Helpers.CacheHelper.RemoveCache(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.Settings, ObjectId = Id.ToString() });
        }

        //public override void Dispose()
        //{
        //    base.Dispose();
        //}
    }
}

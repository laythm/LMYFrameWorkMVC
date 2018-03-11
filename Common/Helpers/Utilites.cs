using LMYFrameWorkMVC.Common.DAL;
using LMYFrameWorkMVC.Common.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMYFrameWorkMVC.Common.Helpers
{
    public static class Utilites
    {
        //public static string GetLocalizedName(object obj,string EnglishProperty, string ArabicProperty)
        //{
        //    System.Reflection.PropertyInfo engObj = obj.GetType().GetProperty(EnglishProperty);
        //    System.Reflection.PropertyInfo arObj = obj.GetType().GetProperty(ArabicProperty);

        //    if (CurrentCulture.ToLower().Split('-')[0] == "en")
        //    {
        //        if (engObj != null && engObj.GetValue(obj) != null)
        //            return engObj.GetValue(obj).ToString();

        //        if (arObj != null && arObj.GetValue(obj) != null)
        //            return arObj.GetValue(obj).ToString();
        //    }
        //    else
        //    {
        //        if (arObj != null && arObj.GetValue(obj) != null)
        //            return arObj.GetValue(obj).ToString();

        //        if (engObj != null && engObj.GetValue(obj) != null)
        //            return engObj.GetValue(obj).ToString();
        //    }

        //    return "";
        //}

        #region  common

        public static string GetLocalizedName(string english, string arabic)
        {
            if (CurrentCulture.ToLower().Split('-')[0] == "en")
            {
                return english;
            }
            else
            {
                return arabic;
            }

            return "";
        }

        public static List<string> SepiratedToList(string sepiratedString, char sepirateChar)
        {
            return sepiratedString.Split(sepirateChar).ToList();
        }

        public static bool IsRTL
        {
            get
            {
                return Resources.Resources.Direction == "RTL";
            }
        }

        public static string CurrentCulture
        {
            get
            {
                return System.Globalization.CultureInfo.CurrentCulture.Name.ToLower();
            }
        }

        //you can write for select name by culture and
        public static IOrderedQueryable<TSource> GetLocalizedOrderBy<TSource>(IOrderedQueryable<TSource> englishKeySelector, IOrderedQueryable<TSource> arabicKeySelector)
        {
            if (CurrentCulture.ToLower() == Constants.EnglishCulture)
            {
                return englishKeySelector;
            }
            else if (CurrentCulture.ToLower() == Constants.ArabicCulture)
            {
                return arabicKeySelector;
            }

            return null;
        }
 
        public static string getBinFolderLocation()
        {
            return System.IO.Path.GetDirectoryName((new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).LocalPath);
        }

        #endregion

        #region settings
        public static string GetSettingValue(LMYFrameWorkMVC.Common.LookUps.SettingsKeys settingKey)
        {
            //if (settingKey.ToString() exists in cache)
            if ((CacheHelper.GetValue(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.Settings, ObjectId = settingKey.ToString() }) is string))
                return CacheHelper.GetValue(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.Settings, ObjectId = settingKey.ToString() }).ToString();

            using (LMYFrameWorkMVCEntities dbContext = new DAL.LMYFrameWorkMVCEntities(false))
            {
                //return dbContext.Settings.AsNoTracking().Where(x => x.Key == settingKey.ToString()).FirstOrDefault().Value;
                return CacheHelper.Insert(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.Settings, ObjectId = settingKey.ToString() }, dbContext.Settings.Where(x => x.Key == settingKey.ToString()).FirstOrDefault().Value).ToString();
            }
        }

        public static bool GetBoolSettingValue(LMYFrameWorkMVC.Common.LookUps.SettingsKeys settingKey)
        {
            if ((CacheHelper.GetValue(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.Settings, ObjectId = settingKey.ToString() }) is bool))
                return (bool)CacheHelper.GetValue(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.Settings, ObjectId = settingKey.ToString() });

            using (DAL.LMYFrameWorkMVCEntities dbContext = new DAL.LMYFrameWorkMVCEntities(false))
            {
                //return dbContext.Settings.AsNoTracking().Where(x => x.Key == settingKey.ToString()).FirstOrDefault().Value;
                return (bool)CacheHelper.Insert(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.Settings, ObjectId = settingKey.ToString() }, dbContext.Settings.Where(x => x.Key == settingKey.ToString()).FirstOrDefault().Value.ToLower() == "true" ? true : false);
            }
        }

        public static int GetIntSettingValue(LMYFrameWorkMVC.Common.LookUps.SettingsKeys settingKey)
        {
            if ((CacheHelper.GetValue(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.Settings, ObjectId = settingKey.ToString() }) is bool))
                return Convert.ToInt32(CacheHelper.GetValue(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.Settings, ObjectId = settingKey.ToString() }));

            using (DAL.LMYFrameWorkMVCEntities dbContext = new DAL.LMYFrameWorkMVCEntities(false))
            {
                //return dbContext.Settings.AsNoTracking().Where(x => x.Key == settingKey.ToString()).FirstOrDefault().Value;
                return Convert.ToInt32(CacheHelper.Insert(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.Settings, ObjectId = settingKey.ToString() }, Convert.ToInt32(dbContext.Settings.Where(x => x.Key == settingKey.ToString()).FirstOrDefault().Value)));
            }
        }

        #endregion

        #region roles

        public static List<string> GetNodeRolesByKey(string key)
        {
            //if ((CacheHelper.GetValue(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_Roles, ObjectId = key }) is List<string>))
            //    return CacheHelper.GetValue(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_Roles, ObjectId = key }) as List<string>;

            //using (DAL.LMY_FrameWork_MVCEntities dbContext = new DAL.LMY_FrameWork_MVCEntities(false))
            //{
            //    List<CacheMemberKey> dependencyList = dbContext.AspNetRole_NodesKeys.Where(x => x.NodeKey == key).Select(
            //        x => new CacheMemberKey
            //        {
            //            CacheKey = LookUps.CacheKeys.Roles,
            //            ObjectId = x.Id.ToString()
            //        }).ToList();

            //      List<string> roles = dbContext.AspNetRole_NodesKeys.Where(x => x.NodeKey == key).Select(x => x.AspNetRole.Id).ToList();
            //    return CacheHelper.Insert(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_NodeRoles, ObjectId = key }, roles, dependencyList) as List<string>;
            //}
            using (LMYFrameWorkMVCEntities dbContext = new DAL.LMYFrameWorkMVCEntities(false))
            {
                return dbContext.AspNetRoleSiteMapNodes.Where(x => x.NodeKey == key).Select(x => x.AspNetRole.Id).ToList();
            }
        }

        public static List<string> GetUserRolesByUserID(string id)
        {
            using (LMYFrameWorkMVCEntities dbContext = new LMYFrameWorkMVCEntities(false))
            {
                return dbContext.AspNetUsers.Where(x => x.Id == id).First().AspNetRoles.Select(a => a.Id).ToList();
            }
        }

        #endregion

        #region time 
        public static string ToStringFullDateFormat(DateTime value)
        {
            return value.ToString(GetSettingValue(LMYFrameWorkMVC.Common.LookUps.SettingsKeys.FullDateFormat));
        }

        public static string ToStringDateFormat(DateTime value)
        {
            return value.ToString(GetSettingValue(LMYFrameWorkMVC.Common.LookUps.SettingsKeys.DateFormat));
        }

        public static string ToStringTimeFormat(DateTime value)
        {
            return value.ToString(GetSettingValue(LMYFrameWorkMVC.Common.LookUps.SettingsKeys.TimeFormat));
        }

        public static string ToStringFullDateFormat(DateTime? value)
        {
            if (value.HasValue)
                return ToStringFullDateFormat(Convert.ToDateTime(value.Value));
            else
                return null;
        }

        public static string ToStringDateFormat(DateTime? value)
        {
            if (value.HasValue)
                return ToStringDateFormat(Convert.ToDateTime(value.Value));
            else
                return null;
        }

        public static string ToStringTimeFormat(DateTime? value)
        {
            if (value.HasValue)
                return ToStringDateFormat(Convert.ToDateTime(value.Value));
            else
                return null;
        }
        #endregion
    }
}

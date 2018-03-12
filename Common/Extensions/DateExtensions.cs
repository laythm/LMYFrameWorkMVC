using LMYFrameWorkMVC.Common.Models.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LMYFrameWorkMVC.Common.Extensions
{
    public static class DateExtensions
    {
        private static List<string> _formats = new List<string>();

        static DateExtensions()
        {
            _formats.Add(Helpers.Utilites.GetSettingValue(LMYFrameWorkMVC.Common.LookUps.SettingsKeys.FullDateFormat));
            _formats.Add(Helpers.Utilites.GetSettingValue(LMYFrameWorkMVC.Common.LookUps.SettingsKeys.DateFormat));
            _formats.Add(Helpers.Utilites.GetSettingValue(LMYFrameWorkMVC.Common.LookUps.SettingsKeys.TimeFormat));
        }
 
        public static DateTime ConvertToDateTime(this string value)
        {
            return DateTime.ParseExact(value, _formats.ToArray(), CultureInfo.CurrentUICulture, DateTimeStyles.None);
        }

        public static string ToStringFullDateFormat(this DateTime value)
        {
            return value.ToString(Helpers.Utilites.GetSettingValue(LMYFrameWorkMVC.Common.LookUps.SettingsKeys.FullDateFormat));
        }

        public static string ToStringDateFormat(this DateTime value)
        {
            return value.ToString(Helpers.Utilites.GetSettingValue(LMYFrameWorkMVC.Common.LookUps.SettingsKeys.DateFormat));
        }

        public static string ToStringTimeFormat(this DateTime value)
        {
            return value.ToString(Helpers.Utilites.GetSettingValue(LMYFrameWorkMVC.Common.LookUps.SettingsKeys.TimeFormat));
        }

        public static string ToStringFullDateFormat(this DateTime? value)
        {
            if (value.HasValue)
                return ToStringFullDateFormat(Convert.ToDateTime(value.Value));
            else
                return null;
        }

        public static string ToStringDateFormat(this DateTime? value)
        {
            if (value.HasValue)
                return ToStringDateFormat(Convert.ToDateTime(value.Value));
            else
                return null;
        }

        public static string ToStringTimeFormat(this DateTime? value)
        {
            if (value.HasValue)
                return ToStringDateFormat(Convert.ToDateTime(value.Value));
            else
                return null;
        }

    }
}

using LMYFrameWorkMVC.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LMYFrameWorkMVC.Common
{
    public static class FileHelper
    {
        public static string CreateFullFilePath(LookUps.FolderName folderName, string fileName)
        {
            string filePath;

            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LMYFrameWorkMVC.Common.Helpers.Utilites.GetSettingValue(LookUps.SettingsKeys.UploadFolder), folderName.ToString());

            if (!System.IO.Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            // if (File.Exists(filePath))
            //every should has unique name "because of that i use new guid"
            fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);

            filePath = Path.Combine(filePath, fileName);
            return filePath;
        }

        public static string CreateShortFilePath(LookUps.FolderName folderName, string fileName)
        {
            string filePath;

            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LMYFrameWorkMVC.Common.Helpers.Utilites.GetSettingValue(LookUps.SettingsKeys.UploadFolder), folderName.ToString());

            if (!System.IO.Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            // if (File.Exists(filePath))
            //every should has unique name "because of that i use new guid"
            fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);

            filePath = Path.Combine(folderName.ToString(), fileName);

            return filePath;
        }

        public static string GetFullFilePathFromShortPath(string shortPath)
        {
            string filePath;

            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LMYFrameWorkMVC.Common.Helpers.Utilites.GetSettingValue(LookUps.SettingsKeys.UploadFolder), shortPath);

            return filePath;
        }

        public static string GetFullFilePath(LookUps.FolderName folderName, string fileName)
        {
            string filePath;

            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LMYFrameWorkMVC.Common.Helpers.Utilites.GetSettingValue(LookUps.SettingsKeys.UploadFolder), folderName.ToString());

            filePath = Path.Combine(filePath, fileName);
            return filePath;
        }

        public static bool DeleteFile(string shortPath)
        {
            if (File.Exists(GetFullFilePathFromShortPath(shortPath)))
            {
                File.Delete(GetFullFilePathFromShortPath(shortPath));
                return true;
            }

            return false;
        }

        public static bool SaveFileByShortPath(string shortPath, HttpPostedFileBase httpPostedFileBase)
        {
            httpPostedFileBase.SaveAs(GetFullFilePathFromShortPath(shortPath));

            return true;
        }

        public static bool DeleteFileByShortPath(string shortPath )
        {
            File.Delete(GetFullFilePathFromShortPath(shortPath));

            return true;
        }
    }
}
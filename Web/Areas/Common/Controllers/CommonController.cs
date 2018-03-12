using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Web.CommonCode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMYFrameWorkMVC.Common.Extensions;

namespace LMYFrameWorkMVC.Web.Areas.Common.Controllers
{
    public class CommonController : BaseController
    {
        public ActionResult ChangeLanguage(string lang, string returnUrl)
        {
            Session["CurrentCulture"] = lang;
            HttpContext.
            Response.Redirect(returnUrl);
            return null;
        }

        public ActionResult JavaScriptNotEnabled()
        {
            return View();
        }
 
        //public ActionResult GetImageAndSave(string imageShortPath, int width = 0, int height = 0)
        //{
        //    string resizedImagePath = "";
        //    bool isFirstResize = false;
        //    string imageFilePath = FileHelper.CreateFullFilePathFromShortPath(imageShortPath);

        //    if (!System.IO.File.Exists(imageFilePath))
        //        imageFilePath = FileHelper.CreateFullFilePath(LookUps.FolderName.Common, "No_Image_Available.png");

        //    if (width != 0)
        //    {
        //        if (height == 0)
        //            height = width;

        //        isFirstResize = true;
        //        resizedImagePath = Path.Combine(Path.GetDirectoryName(imageFilePath), (width + "_" + height + Path.GetFileName(imageFilePath)));

        //        if (System.IO.File.Exists(resizedImagePath))
        //        {
        //            imageFilePath = resizedImagePath;
        //            isFirstResize = false;
        //        }
        //    }

        //    if (!System.IO.File.Exists(imageFilePath))
        //        return null;

        //    if (isFirstResize)
        //    {
        //        using (System.Drawing.Image srcImage = System.Drawing.Image.FromFile(imageFilePath))
        //        using (Bitmap newImage = new Bitmap(width, height))
        //        using (Graphics graphics = Graphics.FromImage(newImage))
        //        {
        //            graphics.SmoothingMode = SmoothingMode.AntiAlias;
        //            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        //            graphics.DrawImage(srcImage, new Rectangle(0, 0, width, height));
        //            newImage.Save(resizedImagePath);
        //            imageFilePath = resizedImagePath;
        //        }
        //    }

        //    using (Image srcImage = System.Drawing.Image.FromFile(imageFilePath))
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        srcImage.Save(stream, ImageFormat.Png);
        //        return File(stream.ToArray(), "image/png");
        //    }
        //}

        public ActionResult GetImage(string imageShortPath, int width = 0, int height = 0)
        {
            string imageFilePath = imageShortPath.Replace("~", AppDomain.CurrentDomain.BaseDirectory);

            if (!System.IO.File.Exists(imageFilePath))
                imageFilePath = FileHelper.GetFullFilePathFromShortPath(imageShortPath);

            if (!System.IO.File.Exists(imageFilePath))
                imageFilePath = FileHelper.GetFullFilePath(LookUps.FolderName.Common, "No_Image_Available.png");

            if (!System.IO.File.Exists(imageFilePath))
                return null;

            if (width != 0)
            {
                if (height == 0)
                    height = width;

                using (System.Drawing.Image srcImage = System.Drawing.Image.FromFile(imageFilePath))
                using (Bitmap newImage = new Bitmap(width, height))
                using (Graphics graphics = Graphics.FromImage(newImage))
                using (MemoryStream stream = new MemoryStream())
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(srcImage, new Rectangle(0, 0, width, height));

                    newImage.Save(stream, ImageFormat.Png);
                    return File(stream.ToArray(), "image/png");
                }
            }

            using (Image srcImage = System.Drawing.Image.FromFile(imageFilePath))
            using (MemoryStream stream = new MemoryStream())
            {
                srcImage.Save(stream, ImageFormat.Png);
                return File(stream.ToArray(), "image/png");
            }
        }
    }
}
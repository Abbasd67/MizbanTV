using MizbanTV.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Unicorn;

namespace MizbanTV.Services
{
    public static class Helper
    {

        private const Decimal OneKiloByte = 1024M;
        private const Decimal OneMegaByte = OneKiloByte * 1024M;
        private const Decimal OneGigaByte = OneMegaByte * 1024M;
        private const string ImageExtention = ".jpg";
        public const string LocalTempPath = "~/Content/Temp";
        public const string LocalVideoPath = "~/Content/Video";
        public const string LocalThumbPath = "~/Content/Thumb";
        public const string LocalCategoriesPath = "~/Content/Categories";
        public const string LocalAdvertiesePath = "~/Content/Advertises";
        public static string ConvertFileSizeToString(long fileSize)
        {
            decimal size = fileSize;
            string suffix;
            if (size > OneGigaByte)
            {
                size /= OneGigaByte;
                suffix = "GB";
            }
            else if (size > OneMegaByte)
            {
                size /= OneMegaByte;
                suffix = "MB";
            }
            else if (size > OneKiloByte)
            {
                size /= OneKiloByte;
                suffix = "KB";
            }
            else
            {
                suffix = "B";
            }
            return String.Format("{0:N2}{1}", size, suffix);
        }

        public static void ClearTempFolder()
        {
            foreach (var file in Directory.GetFiles(GetTempPath()))
            {
                DateTime modificationDate = File.GetLastWriteTime(file);
                if ((DateTime.Now - modificationDate).TotalHours > 24)
                    File.Delete(file);
            }
        }

        public static string GetTempPath() => HttpContext.Current.Server.MapPath(LocalTempPath);

        public static string GetVideoPath() => HttpContext.Current.Server.MapPath(LocalVideoPath);

        public static string GetThumbPath() => HttpContext.Current.Server.MapPath(LocalThumbPath);

        public static string GetCategoryPath() => HttpContext.Current.Server.MapPath(LocalCategoriesPath);

        public static string GetAdvertisePath() => HttpContext.Current.Server.MapPath(LocalAdvertiesePath);

        public static Video SaveVideo(Video video, string fileName, List<HttpPostedFileBase> backGroundImages)
        {
            var tumbPath = GetThumbPath();
            var videoPath = GetVideoPath();
            string fileExtension = Path.GetExtension(fileName);
            string fileNameWithoutExtention = Path.GetFileNameWithoutExtension(fileName);
            string currentVideoPath = Path.Combine(videoPath, video.FileName);
            string tempPath = Path.Combine(GetTempPath(), video.ID.ToString() + "." + fileExtension);
            string newVideoPath = Path.Combine(videoPath, fileName);
            string currentThumbPath = Path.Combine(tumbPath, fileNameWithoutExtention + ImageExtention);
            string newThumbPath = Path.Combine(tumbPath, fileNameWithoutExtention + ImageExtention);
            if (File.Exists(currentVideoPath))
                File.Delete(currentVideoPath);
            if (File.Exists(currentThumbPath))
                File.Delete(currentThumbPath);
            File.Copy(tempPath, newVideoPath, true);

            if (backGroundImages.Count > 0 && backGroundImages[0] != null)
            {
                var image = backGroundImages[0];
                image.SaveAs(newThumbPath);
            }
            else
            {
                var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                ffMpeg.GetVideoThumbnail(newVideoPath, newThumbPath, 5);
            }
            File.Delete(tempPath);
            video.LastModifiedDate = DateTime.Now;
            video.FileName = fileName;
            video.ThumbName = fileNameWithoutExtention + ImageExtention;
            video.Size = new FileInfo(newVideoPath).Length;
            return video;
        }

        public static PersianDateTime ConvertMiladiToShamsi(DateTime dateTime) => new PersianDateTime(dateTime);

    }
}
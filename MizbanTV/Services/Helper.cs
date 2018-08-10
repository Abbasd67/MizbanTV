using MizbanTV.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MizbanTV.Services
{
    public static class Helper
    {

        private const Decimal OneKiloByte = 1024M;
        private const Decimal OneMegaByte = OneKiloByte * 1024M;
        private const Decimal OneGigaByte = OneMegaByte * 1024M;
        private const string ImageExtention = ".jpg";
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
                suffix = "kB";
            }
            else
            {
                suffix = " B";
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

        public static string GetTempPath() => HttpContext.Current.Server.MapPath("~/App_Data/Temp");

        public static string GetVideoPath() => HttpContext.Current.Server.MapPath("~/App_Data/Video");

        public static string GetThumbPath() => HttpContext.Current.Server.MapPath("~/App_Data/Thumb");

        public static Video SaveVideo(Video video, string fileName)
        {
            string fileExtension = Path.GetExtension(fileName);
            string fileNameWithoutExtention = Path.GetFileNameWithoutExtension(fileName);
            string currentVideoPath = Path.Combine(GetVideoPath(), video.FileName);
            string tempPath = Path.Combine(GetTempPath(), video.ID.ToString() + "." + fileExtension);
            string newVideoPath = Path.Combine(GetVideoPath(), fileName);
            string currentThumbPath = Path.Combine(GetThumbPath(), video.ThumbName);
            string newThumbPath = Path.Combine(GetThumbPath(), fileNameWithoutExtention + ImageExtention);
            if (File.Exists(currentVideoPath))
                File.Delete(currentVideoPath);
            if (File.Exists(currentThumbPath))
                File.Delete(currentThumbPath);
            File.Copy(tempPath, newVideoPath, true);
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            ffMpeg.GetVideoThumbnail(newVideoPath, newThumbPath, 5);
            File.Delete(tempPath);
            video.LastModifiedDate = DateTime.Now;
            video.FileName = fileName;
            video.ThumbName = fileNameWithoutExtention + ImageExtention;
            video.Size = new FileInfo(newVideoPath).Length;
            return video;
        }
    }
}
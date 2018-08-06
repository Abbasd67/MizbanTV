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
    }
}
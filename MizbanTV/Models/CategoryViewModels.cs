using MizbanTV.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MizbanTV.Models
{
    public class ViewCategoriesViewModel
    {
        [Display(Name = "نام دسته بندی")]
        public Category Category { get; set; }

        [Display(Name = "ویدیو ها")]
        public List<ThumbnailViewModel> Videos { get; set; }

        public int IdNumber { get; set; }

        public string BackgroundImage { get; set; }

        public bool IsHotVideos { get; set; }

        [Display(Name = "فایل تبلیغ")]
        public string AdvertiseFileName { get; set; }

        [Display(Name = "لینک تبلیغ")]
        public string AdvertiseLink { get; set; }
    }
}
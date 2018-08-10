using MizbanTV.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MizbanTV.Models
{
    public class AdminIndexVideoViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "اندازه فایل")]
        public string Size { get; set; }

        [Display(Name = "نام فایل")]
        public string FileName { get; set; }

        [Display(Name = "دسته بندی")]
        public string CategoryName { get; set; }
    }

    public class AdminCreateVideoViewModel
    {
        public Guid ID { get; set; }

        [CustomRequired]
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [CustomRequired]
        [Display(Name = "نام فایل")]
        public string FileName { get; set; }

        [CustomRequired]
        [Display(Name = "دسته بندی")]
        public Guid CategoryID { get; set; }
    }

    public class AdminEditVideoViewModel
    {
        public Guid ID { get; set; }

        [CustomRequired]
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "اندازه فایل")]
        public long Size { get; set; }

        [CustomRequired]
        [Display(Name = "نام فایل")]
        public string FileName { get; set; }

        [CustomRequired]
        [Display(Name = "دسته بندی")]
        public Guid CategoryID { get; set; }

        public bool IsNewFileUploaded { get; set; }

        public string Extension { get; set; }
    }
    
}
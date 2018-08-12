using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MizbanTV.Models
{
    public class ViewVideoViewModels
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

        public string ThumbName { get; set; }

        [Display(Name = "دسته بندی")]
        public string CategoryName { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public string CreationDate { get; set; }

        public IEnumerable<ThumbnailViewModel> ThumbNails { get; set; }
    }

    public class ThumbnailViewModel
    {
        public Guid ID { get; set; }

        public string TumbName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Random { get; set; }
    }
}
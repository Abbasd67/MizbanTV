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
        public string Title { get; set; }

        public string Description { get; set; }

        public string Size { get; set; }

        public string FileName { get; set; }

        public string CategoryName { get; set; }
    }

    public class AdminCreateVideoViewModel
    {
        public Guid ID { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "اندازه فایل")]
        public long Size { get; set; }

        [Display(Name = "نام فایل")]
        public string FileName { get; set; }

        [Display(Name = "دسته بندی")]
        public Guid CategoryID { get; set; }

        public List<CategoryModel> Categories { get; set; }
    }

    public class CategoryModel
    {
        public Guid Value { get; set; }

        public string Text { get; set; }
    }
}
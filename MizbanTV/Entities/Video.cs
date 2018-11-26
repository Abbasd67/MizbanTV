using MizbanTV.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MizbanTV.Entities
{
    public class Video
    {
        [Key]
        public Guid ID { get; set; }

        [Display(Name = "عنوان ویدیو")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "اندازه فایل")]
        public long Size { get; set; }

        [Display(Name = "نام فایل")]
        public string FileName { get; set; }
        public string ThumbName { get; set; }

        [Display(Name = "دسته بندی")]
        public Guid CategoryID { get; set; }

        public virtual Category Category { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "تاریخ آخرین تغییر")]
        public DateTime LastModifiedDate { get; set; }

        public int Hits { get; set; }

        [Display(Name = "فعال سازی")]
        public bool IsActivated { get; set; } = false;

        [NotMapped]
        public string FileSizeString { get => Helper.ConvertFileSizeToString(Size); }

        public List<Comment> Comments { get; set; }
    }

}
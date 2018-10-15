using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MizbanTV.Entities
{
    public class Advertise
    {
        [Key]
        public Guid ID { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "نام فایل")]
        public string FileName { get; set; }

        [Display(Name = "لینک تبلیغ")]
        public string Link { get; set; }

        [Display(Name = "نوع تبلیغ")]
        public AdvertiseType AdvertiseType { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }

    }
    public enum AdvertiseType
    {
        [Display(Name = "افقی")]
        Horizontal = 1,
        [Display(Name = "عمودی")]
        Vertical = 2
    }
}
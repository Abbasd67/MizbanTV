using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MizbanTV.Entities
{
    public class Category
    {
        [Key]
        public Guid ID { get; set; }

        [CustomRequired]
        [Display(Name = "نام دسته بندی")]
        public string Name { get; set; }

        [CustomRequired]
        [Display(Name = "شماره ترتیب")]
        public int Order { get; set; }

        [Display(Name = "عکس پس زمینه")]
        public string BackgroundImage { get; set; }

        public virtual List<Video> Videos { get; set; }
    }
}
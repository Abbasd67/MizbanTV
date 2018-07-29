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

        [Display(Name = "نام دسته بندی")]
        public string Name { get; set; }

        public virtual List<Video> Videos { get; set; }
    }
}
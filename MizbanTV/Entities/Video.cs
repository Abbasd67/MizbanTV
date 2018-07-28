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

        public string Name { get; set; }
        
        public int Size { get; set; }

        public string Path { get; set; }

        public virtual Category Category { get; set; }
    }
}
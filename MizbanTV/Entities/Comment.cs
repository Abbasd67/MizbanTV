using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MizbanTV.Entities
{
    public class Comment
    {
        [Key]
        public Guid ID { get; set; }

        public Video Video { get; set; }

        public Guid VideoID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "ntext")]
        public string Text { get; set; }

        public bool IsApproved { get; set; } = false;

        public DateTime DateTime { get; set; } = DateTime.Now;

        [NotMapped]
        public string DateTimeString
        {
            get
            {
                return Unicorn.PersianDateTimeConverter.MiladiToShamsi(DateTime).ToMediumDateTimeString();
            }
        }
    }
}
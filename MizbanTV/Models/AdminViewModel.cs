using MizbanTV.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MizbanTV.Models
{
    public class AdminIndexViewModel
    {
        public List<Category> Categories { get; set; }

        public List<Video> Videos { get; set; }
    }
}
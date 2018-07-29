using MizbanTV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace MizbanTV.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            var data = new AdminIndexViewModel
            {
                Categories = db.Categories.ToList(),
                Videos = db.Videos.Include(v => v.Category).ToList()
            };
            return View(data);
        }
    }
}
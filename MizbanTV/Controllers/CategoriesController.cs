using MizbanTV.Entities;
using MizbanTV.Models;
using MizbanTV.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MizbanTV.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext DbContext { get; set; }

        public CategoriesController(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        // GET: Categories
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult View(Guid id)
        {
            var category = DbContext.Categories.Where(c => c.ID == id).FirstOrDefault();
            if (category == null)
                RedirectToAction("Index", "Home");
            var videos = DbContext.Videos.Where(v => v.CategoryID == id && v.IsActivated).OrderBy(v => v.Hits).ToList();
            var thumbs = new List<ThumbnailViewModel>();
            var rand = new Random();
            foreach(var video in videos)
            {
                thumbs.Add(new ThumbnailViewModel(video, rand.Next(1, 3)));
            }
            var advertises = new List<Advertise>();

            foreach (var advertise in DbContext.Advertises.Where(a => a.AdvertiseType == AdvertiseType.Vertical)
                .OrderBy(a => Guid.NewGuid()).Take(3).ToList())
            {
                advertises.Add(new Advertise
                {
                    AdvertiseType = AdvertiseType.Horizontal,
                    FileName = Path.Combine(Helper.LocalAdvertiesePath, advertise.FileName),
                    Link = advertise.Link,
                    Title = advertise.Title
                });
            }
            if (advertises.Count < 3)
            {

                advertises.Add(new Advertise
                {
                    AdvertiseType = AdvertiseType.Vertical,
                    FileName = Path.Combine(Helper.LocalAdvertiesePath, "Advertising-V.png"),
                    Link = "/Home/Contact"
                });
            }
            ViewBag.Advertises = advertises;
            var model = new ViewCategoriesViewModel
            {
                //BackgroundImage = Path.Combine(Helper.LocalCategoriesPath, category.BackgroundImage),
                Category = category,
                IsHotVideos = false,
                Videos = thumbs
            };
            return View(model);
        }
    }
}
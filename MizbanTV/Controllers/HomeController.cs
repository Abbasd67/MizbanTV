using MizbanTV.Entities;
using MizbanTV.Models;
using MizbanTV.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;

namespace MizbanTV.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext DbContext { get; set; }

        public HomeController(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public ActionResult Index()
        {
            var model = new HomeIndexViewModel();
            var videos = DbContext.Videos.Where(v=>v.IsActivated).Include(v => v.Category).ToList();
            model.NewVideos = new List<ThumbnailViewModel>();
            var random = new Random();
            foreach (var video in videos.OrderByDescending(v => v.CreateDate).Take(6))
            {
                model.NewVideos.Add(new ThumbnailViewModel(video, random.Next(1, 3)));
            }
            model.HotVideos = new List<ThumbnailViewModel>();
            foreach (var video in videos.OrderByDescending(v => v.Hits).Take(20))
            {
                model.HotVideos.Add(new ThumbnailViewModel(video, random.Next(1, 3)));
            }
            model.Categories = new List<ViewCategoriesViewModel>();
            int i = 0;
            foreach (var category in DbContext.Categories.OrderBy(c => c.Order).ToList())
            {
                var videoList = new List<ThumbnailViewModel>();
                foreach (var video in videos.Where(v => v.CategoryID == category.ID).Take(20))
                {
                    videoList.Add(new ThumbnailViewModel(video, random.Next(1, 3)));
                }
                if (string.IsNullOrEmpty(category.BackgroundImage))
                {
                    category.BackgroundImage = "blank.png";
                }
                model.Categories.Add(new ViewCategoriesViewModel
                {
                    Category = category,
                    Videos = videoList,
                    IdNumber = ++i,
                    BackgroundImage = Path.Combine(Helper.LocalCategoriesPath, category.BackgroundImage),
                    IsHotVideos = false
                });
            }
            model.Advertises = new List<Advertise>();
            foreach (var advertise in DbContext.Advertises.Where(a => a.AdvertiseType == AdvertiseType.Horizontal)
                .OrderBy(a => Guid.NewGuid()).Take(model.Categories.Count).ToList())
            {
                model.Advertises.Add(new Advertise
                {
                    AdvertiseType = AdvertiseType.Horizontal,
                    FileName = Path.Combine(Helper.LocalAdvertiesePath, advertise.FileName),
                    Link = advertise.Link,
                    Title = advertise.Title
                });
            }
            for (i = model.Advertises.Count; i < model.Categories.Count; i++)
            {
                model.Advertises.Add(new Advertise
                {
                    AdvertiseType = AdvertiseType.Horizontal,
                    FileName = Path.Combine(Helper.LocalAdvertiesePath, "Advertising-H.gif"),
                    Link = "/Home/Contact"
                });
            }



            return View(model);
        }

        public PartialViewResult MenuPartial()
        {
            var categories = DbContext.Categories.OrderBy(c => c.Order).ToList();
            return PartialView("_ViewMenuPartial", categories);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
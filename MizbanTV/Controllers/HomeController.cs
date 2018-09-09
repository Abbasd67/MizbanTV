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
        public ActionResult Index()
        {
            var model = new HomeIndexViewModel();
            using (var db = new ApplicationDbContext())
            {
                var videos = db.Videos.Include(v => v.Category).ToList();
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
                foreach (var category in db.Categories.OrderBy(c=>c.Order).ToList())
                {
                    var videoList = new List<ThumbnailViewModel>();
                    foreach (var video in videos.Where(v=>v.CategoryID == category.ID).Take(20))
                    {
                        videoList.Add(new ThumbnailViewModel(video, random.Next(1, 3)));
                    }
                    if(string.IsNullOrEmpty(category.BackgroundImage))
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
            }
            return View(model);
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
using MizbanTV.Entities;
using MizbanTV.Models;
using MizbanTV.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MizbanTV.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new HomeIndexViewModel();
            using (var db = new ApplicationDbContext())
            {
                var videoService = new VideoService(db);
                var videos = videoService.GetAll();
                model.NewVideos = new List<ThumbnailViewModel>();
                var random = new Random();
                foreach (var video in videos.OrderByDescending(v => v.CreateDate).Take(10))
                {
                    model.NewVideos.Add(new ThumbnailViewModel(video, random.Next(1, 3)));
                }
                model.HotVideos = new List<ThumbnailViewModel>();
                foreach (var video in videos.OrderByDescending(v => v.Hits).Take(10))
                {
                    model.HotVideos.Add(new ThumbnailViewModel(video, random.Next(1, 3)));
                }
                model.Categories = new List<ViewCategoriesViewModel>();
                int i = 0;
                foreach (var category in db.Categories.OrderBy(c=>c.Order).ToList())
                {
                    var videoList = new List<ThumbnailViewModel>();
                    foreach (var video in videos.Where(v=>v.CategoryID == category.ID).Take(10))
                    {
                        videoList.Add(new ThumbnailViewModel(video, random.Next(1, 3)));
                    }
                    model.Categories.Add(new ViewCategoriesViewModel
                    {
                        Category = category,
                        Videos = videoList,
                        IdNumber = ++i,
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
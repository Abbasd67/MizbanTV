using MizbanTV.Models;
using MizbanTV.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MizbanTV.Entities;
using System.Net;
using CaptchaMvc.HtmlHelpers;

namespace MizbanTV.Controllers
{
    public class VideosController : Controller
    {
        private ApplicationDbContext DbContext { get; set; }

        public VideosController(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        // GET: Videos
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ViewVideo(Guid? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Home");
            var video = DbContext.Videos.Include(c => c.Category).FirstOrDefault(v => v.ID == id);
            if (video == null)
                return RedirectToAction("Index", "Home");
            video.Hits++;
            DbContext.Entry(video).State = EntityState.Modified;
            DbContext.SaveChanges();
            var comments = DbContext.Comments.Where(c => c.VideoID == video.ID && c.IsApproved).ToList();
            var relatedVideos = DbContext.Videos.Where(v => v.ID != video.ID && v.CategoryID == video.CategoryID)
                .OrderBy(v => Guid.NewGuid()).Take(6).ToList();
            var thumbModel = new List<ThumbnailViewModel>();
            var rand = new Random();
            foreach (var thumb in relatedVideos)
            {
                thumbModel.Add(new ThumbnailViewModel(thumb, rand.Next(1, 3)));
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
            var model = new ViewVideoViewModels()
            {
                ID = video.ID,
                CategoryName = video.Category.Name,
                Title = video.Title,
                Description = video.Description,
                FileName = Path.Combine(Helper.LocalVideoPath, video.FileName),
                Size = video.FileSizeString,
                ThumbName = Path.Combine(Helper.LocalThumbPath, video.ThumbName),
                CreationDate = Helper.ConvertMiladiToShamsi(video.CreateDate).ToLongDateString(),
                ThumbNails = thumbModel,
                Advertises = advertises,
                Comments = comments,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult ViewVideo(ViewVideoViewModels model)
        {

            if (!ModelState.IsValid)
            {
                return Json("Not valid model");
            }
            if (!this.IsCaptchaValid("Validate your captcha"))
            {
                return Json("Validate your captcha");
            }
            var comment = new Comment
            {
                DateTime = DateTime.Now,
                Email = model.CommentEmail,
                Name = model.CommentName,
                Text = model.CommentText,
                ID = Guid.NewGuid(),
                IsApproved = true,
                VideoID = model.ID
            };
            DbContext.Comments.Add(comment);
            DbContext.SaveChanges();
            return Json("OK");
        }

    }
}
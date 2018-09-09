using MizbanTV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MizbanTV.Services;
using Kendo.Mvc.Extensions;
using MizbanTV.Entities;
using Kendo.Mvc.UI;
using System.IO;
using Newtonsoft.Json;

namespace MizbanTV.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private ApplicationDbContext DbContext { get; set; }

        public AdminController(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [Route("CategoryRead")]
        public ActionResult CategoryRead([DataSourceRequest] DataSourceRequest request)
        {
            var data = DbContext.Categories.OrderBy(c => c.Order).ToList();
            var list = JsonConvert.SerializeObject(data.ToDataSourceResult(request), Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
            return Content(list, "application/json");
        }
        

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CategoryDestroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Category> products)
        {
            if (products.Any())
            {
                foreach (var product in products)
                {
                    var category = DbContext.Categories.FirstOrDefault(v => v.ID == product.ID);
                    if (category != null)
                    {
                        DbContext.Categories.Remove(product);
                        DbContext.SaveChanges();
                    }
                }
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }
        [Route("VideoRead")]
        public ActionResult VideoRead([DataSourceRequest] DataSourceRequest request)
        {
            var videos = DbContext.Videos.Include(v => v.Category).ToList();
            var data = new List<AdminIndexVideoViewModel>();
            foreach (var video in videos)
            {
                data.Add(new AdminIndexVideoViewModel()
                {
                    ID = video.ID,
                    Title = video.Title,
                    Description = video.Description,
                    CategoryName = video.Category.Name,
                    FileName = video.FileName,
                    Size = video.FileSizeString
                });
            }
            var list = JsonConvert.SerializeObject(data.ToDataSourceResult(request), Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
            return Content(list, "application/json");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult VideoDestroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AdminIndexVideoViewModel> products)
        {
            if (products.Any())
            {
                foreach (var product in products)
                {
                    var video = DbContext.Videos.FirstOrDefault(v => v.ID == product.ID);
                    if (video != null)
                    {
                        DbContext.Videos.Remove(video);
                    }
                }
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }

        public ActionResult CreateCategory()
        {
            var model = new AdminCreateCategoryViewModel();
            model.Category.ID = Guid.NewGuid();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory(AdminCreateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category()
                {
                    ID = model.Category.ID,
                    Name = model.Category.BackgroundImage,
                    Order = model.Category.Order
                };
                if (model.Images.Count > 0)
                {
                    var image = model.Images[0];
                    var fileExtension = Path.GetExtension(image.FileName);
                    var fileName = category.ID.ToString() + fileExtension;
                    var physicalPath = Path.Combine(Helper.GetCategoryPath(), fileName);
                    image.SaveAs(physicalPath);
                    category.BackgroundImage = fileName;
                }
                else
                {
                    category.BackgroundImage = "blank.png";
                }
                DbContext.Categories.Add(category);
                return RedirectToAction("Index");
            }
            return View(model);
        }


        public ActionResult EditCategory(Guid id)
        {
            var category = DbContext.Categories.FirstOrDefault(v => v.ID == id);
            if (category == null)
                RedirectToAction("index");
            var model = new AdminCreateCategoryViewModel()
            {
                Category = category
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory(AdminCreateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = DbContext.Categories.FirstOrDefault(v => v.ID == model.Category.ID);
                if (category == null)
                {
                    ModelState.AddModelError("", "Category Not Found");
                    return View(model);
                }
                category.Name = model.Category.Name;
                category.Order = model.Category.Order;
                if (model.Images.Count > 0)
                {
                    var image = model.Images[0];
                    var fileExtension = Path.GetExtension(image.FileName);
                    var fileName = category.ID.ToString() + fileExtension;
                    var physicalPath = Path.Combine(Helper.GetCategoryPath(), fileName);
                    image.SaveAs(physicalPath);
                    category.BackgroundImage = fileName;
                }
                DbContext.Entry(category).State = EntityState.Modified;
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult CreateVideo()
        {
            var model = new AdminCreateVideoViewModel();
            model.ID = Guid.NewGuid();
            ViewBag.Categories = DbContext.Categories.OrderBy(c => c.Order).ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateVideo([Bind(Include = "ID,Title,Description,FileName,CategoryID")] AdminCreateVideoViewModel model)
        {
            ViewBag.Categories = DbContext.Categories.OrderBy(c => c.Order).ToList();
            if (ModelState.IsValid)
            {
                var video = new Video()
                {
                    ID = model.ID,
                    CategoryID = model.CategoryID,
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Title = model.Title,
                    Description = model.Description,
                    FileName = model.FileName
                };
                video = Helper.SaveVideo(video, model.FileName);
                DbContext.Videos.Add(video);
                return RedirectToAction("Index");
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult SaveVideo(IEnumerable<HttpPostedFileBase> videos, Guid ID)
        {
            // The Name of the Upload component is "videos"
            if (videos != null)
            {
                foreach (var file in videos)
                {
                    var fileExtension = Path.GetExtension(file.FileName);
                    var physicalPath = Path.Combine(Helper.GetTempPath(), ID.ToString() + "." + fileExtension);
                    file.SaveAs(physicalPath);
                }
            }
            // Return an empty string to signify success
            return Content("");
        }


        public ActionResult EditVideo(Guid id)
        {
            var video = DbContext.Videos.FirstOrDefault(v => v.ID == id);
            if (video == null)
                RedirectToAction("index");
            var model = new AdminEditVideoViewModel()
            {
                ID = video.ID,
                Title = video.Title,
                Description = video.Description,
                FileName = video.FileName,
                CategoryID = video.CategoryID,
                Extension = Path.GetExtension(video.FileName),
                IsNewFileUploaded = false,
                Size = video.Size,
            };
            ViewBag.Categories = DbContext.Categories.OrderBy(c => c.Order).ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditVideo([Bind(Include = "ID,Title,Description,Size,FileName,CategoryID,IsNewFileUploaded,Categories,Extension")] AdminEditVideoViewModel model)
        {
            ViewBag.Categories = DbContext.Categories.OrderBy(c => c.Order).ToList();
            if (ModelState.IsValid)
            {
                var video = DbContext.Videos.FirstOrDefault(v => v.ID == model.ID);
                if (video == null)
                {
                    ModelState.AddModelError("", "Video Not Found");
                    return View(model);
                }
                if (model.IsNewFileUploaded)
                {
                    video = Helper.SaveVideo(video, model.FileName);
                }
                video.LastModifiedDate = DateTime.Now;
                video.Title = model.Title;
                video.Description = model.Description;
                video.CategoryID = model.CategoryID;
                DbContext.Entry(video).State = EntityState.Modified;
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

    }

}
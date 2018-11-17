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
using System.Reflection;
using System.ComponentModel.DataAnnotations;

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
                        string path = Path.Combine(Helper.GetCategoryPath(), category.BackgroundImage);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        DbContext.Categories.Remove(category);
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
                    IsActivated = video.IsActivated,
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
                        string videoPath = Path.Combine(Helper.GetVideoPath(), video.FileName);
                        if (System.IO.File.Exists(videoPath))
                        {
                            System.IO.File.Delete(videoPath);
                        }
                        string thumbPath = Path.Combine(Helper.GetThumbPath(), video.ThumbName);
                        if (System.IO.File.Exists(thumbPath))
                        {
                            System.IO.File.Delete(thumbPath);
                        }
                        DbContext.Videos.Remove(video);
                        DbContext.SaveChanges();
                    }
                }
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }


        [Route("AdvertiseRead")]
        public ActionResult AdvertiseRead([DataSourceRequest] DataSourceRequest request)
        {
            var data = DbContext.Advertises.OrderBy(c => c.CreateDate).ToList();
            var list = JsonConvert.SerializeObject(data.ToDataSourceResult(request), Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
            return Content(list, "application/json");
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AdvertiseDestroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Advertise> products)
        {
            if (products.Any())
            {
                foreach (var product in products)
                {
                    var advertise = DbContext.Advertises.FirstOrDefault(v => v.ID == product.ID);
                    if (advertise != null)
                    {
                        string path = Path.Combine(Helper.GetAdvertisePath(), advertise.FileName);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        DbContext.Advertises.Remove(advertise);
                        DbContext.SaveChanges();
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
                if (model.Images.Count > 0 && model.Images[0] != null)
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
                DbContext.SaveChanges();
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
                if (model.Images.Count > 0 && model.Images[0] != null)
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
        [Authorize]
        public ActionResult CreateVideo()
        {
            var model = new AdminCreateVideoViewModel();
            model.ID = Guid.NewGuid();
            ViewBag.Categories = DbContext.Categories.OrderBy(c => c.Order).ToList();
            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateVideo(AdminCreateVideoViewModel model)
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
                video = Helper.SaveVideo(video, model.FileName, model.Images);
                if (User.IsInRole("Administrator"))
                    video.IsActivated = true;
                DbContext.Videos.Add(video);
                DbContext.SaveChanges();
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
                IsActivated = video.IsActivated,
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
        public ActionResult EditVideo(AdminEditVideoViewModel model)
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
                    video = Helper.SaveVideo(video, model.FileName, model.Images);
                }
                video.LastModifiedDate = DateTime.Now;
                video.Title = model.Title;
                video.Description = model.Description;
                video.CategoryID = model.CategoryID;
                video.IsActivated = model.IsActivated;
                DbContext.Entry(video).State = EntityState.Modified;
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult CreateAdvertise()
        {
            var model = new AdminCreateAdvertiseViewModel();
            model.Advertise.ID = Guid.NewGuid();
            ViewBag.AdvertiseTypes = Extensions.GetAdvertiseTypes();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdvertise(AdminCreateAdvertiseViewModel model)
        {
            ViewBag.AdvertiseTypes = Extensions.GetAdvertiseTypes();
            if (ModelState.IsValid)
            {
                var advertise = new Advertise()
                {
                    ID = model.Advertise.ID,
                    Link = model.Advertise.Link,
                    Title = model.Advertise.Title,
                    AdvertiseType = model.Advertise.AdvertiseType,
                    CreateDate = DateTime.Now
                };
                if (model.Images.Count > 0 && model.Images[0] != null)
                {
                    var image = model.Images[0];
                    var fileExtension = Path.GetExtension(image.FileName);
                    var fileName = advertise.ID.ToString() + fileExtension;
                    var physicalPath = Path.Combine(Helper.GetAdvertisePath(), fileName);
                    image.SaveAs(physicalPath);
                    advertise.FileName = fileName;
                }
                else
                {
                    if (model.Advertise.AdvertiseType == AdvertiseType.Horizontal)
                        advertise.FileName = "Advertising-H.gif";
                    else
                        advertise.FileName = "Advertising-V.png";
                }
                DbContext.Advertises.Add(advertise);
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }


        public ActionResult EditAdvertise(Guid id)
        {
            ViewBag.AdvertiseTypes = Extensions.GetAdvertiseTypes();
            var advertises = DbContext.Advertises.FirstOrDefault(v => v.ID == id);
            if (advertises == null)
                RedirectToAction("index");
            var model = new AdminCreateAdvertiseViewModel()
            {
                Advertise = advertises
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAdvertise(AdminCreateAdvertiseViewModel model)
        {
            ViewBag.AdvertiseTypes = Extensions.GetAdvertiseTypes();
            if (ModelState.IsValid)
            {
                var advertise = DbContext.Advertises.FirstOrDefault(v => v.ID == model.Advertise.ID);
                if (advertise == null)
                {
                    ModelState.AddModelError("", "Category Not Found");
                    return View(model);
                }
                advertise.Title = model.Advertise.Title;
                advertise.Link = model.Advertise.Link;
                advertise.AdvertiseType = model.Advertise.AdvertiseType;
                if (model.Images.Count > 0 && model.Images[0] != null)
                {
                    var image = model.Images[0];
                    var fileExtension = Path.GetExtension(image.FileName);
                    var fileName = advertise.ID.ToString() + fileExtension;
                    var physicalPath = Path.Combine(Helper.GetAdvertisePath(), fileName);
                    image.SaveAs(physicalPath);
                    advertise.FileName = fileName;
                }
                DbContext.Entry(advertise).State = EntityState.Modified;
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }


    }
    public static class Extensions
    {

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
        public static dynamic GetAdvertiseTypes()
        {
            return from AdvertiseType e in Enum.GetValues(typeof(AdvertiseType))
            select new
            {
                ID = (int)e,
                Name = GetDisplayName(e)
            };
        }
    }

}
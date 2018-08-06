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
        private ApplicationDbContext db;
        private CategoryService categoryService;
        private VideoService videoService;

        public AdminController()
        {
            db = new ApplicationDbContext();
            categoryService = new CategoryService(db);
            videoService = new VideoService(db);
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [Route("CategoryRead")]
        public ActionResult CategoryRead([DataSourceRequest] DataSourceRequest request)
        {
            var data = categoryService.Read().ToDataSourceResult(request);
            var list = JsonConvert.SerializeObject(data, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
            return Content(list, "application/json");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CategoryCreate([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Category> products)
        {
            var results = new List<Category>();

            if (products != null && ModelState.IsValid)
            {
                foreach (var product in products)
                {
                    product.ID = Guid.NewGuid();
                    categoryService.Insert(product);
                    results.Add(product);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CategoryUpdate([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Category> products)
        {
            if (products != null && ModelState.IsValid)
            {
                foreach (var product in products)
                {
                    categoryService.Update(product);
                }
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CategoryDestroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Category> products)
        {
            if (products.Any())
            {
                foreach (var product in products)
                {
                    categoryService.Delete(product);
                }
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }
        [Route("VideoRead")]
        public ActionResult VideoRead([DataSourceRequest] DataSourceRequest request)
        {

            var data = videoService.Read().ToDataSourceResult(request);
            var list = JsonConvert.SerializeObject(data, Formatting.None,
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
                    videoService.Delete(product);
                }
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }

        public ActionResult CreateVideo()
        {
            var model = new AdminCreateVideoViewModel();
            model.ID = Guid.NewGuid();
            var categories = categoryService.GetAll();
            model.Categories = new List<CategoryModel>();
            foreach (var category in categories)
            {
                model.Categories.Add(new CategoryModel
                {
                    Value = category.ID,
                    Text = category.Name
                });
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateVideo([Bind(Include = "ID,Title,Description,FileName,CategoryID")] AdminCreateVideoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var fileExtension = Path.GetExtension(model.FileName);
                var filePath = Path.Combine(Helper.GetTempPath(), model.ID.ToString() + "." + fileExtension);
                if (!System.IO.File.Exists(filePath))
                {
                    ModelState.AddModelError("", "File Not Found!");
                    return View(model);
                }
                var fileInfo = new FileInfo(filePath);
                videoService.Insert(new Video()
                {
                    ID = model.ID,
                    CategoryID = model.CategoryID,
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Title = model.Title,
                    Description = model.Description,
                    FileName = model.FileName,
                    Size = fileInfo.Length
                });
                var newPath = Path.Combine(Helper.GetVideoPath(), model.FileName);
                System.IO.File.Copy(filePath, newPath, true);
                //db.Entry(video).State = EntityState.Modified;
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



    }

}
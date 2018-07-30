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

namespace MizbanTV.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db;
        private CategoryService service;
        public AdminController()
        {
            db = new ApplicationDbContext();
            service = new CategoryService(db);
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CategoryRead([DataSourceRequest] DataSourceRequest request)
        {
            return Json(service.Read().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CategoryCreate([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Category> products)
        {
            var results = new List<Category>();

            if (products != null && ModelState.IsValid)
            {
                foreach (var product in products)
                {
                    service.Insert(product);
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
                    service.Update(product);
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
                    service.Delete(product);
                }
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }
    }
}
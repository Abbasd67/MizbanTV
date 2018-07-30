using MizbanTV.Entities;
using MizbanTV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MizbanTV.Services
{
    public class CategoryService : IDisposable
    {
        ApplicationDbContext Context { get; set; }
        public CategoryService(ApplicationDbContext context) => Context = context;

        public IList<Category> GetAll() => Context.Categories.ToList();

        public IEnumerable<Category> Read() => GetAll();

        public void Insert(Category category)
        {
            Context.Categories.Add(category);
            Context.SaveChanges();
        }

        public void Update(Category category)
        {
            var target = One(e => e.ID == category.ID);
            if (target != null)
            {
                target.Name = category.Name;
                Context.SaveChanges();
            }
        }

        public void Delete(Category category)
        {
            var target = One(e => e.ID == category.ID);
            if (target != null)
            {
                Context.Categories.Remove(target);
                Context.SaveChanges();
            }
        }

        public Category One(Func<Category, bool> predicate) => GetAll().FirstOrDefault(predicate);
        public void Dispose() => Context.Dispose();
    }
}
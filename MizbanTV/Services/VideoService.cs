using MizbanTV.Entities;
using MizbanTV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MizbanTV.Services
{
    public class VideoService : IDisposable
    {
        ApplicationDbContext Context { get; set; }
        public VideoService(ApplicationDbContext context) => Context = context;


        public IList<Video> GetAll() => Context.Videos.Include(v => v.Category).ToList();

        public IEnumerable<Video> Read() => GetAll();

        public IEnumerable<AdminIndexVideoViewModel> ReadToAdminIndexModel()
        {
            var model = new List<AdminIndexVideoViewModel>();
            var videos = GetAll();
            foreach (var video in videos)
            {
                model.Add(new AdminIndexVideoViewModel()
                {
                    ID = video.ID,
                    Title = video.Title,
                    Description = video.Description,
                    CategoryName = video.Category.Name,
                    FileName = video.FileName,
                    Size = Helper.ConvertFileSizeToString(video.Size)
                });
            }
            return model;
        }

        public void Insert(Video video)
        {
            Context.Videos.Add(video);
            Context.SaveChanges();
        }

        public void Update(Video video)
        {
            var target = One(e => e.ID == video.ID);
            if (target != null)
            {
                target.Title = video.Title;
                target.Description = video.Description;
                target.FileName = video.FileName;
                target.Category = video.Category;
                target.Size = video.Size;
                Context.SaveChanges();
            }
        }

        public void Delete(Video video)
        {
            var target = One(e => e.ID == video.ID);
            if (target != null)
            {
                Context.Videos.Remove(target);
                Context.SaveChanges();
            }
        }

        public void Delete(AdminIndexVideoViewModel model) => Delete(new Video { ID = model.ID });

        public Video One(Func<Video, bool> predicate) => GetAll().FirstOrDefault(predicate);
        public void Dispose() => Context.Dispose();
    }
}
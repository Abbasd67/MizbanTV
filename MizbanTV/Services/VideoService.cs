using MizbanTV.Entities;
using MizbanTV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.IO;

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
                    Size = video.FileSizeString
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


        public void Delete (Guid id)
        {
            var target = One(e => e.ID == id);
            if (target != null)
            {
                File.Delete(Path.Combine(Helper.GetVideoPath(), target.FileName));
                File.Delete(Path.Combine(Helper.GetThumbPath(), target.ThumbName));
                Context.Videos.Remove(target);
                Context.SaveChanges();
            }
        }
        public void Delete(Video video) => Delete(video.ID);

        public void Delete(AdminIndexVideoViewModel model) => Delete(model.ID);

        public Video One(Func<Video, bool> predicate) => GetAll().FirstOrDefault(predicate);
        public void Dispose() => Context.Dispose();
    }
}
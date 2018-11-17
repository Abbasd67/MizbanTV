using MizbanTV.Entities;
using MizbanTV.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace MizbanTV.Models
{
    public class ViewVideoViewModels
    {
        public Guid ID { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "اندازه فایل")]
        public string Size { get; set; }

        [Display(Name = "نام فایل")]
        public string FileName { get; set; }

        public string ThumbName { get; set; }

        [Display(Name = "دسته بندی")]
        public string CategoryName { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public string CreationDate { get; set; }

        public IEnumerable<ThumbnailViewModel> ThumbNails { get; set; }
        public IEnumerable<Comment> Comments { get; set; }

        public List<Advertise> Advertises { get; set; }

        [Required]
        [Display(Name = "نام")]
        public string CommentName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "ایمیل")]
        public string CommentEmail { get; set; }

        [Required]
        [Display(Name = "نظر شما...")]
        public string CommentText { get; set; }
    }

    public class ThumbnailViewModel
    {
        public ThumbnailViewModel(Video video, int random)
        {
            this.ID = video.ID;
            this.Title = video.Title;
            this.Description = video.Description;
            this.ThumbName = Path.Combine(Helper.LocalThumbPath, video.ThumbName);
            this.Random = random;
        }
        public Guid ID { get; set; }

        public string ThumbName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Random { get; set; }

    }

    public class HomeIndexViewModel
    {
        [Display(Name = "جدیدترین ویدیوها")]
        public List<ThumbnailViewModel> NewVideos { get; set; }

        [Display(Name = "داغ ترین ویدیوها")]
        public List<ThumbnailViewModel> HotVideos { get; set; }

        [Display(Name = "دسته بندی ویدیوها")]
        public List<ViewCategoriesViewModel> Categories { get; set; }

        [Display(Name = "تبلیغات")]
        public List<Advertise> Advertises { get; set; }
    }
}
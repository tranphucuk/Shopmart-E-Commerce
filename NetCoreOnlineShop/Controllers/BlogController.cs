using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Data.Enums;
using NetCoreOnlineShop.Models.BlogViewModels;

namespace NetCoreOnlineShop.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IConfiguration _configuration;
        private readonly ITagService _tagService;
        public BlogController(IBlogService blogService, IConfiguration configuration, ITagService tagService)
        {
            this._blogService = blogService;
            this._configuration = configuration;
            this._tagService = tagService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("/blog.html")]
        public IActionResult BlogList(int pageSize, int page = 1)
        {
            pageSize = int.Parse(_configuration["PageSizeBlog"]);
            ViewData["BodyClass"] = "blog_page";
            var blogs = _blogService.GetAllPaging(page, pageSize, string.Empty, string.Empty);
            return View(blogs);
        }

        [Route("/blog-{alias}-{id}.html")]
        public IActionResult BlogDetail(int id)
        {
            var blog = _blogService.GetBlogDetails(id);
            ViewData["BodyClass"] = "single_post_page";

            var blogVms = _blogService.GetAll().Where(x => x.Status == Status.Active);
            BlogDetailViewModel blogDetail = new BlogDetailViewModel()
            {
                Blog = blog,
                blogImageViewModels = _blogService.GetGalleryImage(id),
                LatestBlogs = blogVms.OrderByDescending(x => x.CreatedDate).Take(5).ToList(),
                PopularBlogs = blogVms.OrderByDescending(x => x.ViewCount).Take(5).ToList(),
                RelatedBlogs = blogVms.Take(5).ToList(),
                blogTagViewModels = _blogService.GetTagsByBlogId(id),
                AllTags = _blogService.GetAllTags(),
            };
            return View(blogDetail);
        }

        [Route("/blogs-by-tag-{tagId}.html")]
        public IActionResult BlogsbyTag(string tagId, int pageSize, int page = 1)
        {
            pageSize = int.Parse(_configuration["PageSizeBlog"]);
            ViewData["BodyClass"] = "blog_page";
            ViewData["TagName"] = _tagService.FindById(tagId).Name;
            var blogs = _blogService.GetBlogsByTagId(tagId, page, pageSize);
            return View(blogs);
        }
    }
}
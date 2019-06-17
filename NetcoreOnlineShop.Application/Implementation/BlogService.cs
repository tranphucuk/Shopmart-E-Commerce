using AutoMapper;
using AutoMapper.QueryableExtensions;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Application.ViewModels.Blog;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.Enums;
using NetcoreOnlineShop.Data.IRepositories;
using NetcoreOnlineShop.Infrastructure.Interfaces;
using NetcoreOnlineShop.Utilities.Constants;
using NetcoreOnlineShop.Utilities.Dtos;
using NetcoreOnlineShop.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetcoreOnlineShop.Application.Implementation
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IBlogTagRepository _blogTagRepository;
        private readonly IBlogImageRepository _blogImageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BlogService(IBlogRepository blogRepository, IUnitOfWork unitOfWork, ITagRepository tagRepository,
            IBlogTagRepository blogTagRepository, IBlogImageRepository blogImageRepository)
        {
            this._blogRepository = blogRepository;
            this._tagRepository = tagRepository;
            this._blogTagRepository = blogTagRepository;
            this._blogImageRepository = blogImageRepository;
            this._unitOfWork = unitOfWork;

        }

        public void Add(BlogViewModel blogViewModel)
        {
            var blog = Mapper.Map<BlogViewModel, Blog>(blogViewModel);
            var tags = blogViewModel.Tags.Split(",");
            if (tags.Length > 0)
            {
                foreach (var item in tags)
                {
                    var tag = TextHelper.ToUnsignString(item);
                    _tagRepository.Add(new Tag()
                    {
                        Id = tag,
                        Name = item,
                        Type = CommonConstants.blogTag
                    });

                    blog.BlogTags.Add(new BlogTag()
                    {
                        TagId = tag,
                    });
                }
            }
            _blogRepository.Add(blog);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<BlogViewModel> GetAll()
        {
            var blogs = _blogRepository.FindAll();
            var blogVms = Mapper.Map<List<Blog>, List<BlogViewModel>>(blogs.ToList());
            //var blogVms = blogs.ProjectTo<BlogViewModel>(blogs).ToList();
            return blogVms;
        }

        public PageResult<BlogViewModel> GetAllPaging(int page, int pageSize, string keyword, string option)
        {
            var query = _blogRepository.FindAll();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword)
                                    || x.Description.Contains(keyword)
                                    || x.Content.Contains(keyword));
            }
            if (option == "1")
            {
                query = query.OrderByDescending(x => x.ViewCount);
            }
            else
            {
                query = query.OrderByDescending(x => x.CreatedDate);
            }
            var total = query.Count();
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var result = new PageResult<BlogViewModel>()
            {
                PageSize = pageSize,
                Results = query.ProjectTo<BlogViewModel>().ToList(),
                RowCount = total,
                CurentPage = page
            };

            return result;
        }

        public BlogViewModel GetBlogDetails(int id)
        {
            var blog = _blogRepository.FindById(id);
            if (blog != null)
            {
                var blogVm = Mapper.Map<Blog, BlogViewModel>(blog);
                return blogVm;
            }
            return null;
        }

        public List<BlogImageViewModel> GetGalleryImage(int blogId)
        {
            var images = _blogImageRepository.FindAll(x => x.BlogId == blogId);
            if (images.Count() > 0)
            {
                var imageVms = images.ProjectTo<BlogImageViewModel>().ToList();
                return imageVms;
            }
            return new List<BlogImageViewModel>();
        }

        public List<TagViewModel> GetTagsByBlogId(int blogId)
        {
            var blogTags = _blogTagRepository.FindAll();
            var tags = _tagRepository.FindAll();
            var query = from t in tags
                        join bt in blogTags
                        on t.Id equals bt.TagId
                        where bt.BlogId == blogId
                        select t;

            var tagVms = query.ProjectTo<TagViewModel>().ToList();
            return tagVms;
        }

        public void RemoveBlog(int id)
        {
            _blogRepository.Remove(id);
        }

        public GenericResult RemoveImage(int id)
        {
            try
            {
                _blogImageRepository.Remove(id);
            }
            catch (Exception)
            {
                return new GenericResult(false);
            }
            return new GenericResult(true);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(BlogViewModel blogViewModel)
        {
            var blog = Mapper.Map<BlogViewModel, Blog>(blogViewModel);
            var tags = blogViewModel.Tags.Split(",");

            foreach (var item in tags)
            {
                var tag = TextHelper.ToUnsignString(item);
                var blogTagExisted = _blogTagRepository.FindSingle(x => x.BlogId == blogViewModel.Id && x.TagId == tag);
                if (blogTagExisted == null)
                {
                    blog.BlogTags.Add(new BlogTag()
                    {
                        BlogId = blogViewModel.Id,
                        TagId = tag,
                    });
                }

                var isExisted = _tagRepository.FindById(tag);
                if (isExisted == null)
                {
                    _tagRepository.Add(new Tag()
                    {
                        Id = tag,
                        Name = item,
                        Type = CommonConstants.blogTag
                    });
                }
            }
            _blogRepository.Update(blog);
        }

        public GenericResult UploadMultiImages(List<BlogImageViewModel> blogImageViewModels, int blogId)
        {
            var blogImages = Mapper.Map<List<BlogImageViewModel>, List<BlogImage>>(blogImageViewModels);
            var blog = _blogRepository.FindById(blogId);
            if (blog != null)
            {
                foreach (var img in blogImages)
                {
                    try
                    {
                        blog.BlogImages.Add(img);
                    }
                    catch (Exception ex)
                    {
                        return new GenericResult(false, $"Error while uploading image: {img.Caption}.");
                    }
                }
            }
            return new GenericResult(true);
        }

        public List<TagViewModel> GetAllTags()
        {
            var tags = _tagRepository.FindAll(x => x.Type == CommonConstants.blogTag);
            var tagVms = tags.ProjectTo<TagViewModel>().ToList();

            return tagVms;
        }

        public PageResult<BlogViewModel> GetBlogsByTagId(string tagId, int page, int pageSize)
        {
            var listBlogId = _blogTagRepository.FindAll(x => x.TagId == tagId).Select(x => x.BlogId);
            List<Blog> listBlogs = new List<Blog>();

            foreach (var id in listBlogId)
            {
                var blog = _blogRepository.FindById(id);
                if (blog != null)
                {
                    listBlogs.Add(blog);
                }
            }
            var blogVms = Mapper.Map<List<Blog>, List<BlogViewModel>>(listBlogs);
            var total = blogVms.Count();

            blogVms = blogVms.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var result = new PageResult<BlogViewModel>()
            {
                CurentPage = page,
                Results = blogVms,
                RowCount = total,
                PageSize = pageSize,
            };

            return result;
        }
    }
}

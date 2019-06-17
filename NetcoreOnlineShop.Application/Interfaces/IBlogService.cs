using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Application.ViewModels.Blog;
using NetcoreOnlineShop.Utilities.Dtos;
using System.Collections.Generic;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface IBlogService
    {
        void Add(BlogViewModel blogViewModel);
        void Update(BlogViewModel blogViewModel);

        void Delete(int id);
        BlogViewModel GetBlogDetails(int id);
        List<BlogViewModel> GetAll();

        PageResult<BlogViewModel> GetAllPaging(int page, int pageSize, string keyword, string option);

        GenericResult UploadMultiImages(List<BlogImageViewModel> blogImageViewModels, int blogId);
        List<BlogImageViewModel> GetGalleryImage(int blogId);

        GenericResult RemoveImage(int id);
        void RemoveBlog(int id);

        List<TagViewModel> GetTagsByBlogId(int blogId);
        List<TagViewModel> GetAllTags();
        PageResult<BlogViewModel> GetBlogsByTagId(string tagId, int page, int pageSize);
        void Save();
    }
}
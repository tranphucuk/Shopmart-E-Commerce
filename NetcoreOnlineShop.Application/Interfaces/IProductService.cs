using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;
using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Application.ViewModels.Product;
using NetcoreOnlineShop.Application.ViewModels.Wishlist;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using static NetcoreOnlineShop.Utilities.Constants.CommonConstants;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface IProductService : IDisposable
    {

        List<ProductQuantityViewModel> GetQuantity(int productId);
        void AddProductQuantity(List<ProductQuantityViewModel> productQuantityVm);
        void UpdateProductQuantity(List<ProductQuantityViewModel> productQuantityVm, int productId);

        List<ProductImageViewModel> GetProductImages(int productId);
        void DeleteProductImage(int id);

        // product picture thumbnail
        void SaveImages(int productId, List<ProductImageViewModel> productImageVms);

        //whole price
        List<WholePriceViewModel> GetListProductPrice(int id);
        void SaveProductPrice(List<WholePriceViewModel> wholePriceViewModels, int productId);

        List<ProductViewModel> GetAll();

        List<ProductViewModel> GetProductsByCategoryId(int categoryId);
        List<ProductViewModel> GetProductsByCategoryIdNoMapping(int categoryId);
        PageResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize);
        ProductViewModel Add(ProductViewModel productViewModel);
        ProductViewModel Update(ProductViewModel productViewModel);
        void Delete(int id);
        ProductViewModel GetById(int id);
        int ImportExcel(string filePath, int categoryId);

        //tags
        List<TagViewModel> GetTagsByProduct(int productId);
        List<TagViewModel> GetAllTags();

        //Sort Products Client page 
        List<ProductViewModel> GetAllNoMapping();
        PageResult<ProductViewModel> GetProductsType(int pageSize, string sortBy, int page, string typeProduct);
        PageResult<ProductViewModel> GetProductbyKeyword(string keyword, string sortBy, int pageSize, int page);
        PageResult<ProductViewModel> GetProductsByTagId(string tagId, string sortBy, int pageSize, int page);
        PageResult<ProductViewModel> SortProductByConditions(int? categoryId, string priceRange, string sortBy, int page, int pageSize);

        // wishlist
        void AddToWishlist(int productId, Guid userId);

        bool IsExistedInWishlist(int productId, Guid userId);

        PageResult<ProductViewModel> WishlistProduct(Guid userId, int? pageSize, int page = 1);

        bool RemoveProductInWishlist(int productId, Guid userId);

        PageResult<WishlistInfo> GetListWishlist(string keyword, int page, int pageSize);

        List<string> GetWishlistEmais(int productId);
        void Save();
    }
}

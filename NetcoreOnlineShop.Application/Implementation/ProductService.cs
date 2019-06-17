using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Application.ViewModels.Product;
using NetcoreOnlineShop.Application.ViewModels.Wishlist;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.Enums;
using NetcoreOnlineShop.Data.IRepositories;
using NetcoreOnlineShop.Infrastructure.Interfaces;
using NetcoreOnlineShop.Utilities.Constants;
using NetcoreOnlineShop.Utilities.Dtos;
using NetcoreOnlineShop.Utilities.Helpers;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using static NetcoreOnlineShop.Utilities.Constants.CommonConstants;

namespace NetcoreOnlineShop.Application.Implementation
{
    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;
        private IProductCategoryRepository _productCategoryRepository;
        private IColorRepository _colorRepository;
        private ISizeRepository _sizeRepository;
        private IProductQuantityRepository _productQuantityRepository;
        private readonly IProductImageRepository _productImageRepository;
        private IUnitOfWork _unitOfWork;
        private IProductTagRepository _productTagRepository;
        private ITagRepository _tagRepository;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IWholePriceRepository _wholePriceRepository;
        private readonly IUserWishlistRepository _userWishlistRepository;

        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, IProductTagRepository productTagRepository,
            ITagRepository tagRepository, IColorRepository colorRepository, ISizeRepository sizeRepository,
            IProductQuantityRepository productQuantityRepository, IProductImageRepository productImageRepository,
            IHostingEnvironment hostingEnvironment, IWholePriceRepository wholePriceRepository,
            IProductCategoryRepository productCategoryRepository, IUserWishlistRepository userWishlistRepository)
        {
            this._productRepository = productRepository;
            this._unitOfWork = unitOfWork;
            this._productTagRepository = productTagRepository;
            this._tagRepository = tagRepository;
            this._colorRepository = colorRepository;
            this._sizeRepository = sizeRepository;
            this._productQuantityRepository = productQuantityRepository;
            this._productImageRepository = productImageRepository;
            this._hostingEnvironment = hostingEnvironment;
            this._wholePriceRepository = wholePriceRepository;
            this._productCategoryRepository = productCategoryRepository;
            this._userWishlistRepository = userWishlistRepository;
        }

        public ProductViewModel Add(ProductViewModel productViewModel)
        {
            var tags = productViewModel.Tags.Split(",");
            List<ProductTag> productTags = new List<ProductTag>();
            foreach (var t in tags)
            {
                var tagId = TextHelper.ToUnsignString(t);
                if (!_tagRepository.FindAll(x => x.Id == tagId).Any())
                {
                    _tagRepository.Add(new Tag()
                    {
                        Id = tagId,
                        Name = t,
                        Type = CommonConstants.productTag
                    });
                }
                productTags.Add(new ProductTag()
                {
                    TagId = tagId
                });
            }
            var model = Mapper.Map<ProductViewModel, Product>(productViewModel);

            foreach (var item in productTags)
            {
                model.ProductTags.Add(item);
            }
            _productRepository.Add(model);
            return productViewModel;
        }

        public void AddProductQuantity(List<ProductQuantityViewModel> productQuantityVm)
        {
            var productQuantities = Mapper.Map<List<ProductQuantityViewModel>, List<ProductQuantity>>(productQuantityVm);
            foreach (var item in productQuantities)
            {
                _productQuantityRepository.Add(item);
            }
        }

        public void Delete(int id)
        {
            _productRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<ProductViewModel> GetAll()
        {
            var products = _productRepository.FindAll();
            var productVms = products.ProjectTo<ProductViewModel>();

            return productVms.ToList();
        }

        public PageResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var query = _productRepository.FindAll();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword)
                                   || x.Description.Contains(keyword)
                                   || x.Id.ToString().Contains(keyword));
            }
            if (categoryId.HasValue)
            {
                var childCates = _productCategoryRepository.FindAll(x => x.ParentId == categoryId);
                if (childCates.Count() > 0)
                {
                    var productByCategories = from p in query
                                              join c in childCates
                                              on p.CategoryId equals c.Id
                                              select p;

                    var productByParentId = query.Where(x => x.CategoryId == categoryId);
                    query = productByCategories.Concat(productByParentId);
                }
                else
                {
                    query = query.Where(x => x.CategoryId == categoryId.Value);
                }
            }
            int totalRow = query.Count();
            query = query.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
            var data = query.ProjectTo<ProductViewModel>().ToList();

            return new PageResult<ProductViewModel>()
            {
                CurentPage = page,
                PageSize = pageSize,
                Results = data,
                RowCount = totalRow
            };
        }

        public ProductViewModel GetById(int id)
        {
            var product = _productRepository.FindById(id);
            var productVm = Mapper.Map<Product, ProductViewModel>(product);
            return productVm;
        }

        public List<ProductQuantityViewModel> GetQuantity(int productId)
        {
            var productQuantityModel = _productQuantityRepository.FindAll(x => x.ProductId == productId);
            var productQuantityVm = Mapper.Map<List<ProductQuantity>, List<ProductQuantityViewModel>>(productQuantityModel.ToList());
            return productQuantityVm;
        }

        public List<ProductImageViewModel> GetProductImages(int productId)
        {
            var productImages = _productImageRepository.FindAll(x => x.ProductId == productId);
            var productImageVm = Mapper.Map<List<ProductImage>, List<ProductImageViewModel>>(productImages.ToList());
            return productImageVm;
        }

        public int ImportExcel(string filePath, int categoryId)
        {
            var totalProducts = 0;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                totalProducts = workSheet.Dimension.End.Row - 1;
                Product product;
                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    product = new Product();
                    product.CategoryId = categoryId;
                    product.Name = workSheet.Cells[i, 1].Value.ToString();
                    product.Description = workSheet.Cells[i, 2].Value.ToString();
                    decimal.TryParse(workSheet.Cells[i, 3].Value.ToString(), out var price);
                    decimal.TryParse(workSheet.Cells[i, 4].Value.ToString(), out var promotionPrice);
                    decimal.TryParse(workSheet.Cells[i, 5].Value.ToString(), out var originalPrice);
                    product.Price = price;
                    product.PromotionPrice = promotionPrice;
                    product.OriginalPrice = originalPrice;
                    product.Content = workSheet.Cells[i, 6].Value.ToString();
                    int.TryParse(workSheet.Cells[i, 7].Value.ToString(), out var homeFlag);
                    int.TryParse(workSheet.Cells[i, 8].Value.ToString(), out var hotFlag);
                    product.HomeFlag = homeFlag == 1 ? Status.Active : Status.InActive;
                    product.HotFlag = hotFlag == 1 ? Status.Active : Status.InActive;
                    product.Image = "/admin-side/images/loading.gif";
                    product.Status = Status.Active;

                    _productRepository.Add(product);
                }
            }
            return totalProducts;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public ProductViewModel Update(ProductViewModel productViewModel)
        {
            var tags = productViewModel.Tags.Split(",");
            var productTags = new List<ProductTag>();
            foreach (var t in tags)
            {
                var tagId = TextHelper.ToUnsignString(t);
                if (_tagRepository.FindById(tagId) == null)
                {
                    _tagRepository.Add(new Tag()
                    {
                        Id = tagId,
                        Name = t,
                        Type = CommonConstants.productTag
                    });
                }
                productTags.Add(new ProductTag()
                {
                    ProductId = productViewModel.Id,
                    TagId = tagId
                });
            }
            var model = Mapper.Map<ProductViewModel, Product>(productViewModel);
            _productTagRepository.RemoveMultiple(_productTagRepository.FindAll(x => x.ProductId == model.Id).ToList());
            foreach (var pt in productTags)
            {
                model.ProductTags.Add(pt);
            }

            _productRepository.Update(model);
            return productViewModel;
        }

        public void UpdateProductQuantity(List<ProductQuantityViewModel> productQuantityVm, int productId)
        {
            var productQuantities = Mapper.Map<List<ProductQuantityViewModel>, List<ProductQuantity>>(productQuantityVm);

            // Remove quantities in Database that are removed on View
            var productQuantitiesToDelete = _productQuantityRepository.FindAll(x => x.ProductId == productId && !productQuantities.Contains(x));
            _productQuantityRepository.RemoveMultiple(productQuantitiesToDelete.ToList());

            foreach (var quantityViewModel in productQuantities)
            {
                var quantityModel = _productQuantityRepository.FindById(quantityViewModel.Id);
                quantityModel.Quantity = quantityViewModel.Quantity;
                quantityModel.SizeId = quantityViewModel.SizeId;
                quantityModel.ColorId = quantityViewModel.ColorId;

                _productQuantityRepository.Update(quantityModel);
            }
        }

        public void SaveImages(int productId, List<ProductImageViewModel> productImageVms)
        {
            var productImageModels = Mapper.Map<List<ProductImageViewModel>, List<ProductImage>>(productImageVms);
            foreach (var img in productImageModels)
            {
                _productImageRepository.Add(img);
            }
        }

        public void DeleteProductImage(int id)
        {
            _productImageRepository.Remove(id);
        }

        public List<WholePriceViewModel> GetListProductPrice(int id)
        {
            var wholePrice = _wholePriceRepository.FindAll(x => x.ProductId == id);
            var wholePriceVms = wholePrice.ProjectTo<WholePriceViewModel>().ToList();
            return wholePriceVms;
        }

        public void SaveProductPrice(List<WholePriceViewModel> wholePriceViewModels, int productId)
        {
            var wholePrices = Mapper.Map<List<WholePriceViewModel>, List<WholePrice>>(wholePriceViewModels);

            var pricesToDelete = _wholePriceRepository.FindAll(x => x.ProductId == productId && !wholePrices.Contains(x)).ToList();
            _wholePriceRepository.RemoveMultiple(pricesToDelete);

            foreach (WholePrice item in wholePrices)
            {
                if (item.Id != 0)
                {
                    _wholePriceRepository.Update(item);
                }
                else
                {
                    _wholePriceRepository.Add(item);
                }
            }
        }

        public List<ProductViewModel> GetProductsByCategoryId(int categoryId)
        {
            var products = _productRepository.FindAll(x => x.CategoryId == categoryId);
            var productVms = products.ProjectTo<ProductViewModel>(products).AsNoTracking().ToList();
            return productVms;
        }

        public List<ProductViewModel> GetProductsByCategoryIdNoMapping(int categoryId)
        {
            var products = _productRepository.FindAll(x => x.CategoryId == categoryId);

            var productVms = Mapper.Map<List<Product>, List<ProductViewModel>>(products.ToList());
            return productVms;
        }

        public List<TagViewModel> GetTagsByProduct(int productId)
        {
            var tags = _tagRepository.FindAll();
            var productTags = _productTagRepository.FindAll();

            var query = from t in tags
                        join pt in productTags
                        on t.Id equals pt.TagId
                        where pt.ProductId == productId
                        select t;
            return query.ProjectTo<TagViewModel>().ToList();
        }

        public List<TagViewModel> GetAllTags()
        {
            var tags = _tagRepository.FindAll(x => x.Type == CommonConstants.productTag);
            return tags.ProjectTo<TagViewModel>().ToList();
        }

        //Client side
        public List<ProductViewModel> GetAllNoMapping()
        {
            var products = _productRepository.FindAll();
            var productVms = Mapper.Map<List<Product>, List<ProductViewModel>>(products.ToList());
            return productVms;
        }
        public PageResult<ProductViewModel> GetProductsByTagId(string tagId, string sortBy, int pageSize, int page)
        {
            var productTags = _productTagRepository.FindAll();
            var products = _productRepository.FindAll(x => x.ProductImages);

            var query = from pt in productTags
                        join p in products
                        on pt.ProductId equals p.Id
                        where pt.TagId == tagId
                        select p;

            var total = query.Count();
            var productVms = Mapper.Map<List<Product>, List<ProductViewModel>>(query.ToList()).OrderByDescending(x => x.CreatedDate).ToList();
            if (sortBy == SortType.Price)
            {
                productVms = productVms.OrderBy(x => x.PromotionPrice ?? x.Price).ToList();
            }
            else if (sortBy == SortType.Name)
            {
                productVms = productVms.OrderBy(x => x.Name).ToList();
            }
            productVms = productVms.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var result = new PageResult<ProductViewModel>()
            {
                CurentPage = page,
                PageSize = pageSize,
                Results = productVms,
                RowCount = total
            };
            return result;
        }
        public PageResult<ProductViewModel> SortProductByConditions(int? categoryId, string priceRange, string sortBy, int page, int pageSize)
        {
            var query = _productRepository.FindAll(x => x.ProductImages);

            if (categoryId.HasValue) // Get all products from categories child and parents
            {
                var childCatesId = _productCategoryRepository.FindAll(x => x.ParentId == categoryId).Select(x => x.Id).ToList();
                if (childCatesId.Count() > 0)
                {
                    childCatesId.Add(categoryId.Value);
                    var productByCategories = from p in query
                                              join c in childCatesId
                                              on p.CategoryId equals c
                                              select p;

                    query = productByCategories;
                }
                else
                {
                    query = query.Where(x => x.CategoryId == categoryId.Value);
                }
            }

            if (!string.IsNullOrEmpty(priceRange)) // Sort product by price
            {
                var range = priceRange.Split("-");
                var productSortPrice = query.Where(x => int.Parse(range[0]) <= x.PromotionPrice && x.PromotionPrice <= int.Parse(range[1]));
                query = productSortPrice;
            }

            var data = Mapper.Map<List<Product>, List<ProductViewModel>>(query.ToList()).OrderByDescending(x => x.CreatedDate).ToList();

            if (sortBy == SortType.Price)
            {
                data = data.OrderBy(x => x.PromotionPrice ?? x.Price).ToList();
            }
            else if (sortBy == SortType.Name)
            {
                data = data.OrderBy(x => x.Name).ToList();
            }

            int totalRow = data.Count();
            data = data.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return new PageResult<ProductViewModel>()
            {
                CurentPage = page,
                PageSize = pageSize,
                Results = data,
                RowCount = totalRow
            };
        }
        public PageResult<ProductViewModel> GetProductbyKeyword(string keyword, string sortBy, int pageSize, int page)
        {
            var query = _productRepository.FindAll(x => x.ProductImages);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword)).OrderByDescending(x => x.CreatedDate);
            }
            int totalRow = query.Count();

            var productVms = Mapper.Map<List<Product>, List<ProductViewModel>>(query.ToList());
            if (sortBy == SortType.Price)
            {
                productVms = productVms.OrderBy(x => x.PromotionPrice ?? x.Price).ToList();
            }
            else if (sortBy == SortType.Name)
            {
                productVms = productVms.OrderBy(x => x.Name).ToList();
            }

            productVms = productVms.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return new PageResult<ProductViewModel>()
            {
                CurentPage = page,
                PageSize = pageSize,
                Results = productVms,
                RowCount = totalRow
            };
        }
        public PageResult<ProductViewModel> GetProductsType(int pageSize, string sortBy, int page, string typeProduct)
        {
            var query = _productRepository.FindAll(x => x.ProductImages);

            switch (typeProduct)
            {
                case ProductType.NewArrival:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
                case ProductType.SpeicalOffer:
                    query = query.Where(x => x.PromotionPrice.HasValue).OrderByDescending(x => x.CreatedDate);
                    break;
                case ProductType.BestSeller:
                    query = query.Where(x => x.HotFlag == Status.Active).OrderByDescending(x => x.CreatedDate);
                    break;
                default:
                    break;
            }

            var total = query.Count();
            var productVms = Mapper.Map<List<Product>, List<ProductViewModel>>(query.ToList());
            if (sortBy == SortType.Price)
            {
                productVms = productVms.OrderBy(x => x.PromotionPrice ?? x.Price).ToList();
            }
            else if (sortBy == SortType.Name)
            {
                productVms = productVms.OrderBy(x => x.Name).ToList();
            }

            productVms = productVms.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var result = new PageResult<ProductViewModel>()
            {
                CurentPage = page,
                PageSize = pageSize,
                Results = productVms,
                RowCount = total
            };
            return result;
        }

        // Add to wishlist
        public void AddToWishlist(int productId, Guid userId)
        {
            _userWishlistRepository.Add(new UserWishlist()
            {
                ProductId = productId,
                UserId = userId
            });
        }

        public bool IsExistedInWishlist(int productId, Guid userId)
        {
            var isExisted = _userWishlistRepository.FindSingle(x => x.ProductId == productId && x.UserId == userId);
            if (isExisted != null)
            {
                return true;
            }
            return false;
        }

        public PageResult<ProductViewModel> WishlistProduct(Guid userId, int? pageSize, int page = 1)
        {
            var products = _userWishlistRepository.FindAll(x => x.UserId == userId).Select(x => x.Product);
            var productVms = Mapper.Map<List<Product>, List<ProductViewModel>>(products.ToList());
            var total = productVms.Count;

            var query = productVms.Skip((page - 1) * pageSize.Value).Take(pageSize.Value);
            var pagination = new PageResult<ProductViewModel>()
            {
                CurentPage = page,
                PageSize = pageSize.Value,
                Results = query.ToList(),
                RowCount = total
            };
            return pagination;
        }

        public PageResult<WishlistInfo> GetListWishlist(string keyword, int page, int pageSize)
        {
            var productIds = _userWishlistRepository.FindAll().Select(x => x.ProductId).Distinct();

            var listWishlist = new List<WishlistInfo>();
            foreach (var id in productIds)
            {
                var wishProduct = new WishlistInfo();
                wishProduct.Product = Mapper.Map<Product, ProductViewModel>(_productRepository.FindById(id));
                wishProduct.Total = _userWishlistRepository.FindAll(x => x.ProductId == id).Count();

                listWishlist.Add(wishProduct);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                listWishlist = listWishlist.Where(x => x.Product.Name.Contains(keyword)).ToList();
            }
            var wishlistSorted = listWishlist.OrderByDescending(x => x.Total).ToList();
            var total = wishlistSorted.Count;

            var query = wishlistSorted.Skip((page - 1) * pageSize).Take(pageSize);
            var pagination = new PageResult<WishlistInfo>()
            {
                CurentPage = page,
                Results = query.ToList(),
                PageSize = pageSize,
                RowCount = total
            };
            return pagination;
        }

        public List<string> GetWishlistEmais(int productId)
        {
            var users = _userWishlistRepository.FindAll(x => x.ProductId == productId).Select(x => x.AppUser);
            var emails = users.Select(x => x.Email);
            return emails.ToList();
        }

        public bool RemoveProductInWishlist(int productId, Guid userId)
        {
            var product = _userWishlistRepository.FindSingle(x => x.ProductId == productId && x.UserId == userId);
            if (product != null)
            {
                _userWishlistRepository.Remove(product);
                return true;
            }
            return false;
        }
    }
}
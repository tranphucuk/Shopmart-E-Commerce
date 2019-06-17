using AutoMapper;
using AutoMapper.QueryableExtensions;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using NetcoreOnlineShop.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetcoreOnlineShop.Application.Implementation
{
    public class ProductCategoryService : IProductCategoryService
    {
        private IProductCategoryRepository _productCategoryRepository;
        private IUnitOfWork _unitOfWork;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository, IUnitOfWork unitOfWork)
        {
            this._productCategoryRepository = productCategoryRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Add(ProductCategoryViewModel productCategoryViewModel)
        {
            var productCategory = Mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryViewModel);
            _productCategoryRepository.Add(productCategory);
        }

        public void Delete(int id)
        {
            _productCategoryRepository.Remove(id);
        }

        public List<ProductCategoryViewModel> GetAll()
        {
            var categoryVms = _productCategoryRepository.FindAll().OrderBy(x => x.ParentId).ThenBy(x => x.SortOrder).ProjectTo<ProductCategoryViewModel>().ToList();
            return categoryVms;
        }

        public List<ProductCategoryViewModel> GetAllNoMapping()
        {
            var category = _productCategoryRepository.FindAll().OrderBy(x => x.ParentId).ThenBy(x => x.SortOrder);
            var categoryVms = Mapper.Map<List<ProductCategory>, List<ProductCategoryViewModel>>(category.ToList());
            return categoryVms;
        }

        public List<ProductCategoryViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _productCategoryRepository.FindAll(x => x.Name.Contains(keyword) || x.Description.Contains(keyword))
                    .OrderBy(x => x.ParentId).ProjectTo<ProductCategoryViewModel>().ToList();
            }
            else
            {
                return _productCategoryRepository.FindAll().OrderBy(x => x.ParentId).ProjectTo<ProductCategoryViewModel>().ToList();
            }
        }

        public List<ProductCategoryViewModel> GetAllByParentId(int parentId)
        {
            return _productCategoryRepository.FindAll(x => x.Status == Data.Enums.Status.Active && x.ParentId == parentId)
                .ProjectTo<ProductCategoryViewModel>().ToList();
        }

        public ProductCategoryViewModel GetById(int id)
        {
            return Mapper.Map<ProductCategory, ProductCategoryViewModel>(_productCategoryRepository.FindById(id));
        }

        public void ReOrder(int sourceId, int targetId, string point)
        {
            var source = _productCategoryRepository.FindById(sourceId);
            if (source.ParentId != null)
            {
                source.ParentId = null;
            }
            var target = _productCategoryRepository.FindById(targetId);
            source.ParentId = target.ParentId;

            if (point == "top")
            {
                source.SortOrder = target.SortOrder;
                target.SortOrder += 1;
            }
            else
            {
                source.SortOrder = target.SortOrder + 1;
            }
            //var tempOrder = source.SortOrder;
            //source.SortOrder = target.SortOrder;
            //source.ParentId = target.ParentId;
            //target.SortOrder = tempOrder;

            _productCategoryRepository.Update(source);
            _productCategoryRepository.Update(target);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductCategoryViewModel productCategoryViewModel)
        {
            _productCategoryRepository.Update(Mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryViewModel));
        }

        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            var sourceCategory = _productCategoryRepository.FindById(sourceId);
            sourceCategory.ParentId = targetId;
            _productCategoryRepository.Update(sourceCategory);

            // getAll siblings
            var siblings = _productCategoryRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in siblings)
            {
                child.SortOrder = items[child.Id];
                _productCategoryRepository.Update(child);
            }
        }
    }
}

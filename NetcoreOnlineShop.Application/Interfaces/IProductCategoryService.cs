using NetcoreOnlineShop.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface IProductCategoryService
    {
        void Add(ProductCategoryViewModel productCategoryViewModel);

        void Update(ProductCategoryViewModel productCategoryViewModel);

        void Delete(int id);

        List<ProductCategoryViewModel> GetAll();
        List<ProductCategoryViewModel> GetAllNoMapping();

        List<ProductCategoryViewModel> GetAll(string keyword);

        List<ProductCategoryViewModel> GetAllByParentId(int parentId);

        ProductCategoryViewModel GetById(int id);

        void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items);    

        void ReOrder(int sourceId, int targetId, string point);

        void Save();
    }
}

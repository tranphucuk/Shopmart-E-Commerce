using Microsoft.AspNetCore.Mvc.Rendering;
using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Models.ProductViewModels
{
    public class CatalogViewModel
    {
        public PageResult<ProductViewModel> Data { get; set; }

        public ProductCategoryViewModel ProductCategory { get; set; }

        public List<ProductCategoryViewModel> AllCategories { get; set; }

        public List<TagViewModel> ProductTags { get; set; }

        public List<BlogViewModel> Blogs { get; set; }

        public string SortType { get; set; }
        public List<SelectListItem> SortTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "latest",Text="Latest"},
            new SelectListItem(){Value = "price", Text="Price"},
            new SelectListItem(){Value = "name", Text ="Name"}
        };

        public bool IsSorted { get; set; }

        public string PriceSort { get; set; }
        public List<PriceItem> PriceSorts { get; set; } = new List<PriceItem>
        {
           new PriceItem(){Name="$10 - $100", Value1=10, Value2=100},
           new PriceItem(){Name="$100 - $250", Value1=100, Value2=250},
           new PriceItem(){Name="$250 - $500", Value1=250, Value2=500},
           new PriceItem(){Name="$500 - $1000", Value1=500, Value2=1000},
        };
    }
}

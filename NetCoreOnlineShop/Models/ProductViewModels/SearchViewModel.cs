using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Models.ProductViewModels
{
    public class SearchViewModel : CatalogViewModel
    {
        public string Keyword { get; set; }
        public int? PageSize { get; set; }

        public List<SelectListItem> PageSizes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "8", Text="8"},
            new SelectListItem(){Value = "12", Text="12"},
            new SelectListItem(){Value = "16", Text="16"},
        };
    }
}

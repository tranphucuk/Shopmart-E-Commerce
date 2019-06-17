using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.Product
{
   public class ProductsToExcel
    {
        public string Name { get; set; }//

        public string Image { get; set; }//

        public decimal Price { get; set; }//

        public decimal? PromotionPrice { get; set; } //

        public decimal? OriginalPrice { get; set; }//

        public string Description { get; set; }//

        public int? ViewCount { get; set; }

        public int Unit { get; set; } //
    }
}

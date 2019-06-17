using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels
{
    public class ProductTagViewModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string TagId { get; set; }

        public ProductViewModel Product { get; set; }

        public TagViewModel Tag { get; set; }
    }
}

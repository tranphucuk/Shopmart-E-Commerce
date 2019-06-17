using NetcoreOnlineShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels
{
    public class ProductImageViewModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public virtual ProductViewModel Product { get; set; }

        [StringLength(250)]
        public string Path { get; set; }

        public double? Size { get; set; }

        [StringLength(250)]
        public string Caption { get; set; }

        public Status Status { get; set; }
    }
}

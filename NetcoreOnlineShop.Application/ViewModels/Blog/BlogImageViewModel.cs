using NetcoreOnlineShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.Blog
{
    public class BlogImageViewModel
    {
        public int Id { get; set; }

        public int BlogId { get; set; }

        public BlogViewModel Blog { get; set; }

        [StringLength(250)]
        public string Path { get; set; }

        public decimal? Size { get; set; }

        [StringLength(250)]
        public string Caption { get; set; }
        public Status Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.Blog
{
    public class BlogTagViewModel
    {
        public int Id { get; set; }

        public int BlogId { get; set; }

        [Required]
        public string TagId { get; set; }
    }
}

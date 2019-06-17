using NetcoreOnlineShop.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace NetcoreOnlineShop.Application.ViewModels.Page
{
    public class PageViewModel
    {
        public int Id { get; set; }

        [StringLength(256)]
        [Required]
        public string Name { get; set; }

        [Required]
        public string Alias { get; set; }

        [Required]
        public string Content { get; set; }

        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
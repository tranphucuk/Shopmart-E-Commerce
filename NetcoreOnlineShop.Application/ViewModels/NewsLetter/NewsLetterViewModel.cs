using NetcoreOnlineShop.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace NetcoreOnlineShop.Application.ViewModels.NewsLetter
{
    public class NewsLetterViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public int TotalReceiver { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
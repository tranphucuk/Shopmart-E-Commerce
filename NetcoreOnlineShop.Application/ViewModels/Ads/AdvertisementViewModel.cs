using NetcoreOnlineShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetcoreOnlineShop.Application.ViewModels.Ads
{
    public class AdvertisementViewModel
    {
        public int Id { get; set; }

        [StringLength(250)]
        [Required]
        public string Name { get; set; }

        [StringLength(250)]
        [Required]
        public string Description { get; set; }

        [StringLength(250)]
        [Required]
        public string Image { get; set; }

        [StringLength(250)]
        [Required]
        public string Url { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SortOrder { get; set; }

        public ICollection<AdvertisementPageViewModel> AdvertisementPages { get; set; }
    }
}

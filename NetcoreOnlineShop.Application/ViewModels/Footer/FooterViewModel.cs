using NetcoreOnlineShop.Application.ViewModels.Page;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetcoreOnlineShop.Application.ViewModels.Footer
{
    public class FooterViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public int Order { get; set; }

        public ICollection<FooterPageViewModel> FooterPages { get; set; }
    }
}
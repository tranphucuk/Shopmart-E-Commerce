using NetcoreOnlineShop.Infrastructure.SharedKernel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetcoreOnlineShop.Data.Entities
{
    [Table("Footers")]
    public class Footer : DomainEntity<int>
    {
        public Footer()
        {
            FooterPages = new List<FooterPage>();
        }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        public virtual ICollection<FooterPage> FooterPages { get; set; }
    }
}
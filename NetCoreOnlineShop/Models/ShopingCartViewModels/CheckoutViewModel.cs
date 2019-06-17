using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Application.ViewModels.Product;
using NetcoreOnlineShop.Application.ViewModels.System;
using NetcoreOnlineShop.Data.Enums;
using NetCoreOnlineShop.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Models.ShopingCartViewModels
{
    public class CheckoutViewModel : BillViewModel
    {
        public CheckoutViewModel()
        {
            BasketDetails = new List<BasketDetails>();
        }
        public List<BasketDetails> BasketDetails { get; set; }

        public IEnumerable<EnumViewModel> PaymentMethods
        {
            get
            {
                return ((PaymentMethod[])Enum.GetValues(typeof(PaymentMethod))).Select(p => new EnumViewModel()
                {
                    Value = (int)p,
                    Name = p.GetDescription<PaymentMethod>()
                });
            }
        }

        public decimal TotalBill { get; set; }

        public AppUserViewModel User { get; set; }
    }
}

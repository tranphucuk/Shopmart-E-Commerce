using NetcoreOnlineShop.Application.ViewModels.Bill;
using NetcoreOnlineShop.Application.ViewModels.Product;
using NetcoreOnlineShop.Data.Enums;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface IBillService
    {
        void CreateBill(BillViewModel billViewModel);

        BillViewModel UpdateBill(BillViewModel billViewModel);
        BillViewModel GetBill(int id);

        PageResult<BillViewModel> GetAllBillPaging(int? option, DateTime fromDate, DateTime toDate, string keyword, int page, int pageSize);

        List<BillDetailViewModel> GetBillDetails(int billId);
        void DeleteBillDetail(int id);
        BillViewModel DeleteBill(int id);

        List<ColorViewModel> GetAllColor();
        List<SizeViewModel> GetAllSize();

        BillViewModel GetByBillId(int id);

        void Save();

        //Client active controller
        BillViewModel GetLatestBillByCustomerId(Guid id);
        List<BillViewModel> GetAll();
        int GetTotalSoldThisMonth();
        List<LatestPurchase> GetLatestPurchases();

        PaymentDetails SortPaymentType();
        List<BillCondition> BillConditionReport();

        List<BillViewModel> GetBillsByCustomerId(Guid customerId);
    }
}

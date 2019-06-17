using AutoMapper;
using AutoMapper.QueryableExtensions;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels.Bill;
using NetcoreOnlineShop.Application.ViewModels.Product;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.Enums;
using NetcoreOnlineShop.Data.IRepositories;
using NetcoreOnlineShop.Infrastructure.Interfaces;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NetcoreOnlineShop.Application.Implementation
{
    public class BillService : IBillService
    {
        private readonly IBillRepository _billRepository;
        private readonly IBillDetailRepository _billDetailRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BillService(IBillRepository billRepository, IBillDetailRepository billDetailRepository,
           IColorRepository colorRepository, ISizeRepository sizeRepository, IUnitOfWork unitOfWork,
           IProductRepository productRepository)
        {
            this._unitOfWork = unitOfWork;
            this._billRepository = billRepository;
            this._billDetailRepository = billDetailRepository;
            this._colorRepository = colorRepository;
            this._sizeRepository = sizeRepository;
            this._productRepository = productRepository;
        }

        public void CreateBill(BillViewModel billViewModel)
        {
            try
            {
                var bill = Mapper.Map<BillViewModel, Bill>(billViewModel);
                var billDetails = Mapper.Map<List<BillDetailViewModel>, List<BillDetail>>(billViewModel.BillDetails.ToList());
                foreach (var item in billDetails)
                {
                    var product = _productRepository.FindById(item.ProductId);
                    item.Price = product.PromotionPrice ?? product.Price;
                }

                bill.BillDetails = billDetails;
                _billRepository.Add(bill);
            }
            catch (Exception ex)
            {
            }
        }

        public BillViewModel DeleteBill(int id)
        {
            var bill = _billRepository.FindById(id);
            var billViewModel = Mapper.Map<Bill, BillViewModel>(bill);

            var billDetails = _billDetailRepository.FindAll(x => x.BillId == id);
            _billDetailRepository.RemoveMultiple(billDetails.ToList());
            _billRepository.Remove(bill);

            return billViewModel;
        }

        public void DeleteBillDetail(int id)
        {
            _billDetailRepository.Remove(id);
        }

        public PageResult<BillViewModel> GetAllBillPaging(int? option, DateTime fromDate, DateTime toDate, string keyword, int page, int pageSize)
        {
            var billModels = _billRepository.FindAll();

            if (fromDate != DateTime.MinValue & toDate != default(DateTime)) // sort by date time picker
            {
                billModels = billModels.Where(x => fromDate < x.CreatedDate && x.CreatedDate < toDate);
            }

            if (!string.IsNullOrEmpty(keyword)) // sort by keyword
            {
                billModels = billModels.Where(x => x.CustomerName.Contains(keyword)
                                             ||x.Id.ToString().Contains(keyword));
            }

            if (option.HasValue && keyword == null) // sort by Bill status
            {
                billModels = billModels.Where(x => x.BillStatus == (BillStatus)option);
            }

            if (option == null && fromDate == DateTime.MinValue) // this sort for all conditions == null
            {
                billModels = billModels.OrderByDescending(x => x.CreatedDate);
            }

            var total = billModels.Count();
            billModels = billModels.Skip((page - 1) * pageSize).Take(pageSize);

            var pageResult = new PageResult<BillViewModel>()
            {
                CurentPage = page,
                Results = Mapper.Map<List<Bill>, List<BillViewModel>>(billModels.ToList()),
                PageSize = pageSize,
                RowCount = total,
            };
            return pageResult;
        }

        public List<ColorViewModel> GetAllColor()
        {
            var colors = _colorRepository.FindAll().ToList();
            return Mapper.Map<List<Color>, List<ColorViewModel>>(colors);
        }

        public List<SizeViewModel> GetAllSize()
        {
            var sizes = _sizeRepository.FindAll().ToList();
            return Mapper.Map<List<Size>, List<SizeViewModel>>(sizes);
        }

        public BillViewModel GetBill(int id)
        {
            try
            {
                var billModel = _billRepository.FindById(id);
                var billDetails = _billDetailRepository.FindAll(x => x.BillId == billModel.Id);
                var billDetailVm = billDetails.ProjectTo<BillDetailViewModel>(billDetails).ToList();
                var billVm = Mapper.Map<Bill, BillViewModel>(billModel);
                billVm.BillDetails = billDetailVm;
                return billVm;
            }
            catch (Exception ex)
            {

            }
            return new BillViewModel();
        }

        public List<BillDetailViewModel> GetBillDetails(int billId)
        {
            var billDetails = _billDetailRepository.FindAll(x => x.BillId == billId);

            var billDetailViewModels = billDetails.ProjectTo<BillDetailViewModel>(billDetails);
            return billDetailViewModels.ToList();
        }

        public BillViewModel GetByBillId(int id)
        {
            var bill = _billRepository.FindSingle(x => x.Id == id, x => x.BillDetails);
            return Mapper.Map<Bill, BillViewModel>(bill);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public BillViewModel UpdateBill(BillViewModel billViewModel)
        {
            var billDetails = Mapper.Map<List<BillDetailViewModel>, List<BillDetail>>(billViewModel.BillDetails.ToList());

            var bill = Mapper.Map<BillViewModel, Bill>(billViewModel);
            bill.BillDetails.Clear();
            foreach (var item in billDetails)
            {
                var product = _productRepository.FindById(item.ProductId);
                item.Price = product.Price;
                var billDetail = _billDetailRepository.FindById(item.Id);

                if (billDetail == null) // add new bill detail
                {
                    bill.BillDetails.Add(item);
                }
                else // edit existed bill detail
                {
                    billDetail.ColorId = item.ColorId;
                    billDetail.ProductId = item.ProductId;
                    billDetail.SizeId = item.SizeId;
                    billDetail.Quantity = item.Quantity;
                    billDetail.Price = item.Price;

                    bill.BillDetails.Add(billDetail);
                }
            }

            // remove bill detail that
            var billDetailToRemove = _billDetailRepository.FindAll(x => !billDetails.Contains(x) && x.BillId == bill.Id).ToList();
            foreach (var item in billDetailToRemove)
            {
                _billDetailRepository.Remove(item.Id);
            }

            _billRepository.Update(bill);
            return billViewModel;
        }

        // Client active controller
        public BillViewModel GetLatestBillByCustomerId(Guid id)
        {
            var bill = _billRepository.FindAll(x => x.CustomerId == id, x => x.BillDetails).OrderByDescending(x => x.CreatedDate).First();
            return Mapper.Map<Bill, BillViewModel>(bill);
        }

        public List<BillViewModel> GetAll()
        {
            var bills = _billRepository.FindAll();
            return Mapper.Map<List<Bill>, List<BillViewModel>>(bills.ToList());
        }

        public int GetTotalSoldThisMonth()
        {
            var bills = _billRepository.FindAll();
            var billDetails = _billDetailRepository.FindAll();

            var query = from b in bills
                        join bd in billDetails
                        on b.Id equals bd.BillId
                        where b.CreatedDate.Month == DateTime.Now.Month
                        select bd.Quantity;
            return query.Sum(quantity => quantity);
        }

        public List<LatestPurchase> GetLatestPurchases()
        {
            var bills = _billRepository.FindAll().OrderByDescending(x => x.CreatedDate).Take(5);
            var products = _productRepository.FindAll();
            var listLatestPurchase = new List<LatestPurchase>();

            foreach (var item in bills)
            {
                var purchase = new LatestPurchase();
                purchase.Bill = Mapper.Map<Bill, BillViewModel>(item);
                purchase.TotalCost = 0;
                purchase.Quantity = 0;

                var billDetail = _billDetailRepository.FindAll(x => x.BillId == item.Id);
                foreach (var bd in billDetail)
                {
                    purchase.Quantity += bd.Quantity;
                    var product = _productRepository.FindById(bd.ProductId);
                    var price = product.PromotionPrice ?? product.Price;
                    purchase.TotalCost += bd.Quantity * price;
                }
                listLatestPurchase.Add(purchase);
            }
            return listLatestPurchase;
        }

        public PaymentDetails SortPaymentType()
        {
            var paymentTypes = new PaymentDetails();
            paymentTypes.Payments.CashOnDelivery = _billRepository.FindAll(x => x.PaymentMethod == PaymentMethod.CashOnDelivery).Count();
            paymentTypes.Payments.Atm = _billRepository.FindAll(x => x.PaymentMethod == PaymentMethod.Atm).Count();
            paymentTypes.Payments.MasterCard = _billRepository.FindAll(x => x.PaymentMethod == PaymentMethod.MasterCard).Count();
            paymentTypes.Payments.OnlineBanking = _billRepository.FindAll(x => x.PaymentMethod == PaymentMethod.OnlineBanking).Count();
            paymentTypes.Payments.PaymentGateway = _billRepository.FindAll(x => x.PaymentMethod == PaymentMethod.PaymentGateway).Count();
            paymentTypes.Payments.PayPal = _billRepository.FindAll(x => x.PaymentMethod == PaymentMethod.PayPal).Count();
            paymentTypes.Payments.Visa = _billRepository.FindAll(x => x.PaymentMethod == PaymentMethod.Visa).Count();

            paymentTypes.Total = paymentTypes.Payments.CashOnDelivery + paymentTypes.Payments.Atm + paymentTypes.Payments.MasterCard
                + paymentTypes.Payments.OnlineBanking + paymentTypes.Payments.PaymentGateway + paymentTypes.Payments.PayPal + paymentTypes.Payments.Visa;

            return paymentTypes;
        }

        public List<BillCondition> BillConditionReport()
        {
            var total = _billRepository.FindAll().Count();

            var valuesAsList = Enum.GetValues(typeof(BillStatus)).Cast<BillStatus>().ToList();
            var listConditions = new List<BillCondition>();
            foreach (var item in valuesAsList)
            {
                var bill = new BillCondition();
                bill.BillName = ((BillStatus)item).ToString();
                bill.Percent = Math.Round((double)_billRepository.FindAll(x => x.BillStatus == item).Count() / total * 100, 2);
                listConditions.Add(bill);
            }

            return listConditions;
        }

        public List<BillViewModel> GetBillsByCustomerId(Guid customerId)
        {
            var bills = _billRepository.FindAll(x => x.CustomerId == customerId);
            var billVms = bills.ProjectTo<BillViewModel>(bills);
            return billVms.ToList();
        }
    }
}

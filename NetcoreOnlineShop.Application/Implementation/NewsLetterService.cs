using AutoMapper;
using AutoMapper.QueryableExtensions;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels.NewsLetter;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using NetcoreOnlineShop.Infrastructure.Interfaces;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetcoreOnlineShop.Application.Implementation
{
    public class NewsLetterService : INewsLetterService
    {
        private readonly INewsLetterRepository _newsLetterRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public NewsLetterService(INewsLetterRepository newsLetterRepository, IUnitOfWork unitOfWork,
            ISubscriptionRepository subscriptionRepository)
        {
            this._subscriptionRepository = subscriptionRepository;
            this._newsLetterRepository = newsLetterRepository;
            this._unitOfWork = unitOfWork;
        }

        public void AddNewsLetter(NewsLetterViewModel newsLetterVm)
        {
            var newsLetter = Mapper.Map<NewsLetterViewModel, NewsLetter>(newsLetterVm);
            _newsLetterRepository.Add(newsLetter);
        }

        public List<NewsLetterViewModel> GetAll()
        {
            var newsLetter = _newsLetterRepository.FindAll();
            var newsLetterVms = newsLetter.ProjectTo<NewsLetterViewModel>(newsLetter);
            return newsLetterVms.ToList();
        }

        public List<SubscriptionViewModel> GetAllEmail()
        {
            var emails = _subscriptionRepository.FindAll();
            return emails.ProjectTo<SubscriptionViewModel>().ToList();
        }

        public PageResult<NewsLetterViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var newsLetter = _newsLetterRepository.FindAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                newsLetter = newsLetter.Where(x => x.Title.Contains(keyword)
                                              || x.Id.ToString().Contains(keyword));
            }

            newsLetter = newsLetter.OrderByDescending(x => x.CreatedDate);
            var total = newsLetter.Count();

            var query = newsLetter.Skip((page - 1) * pageSize).Take(pageSize);
            var pagination = new PageResult<NewsLetterViewModel>()
            {
                CurentPage = page,
                PageSize = pageSize,
                RowCount = total,
                Results = query.ProjectTo<NewsLetterViewModel>().ToList(),
            };
            return pagination;
        }

        public NewsLetterViewModel GetDetailById(int id)
        {
            var newsletter = _newsLetterRepository.FindById(id);
            var newsletterVm = Mapper.Map<NewsLetter, NewsLetterViewModel>(newsletter);
            return newsletterVm;
        }

        public bool isSentToUsers(int newsId)
        {
            var isExisted = _newsLetterRepository.FindAll(x =>x.Id == newsId && x.TotalReceiver > 0);

            if (isExisted.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void SubScribeEmail(SubscriptionViewModel subscriptionVm)
        {
            var subscription = Mapper.Map<SubscriptionViewModel, Subscription>(subscriptionVm);
            _subscriptionRepository.Add(subscription);
        }

        public void UpdateNewsLetter(NewsLetterViewModel newsLetterVm)
        {
            var newsLetter = _newsLetterRepository.FindById(newsLetterVm.Id);
            if (newsLetter != null)
            {
                newsLetter.Status = newsLetterVm.Status;
                newsLetter.TotalReceiver = newsLetterVm.TotalReceiver;
                newsLetter.Title = newsLetterVm.Title;
                newsLetter.Content = newsLetterVm.Content;

                _newsLetterRepository.Update(newsLetter);
            }
        }

        public void UpdateStatusSent(int newsId, int emailCount)
        {
            var news = _newsLetterRepository.FindById(newsId);
            news.TotalReceiver = emailCount;

            _newsLetterRepository.Update(news);
        }
    }
}

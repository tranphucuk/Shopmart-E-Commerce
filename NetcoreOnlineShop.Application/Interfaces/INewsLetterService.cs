using NetcoreOnlineShop.Application.ViewModels.NewsLetter;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface INewsLetterService
    {
        void AddNewsLetter(NewsLetterViewModel newsLetterVm);

        NewsLetterViewModel GetDetailById(int id);

        void SubScribeEmail(SubscriptionViewModel subscription);

        List<SubscriptionViewModel> GetAllEmail();

        void UpdateNewsLetter(NewsLetterViewModel newsLetterVm);

        List<NewsLetterViewModel> GetAll();

        PageResult<NewsLetterViewModel> GetAllPaging(string keyword, int page, int pageSize);

        void UpdateStatusSent(int newsId, int emailCount);

        bool isSentToUsers(int newsId);
        void Save();
    }
}

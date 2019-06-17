using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface IFeedbackService
    {
        void AddFeedback(FeedbackViewModel feedbackViewModel);

        List<FeedbackViewModel> GetAll();

        PageResult<FeedbackViewModel> GetAllPagination(int page, int pageSize, string keyword);

        FeedbackViewModel GetFeedbackDetail(int id);

        void SaveFeedback(FeedbackViewModel feedbackViewModel);

        GenericResult DeleteMulti(List<int> ids);
        GenericResult DeleteSingle(int id);

        void Save();
    }
}

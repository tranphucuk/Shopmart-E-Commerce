using AutoMapper;
using AutoMapper.QueryableExtensions;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels;
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
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUnitOfWork _unitOfWork;
        public FeedbackService(IFeedbackRepository feedbackRepository, IUnitOfWork unitOfWork)
        {
            this._feedbackRepository = feedbackRepository;
            this._unitOfWork = unitOfWork;
        }

        public void AddFeedback(FeedbackViewModel feedbackViewModel)
        {
            var feedback = Mapper.Map<FeedbackViewModel, Feedback>(feedbackViewModel);
            _feedbackRepository.Add(feedback);
        }

        public GenericResult DeleteMulti(List<int> ids)
        {
            foreach (var id in ids)
            {
                try
                {
                    _feedbackRepository.Remove(id);
                }
                catch (Exception)
                {
                    return new GenericResult(false, $"An error has occurred while deleting feedback Id: {id}.");
                }
            }
            return new GenericResult(true);
        }

        public GenericResult DeleteSingle(int id)
        {
            try
            {
                _feedbackRepository.Remove(id);
            }
            catch (Exception)
            {
                return new GenericResult(false, $"An error has occurred while deleting feedback Id: {id}.");
            }
            return new GenericResult(true);
        }

        public List<FeedbackViewModel> GetAll()
        {
            var feedbacks = _feedbackRepository.FindAll();
            var feedbackVms = feedbacks.ProjectTo<FeedbackViewModel>(feedbacks).ToList();
            return feedbackVms;
        }

        public PageResult<FeedbackViewModel> GetAllPagination(int page, int pageSize, string keyword)
        {
            var query = _feedbackRepository.FindAll();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword)
                                    || x.Email.Contains(keyword)
                                    || x.Message.Contains(keyword)
                                    || x.Address.Contains(keyword));
            }

            var total = query.Count();
            query = query.OrderBy(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
            var pageResult = new PageResult<FeedbackViewModel>()
            {
                Results = query.ProjectTo<FeedbackViewModel>().ToList(),
                PageSize = pageSize,
                RowCount = total

            };

            return pageResult;
        }

        public FeedbackViewModel GetFeedbackDetail(int id)
        {
            var feedback = _feedbackRepository.FindById(id);
            return Mapper.Map<Feedback, FeedbackViewModel>(feedback);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void SaveFeedback(FeedbackViewModel feedbackViewModel)
        {
            var feedback = Mapper.Map<FeedbackViewModel, Feedback>(feedbackViewModel);
            _feedbackRepository.Update(feedback);
        }
    }
}

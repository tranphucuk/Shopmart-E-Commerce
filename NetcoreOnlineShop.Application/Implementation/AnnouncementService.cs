using AutoMapper;
using AutoMapper.QueryableExtensions;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels.Announcement;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.Enums;
using NetcoreOnlineShop.Data.IRepositories;
using NetcoreOnlineShop.Infrastructure.Interfaces;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetcoreOnlineShop.Application.Implementation
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IAnnouncementUserRepository _announcementUserRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public AnnouncementService(IAnnouncementRepository announcementRepository, IUnitOfWork unitOfWork,
            IUserService userService, IAnnouncementUserRepository announcementUserRepository)
        {
            this._announcementUserRepository = announcementUserRepository;
            this._announcementRepository = announcementRepository;
            this._userService = userService;
            this._unitOfWork = unitOfWork;
        }

        public void Add(AnnouncementViewModel announcementVm)
        {
            var announcement = Mapper.Map<AnnouncementViewModel, Announcement>(announcementVm);
            _announcementRepository.Add(announcement);
        }

        public void Delete(int id)
        {
            _announcementRepository.Remove(id);

            var listAnnounUserToRemove = _announcementUserRepository.FindAll(x => x.AnnouncementId == id);
            _announcementUserRepository.RemoveMultiple(listAnnounUserToRemove.ToList());
        }

        public List<AnnouncementViewModel> Getall()
        {
            var announcements = _announcementRepository.FindAll();
            var announcementVms = announcements.ProjectTo<AnnouncementViewModel>(announcements);
            return announcementVms.ToList();
        }

        public PageResult<AnnouncementViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var announcements = _announcementRepository.FindAll();
            if (!string.IsNullOrEmpty(keyword))
            {
                announcements = announcements.Where(x => x.Title.Contains(keyword));
            }

            announcements = announcements.OrderByDescending(x => x.CreatedDate);

            var total = announcements.Count();
            var query = announcements.Skip((page - 1) * pageSize).Take(pageSize);

            var announcementVms = announcements.ProjectTo<AnnouncementViewModel>(query);
            var pageination = new PageResult<AnnouncementViewModel>()
            {
                CurentPage = page,
                Results = announcementVms.ToList(),
                RowCount = total,
                PageSize = pageSize
            };

            return pageination;
        }

        public AnnouncementViewModel GetDetail(int id)
        {
            var announ = _announcementRepository.FindById(id);
            var announVm = Mapper.Map<Announcement, AnnouncementViewModel>(announ);

            return announVm;
        }

        public List<AnnouncementViewModel> GetUserAnnouncement(Guid userId)
        {
            var announs = _announcementRepository.FindAll();
            var announUsers = _announcementUserRepository.FindAll();

            var query = from anoun in announs
                        join auser in announUsers
                        on anoun.Id equals auser.AnnouncementId
                        where auser.UserId == userId
                        select anoun;

            return query.ProjectTo<AnnouncementViewModel>().ToList();
        }

        public bool GetUserAnnouncementStatus(int announId, Guid userId)
        {
            var isExistedAnnoun = _announcementUserRepository.FindSingle(x => x.AnnouncementId == announId && x.UserId == userId);
            if (isExistedAnnoun != null)
            {
                return isExistedAnnoun.HasRead.Value;
            }
            return true;
        }

        public bool isSentToUsers(int announId)
        {
            var isExisted = _announcementUserRepository.FindAll(x => x.AnnouncementId == announId);

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

        public async Task SendAnnouncementToUser(int announId)
        {
            var announ = _announcementRepository.FindById(announId);
            var userIdList = (await _userService.GetAllAsync()).Where(x => x.Status == Status.Active).Select(x => x.Id);
            foreach (var id in userIdList)
            {
                announ.AnnouncementUsers.Add(new AnnouncementUser()
                {
                    AnnouncementId = announId,
                    HasRead = false,
                    Status = Status.Active,
                    UserId = id,
                });
            }
        }

        public void UpdateUserAnnouncement(int announId, Guid userId)
        {
            var announUser = _announcementUserRepository.FindSingle(x => x.AnnouncementId == announId && x.UserId == userId);
            announUser.HasRead = true;
            _announcementUserRepository.Update(announUser);
        }

        public void Update(AnnouncementViewModel announcementVm)
        {
            var announcement = _announcementRepository.FindById(announcementVm.Id);
            if (announcement != null)
            {
                announcement.UserId = announcementVm.UserId;
                announcement.Content = announcementVm.Content;
                announcement.CreatedDate = announcementVm.CreatedDate;
                announcement.Status = announcementVm.Status;
                announcement.Title = announcementVm.Title;
                announcement.Username = announcementVm.Username;

                _announcementRepository.Update(announcement);
            }
        }

        public DateTime GetDateSend(int announId, Guid userId)
        {
            var announUser = _announcementUserRepository.FindSingle(x => x.AnnouncementId == announId && x.UserId == userId);
            return announUser.CreatedDate;
        }
    }
}
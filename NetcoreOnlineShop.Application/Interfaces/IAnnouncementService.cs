using NetcoreOnlineShop.Application.ViewModels.Announcement;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface IAnnouncementService
    {
        List<AnnouncementViewModel> Getall();
        PageResult<AnnouncementViewModel> GetAllPaging(string keyword, int page, int pageSize);

        AnnouncementViewModel GetDetail(int id);
        void Add(AnnouncementViewModel announcementVm);

        void Update(AnnouncementViewModel announcementVm);

        Task SendAnnouncementToUser(int announId);

        List<AnnouncementViewModel> GetUserAnnouncement(Guid userId);

        DateTime GetDateSend(int announId, Guid userId);

        bool isSentToUsers(int announId);

        void UpdateUserAnnouncement(int announId, Guid userId);

        bool GetUserAnnouncementStatus(int announId, Guid userId);

        void Delete(int id);
        void Save();
    }
}

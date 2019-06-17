using NetcoreOnlineShop.Application.ViewModels.System;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface IUserService
    {
        Task<IList<AppUserViewModel>> GetAllAsync();

        PageResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);

        Task<GenericResult> AddAsync(AppUserViewModel appUserVm);

        Task<GenericResult> UpdateAsync(AppUserViewModel appUserVm);

        Task<GenericResult> DeleteAsync(Guid guid);

        Task<AppUserViewModel> FindByIdAsync(string id);

        Task<AppUserViewModel> FindByNameAsync(string name);

        // User activities

        void AddActivity(AppUserActivityViewModel userActivity);

        Task<PageResult<AppUserViewModel>> GetListUserByRoleName(string roleName, string keyword, int page, int pageSize);

        PageResult<AppUserActivityViewModel> GetUserActivity(Guid userId, int page, int pageSize);

        void Save();
    }
}

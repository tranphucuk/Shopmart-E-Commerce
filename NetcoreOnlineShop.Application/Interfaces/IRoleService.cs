using NetcoreOnlineShop.Application.ViewModels.System;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface IRoleService
    {
        Task<List<AppRoleViewModel>> GetAllAsync();

        PageResult<AppRoleViewModel> GetAllPaging(string keyword, int page, int pageSize);

        Task<AppRoleViewModel> GetById(Guid id);

        Task<GenericResult> UpdateAsync(AppRoleViewModel appRoleVm);

        Task<GenericResult> AddAsync(AppRoleViewModel appRoleVm);

        Task<GenericResult> DeleteAsync(Guid id);

        Task<List<FunctionViewModel>> GetFunctionsByRole(string[] roles);

        void SavePermission(List<PermissionViewModel> permissions, Guid roleId);

        List<PermissionViewModel> LoadPermission(Guid roleId);

        Task<bool> CheckPermission(string functionId, string action, string[] roles);
        GenericResult CheckParentPermission(List<PermissionViewModel> permissionViewModels);

        void Save();
    }
}

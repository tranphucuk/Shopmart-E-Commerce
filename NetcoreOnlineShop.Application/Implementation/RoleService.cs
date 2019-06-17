using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels.System;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using NetcoreOnlineShop.Infrastructure.Interfaces;
using NetcoreOnlineShop.Utilities.Constants;
using NetcoreOnlineShop.Utilities.Dtos;
using NetcoreOnlineShop.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NetcoreOnlineShop.Application.Implementation
{
    public class RoleService : IRoleService
    {
        private RoleManager<AppRole> _roleManager;
        private IUnitOfWork _unitOfWork;
        private IFunctionRepository _functionRepository;
        private IPermissionRepository _permissionRepository;
        private UserManager<AppUser> _userManager;

        public RoleService(RoleManager<AppRole> roleManager, IUnitOfWork unitOfWork, IFunctionRepository functionRepository,
            IPermissionRepository permissionRepository, UserManager<AppUser> userManager)
        {
            this._roleManager = roleManager;
            this._unitOfWork = unitOfWork;
            this._functionRepository = functionRepository;
            this._permissionRepository = permissionRepository;
            this._userManager = userManager;
        }

        public async Task<GenericResult> AddAsync(AppRoleViewModel appRoleVm)
        {
            var appRole = new AppRole(appRoleVm.Name, appRoleVm.Description);
            var isAdded = await _roleManager.CreateAsync(appRole);
            if (isAdded.Succeeded)
            {
                return new GenericResult(true, appRole.Name);
            }
            else
            {
                return new GenericResult(false, ConcenateErrors.Error(isAdded.Errors));
            }
        }

        public List<PermissionViewModel> LoadPermission(Guid roleId)
        {
            var permissionModels = _permissionRepository.FindAll(x => x.RoleId == roleId);
            var permissionViewModels = permissionModels.ProjectTo<PermissionViewModel>(permissionModels).ToList();
            return permissionViewModels;
        }

        public async Task<GenericResult> DeleteAsync(Guid id)
        {
            var appRole = await _roleManager.FindByIdAsync(id.ToString());
            if (appRole != null)
            {
                // Remove all Users from Role before deleting role
                var UsersInRole = await _userManager.GetUsersInRoleAsync(appRole.Name);
                foreach (var user in UsersInRole)
                {
                    await _userManager.RemoveFromRoleAsync(user, appRole.Name);
                }
                // Remove Role
                var isDeleted = await _roleManager.DeleteAsync(appRole);
                if (isDeleted.Succeeded)
                {
                    return new GenericResult(true, appRole.Name);
                }
                else
                {
                    return new GenericResult(false, ConcenateErrors.Error(isDeleted.Errors));
                }
            }
            return new GenericResult(false, CommonConstants.notFound);
        }

        public async Task<List<AppRoleViewModel>> GetAllAsync()
        {
            var roles = _roleManager.Roles;
            var listRolesVm = await roles.ProjectTo<AppRoleViewModel>().ToListAsync();
            return listRolesVm;
        }

        public PageResult<AppRoleViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _roleManager.Roles.Select(x => new AppRoleViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            });

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));
            }

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var totalRow = query.Count();

            var pagination = new PageResult<AppRoleViewModel>()
            {
                CurentPage = page,
                PageSize = pageSize,
                Results = query.ToList(),
                RowCount = totalRow,
            };
            return pagination;
        }

        public async Task<AppRoleViewModel> GetById(Guid id)
        {
            var appRole = await _roleManager.FindByIdAsync(id.ToString());
            var roleViewModel = Mapper.Map<AppRole, AppRoleViewModel>(appRole);
            return roleViewModel;
        }

        public async Task<List<FunctionViewModel>> GetFunctionsByRole(string[] roles)
        {
            var listFunctionViewModel = new List<FunctionViewModel>();
            foreach (var roleName in roles)
            {
                var roleModel = await _roleManager.FindByNameAsync(roleName);
                var permissions = _permissionRepository.FindAll(x => x.CanRead != false);
                var functions = _functionRepository.FindAll();
                var query = from p in permissions
                            join f in functions on p.FunctionId equals f.Id
                            where p.RoleId == roleModel.Id
                            select f;
                var functionVm = query.ProjectTo<FunctionViewModel>(query).ToList();
                listFunctionViewModel.AddRange(functionVm);
            }

            var parentFunctions = listFunctionViewModel.Where(x => x.ParentId == null).OrderBy(x=>x.SortOrder);
            var functionsSorted = new List<FunctionViewModel>();
            foreach (var function in parentFunctions)
            {
                functionsSorted.Add(function);
                var childrendFunc = listFunctionViewModel.Where(x => x.ParentId == function.Id).OrderBy(x => x.SortOrder);
                functionsSorted.AddRange(childrendFunc);
            }
            return functionsSorted;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void SavePermission(List<PermissionViewModel> permissions, Guid roleId)
        {
            var allPermissions = _permissionRepository.FindAll(x => x.RoleId == roleId);
            _permissionRepository.RemoveMultiple(allPermissions.ToList());
            if (permissions.Count > 0)
            {
                var permissionModels = Mapper.Map<List<PermissionViewModel>, List<Permission>>(permissions);
                foreach (var pm in permissionModels)
                {
                    _permissionRepository.Add(pm);
                }
            }
        }

        public async Task<GenericResult> UpdateAsync(AppRoleViewModel appRoleVm)
        {
            var appRole = await _roleManager.FindByIdAsync(appRoleVm.Id.ToString());
            if (appRoleVm != null)
            {
                appRole.Name = appRoleVm.Name;
                appRole.Description = appRoleVm.Description;

                var isUpdated = await _roleManager.UpdateAsync(appRole);
                if (isUpdated.Succeeded)
                {
                    return new GenericResult(true, appRole.Name);
                }
                else
                {
                    return new GenericResult(false, ConcenateErrors.Error(isUpdated.Errors));
                }
            }
            return new GenericResult(false, CommonConstants.notFound);
        }

        public Task<bool> CheckPermission(string functionId, string action, string[] roles)
        {
            var functions = _functionRepository.FindAll(f => f.Id == functionId);
            var permissions = _permissionRepository.FindAll(p => p.FunctionId == functionId);

            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId
                        join r in _roleManager.Roles on p.RoleId equals r.Id
                        where roles.Contains(r.Name)
                        && ((p.CanCreate && action == "Create")
                        || (p.CanDelete && action == "Delete")
                        || (p.CanRead && action == "Read")
                        || (p.CanUpdate && action == "Update"))
                        select p;
            return query.AnyAsync();
        }

        public GenericResult CheckParentPermission(List<PermissionViewModel> permissionViewModels)
        {
            foreach (var permission in permissionViewModels)
            {
                var function = _functionRepository.FindSingle(x => x.Id == permission.FunctionId && x.ParentId != null);
                if (function != null)
                {
                    var parentFunc = _functionRepository.FindSingle(x => x.Id == function.ParentId);
                    var parentPermission = permissionViewModels.FirstOrDefault(x => x.FunctionId == parentFunc.Id);

                    if (parentPermission.CanRead == false && permission.CanRead) // compare parent and child 'read' permission
                    {
                        return new GenericResult(false, "Read", function.Name);
                    }
                    if (parentPermission.CanCreate == false && permission.CanCreate) // compare parent and child 'create' permission
                    {
                        return new GenericResult(false, "Create", function.Name);
                    }
                    if (parentPermission.CanUpdate == false && permission.CanUpdate)
                    {
                        return new GenericResult(false, "Edit", function.Name);
                    }
                    if (parentPermission.CanDelete == false && permission.CanDelete)
                    {
                        return new GenericResult(false, "Delete", function.Name);
                    }
                }
            }
            return new GenericResult(true);
        }
    }
}

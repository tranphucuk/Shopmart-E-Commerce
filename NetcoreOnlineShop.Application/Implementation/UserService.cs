using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels.System;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using NetcoreOnlineShop.Infrastructure.Interfaces;
using NetcoreOnlineShop.Utilities.Dtos;
using NetcoreOnlineShop.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetcoreOnlineShop.Application.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IAppUserActivityRepository _appUserActivityRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
            IAppUserActivityRepository appUserActivityRepository, IUnitOfWork unitOfWork)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._appUserActivityRepository = appUserActivityRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<GenericResult> AddAsync(AppUserViewModel appUserVm)
        {
            var user = new AppUser()
            {
                UserName = appUserVm.UserName,
                Email = appUserVm.Email,
                Avatar = appUserVm.Avatar,
                Balance = appUserVm.Balance,
                CreatedDate = DateTime.Now,
                FullName = appUserVm.FullName,
                PhoneNumber = appUserVm.PhoneNumber,
                Status = appUserVm.Status,
            };
            var result = await _userManager.CreateAsync(user, appUserVm.Password);
            if (result.Succeeded)
            {
                var isUserExisted = await _userManager.FindByNameAsync(user.UserName);
                if (isUserExisted != null)
                {
                    if (appUserVm.Roles.Count > 0)
                    {
                        var isSuccess = await _userManager.AddToRolesAsync(user, appUserVm.Roles);
                        if (isSuccess.Succeeded)
                        {
                            return new GenericResult(user.UserName);
                        }
                        else
                        {
                            var errors = ConcenateErrors.Error(result.Errors);
                            return new GenericResult(errors);
                        }
                    }
                }
                else
                {
                    return new GenericResult(false);
                }
                return new GenericResult(user.UserName);
            }
            else
            {
                var errors = ConcenateErrors.Error(result.Errors);
                return new GenericResult(errors);
            }
        }

        public async Task<GenericResult> DeleteAsync(Guid guid)
        {
            var appUser = await _userManager.FindByIdAsync(guid.ToString());
            if (appUser != null)
            {
                var roles = await _userManager.GetRolesAsync(appUser);
                await _userManager.RemoveFromRolesAsync(appUser, roles);
                var result = await _userManager.DeleteAsync(appUser);
                if (result.Succeeded)
                {
                    return new GenericResult(true, appUser.UserName);
                }
                else
                {
                    var errors = string.Join(",", result.Errors);
                    return new GenericResult(false, appUser.UserName);
                }
            }
            else
            {
                return new GenericResult(false, appUser.UserName);
            }
        }

        public async Task<AppUserViewModel> FindByIdAsync(string id)
        {
            var appUser = await _userManager.FindByIdAsync(id);
            if (appUser != null)
            {
                var roles = await _userManager.GetRolesAsync(appUser);
                var userViewModel = Mapper.Map<AppUser, AppUserViewModel>(appUser);
                userViewModel.Roles = roles.ToList();
                return userViewModel;
            }
            return null;
        }

        public async Task<AppUserViewModel> FindByNameAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            return Mapper.Map<AppUser, AppUserViewModel>(user);
        }

        public async Task<IList<AppUserViewModel>> GetAllAsync()
        {
            var totalUsers = await _userManager.Users.ProjectTo<AppUserViewModel>().ToListAsync();
            return totalUsers;
        }

        public PageResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.UserName.Contains(keyword) || x.Email.Contains(keyword) || x.FullName.Contains(keyword));
            }
            int totalRow = query.Count();

            query = query.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);

            var data = query.Select(x => new AppUserViewModel()
            {
                UserName = x.UserName,
                Avatar = x.Avatar,
                Email = x.Email,
                FullName = x.FullName,
                Id = x.Id,
                PhoneNumber = x.PhoneNumber,
                Status = x.Status,
                CreatedDate = x.CreatedDate
            }).ToList();

            var pagination = new PageResult<AppUserViewModel>();
            pagination.CurentPage = page;
            pagination.PageSize = pageSize;
            pagination.Results = data;
            pagination.RowCount = totalRow;
            return pagination;
        }

        public async Task<GenericResult> UpdateAsync(AppUserViewModel appUserVm)
        {
            var appUser = await _userManager.FindByIdAsync(appUserVm.Id.ToString());
            if (appUser != null)
            {
                if (appUserVm.Roles.Count > 0)
                {
                    var roles = await _userManager.GetRolesAsync(appUser);
                    var isDeleted = await _userManager.RemoveFromRolesAsync(appUser, roles);
                    if (isDeleted.Succeeded == false)
                    {
                        var errors = ConcenateErrors.Error(isDeleted.Errors);
                        return new GenericResult(errors);
                    }
                    var isAdded = await _userManager.AddToRolesAsync(appUser, appUserVm.Roles);
                    if (isAdded.Succeeded == false)
                    {
                        var errors = ConcenateErrors.Error(isDeleted.Errors);
                        return new GenericResult(errors);
                    }
                }
                else
                {
                    var roles = await _userManager.GetRolesAsync(appUser);
                    var isDeleted = await _userManager.RemoveFromRolesAsync(appUser, roles);
                    if (isDeleted.Succeeded == false)
                    {
                        var errors = ConcenateErrors.Error(isDeleted.Errors);
                        return new GenericResult(errors);
                    }
                }
                appUser.Email = appUserVm.Email;
                appUser.PhoneNumber = appUserVm.PhoneNumber;
                appUser.FullName = appUserVm.FullName;
                appUser.Balance = appUserVm.Balance;
                appUser.Avatar = appUserVm.Avatar;
                appUser.Status = appUserVm.Status;
                appUser.ModifiedDate = DateTime.Now;

                var result = await _userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                {
                    return new GenericResult(true, appUser.UserName);
                }
                else
                {
                    var errors = ConcenateErrors.Error(result.Errors);
                    return new GenericResult(false, errors);
                }
            }
            return new GenericResult(false);
        }

        // User activity
        public void AddActivity(AppUserActivityViewModel userActivityVm)
        {
            var userActivity = Mapper.Map<AppUserActivityViewModel, AppUserActivity>(userActivityVm);
            _appUserActivityRepository.Add(userActivity);
        }

        public PageResult<AppUserActivityViewModel> GetUserActivity(Guid userId, int page, int pageSize)
        {
            var listActivities = _appUserActivityRepository.FindAll(x => x.UserId == userId);

            listActivities = listActivities.OrderByDescending(x => x.LastSession);

            var total = listActivities.Count();
            listActivities = listActivities.Skip((page - 1) * pageSize).Take(pageSize);

            var activitiesVm = Mapper.Map<List<AppUserActivity>, List<AppUserActivityViewModel>>(listActivities.ToList());
            var pageResult = new PageResult<AppUserActivityViewModel>()
            {
                CurentPage = page,
                Results = activitiesVm,
                RowCount = total,
                PageSize = pageSize,
            };
            return pageResult;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public async Task<PageResult<AppUserViewModel>> GetListUserByRoleName(string roleName, string keyword, int page, int pageSize)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);

            if (!string.IsNullOrEmpty(keyword))
            {
                users = users.Where(x => x.UserName.Contains(keyword)).ToList();
            }
            var userVms = Mapper.Map<List<AppUser>, List<AppUserViewModel>>(users.ToList());

            var total = userVms.Count;
            var listUserPaging = userVms.Skip((page - 1) * pageSize).Take(pageSize);

            var pageination = new PageResult<AppUserViewModel>()
            {
                RowCount = total,
                PageSize = pageSize,
                Results = listUserPaging.ToList(),
                CurentPage = page
            };
            return pageination;
        }
    }
}

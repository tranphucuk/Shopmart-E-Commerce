using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels.System;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.Enums;
using NetcoreOnlineShop.Data.IRepositories;
using NetcoreOnlineShop.Infrastructure.Interfaces;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetcoreOnlineShop.Application.Implementation
{
    public class FunctionService : IFuntionService
    {
        private readonly IFunctionRepository _functionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FunctionService(IFunctionRepository functionRepository, IUnitOfWork unitOfWork)
        {
            this._functionRepository = functionRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<List<FunctionViewModel>> GetAll()
        {
            var functions = await _functionRepository.FindAll(x => x.Status == Status.Active)
                                                     .ProjectTo<FunctionViewModel>().ToListAsync();
            var parentFunctions = functions.Where(x => x.ParentId == null).OrderBy(x => x.SortOrder);

            var functionsSorted = new List<FunctionViewModel>();
            foreach (var function in parentFunctions)
            {
                functionsSorted.Add(function);
                var childrendFunc = functions.Where(x => x.ParentId == function.Id).OrderBy(x => x.SortOrder);
                functionsSorted.AddRange(childrendFunc);
            }
            return functionsSorted;
        }

        public Task<List<FunctionViewModel>> GetAllByPermission(Guid userId)
        {
            throw new NotImplementedException();
        }

        public PageResult<FunctionViewModel> GetAllPaging(int page, int pageSize, string keyword)
        {
            var query = _functionRepository.FindAll().ProjectTo<FunctionViewModel>();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Id.ToLower().Contains(keyword)
                                        || x.Name.ToLower().Contains(keyword)
                                        || x.ParentId.ToLower().Contains(keyword));
            }
            var total = query.Count();
            if (string.IsNullOrEmpty(keyword))
            {
                var parentFunctions = query.Where(x => x.ParentId == null);
                var functionsSorted = new List<FunctionViewModel>();
                foreach (var function in parentFunctions)
                {
                    functionsSorted.Add(function);
                    var childrendFunc = query.Where(x => x.ParentId == function.Id).OrderBy(x => x.SortOrder);
                    functionsSorted.AddRange(childrendFunc);
                }

                functionsSorted = functionsSorted.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return new PageResult<FunctionViewModel>()
                {
                    PageSize = pageSize,
                    RowCount = total,
                    Results = functionsSorted
                };
            }

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            return new PageResult<FunctionViewModel>()
            {
                PageSize = pageSize,
                RowCount = total,
                Results = query.ToList()
            };
        }

        public FunctionViewModel GetFunctionDetail(string id)
        {
            var function = _functionRepository.FindById(id);
            var functionVm = Mapper.Map<Function, FunctionViewModel>(function);
            return functionVm;
        }

        public List<FunctionViewModel> GetParents()
        {
            var parents = _functionRepository.FindAll(x => x.ParentId == null);
            return parents.ProjectTo<FunctionViewModel>().ToList();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void SaveFunction(FunctionViewModel functionViewModel)
        {
            var function = _functionRepository.FindById(functionViewModel.Id);
            if (function != null)
            {
                function.IconCss = functionViewModel.IconCss;
                function.Name = functionViewModel.Name;
                function.Status = functionViewModel.Status;
                function.SortOrder = functionViewModel.SortOrder;

                _functionRepository.Update(function);
            }
        }
    }
}

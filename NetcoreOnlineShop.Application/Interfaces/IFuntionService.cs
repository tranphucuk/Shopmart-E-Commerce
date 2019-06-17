using NetcoreOnlineShop.Application.ViewModels.System;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface IFuntionService : IDisposable
    {
        Task<List<FunctionViewModel>> GetAll();

        Task<List<FunctionViewModel>> GetAllByPermission(Guid userId);

        FunctionViewModel GetFunctionDetail(string id);

        List<FunctionViewModel> GetParents();

        PageResult<FunctionViewModel> GetAllPaging(int page, int pageSize, string keyword);

        void Save();

        void SaveFunction(FunctionViewModel functionViewModel);
    }
}
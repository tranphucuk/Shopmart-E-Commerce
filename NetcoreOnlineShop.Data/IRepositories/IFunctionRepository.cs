using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.IRepositories
{
    public interface IFunctionRepository : IRepository<Function, string>
    {
    }
}

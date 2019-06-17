using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Call SaveChanges() from DbContext;
        /// </summary>
        void Commit();
    }
}

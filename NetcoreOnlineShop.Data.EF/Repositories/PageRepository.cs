﻿using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Data.EF.Repositories
{
    public class PageRepository : EFRepository<Page, int>, IPageRepository
    {
        public PageRepository(AppDbContext context) : base(context)
        {
        }
    }
}

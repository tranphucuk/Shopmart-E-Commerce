using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface ITagService
    {
        void Add(Tag tag);

        Tag FindById(string tagId);

        List<TagViewModel> GetByType(string tagType);

        void Save();
    }
}

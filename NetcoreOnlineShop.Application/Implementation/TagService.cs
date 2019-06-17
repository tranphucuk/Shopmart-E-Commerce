using AutoMapper;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using NetcoreOnlineShop.Infrastructure.Interfaces;
using NetcoreOnlineShop.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetcoreOnlineShop.Application.Implementation
{
    public class TagService : ITagService
    {
        ITagRepository _tagRepository;
        IUnitOfWork _unitOfWork;

        public TagService(ITagRepository tagRepository, IUnitOfWork unitOfWork)
        {
            this._tagRepository = tagRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Add(Tag tag)
        {
            _tagRepository.Add(tag);
        }


        public Tag FindById(string tagId)
        {
            return _tagRepository.FindById(tagId);
        }

        public List<TagViewModel> GetByType(string tagType)
        {
            var tags = _tagRepository.FindAll(x => x.Type == CommonConstants.productTag);
            return Mapper.Map<List<Tag>, List<TagViewModel>>(tags.ToList());
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}

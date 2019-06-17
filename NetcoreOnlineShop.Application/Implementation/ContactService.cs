using AutoMapper;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.Enums;
using NetcoreOnlineShop.Data.IRepositories;
using NetcoreOnlineShop.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetcoreOnlineShop.Application.Implementation
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ContactService(IContactRepository contactRepository, IUnitOfWork unitOfWork)
        {
            this._contactRepository = contactRepository;
            this._unitOfWork = unitOfWork;
        }
        public ContactViewModel LoadContactDetail()
        {
            var contact = _contactRepository.FindAll();
            var contactVm = Mapper.Map<List<Contact>, List<ContactViewModel>>(contact.ToList());
            return contactVm.First();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void SaveContact(ContactViewModel contactViewModel)
        {
            var contact = _contactRepository.FindById(contactViewModel.Id);

            contact.Name = contactViewModel.Name;
            contact.Address = contactViewModel.Address;
            contact.Email = contactViewModel.Email;
            contact.Other = contactViewModel.Other;
            contact.Phone = contactViewModel.Phone;
            contact.Website = contactViewModel.Website;
            contact.Status = Status.Active;

            _contactRepository.Update(contact);
        }
    }
}

using NetcoreOnlineShop.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface IContactService
    {
        ContactViewModel LoadContactDetail();

        void SaveContact(ContactViewModel contactViewModel);

        void Save();
    }
}

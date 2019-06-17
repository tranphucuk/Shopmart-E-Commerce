using NetcoreOnlineShop.Application.ViewModels.Ticket;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;

namespace NetcoreOnlineShop.Application.Interfaces
{
    public interface ISupportTicketService
    {
        List<SupportTicketViewModel> GetAll();

        PageResult<SupportTicketViewModel> GetAllPaging(string keyword, int page, int pageSize);

        void Add(SupportTicketViewModel ticketVm);

        bool isTicketexisted(Guid userId, int billId);

        SupportTicketViewModel GetDetailById(int id);

        void UpdateTicket(SupportTicketViewModel ticket);

        void Save();
    }
}
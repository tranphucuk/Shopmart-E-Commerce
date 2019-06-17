using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetcoreOnlineShop.Application.Interfaces;
using NetcoreOnlineShop.Application.ViewModels.Ticket;
using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;
using NetcoreOnlineShop.Infrastructure.Interfaces;
using NetcoreOnlineShop.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetcoreOnlineShop.Application.Implementation
{
    public class SupportTicketService : ISupportTicketService
    {
        private readonly ISupportTicketRepository _supportTicketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SupportTicketService(ISupportTicketRepository supportTicketRepository, IUnitOfWork unitOfWork)
        {
            this._supportTicketRepository = supportTicketRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Add(SupportTicketViewModel ticketVm)
        {
            var ticket = Mapper.Map<SupportTicketViewModel, SupportTicket>(ticketVm);
            _supportTicketRepository.Add(ticket);
        }

        public List<SupportTicketViewModel> GetAll()
        {
            var tickets = _supportTicketRepository.FindAll();
            var ticketVms = tickets.ProjectTo<SupportTicketViewModel>(tickets);
            return ticketVms.ToList();
        }

        public PageResult<SupportTicketViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var tickets = _supportTicketRepository.FindAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                tickets = tickets.Where(x => x.Title.Contains(keyword)
                                        || x.BillId.ToString().Contains(keyword)
                                        || x.Email.Contains(keyword));
            }
            tickets = tickets.OrderByDescending(x => x.CreatedDate);
            var total = tickets.Count();
            var result = tickets.Skip((page - 1) * pageSize).Take(pageSize);

            var pagination = new PageResult<SupportTicketViewModel>()
            {
                CurentPage = page,
                PageSize = pageSize,
                Results = result.ProjectTo<SupportTicketViewModel>().ToList(),
                RowCount = total,
            };

            return pagination;
        }

        public SupportTicketViewModel GetDetailById(int id)
        {
            var ticket = _supportTicketRepository.FindById(id);
            var ticketVm = Mapper.Map<SupportTicket, SupportTicketViewModel>(ticket);
            return ticketVm;
        }

        public bool isTicketexisted(Guid userId, int billId)
        {
            var isExisted = _supportTicketRepository.FindAll(x => x.UserId == userId && x.BillId == billId);
            if (isExisted.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateTicket(SupportTicketViewModel ticket)
        {
            var ticketModel = _supportTicketRepository.FindById(ticket.Id);
            ticketModel.Status = ticket.Status;
            _supportTicketRepository.Update(ticketModel);
        }
    }
}
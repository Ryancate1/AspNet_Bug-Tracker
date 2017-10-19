using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using rcate_BugTracker.Models.CodeFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rcate_BugTracker.Models.Helpers
{
    public class TicketHistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public void AssignChange(Ticket ticket, string userId)
        {
            TicketHistory ticketHistory = new TicketHistory();
            Ticket oldTicket = db.Ticket.AsNoTracking().First(t => t.Id == ticket.Id);
            if (oldTicket.AssignToUserId == null)
            {
                ticketHistory.OldValue = "Unassigned";
            }
            else
            {
                ticketHistory.OldValue = oldTicket.AssignToUser.FullName;
            }
            ticketHistory.NewValue = db.Users.Find(ticket.AssignToUserId).FullName;
            ticketHistory.TicketId = ticket.Id;
            ticketHistory.Property = "Assigned User";
            ticketHistory.Created = DateTimeOffset.UtcNow;
            ticketHistory.AuthorId = userId;
            db.TicketHistory.Add(ticketHistory);
            db.SaveChanges();
        }


        public void TitleChange(Ticket ticket, string userId)
        {
            TicketHistory ticketHistory = new TicketHistory();
            Ticket oldTicket = db.Ticket.AsNoTracking().First(t => t.Id == ticket.Id);
            ticketHistory.OldValue = oldTicket.Title;
            ticketHistory.NewValue = ticket.Title;
            ticketHistory.TicketId = ticket.Id;
            ticketHistory.Property = "Title";
            ticketHistory.Created = DateTimeOffset.UtcNow;
            ticketHistory.AuthorId = userId;
            db.TicketHistory.Add(ticketHistory);
            db.SaveChanges();
        }


        public void DescriptionChange(Ticket ticket, string userId)
        {
            TicketHistory ticketHistory = new TicketHistory();
            Ticket oldTicket = db.Ticket.AsNoTracking().First(t => t.Id == ticket.Id);
            ticketHistory.OldValue = oldTicket.Description;
            ticketHistory.NewValue = ticket.Description;
            ticketHistory.TicketId = ticket.Id;
            ticketHistory.Property = "Description";
            ticketHistory.Created = DateTimeOffset.UtcNow;
            ticketHistory.AuthorId = userId;
            db.TicketHistory.Add(ticketHistory);
            db.SaveChanges();
        }
        
        public void StatusChange(Ticket ticket, string userId)
        {
            TicketHistory ticketHistory = new TicketHistory();
            Ticket oldTicket = db.Ticket.AsNoTracking().First(t => t.Id == ticket.Id);
            ticketHistory.OldValue = oldTicket.TicketStatus.Name;
            ticketHistory.NewValue = db.TicketStatus.Find(ticket.TicketStatusId).Name;
            ticketHistory.TicketId = ticket.Id;
            ticketHistory.Property = "Ticket Status";
            ticketHistory.Created = DateTimeOffset.UtcNow;
            ticketHistory.AuthorId = userId;
            db.TicketHistory.Add(ticketHistory);
            db.SaveChanges();
        }
        
        public void PriorityChange(Ticket ticket, string userId)
        {
            TicketHistory ticketHistory = new TicketHistory();
            Ticket oldTicket = db.Ticket.AsNoTracking().First(t => t.Id == ticket.Id);
            ticketHistory.OldValue = oldTicket.TicketPriority.Name;
            ticketHistory.NewValue = db.TicketPriority.Find(ticket.TicketPriorityId).Name;
            ticketHistory.TicketId = ticket.Id;
            ticketHistory.Property = "Ticket Priority";
            ticketHistory.Created = DateTimeOffset.UtcNow;
            ticketHistory.AuthorId = userId;
            db.TicketHistory.Add(ticketHistory);
            db.SaveChanges();
        }
        
        public void TypeChange(Ticket ticket, string userId)
        {
            TicketHistory ticketHistory = new TicketHistory();
            Ticket oldTicket = db.Ticket.AsNoTracking().First(t => t.Id == ticket.Id);
            ticketHistory.OldValue = oldTicket.TicketType.Name;
            ticketHistory.NewValue = db.TicketType.Find(ticket.TicketTypeId).Name;
            ticketHistory.TicketId = ticket.Id;
            ticketHistory.Property = "Ticket Type";
            ticketHistory.Created = DateTimeOffset.UtcNow;
            ticketHistory.AuthorId = userId;
            db.TicketHistory.Add(ticketHistory);
            db.SaveChanges();
        }
    }
}
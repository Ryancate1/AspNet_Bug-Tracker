using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rcate_BugTracker.Models.CodeFirst
{
    public class Ticket
    {
        public Ticket()
        {
            Users = new HashSet<ApplicationUser>();
            Comment = new HashSet<TicketComment>();
            Attatchments = new HashSet<TicketAttatchment>();
            Histories = new HashSet<TicketHistory>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public int ProjectId { get; set; }
        public int TicketTypeId { get; set; }
        public int TicketPriorityId { get; set; }
        public int TicketStatusId { get; set; }
        public string OwnerUserId { get; set; }
        public string AssignToUserId { get; set; }
        public bool Archived { get; set; }

        public virtual Project Project { get; set; }
        public virtual TicketType TicketType { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual ApplicationUser OwnerUser { get; set; }
        public virtual ApplicationUser AssignToUser { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<TicketComment> Comment { get; set; }
        public virtual ICollection<TicketAttatchment> Attatchments { get; set; }
        public virtual ICollection<TicketHistory> Histories { get; set; }
    }
}
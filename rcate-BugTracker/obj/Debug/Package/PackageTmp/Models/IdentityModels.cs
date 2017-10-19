using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using rcate_BugTracker.Models.CodeFirst;
using System.Collections.Generic;

namespace rcate_BugTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TimeZone { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public ApplicationUser()
        {
            Project = new HashSet<Project>();
            Ticket = new HashSet<Ticket>();
            History = new HashSet<TicketHistory>();
            Comment = new HashSet<TicketComment>();
            Attatchment = new HashSet<TicketAttatchment>();
        }

        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<Ticket> Ticket { get; set; }
        public virtual ICollection<TicketHistory> History { get; set; }
        public virtual ICollection<TicketComment> Comment { get; set; }
        public virtual ICollection<TicketAttatchment> Attatchment { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Project> Project { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<TicketAttatchment> TicketAttatchment { get; set; }
        public DbSet<TicketComment> TicketComment { get; set; }
        public DbSet<TicketHistory> TicketHistory { get; set; }
        public DbSet<TicketPriority> TicketPriority { get; set; }
        public DbSet<TicketStatus> TicketStatus { get; set; }
        public DbSet<TicketType> TicketType { get; set; }
    }
}
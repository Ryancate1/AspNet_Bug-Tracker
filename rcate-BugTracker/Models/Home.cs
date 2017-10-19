using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using rcate_BugTracker.Models.CodeFirst;

namespace rcate_BugTracker.Models
{
    public class Home
    {
        public Home()
        {
            Ticket = new HashSet<Ticket>();
        }
        
        public virtual IEnumerable<Ticket> Ticket { get; set; }
        public virtual IEnumerable<Project> Project { get; set; }
    }
}
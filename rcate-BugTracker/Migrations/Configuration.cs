namespace rcate_BugTracker.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using rcate_BugTracker.Models;
    using rcate_BugTracker.Models.CodeFirst;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<rcate_BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(rcate_BugTracker.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }
            if (!context.Roles.Any(r => r.Name == "DemoA"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoA" });
            }
            if (!context.Roles.Any(r => r.Name == "DemoP"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoP" });
            }
            if (!context.Roles.Any(r => r.Name == "DemoD"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoD" });
            }
            if (!context.Roles.Any(r => r.Name == "DemoS"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoS" });
            }


            /////////////////////////////////////////////////////////////////////////////////////

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "ryan.cate@yahoo.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ryan.cate@yahoo.com",
                    Email = "ryan.cate@yahoo.com",
                    FirstName = "Ryan",
                    LastName = "Cate",
                }, "Password1");
            }
            var Admin1 = userManager.FindByEmail("ryan.cate@yahoo.com").Id;
            userManager.AddToRole(Admin1, "Admin");


            if (!context.Users.Any(u => u.Email == "rchapman@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "rchapman@coderfoundry.com",
                    Email = "rchapman@coderfoundry.com",
                    FirstName = "Ryan",
                    LastName = "Chapman",
                }, "Password1");
            }
            var Admin2 = userManager.FindByEmail("rchapman@coderfoundry.com").Id;
            userManager.AddToRole(Admin2, "Admin");


            if (!context.Users.Any(u => u.Email == "mjaang@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "mjaang@coderfoundry.com",
                    Email = "mjaang@coderfoundry.com",
                    FirstName = "Mark",
                    LastName = "Jaang",
                }, "Password1");
            }
            var Admin3 = userManager.FindByEmail("mjaang@coderfoundry.com").Id;
            userManager.AddToRole(Admin3, "Admin");


            if (!context.Users.Any(u => u.Email == "admin.demo@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "admin.demo@coderfoundry.com",
                    Email = "admin.demo@coderfoundry.com",
                    FirstName = "Admin",
                    LastName = "Demo",
                }, "Password1");
            }
            var Admin4 = userManager.FindByEmail("admin.demo@coderfoundry.com").Id;
            userManager.AddToRole(Admin4, "DemoA");


            if (!context.Users.Any(u => u.Email == "projectmanager.demo@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "projectmanager.demo@coderfoundry.com",
                    Email = "projectmanager.demo@coderfoundry.com",
                    FirstName = "Manager",
                    LastName = "Demo",
                }, "Password1");
            }
            var Manager1 = userManager.FindByEmail("projectmanager.demo@coderfoundry.com").Id;
            userManager.AddToRole(Manager1, "DemoP");


            if (!context.Users.Any(u => u.Email == "developer.demo@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "developer.demo@coderfoundry.com",
                    Email = "developer.demo@coderfoundry.com",
                    FirstName = "Developer",
                    LastName = "Demo",
                }, "Password1");
            }
            var Developer1 = userManager.FindByEmail("developer.demo@coderfoundry.com").Id;
            userManager.AddToRole(Developer1, "DemoD");


            if (!context.Users.Any(u => u.Email == "submitter.demo@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "submitter.demo@coderfoundry.com",
                    Email = "submitter.demo@coderfoundry.com",
                    FirstName = "Submitter",
                    LastName = "Demo",
                }, "Password1");
            }
            var Submitter1 = userManager.FindByEmail("submitter.demo@coderfoundry.com").Id;
            userManager.AddToRole(Submitter1, "DemoS");

            /////////////////////////////////////////////////////////////////////////////////////

            if (!context.TicketPriority.Any(p => p.Name == "Urgent"))
            {
                var priority = new TicketPriority();
                priority.Name = "Urgent";
                context.TicketPriority.Add(priority);
            }

            if (!context.TicketPriority.Any(p => p.Name == "Moderate"))
            {
                var priority = new TicketPriority();
                priority.Name = "Moderate";
                context.TicketPriority.Add(priority);
            }

            if (!context.TicketPriority.Any(p => p.Name == "Low"))
            {
                var priority = new TicketPriority();
                priority.Name = "Low";
                context.TicketPriority.Add(priority);
            }

            /////////////////////////////////////////////////////////////////////////////////////

            if (!context.TicketStatus.Any(s => s.Name == "Completed"))
            {
                var status = new TicketStatus();
                status.Name = "Completed";
                context.TicketPriority.Any(s => s.Name == "completed");
            }

            if (!context.TicketStatus.Any(s => s.Name == "In Progress"))
            {
                var status = new TicketStatus();
                status.Name = "In Progress";
                context.TicketPriority.Any(s => s.Name == "In Progress");
            }

            if (!context.TicketStatus.Any(s => s.Name == "In Review"))
            {
                var status = new TicketStatus();
                status.Name = "In Review";
                context.TicketPriority.Any(s => s.Name == "In Review");
            }

            /////////////////////////////////////////////////////////////////////////////////////

            if (!context.TicketType.Any(t => t.Name == "Hardware"))
            {
                var type = new TicketType();
                type.Name = "Hardware";
                context.TicketType.Any(t => t.Name == "Hardware");
            }
            if (!context.TicketType.Any(t => t.Name == "Software"))
            {
                var type = new TicketType();
                type.Name = "Software";
                context.TicketType.Any(t => t.Name == "Software");
            }
        }
    }
}

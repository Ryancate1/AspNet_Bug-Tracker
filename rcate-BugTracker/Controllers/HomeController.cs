using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using rcate_BugTracker.Models;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using rcate_BugTracker.Models.CodeFirst;
using Microsoft.AspNet.Identity;

namespace rcate_BugTracker.Controllers
{
    public class HomeController : Universal
    {
        [Authorize]
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
            List<Ticket> tickets = new List<Ticket>();
            if (User.IsInRole("Admin") || User.IsInRole("DemoA") || User.IsInRole("ProjectManager") || User.IsInRole("DemoP"))
            {
                tickets = db.Ticket.ToList();
            }
            else if (User.IsInRole("Submitter") || User.IsInRole("DemoS"))
            {
                //tickets = user.Ticket.ToList();
                tickets = db.Ticket.Where(t => t.OwnerUserId == user.Id).ToList();
            }
            else if (User.IsInRole("Developer") || User.IsInRole("DemoD"))
            {
                //tickets = user.Ticket.ToList();
                tickets = db.Ticket.Where(t => t.AssignToUserId == user.Id).ToList();
            }
            return View(tickets);
        }

        [Authorize(Roles = ("Admin"))]
        public ActionResult AllProjects(string userId)
        {
            ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
            return View(db.Ticket.ToList());
        }

        [Authorize]
        public ActionResult About()
        {
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        [Authorize]
        public ActionResult Contact()
        {
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        public ActionResult Landing()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email From: <bold>{0}</bold> ({1})</p><p> Message:</p><p>{2}</p> ";
                    var from = "myBugTracker<noreply@gmail.com>";

                    var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = "Bug Tracker",
                        Body = string.Format(body, model.FromName, model.FromEmail,
                        model.Body),
                        IsBodyHtml = true
                    };
                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);
                    return View(new EmailModel());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using rcate_BugTracker.Models;
using rcate_BugTracker.Models.CodeFirst;
using Microsoft.AspNet.Identity;
using rcate_BugTracker.Models.Helpers;
using System.IO;
using System.Web.Services.Description;
using System.Net.Mail;
using System.Configuration;
using System.Threading.Tasks;

namespace rcate_BugTracker.Controllers
{
    public class TicketsController : Universal
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectAssignHelper helper = new ProjectAssignHelper();

        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
                var tickets = db.Ticket.Include(t => t.AssignToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
                if (User.IsInRole("ProjectManager") || User.IsInRole("DemoP"))
                {
                    return View(user.Project.SelectMany(t => t.Tickets).ToList());
                }
                else if (User.IsInRole("Developer") || User.IsInRole("DemoD"))
                {
                    return View(tickets.Where(t => t.AssignToUserId == user.Id).ToList());
                }

                else if (User.IsInRole("Submitter") || User.IsInRole("DemoS"))
                {
                    return View(tickets.Where(t => t.OwnerUserId == user.Id).ToList());
                }

                else if (User.IsInRole("Admin") || User.IsInRole("DemoA"))
                {
                    return View(tickets.Where(t => t.OwnerUserId == user.Id).ToList());
                }

                else if (User.IsInRole("Admin") || User.IsInRole("DemoA"))
                {
                    return View(tickets.Where(t => t.AssignToUserId == user.Id).ToList());
                }
            }
            return RedirectToAction("Index", "Projects");
        }

        // GET: Tickets
        [Authorize]
        public ActionResult AdminIndex()
        {
            if (User.IsInRole("Admin") || User.IsInRole("DemoA"))
            {
                var tickets = db.Ticket.Include(t => t.AssignToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
                ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
                return View(tickets);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Tickets
        [Authorize]
        public ActionResult ArchivedTickets()
        {
            if (Request.IsAuthenticated)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
                var tickets = db.Ticket.Include(t => t.AssignToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
                if (User.IsInRole("ProjectManager") || User.IsInRole("DemoP"))
                {
                    return View(user.Project.SelectMany(t => t.Tickets).ToList());
                }
                else if (User.IsInRole("Developer") || User.IsInRole("DemoD"))
                {
                    return View(tickets.Where(t => t.AssignToUserId == user.Id).ToList());
                }

                else if (User.IsInRole("Submitter") || User.IsInRole("DemoS"))
                {
                    return View(tickets.Where(t => t.OwnerUserId == user.Id).ToList());
                }

                else if (User.IsInRole("Admin") || User.IsInRole("DemoA"))
                {
                    return View(tickets.Where(t => t.OwnerUserId == user.Id).ToList());
                }

                else if (User.IsInRole("Admin") || User.IsInRole("DemoA"))
                {
                    return View(tickets.Where(t => t.AssignToUserId == user.Id).ToList());
                }
            }
            return RedirectToAction("Index", "Projects");
        }

        // GET: Tickets
        [Authorize(Roles = "Admin, DemoA")]
        public ActionResult ArchivedAdmin()
        {
            if (Request.IsAuthenticated)
            {
                var tickets = db.Ticket.Include(t => t.AssignToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
                ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
                return View(tickets);

            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Tickets/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ProjectAssignHelper helper = new ProjectAssignHelper();
            Project project = db.Project.Find(ticket.ProjectId);
            var user = db.Users.Find(User.Identity.GetUserId());
            ViewBag.UserTimeZone = user.TimeZone;

            if (User.IsInRole("Admin") || User.IsInRole("DemoA"))
            {
                return View(ticket);
            }
            else if (User.IsInRole("ProjectManager") || User.IsInRole("DemoP"))
            {
                if (helper.IsUserOnProject(user.Id, project.Id) == true)
                {

                    return View(ticket);
                }
            }
            else if (User.IsInRole("Developer") && ticket.AssignToUserId == user.Id || User.IsInRole("DemoD") && ticket.AssignToUserId == user.Id)
            {
                return View(ticket);
            }
            else if (User.IsInRole("Submitter") && ticket.OwnerUserId == user.Id || User.IsInRole("DemoS") && ticket.OwnerUserId == user.Id)
            {
                return View(ticket);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: Tickets/Create
        [Authorize]
        public ActionResult Create(int? Id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var project = db.Project.Find(Id);

            ViewBag.AssignToUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(helper.ListUserProjects(user.Id), "Id", "Title");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriority, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketType, "Id", "Name");

            if (User.IsInRole("Submitter") || User.IsInRole("DemoS"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignToUserId")] Ticket ticket, int? Id)
        {
            ViewBag.AssignToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(db.Project, "Id", "Title", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriority, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketType, "Id", "Name", ticket.TicketTypeId);

            if (ModelState.IsValid)
            {
                ticket.TicketStatusId = 6;
                ticket.Created = DateTimeOffset.UtcNow;
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.Archived = false;

                db.Ticket.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index", "Tickets");

            }

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            Ticket ticket = db.Ticket.Find(id);
            var user = db.Users.Find(User.Identity.GetUserId());
            var project = db.Project.Find(ticket.ProjectId);

            ViewBag.AssignToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Project, "Id", "Title", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriority, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketType, "Id", "Name", ticket.TicketTypeId);


            if (User.IsInRole("Admin") || User.IsInRole("DemoA") || helper.IsUserOnProject(user.Id, project.Id) == true && User.IsInRole("ProjectManager") || helper.IsUserOnProject(user.Id, project.Id) == true && User.IsInRole("DemoP") || User.Identity.GetUserId() == ticket.AssignToUserId || User.Identity.GetUserId() == ticket.OwnerUserId)
            {
                return View(ticket);
            }
            else if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else if (ticket == null)
            {
                return HttpNotFound();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignToUserId")] Ticket ticket)
        {
            ViewBag.AssignToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Project, "Id", "Title", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriority, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketType, "Id", "Name", ticket.TicketTypeId);

            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                ticket.Updated = DateTimeOffset.UtcNow;

                TicketHistoryHelper helper = new TicketHistoryHelper();
                Ticket oldTicket = db.Ticket.AsNoTracking().First(t => t.Id == ticket.Id);

                if (oldTicket.Title != ticket.Title)
                {
                    helper.TitleChange(ticket, user.Id);
                }
                if (oldTicket.Description != ticket.Description)
                {
                    helper.DescriptionChange(ticket, user.Id);
                }
                if (oldTicket.TicketStatusId != ticket.TicketStatusId)
                {
                    helper.StatusChange(ticket, user.Id);
                }
                if (oldTicket.TicketPriorityId != ticket.TicketPriorityId)
                {
                    helper.PriorityChange(ticket, user.Id);
                }
                if (oldTicket.TicketTypeId != ticket.TicketTypeId)
                {
                    helper.TypeChange(ticket, user.Id);
                }
                if (ticket.TicketStatusId == 10)
                {
                    ticket.Archived = true;
                }
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                if (User.Identity.GetUserId() != ticket.OwnerUserId)
                {
                    try
                    {
                        var body = "<h3>{1}</h3>";
                        var from = "BugTracker<NoReply@BugTracker.com>";
                        var email = new MailMessage(from, db.Users.Find(ticket.AssignToUserId).Email)
                        {
                            Subject = "Ticket Notification",
                            Body = string.Format(body, "subject", "This message was sent per request of *company name*.  A ticket that you have been assigned to has been altered.  Visit your " + "<a href='http://rcate-bugtracker.azurewebsites.net/Tickets/Index'>Bug Tracker</a>" + " for more details. "),
                            IsBodyHtml = true
                        };
                        var svc = new PersonalEmail();
                        await svc.SendAsync(email);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        await Task.FromResult(0);
                    }
                }


                if (User.Identity.GetUserId() != ticket.OwnerUserId)
                {
                    try
                    {
                        var body = "<h3>{1}</h3>";
                        var from = "BugTracker<NoReply@BugTracker.com>";
                        var email = new MailMessage(from, db.Users.Find(ticket.OwnerUserId).Email)
                        {
                            Subject = "Ticket Notification",
                            Body = string.Format(body, "subject", "This message was sent per request of *compnay name*.  A ticket you have created has been altered. Visit your " + "<a href='http://rcate-bugtracker.azurewebsites.net/Tickets/Index'>Bug Tracker</a>" + " for more details. "),
                            IsBodyHtml = true
                        };
                        var svc = new PersonalEmail();
                        await svc.SendAsync(email);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        await Task.FromResult(0);
                    }
                }


                return RedirectToAction("Details", "Tickets", new { id = ticket.Id });
            }

            return View(ticket);
        }

        ////////////////////////////////////////////////////////////////////////


        // POST: Ticket/Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "Id,Body,Created,Updated,TicketId,AuthorId")] TicketComment ticketcomment)
        {
            ViewBag.OwnerUserId = new SelectList(/*db.ApplicationUsers,*/ "Id", "FirstName", ticketcomment.AuthorId);
            ViewBag.CommentId = new SelectList(db.Ticket, "Id", "Title", ticketcomment.Id);

            if (ModelState.IsValid)
            {
                ticketcomment.Id = ticketcomment.TicketId;
                ticketcomment.AuthorId = User.Identity.GetUserId();
                ticketcomment.Created = DateTimeOffset.UtcNow;
                db.TicketComment.Add(ticketcomment);
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = ticketcomment.TicketId });
            }

            return View(ticketcomment);
        }

        // GET: Tickets/Comments/Edit/5
        [Authorize]
        public ActionResult EditComment(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            Ticket ticket = db.Ticket.Find(id);
            TicketComment ticketcomment = db.TicketComment.Find(id);


            if ((User.IsInRole("Admin") || User.IsInRole("DemoA") || (User.IsInRole("ProjectManager") && ticket.OwnerUserId == user.Id) || (User.IsInRole("DemoP") && ticket.OwnerUserId == user.Id) || ticketcomment.AuthorId == user.Id))

            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (ticketcomment == null)
                {
                    return HttpNotFound();
                }
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", ticketcomment.AuthorId);
            ViewBag.BlogPostId = new SelectList(db.TicketComment, "Id", "Title", ticketcomment.TicketId);
            return View(ticketcomment);

        }

        // POST: Ticket/Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditComment([Bind(Include = "Id,Body,Created,Updated,TicketId,AuthorId")] TicketComment ticketcomment)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            Ticket ticket = db.Ticket.Find(ticketcomment.TicketId);
            //TicketComment ticketcomment = db.TicketComments.Find(id);
            if ((User.IsInRole("Admin") || User.IsInRole("DemoA") || (User.IsInRole("ProjectManager") || User.IsInRole("DemoP") || ticket.OwnerUserId == user.Id) || ticketcomment.AuthorId == user.Id))
            {
                if (ModelState.IsValid)
                {
                    //ticketcomment.Id = ticketcomment.TicketId;
                    //ticketcomment.AuthorId = User.Identity.GetUserId();
                    ticketcomment.Updated = DateTimeOffset.UtcNow;
                    db.Entry(ticketcomment).State = EntityState.Modified;
                    db.SaveChanges();



                    //ViewBag.AuthorId = new SelectList("Id", "FirstName", ticketcomment.AuthorId);
                    //ViewBag.CommentId = new SelectList(db.TicketComments, "Id", "Title", ticketcomment.Id);
                    return RedirectToAction("Details", "Tickets", new { id = ticketcomment.TicketId });

                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            return View(ticketcomment);
        }

        // GET: Ticket/Comments/Delete/5
        public ActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment ticketcomment = db.TicketComment.Find(id);
            if (ticketcomment == null)
            {
                return HttpNotFound();
            }
            return View(ticketcomment);
        }

        // POST: Ticket/Comments/Delete/5
        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCommentConfirmed(int id)
        {
            TicketComment ticketcomment = db.TicketComment.Find(id);
            db.TicketComment.Remove(ticketcomment);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = ticketcomment.TicketId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        ////////////////////////////////////////////////////////////////////////


        //POST: Tickets/CreateAttachment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAttachment(
            [Bind(Include = "Id,Description,TicketId")] TicketAttatchment ticketattachment, HttpPostedFileBase attachFile) //Bind Attribute tells it to add these properties when it sends to view
        {
            ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
            var user = db.Users.Find(User.Identity.GetUserId());
            if (attachFile != null)
            {

                var ext = Path.GetExtension(attachFile.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".pdf" && ext != ".bmp" && ext != ".gif")
                    ModelState.AddModelError("image", "Invalid Format.");
            }
            if (ModelState.IsValid) //makes sure all the properties are bound
            {
                if (attachFile != null)

                {
                    ticketattachment.Created = DateTimeOffset.Now;
                    ticketattachment.AuthorId = user.Id;
                    var filePath = "/Assets/UserAttatchments/";
                    var absPath = Server.MapPath("~" + filePath);
                    ticketattachment.FileUrl = filePath + attachFile.FileName;
                    attachFile.SaveAs(Path.Combine(absPath, attachFile.FileName));

                }
                db.TicketAttatchment.Add(ticketattachment);
                db.SaveChanges();
            }
            return RedirectToAction("Details", "Tickets", new { id = ticketattachment.TicketId });

        }


        // POST: Tickets/DeleteAttachment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAttachment(int id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            TicketAttatchment ticketattachment = db.TicketAttatchment.Find(id);
            Ticket ticket = db.Ticket.Find(ticketattachment.TicketId);

            if ((User.IsInRole("Admin") || User.IsInRole("DemoA") || (User.IsInRole("ProjectManager") && ticket.OwnerUserId == user.Id) || (User.IsInRole("DemoP") && ticket.OwnerUserId == user.Id) || ticketattachment.AuthorId == user.Id))
            {
                db.TicketAttatchment.Remove(ticketattachment);
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = ticket.Id });
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        ////////////////////////////////////////////////////////////////////////


        // GET: Tickets/AssignDeveloper/
        [Authorize]
        public ActionResult AssignDeveloper(int? id)
        {
            Ticket ticket = db.Ticket.Find(id);
            UserRoleHelper helper = new UserRoleHelper();

            var developers = helper.UsersInRole("Developer");
            var demoDeveloper = helper.UsersInRole("DemoD");
            var onProject = developers.Where(d => d.Project.Any(p => p.Id == ticket.ProjectId));
            var DemosonProject = demoDeveloper.Where(d => d.Project.Any(p => p.Id == ticket.ProjectId));
            if (User.IsInRole("Admin") || User.IsInRole("ProjectManager"))
            {
                ViewBag.AssignToUserId = new SelectList(onProject, "Id", "FullName", ticket.AssignToUserId);
            }
            if (User.IsInRole("DemoA") || User.IsInRole("DemoP"))
            {
                ViewBag.AssignToUserId = new SelectList(DemosonProject, "Id", "FullName", ticket.AssignToUserId);
            }
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ticket == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin") || User.IsInRole("DemoA") || User.IsInRole("ProjectManager") || User.IsInRole("DemoP"))
            {
                return View(ticket);
            }
            return RedirectToAction("Login", "Account");
        }
        //POST: Tickets/AssignDeveloper/
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignDeveloper(string AssignToUserId, int id, EmailModel model)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            Ticket ticket = db.Ticket.Find(id);
            ticket.AssignToUserId = AssignToUserId;
            ticket.TicketStatusId = db.TicketStatus.FirstOrDefault(t => t.Id == 7).Id;
            TicketHistoryHelper helper = new TicketHistoryHelper();
            Ticket oldTicket = db.Ticket.AsNoTracking().First(t => t.Id == ticket.Id);
            if (oldTicket.AssignToUserId != ticket.AssignToUserId)
            {
                helper.AssignChange(ticket, user.Id);
            }
            db.SaveChanges();

            try
            {
                var body = "<h3>{1}</h3>";
                var from = "BugTracker<NoReply@BugTracker.com>";
                var email = new MailMessage(from, db.Users.Find(ticket.OwnerUserId).Email)
                {
                    Subject = "Ticket Notification",
                    Body = string.Format(body, "subject", "This message was sent per request of *compnay name*.  A ticket you have been created has been asigned to a Developer. Visit your " + "<a href='http://rcate-bugtracker.azurewebsites.net/Tickets/Index'>Bug Tracker</a>" + " for more details. "),
                    IsBodyHtml = true
                };
                var svc = new PersonalEmail();
                await svc.SendAsync(email);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }


            try
            {
                var body = "<h3>{1}</h3>";
                var from = "BugTracker<NoReply@BugTracker.com>";
                var email = new MailMessage(from, db.Users.Find(ticket.AssignToUserId).Email)
                {
                    Subject = "Ticket Notification",
                    Body = string.Format(body, "Subject", "This message has been sent per request of *company name*. You have been selected as the Developer for a ticket. Visit your " + "<a href='http://rcate-bugtracker.azurewebsites.net/Tickets/Index'>Bug Tracker</a>" + " for more details. "),
                    IsBodyHtml = true
                };
                var svc = new PersonalEmail();
                await svc.SendAsync(email);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }

            return RedirectToAction("Details", "Tickets", new { id = ticket.Id });

        }
    }
}

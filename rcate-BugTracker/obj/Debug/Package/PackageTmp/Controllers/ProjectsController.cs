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
using rcate_BugTracker.Models.Helpers;
using Microsoft.AspNet.Identity;

namespace rcate_BugTracker.Controllers
{
    public class ProjectsController : Universal
    {
        private ProjectAssignHelper helper = new ProjectAssignHelper();

        // GET: Projects
        [Authorize]
        public ActionResult Index()
        {
            Project project = new Project();

            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var userProjects = helper.ListUserProjects(userId);
                ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
                return View(userProjects);
            }
            else
            {
                return View();
            }
        }

        // GET: ProjectAdmin
        [Authorize(Roles = "Admin, DemoA")]
        public ActionResult AdminIndex()
        {
            if (User.IsInRole("Admin"))
            {
                ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
                return View(db.Project.ToList());
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        // GET: Projects
        [Authorize]
        public ActionResult ArchiveIndex()
        {
            Project project = new Project();

            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var userProjects = helper.ListUserProjects(userId);
                ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
                return View(userProjects);
            }
            else
            {
                return View();
            }
        }

        // GET: ProjectAdmin
        [Authorize(Roles = "Admin, DemoA")]
        public ActionResult ArchiveAdmin()
        {
            if (User.IsInRole("Admin"))
            {
                ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
                return View(db.Project.ToList());
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        // GET: Projects/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            Project project = db.Project.Find(id);
            var user = db.Users.Find(User.Identity.GetUserId()); //you have already found the current logged in user
            ViewBag.UserTimeZone = user.TimeZone; // we are using the same user object that you found. we don't have to go grab it again
            //we are making sure that we put the data in the viewbag before we render the view with return view statement. before you returned the view then you had viewbag. that's why it was always null
            //any other issues?

            if (User.IsInRole("Admin") || User.IsInRole("DemoA") || helper.IsUserOnProject(user.Id, project.Id) == true && User.IsInRole("ProjectManager") || helper.IsUserOnProject(user.Id, project.Id) == true && User.IsInRole("DemoP") || helper.IsUserOnProject(user.Id, project.Id) == true)
            {
                return View(project);
            }
            else if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else if (project == null)
            {
                return HttpNotFound();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin, ProjectManager, DemoA, DemoP")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, ProjectManager, DemoA, DemoP")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Created,Updated,Title,Description,AuthorId")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Project.Add(project);
                project.Archived = false;
                project.Created = DateTimeOffset.UtcNow;
                project.AuthorId = User.Identity.GetUserId();
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, ProjectManager, DemoA, DemoP")]
        public ActionResult Edit(int? id)
        {
            Project project = db.Project.Find(id);
            var user = db.Users.Find(User.Identity.GetUserId());

            if (Request.IsAuthenticated)
            {
                return View(project);
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, ProjectManager, DemoA, DemoP")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Created,Updated,Title,Description,AuthorId")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                project.Updated = DateTimeOffset.UtcNow;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        //// GET: PROJECT/USERS
        [Authorize(Roles = "Admin, ProjectManager, DemoA, DemoP")]
        public ActionResult ProjectUser(int? id)
        {
            ProjectAssignHelper helper = new ProjectAssignHelper();
            Project project = db.Project.Find(id);
            var user = db.Users.Find(User.Identity.GetUserId());

            ProjectUserViewModel projectuserVM = new ProjectUserViewModel();
            projectuserVM.AssignProjectId = (int)id;
            projectuserVM.AssignProject = project;
            projectuserVM.SelectedUsers = project.Users.Select(u => u.Id).ToArray();
            projectuserVM.Users = new MultiSelectList(db.Users.ToList(), "Id", "FullName", projectuserVM.SelectedUsers);

            if (User.IsInRole("Admin"))
            {
                return View(projectuserVM); //you are returning project object if the user is admin. the view requires projectuserviewmodel object
            }
            else if (helper.IsUserOnProject(user.Id, project.Id) == true && User.IsInRole("ProjectManager") || helper.IsUserOnProject(user.Id, project.Id) == true && User.IsInRole("DemoP"))
            {
                return View(projectuserVM); //same here if the user is a project manager
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (project == null)
            {
                return HttpNotFound();
            }

            return View(projectuserVM); //this projectuserviewmodel instance is what you need to return
        }

        // POST: PROJECT/USERS
        [HttpPost]
        [Authorize(Roles = "Admin, ProjectManager, DemoA, DemoP")]
        public ActionResult ProjectUser(ProjectUserViewModel model)
        {
            ProjectAssignHelper helper = new ProjectAssignHelper();
            foreach (var userId in db.Users.Select(u => u.Id).ToList())
            {
                helper.RemoveUserFromProject(userId, model.AssignProjectId);
            }

            foreach (var userId in model.SelectedUsers)
            {
                helper.AddUserToProject(userId, model.AssignProjectId);
            }
            
            return RedirectToAction("Index");
        }


        // POST: PROJECT/ARCHIVE
        [HttpPost]
        [Authorize(Roles = "Admin, ProjectManager, DemoA, DemoP")]
        public ActionResult Archive(int? Id)
        {
            Project project = db.Project.Find(Id);
            project.Archived = true;
            db.SaveChanges();
            return RedirectToAction("ArchiveIndex");
        }

        // POST: PROJECT/ARCHIVE/UNDO
        [HttpPost]
        [Authorize(Roles = "Admin, ProjectManager, DemoA, DemoP")]
        public ActionResult UndoArchive(int? Id)
        {
            Project project = db.Project.Find(Id);
            project.Archived = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

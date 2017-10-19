using Microsoft.AspNet.Identity;
using rcate_BugTracker.Models;
using rcate_BugTracker.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace rcate_BugTracker.Controllers
{
    [Authorize(Roles = "Admin, DemoA")]
    public class AdminController : Universal
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private UserRoleHelper helper = new UserRoleHelper();
        // GET: Admin
        public ActionResult Index()
        {
            List<AdminUserViewModels> users = new List<AdminUserViewModels>();
            UserRoleHelper helper = new UserRoleHelper();
            foreach (var user in db.Users.ToList())
            {
                var eachUser = new AdminUserViewModels();
                eachUser.User = user;
                eachUser.SelectedRoles = helper.ListUserRoles(user.Id).ToArray();

                users.Add(eachUser);
            }
            return View(users.OrderBy(u => u.User.LastName).ToList());
        }

        // GET: Admin/EditRoles
        public ActionResult EditUserRoles(string id)
        {
            var user = db.Users.Find(id);
            var helper = new UserRoleHelper();
            var model = new AdminUserViewModels();
            var demoRoles = db.Roles.Where(r => r.Name.StartsWith("Demo")).ToList();
            model.User = user;
            model.SelectedRoles = helper.ListUserRoles(id).ToArray();
            if (User.IsInRole("DemoA"))
            {
                model.Roles = new MultiSelectList(demoRoles, "Name", "Name", model.SelectedRoles);
            }
            if (User.IsInRole("Admin"))
            {
                model.Roles = new MultiSelectList(db.Roles, "Name", "Name", model.SelectedRoles);
            }


            return View(model);
        }

        // POST: Admin/EditRoles
        [HttpPost]
        [Authorize(Roles = ("Admin"))]
        public ActionResult EditUserRoles(AdminUserViewModels model)
        {
            var user = db.Users.Find(model.User.Id);
            UserRoleHelper helper = new UserRoleHelper();
            foreach (var role in db.Roles.Select(r => r.Name).ToList())
            {
                helper.RemoveUserFromRole(user.Id, role);
            }
            if (model.SelectedRoles != null)
            {
                foreach (var role in model.SelectedRoles)
                {
                    helper.AddUserToRole(user.Id, role);
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using rcate_BugTracker.Models.CodeFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rcate_BugTracker.Models.Helpers
{
    public class ProjectAssignHelper
    {
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private ApplicationDbContext db = new ApplicationDbContext();

        // CHECK TO SEE IF USER IS IN PROJECT
        public bool IsUserOnProject (string userId, int projectId)
        {
            var project = db.Project.Find(projectId);
            var userBool = project.Users.Any(u => u.Id == userId);

            return userBool;
        }

        // ADD USER TO PROJECT
        public void AddUserToProject (string userId, int projectId)
        {
            var user = db.Users.Find(userId);
            var project = db.Project.Find(projectId);
            project.Users.Add(user);
            db.SaveChanges();
        }
        
        // REMOVE USER FROM PROJECT
        public void RemoveUserFromProject (string userId, int projectId)
        {
            var user = db.Users.Find(userId);
            var project = db.Project.Find(projectId);
            project.Users.Remove(user);
            db.SaveChanges();
        }

        // LIST USERS PROJECTS
        public List<Project> ListUserProjects(string userId)
        {
            var user = db.Users.Find(userId);
            return user.Project.ToList();
        }


        // LIST USERS ON PROJECT
        public List<ApplicationUser>UsersOnProject(int projectId)
        {
            var project = db.Project.Find(projectId);
            return project.Users.ToList();
        }

        // LIST USERS NOT ON PROJECT
        public List<ApplicationUser>UserNotOnProject(int projectId)
        {
            return db.Users.Where(u => u.Project.All(p => p.Id != projectId)).ToList();
        }

    }
}
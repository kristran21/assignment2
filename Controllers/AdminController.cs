using ASM.web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ASM.web.Controllers
{
    [MyCustomAuthorize(Roles = "Administrator")]
    public class AdminController : BaseController
    {
        // GET: Role
        public AdminController()
        {

        }

        public ActionResult Trainer()
        {
            List<ApplicationUser> Trainers = new List<ApplicationUser>();
            var role = Db.Roles
                .Where(r => r.Name == "Trainer")
                .FirstOrDefault();
            if (role != null)
            {
                var users = Db.Users
                    .Where(u => u.Roles.Select(r => r.RoleId).Contains(role.Id))
                    .ToList();
                return View(users);
            }
            return View();
        }

        public ActionResult TrainingStaff()
        {
            List<ApplicationUser> Trainers = new List<ApplicationUser>();
            var role = Db.Roles
                .Where(r => r.Name == "TrainingStaff")
                .FirstOrDefault();
            if (role != null)
            {
                var users = Db.Users
                    .Where(u => u.Roles.Select(r => r.RoleId).Contains(role.Id))
                    .ToList();
                return View(users);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var accountToEdit = this.Db.Users
                .Where(u => u.Id == id)
                .FirstOrDefault();
            if (accountToEdit == null)
            {
                var PreviousUrl = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
                return this.Redirect(PreviousUrl);
            }
            return this.View(accountToEdit);
        }

        [HttpPost]
        public ActionResult Edit(string id, ApplicationUser user)
        {
            var accountToEdit = this.Db.Users
                .Where(u => u.Id == id)
                .FirstOrDefault();
            if (accountToEdit == null)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (user != null && this.ModelState.IsValid)
            {
                accountToEdit.FullName = user.FullName;
                accountToEdit.Age = user.Age;
                accountToEdit.DateofBirth = user.DateofBirth;
                accountToEdit.Education = user.Education;
                accountToEdit.ToeicScore = user.ToeicScore;
                accountToEdit.Location = user.Location;

                this.Db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return this.View(user);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            var url = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            var user = Db.Users.Find(id);
            if (user == null)
            {
                return Redirect(url);
            }
            Db.Users.Remove(user);
            var assign = Db.CoursesAssigned
                .Where(a => a.UserId == id)
                .ToList();
            if (assign != null)
            {
                foreach (var item in assign)
                {
                    Db.CoursesAssigned.Remove(item);
                }
            }
            Db.SaveChanges();
            return Redirect(url);
        }

        public ActionResult Role()
        {
            var Roles = Db.Roles.ToList();
            return View(Roles);
        }

        public ActionResult RoleCreate()
        {
            var Role = new IdentityRole();
            return View(Role);
        }

        [HttpPost]
        public ActionResult RoleCreate(IdentityRole Role)
        {
            Db.Roles.Add(Role);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
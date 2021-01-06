using Laborator8App.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laborator8App.Controllers
{
    public class MembersController : Controller
    {

        public ApplicationDbContext db = ApplicationDbContext.Create();
        // GET: Users
        /*
        public ActionResult Index(int id)
        {
            var users = (from user in db.Users
                                           .Include(u => u.SentRequests)
                                           .Include(u => u.ReceivedRequests)
                         select user).ToList();
            ViewBag.Users = users;
            ViewBag.IdEchip = id;

            return View();
        }

            */
        [HttpPost]
        public ActionResult AddMember(FormCollection formData)
        {
            string friendToAdd = formData.Get("Username"); // TODO: trebuie validare (verificare daca userul exista)
            int tid = Int32.Parse(formData.Get("TeamId"));


            Member friendship = new Member();
            string sUserId = (from u in db.Users
                              where u.UserName == friendToAdd
                              select u.Id).First();
            friendship.UserId = sUserId;
            friendship.TeamId = tid;

            friendship.Accepted = true; // Accepted = false, iar in lista de cereri -> accept
            //friendship.RequestTime = DateTime.Now;

            // TODO: sa existe try si catch astfel incat sa nu se trimita o cerere de doua ori
            // verificare daca userul a primit deja cerere de la userul caruia doreste sa ii trimita
            // de verificat ca user1 sa nu fie deja prieten cu user2
            if (db.Members.Any(anyObjectName => anyObjectName.UserId == friendship.UserId
                                   && anyObjectName.TeamId == friendship.TeamId))
            {

                TempData["message"] = "Membrul exista deja!";

                return RedirectToAction("../Teams/Show/" + tid);
            }
            else
            {
                db.Members.Add(friendship);
                db.SaveChanges();
                string id = sUserId;
                ApplicationUser user = db.Users.Find(sUserId);
                user.AllRoles = GetAllRoles();
                var userRole = user.Roles.FirstOrDefault();
                ViewBag.userRole = userRole.RoleId;

                ApplicationDbContext context = new ApplicationDbContext();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));





                UserManager.AddToRole(id, "Member");

                db.SaveChanges();
                TempData["message"] = "Membrul a fost adaugat!";

                return RedirectToAction("../Teams/Show/" + tid);
            }

        }
        [NonAction]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var selectList = new List<SelectListItem>();

            var roles = from role in db.Roles select role;
            foreach (var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name.ToString()
                });
            }
            return selectList;
        }

    }
}

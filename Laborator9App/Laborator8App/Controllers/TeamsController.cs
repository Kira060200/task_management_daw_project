using Laborator8App.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
namespace Laborator8App.Controllers
{
    public class TeamsController : Controller
    {
        // GET: Teams
        private ApplicationDbContext db = new ApplicationDbContext();
        private string id;
        private string idT;
        // TO:DO Modificam si aici authorize-ul ca sa vada fiecare doar echipele din care fac parte
        [Authorize(Roles = "User,Member,Leader,Admin")]
        public ActionResult Index()
        {
            ViewBag.Uid = User.Identity.GetUserId();
            idT = User.Identity.GetUserId();
            var teams = from team in db.Teams
                        where team.UserId == idT
                        select team;
            ViewBag.Teams = teams;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            var teamswmember = from member in db.Members
                               where member.UserId == idT
                               select member;
            var finalf = from e in db.Members
                         join d in db.Teams on e.TeamId equals d.TeamId into table1
                         from c in table1.ToList()
                         where e.UserId == idT
                         select c;

            ViewBag.TeamsMem = teamswmember;

            ViewBag.TT = finalf;

            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ShowAll()
        {
            ViewBag.Uid = User.Identity.GetUserId();
            idT = User.Identity.GetUserId();
            var teams = from team in db.Teams
                        select team;
            ViewBag.Teams = teams;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }
        [Authorize(Roles = "User,Member,Leader,Admin")]
        public ActionResult Show(int id)
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            Team team = db.Teams.Find(id);
            ViewBag.Team = team;
            var projects = from project in db.Projects.Include("Team")
                           where project.TeamId == id
                           select project;
            ViewBag.Projects = projects;
            ViewBag.UserIdCur = User.Identity.GetUserId();
            return View();
        }
        /*=====================*/




        /*=====================*/
        [Authorize(Roles = "User,Leader,Admin")]
        public ActionResult New()
        {

            /*=================*/






            Team team = new Team();
            team.UserId = User.Identity.GetUserId();
            /*====================*/


            return View(team);
        }

        [HttpPost]
        [Authorize(Roles = "User,Leader,Admin")]
        public ActionResult New(Team cat)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    cat.UserId = User.Identity.GetUserId();
                    db.Teams.Add(cat);
                    db.SaveChanges();
                    TempData["message"] = "Echipa a fost adaugata!";
                    /*==========USER->LEADER ==============*/
                    id = User.Identity.GetUserId();
                    ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                    user.AllRoles = GetAllRoles();
                    var userRole = user.Roles.FirstOrDefault();
                    ViewBag.userRole = userRole.RoleId;

                    ApplicationDbContext context = new ApplicationDbContext();
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));





                    UserManager.AddToRole(id, "Leader");

                    db.SaveChanges();

                    return RedirectToAction("Index");


                }
                else
                {
                    return View(cat);
                }
            }
            catch (Exception e)
            {
                return View(cat);
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
        [Authorize(Roles = "Leader,Admin")]
        public ActionResult Edit(int id)
        {

            Team team = db.Teams.Find(id);
            //team.UserId = User.Identity.GetUserId();
            ViewBag.Team = team;
            if (team.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(team);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari!";
                return RedirectToAction("Index");
            }

        }


        [HttpPut]
        [Authorize(Roles = "Leader,Admin")]
        public ActionResult Edit(int id, Team requestTeam)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Team team = db.Teams.Find(id);
                    if (team.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        if (TryUpdateModel(team))
                        {
                            team.TeamName = requestTeam.TeamName;
                            db.SaveChanges();
                            TempData["message"] = "Echipa a fost modificata!";

                        }
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa faceti modificari!";
                        return RedirectToAction("Index");
                    }


                }
                else
                {
                    return View(requestTeam);
                }
            }
            catch (Exception e)
            {
                return View(requestTeam);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Leader,Admin")]
        public ActionResult Delete(int id)
        {
            Team team = db.Teams.Find(id);
            if (team.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Teams.Remove(team);
                db.SaveChanges();
                TempData["message"] = "Echipa a fost stearsa";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti ";
                return RedirectToAction("Index");

            }

        }
        /*
        [Authorize(Roles = "User,Leader,Admin")]
        public ActionResult AddMember(int id)
        {

            /*=================




            /*====================


            ViewBag.TeamID = id;
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Leader,Admin")]
        public ActionResult AddMember(FormCollection formData)
        {
            string friendToAdd = formData.Get("UserId"); // TODO: trebuie validare (verificare daca userul exista)
            int teamid = Int32.Parse( formData.Get("TeamId"));
            Member membership = new Member();
            membership.UserId = friendToAdd;
            membership.TeamId = teamid;
            //friendship.Accepted = true; // Accepted = false, iar in lista de cereri -> accept

            // TODO: sa existe try si catch astfel incat sa nu se trimita o cerere de doua ori
            // verificare daca userul a primit deja cerere de la userul caruia doreste sa ii trimita
            // de verificat ca user1 sa nu fie deja prieten cu user2

            //db.Members.Add(membership);
            // db.SaveChanges();

            return RedirectToAction("Index");
        }*/
    }
}
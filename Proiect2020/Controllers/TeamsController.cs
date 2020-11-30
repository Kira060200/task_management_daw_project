using Proiect2020.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect2020.Controllers
{
    public class TeamsController : Controller
    {
        // GET: Teams
        private ApplicationDbContext db = new ApplicationDbContext();
        // TO:DO Modificam si aici authorize-ul ca sa vada fiecare doar echipele din care fac parte
        public ActionResult Index()
        {
            var teams = from team in db.Teams
                        select team;
            ViewBag.Teams = teams;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }

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
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            Team team = new Team();
            return View(team);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult New(Team cat)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    db.Teams.Add(cat);
                    db.SaveChanges();
                    TempData["message"] = "Echipa a fost adaugata!";
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {

            Team team = db.Teams.Find(id);
            ViewBag.Team = team;
            return View(team);
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Team requestTeam)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Team team = db.Teams.Find(id);
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
                    return View(requestTeam);
                }
            }
            catch (Exception e)
            {
                return View(requestTeam);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Team team = db.Teams.Find(id);
            db.Teams.Remove(team);
            db.SaveChanges();
            TempData["message"] = "Echipa a fost stearsa";
            return RedirectToAction("Index");
        }
    }
}

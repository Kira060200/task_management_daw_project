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
        private Models.AppContext db = new Models.AppContext();

        public ActionResult Index()
        {
            var teams = from team in db.Teams
                        select team;
            ViewBag.Teams = teams;


            return View();
        }

        public ActionResult Show(int id)
        {
            Team team = db.Teams.Find(id);
            ViewBag.Team = team;
            var tasks = from task in db.Tasks.Include("Team")
                        where task.TeamId == id
                        select task;
            ViewBag.Tasks = tasks;
            return View();
        }
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(Team cat)
        {
            try
            {
                db.Teams.Add(cat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }
        public ActionResult Edit(int id)
        {

            Team team = db.Teams.Find(id);
            ViewBag.Team = team;
            return View();
        }


        [HttpPut]
        public ActionResult Edit(int id, Team requestTeam)
        {
            try
            {
                Team team = db.Teams.Find(id);
                if (TryUpdateModel(team))
                {
                    team.TeamName = requestTeam.TeamName;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Team team = db.Teams.Find(id);
            db.Teams.Remove(team);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
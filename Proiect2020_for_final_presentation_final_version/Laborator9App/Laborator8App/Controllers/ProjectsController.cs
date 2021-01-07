using Laborator8App.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laborator8App.Controllers
{
    public class ProjectsController : Controller
    {
        // GET: Teams
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "Member,Leader,Admin")]

        public ActionResult Index()
        {
            /*var teams = from team in db.Teams
                        select team;
            ViewBag.Teams = teams;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }*/
            return View();
        }
        [Authorize(Roles = "Member,User,Leader,Admin")]
        public ActionResult Show(int id)
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            Project project = db.Projects.Find(id);
            ViewBag.Project = project;
            var tasks = from task in db.Tasks.Include("Project")
                        where task.ProjectId == id
                        select task;
            ViewBag.Tasks = tasks;
            ViewBag.UserNameAsigned = User.Identity.GetUserName();
            ViewBag.UserIdCur = User.Identity.GetUserId();
            /*try
            {
                ViewBag.UserNameAsigned = (from us in db.Users
                                           where us.Id == User.Identity.GetUserId()
                                           select us.UserName).First();
            }catch(Exception e) { }*/
            return View();
        }
        [Authorize(Roles = "Leader,Admin")]
        public ActionResult New(int id)
        {
            Project project = new Project();
            project.TeamId = id;
            Team team = db.Teams.Find(id);

            ViewBag.IdEchip = id;
            project.UserId = User.Identity.GetUserId();
            string idu = User.Identity.GetUserId();
            return idu == team.UserId || User.IsInRole("Admin") ? (ActionResult)View(project) : Redirect("/Teams/Show/" + id);
            //return View(project);
        }

        [HttpPost]
        [Authorize(Roles = "Leader,Admin")]
        public ActionResult New(Project project)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    project.UserId = User.Identity.GetUserId();
                    db.Projects.Add(project);
                    db.SaveChanges();
                    TempData["message"] = "Proiectul a fost adaugat!";
                    return Redirect("/Teams/Show/" + project.TeamId);
                }
                else
                {
                    ViewBag.IdEchip = project.TeamId;
                    return View(project);
                }
            }
            catch (Exception e)
            {
                ViewBag.IdEchip = project.TeamId;
                return View(project);
            }
        }



        [Authorize(Roles = "Leader,Admin")]
        public ActionResult Edit(int id)
        {

            Project project = db.Projects.Find(id);
            if (project.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                ViewBag.Project = project;
                return View(project);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti ";
                return Redirect("/Teams/Show/" + project.TeamId);


            }
        }


        [HttpPut]
        [Authorize(Roles = "Leader,Admin")]
        public ActionResult Edit(int id, Project requestProject)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Project project = db.Projects.Find(id);
                    if (TryUpdateModel(project))
                    {
                        project.ProjectName = requestProject.ProjectName;
                        db.SaveChanges();
                        TempData["message"] = "Proiectul a fost modificat!";
                    }

                    return Redirect("/Teams/Show/" + project.TeamId);
                }
                else
                {
                    return View(requestProject);
                }
            }
            catch (Exception e)
            {
                return View(requestProject);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Leader,Admin")]
        public ActionResult Delete(int id)
        {
            Project project = db.Projects.Find(id);
            if (project.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Projects.Remove(project);
                db.SaveChanges();
                TempData["message"] = "Proiectul a fost sters";
                return Redirect("/Teams/Show/" + project.TeamId);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti ";
                return Redirect("/Teams/Show/" + project.TeamId);


            }

        }
    }
}
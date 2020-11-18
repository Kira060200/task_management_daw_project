using Proiect2020.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect2020.Controllers
{
    public class ProjectsController : Controller
    {
        // GET: Teams
        private Models.AppContext db = new Models.AppContext();

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
            return View();
        }
        public ActionResult New(int id)
        {
            Project project = new Project();
            project.TeamId = id;
            ViewBag.IdEchip = id;

            return View(project);
        }

        [HttpPost]
        public ActionResult New(Project project)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    db.Projects.Add(project);
                    db.SaveChanges();
                    TempData["message"] = "Proiectul a fost adaugata!";
                    return Redirect("/Teams/Show/" + project.TeamId);
                }
                else
                {
                    return View(project);
                }
            }
            catch (Exception e)
            {
                return View(project);
            }
        }
        public ActionResult Edit(int id)
        {

            Project project = db.Projects.Find(id);
            ViewBag.Project = project;
            return View(project);
        }


        [HttpPut]
        public ActionResult Edit(int id, Project requestProject)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Project project= db.Projects.Find(id);
                    if (TryUpdateModel(project))
                    {
                        project.ProjectName = requestProject.ProjectName;
                        db.SaveChanges();
                        TempData["message"] = "Proiectul a fost modificata!";
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
        public ActionResult Delete(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            TempData["message"] = "Proiectul a fost stears";
            return Redirect("/Teams/Show/" + project.TeamId);
        }
    }
}
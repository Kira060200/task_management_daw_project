using Microsoft.AspNet.Identity;
using Proiect2020.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect2020.Controllers
{
    public class TasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Tasks
        [Authorize(Roles = "Membru,Organizator,Admin")]
        public ActionResult Index()
        {
            /*var tasks = from task in db.Tasks.Include("Team")
                        where task.TeamId==2
                        select task;
            ViewBag.Tasks = tasks;
            */
            var tasks = db.Tasks.Include("User");
            ViewBag.Tasks = tasks;
            return View();
        }
        [Authorize(Roles = "Membru,Organizator,Admin")]
        public ActionResult Show(int id)
        {
            Task task = db.Tasks.Find(id);
            ViewBag.Task = task;
            var comments = from comment in db.Comments.Include("Task")
                           where comment.TaskId == task.TaskId
                           select comment;
            ViewBag.Comments = comments;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            SetAccessRights();
            return View();
        }
        [Authorize(Roles = "Organizator,Admin")]
        public ActionResult New(int id)
        {

            ViewBag.IdEchip = id;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Organizator,Admin")]
        public ActionResult New(Task task)
        {
            try
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                TempData["message"] = "Task-ul a fost adaugat!";
                return Redirect("/Projects/Show/" + task.ProjectId);
            }
            catch (Exception e)
            {
                return View();
            }
        }
        [Authorize(Roles = "Organizator,Admin")]
        public ActionResult Delete(int id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
            db.SaveChanges();
            TempData["message"] = "Task-ul a fost sters";
            return Redirect("/Projects/Show/" + task.ProjectId);
        }
        [Authorize(Roles = "Organizator,Admin")]
        public ActionResult Edit(int id)
        {
            Task task = db.Tasks.Find(id);
            ViewBag.Task = task;

            return View();
        }

        [HttpPut]
        [Authorize(Roles = "Organizator,Admin")]
        public ActionResult Edit(int id, Task requestTask)
        {
            try
            {
                Task task = db.Tasks.Find(id);
                if (TryUpdateModel(task))
                {
                    task.Title = requestTask.Title;
                    task.Status = requestTask.Status;
                    task.Content = requestTask.Content;
                    task.StartDate = requestTask.StartDate;
                    task.EndDate = requestTask.EndDate;
                    task.ProjectId = requestTask.ProjectId;
                    db.SaveChanges();
                    TempData["message"] = "Task-ul a fost modificat!";
                }
                return Redirect("/Projects/Show/" + task.ProjectId);
            }
            catch (Exception e)
            {
                return View();
            }

        }
        private void SetAccessRights()
        {
            ViewBag.afisareButoane = false;

            if (User.IsInRole("Organizator") || User.IsInRole("Admin"))
            {
                ViewBag.afisareButoane = true;
            }

            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
        }
    }
}
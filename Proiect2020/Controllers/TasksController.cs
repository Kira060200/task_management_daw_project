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
        private Models.AppContext db = new Models.AppContext();
        // GET: Tasks
        public ActionResult Index()
        {
            /*var tasks = from task in db.Tasks.Include("Team")
                        where task.TeamId==2
                        select task;
            ViewBag.Tasks = tasks;
            */
            return View();
        }
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
            return View();
        }
        public ActionResult New(int id)
        {
            
            ViewBag.IdEchip = id;
            return View();
        }

        [HttpPost]
        public ActionResult New(Task task)
        {
            try
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                TempData["message"] = "Task-ul a fost adaugat!";
                return Redirect("/Teams/Show/" + task.TeamId);
            }
            catch (Exception e)
            {
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
            db.SaveChanges();
            TempData["message"] = "Task-ul a fost sters";
            return Redirect("/Teams/Show/" + task.TeamId);
        }
        public ActionResult Edit(int id)
        {
            Task task = db.Tasks.Find(id);
            ViewBag.Task = task;
            //ViewBag.Title = "Sukoon";

            return View();
        }

        [HttpPut]
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
                    task.TeamId = requestTask.TeamId;
                    db.SaveChanges();
                    TempData["message"] = "Task-ul a fost modificat!";
                }
                return Redirect("/Teams/Show/" + task.TeamId);
            }
            catch (Exception e)
            {
                return View();
            }

        }
    }
}
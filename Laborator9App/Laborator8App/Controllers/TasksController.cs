using Microsoft.AspNet.Identity;
using Laborator8App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laborator8App.Controllers
{
    public class TasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Tasks
        [Authorize(Roles = "Member,Leader,Admin")]
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
        [Authorize(Roles = "Member,Leader,Admin")]
        public ActionResult Show(int id)
        {
            TaskClass task = db.Tasks.Find(id);
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
        [Authorize(Roles = "Leader,Admin")]
        public ActionResult New(int id)
        {

            ViewBag.IdEchip = id;
            ViewBag.Status = GetAllStatusList();
            Project project = db.Projects.Find(id);
            string idu = User.Identity.GetUserId();
            return idu == project.UserId || User.IsInRole("Admin") ? (ActionResult)View() : Redirect("/Projects/Show/" + id);
        }

        [HttpPost]
        [Authorize(Roles = "Leader,Admin")]
        public ActionResult New(TaskClass task)
        {
            try
            {
                string u=(from us  in db.Users
                  where  us.UserName==task.WorkerName
                  select  us.Id).First();
                Project proj = db.Projects.Find(task.ProjectId);
                if (db.Members.Any(anyObjectName => anyObjectName.UserId == u
                                   && anyObjectName.TeamId == proj.TeamId))
                {
                    ViewBag.Status = GetAllStatusList();
                    task.UserId = User.Identity.GetUserId();
                    db.Tasks.Add(task);
                    db.SaveChanges();
                    TempData["message"] = "Task-ul a fost adaugat!";
                    return Redirect("/Projects/Show/" + task.ProjectId);



                }
                else
                {
                    return Redirect("New/" + task.ProjectId);
                }

            }
            catch (Exception e)
            {
                return Redirect("New/" + task.ProjectId);
            }
        }
        [Authorize(Roles = "Leader,Admin")]
        public ActionResult Delete(int id)
        {
            TaskClass task = db.Tasks.Find(id);
            if (task.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Tasks.Remove(task);
                db.SaveChanges();
                TempData["message"] = "Task-ul a fost sters";
                return Redirect("/Projects/Show/" + task.ProjectId);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari!";
                return RedirectToAction("../Projects/Show/" + task.ProjectId);
            }
        }
        [Authorize(Roles = "Leader,Admin")]
        public ActionResult Edit(int id)
        {
            TaskClass task = db.Tasks.Find(id);
            ViewBag.Task = task;
            ViewBag.Status = GetAllStatusList();
            if (task.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                return View();
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari!";
                return RedirectToAction("../Projects/Show/" + task.ProjectId);
            }

        }

        [HttpPut]
        [Authorize(Roles = "Leader,Admin")]
        public ActionResult Edit(int id, TaskClass requestTask)
        {
            try
            {   
                TaskClass task = db.Tasks.Find(id);
                ViewBag.Status = GetAllStatusList();
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
        [NonAction]
        //Define the list which you have to show in Drop down List
        public IEnumerable<SelectListItem> GetAllStatusList()
        {
            List<SelectListItem> myList = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value="Not started",Text="Not started"},
                 new SelectListItem{ Value="In progress",Text="In progress"},
                 new SelectListItem{ Value="Completed",Text="Completed"},
             };
            myList = data.ToList();
            return myList;
        }
    }
}
using Microsoft.AspNet.Identity;
using Proiect2020.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect2020.Controllers
{
    public class CommentsController : Controller
    {
        // GET: Comments
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        [Authorize(Roles = "Membru,Organizator,Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpDelete]
        [Authorize(Roles = "Membru,Organizator,Admin")]
        public ActionResult Delete(int id)
        {
            Comment comm = db.Comments.Find(id);
            if (comm.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Comments.Remove(comm);
                db.SaveChanges();
                TempData["message"] = "Comentariul a fost sters";
                return Redirect("/Tasks/Show/" + comm.TaskId);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti comentariul";
                return Redirect("/Tasks/Show/" + comm.TaskId);
            }
        }
        [Authorize(Roles = "Membru,Organizator,Admin")]
        public ActionResult New(int id)
        {
            Comment comm = new Comment();
            ViewBag.IdTask = id;
            comm.UserId = User.Identity.GetUserId();
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Membru,Organizator,Admin")]
        public ActionResult New(Comment comm)
        {
            comm.UserId = User.Identity.GetUserId();
            comm.Date = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Comments.Add(comm);
                    db.SaveChanges();
                    TempData["message"] = "Comentariul a fost adaugat!";
                    return Redirect("/Tasks/Show/" + comm.TaskId);
                }
                else
                {
                    return View(comm);
                }
            }

            catch (Exception e)
            {
                return Redirect("/Tasks/Show/" + comm.TaskId);
            }

        }

        [Authorize(Roles = "Membru,Organizator,Admin")]
        public ActionResult Edit(int id)
        {
            Comment comm = db.Comments.Find(id);
            ViewBag.Comment = comm;
            return View(comm);
        }

        [HttpPut]
        [Authorize(Roles = "Membru,Organizator,Admin")]
        public ActionResult Edit(int id, Comment requestComment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Comment comm = db.Comments.Find(id);
                    if (TryUpdateModel(comm))
                    {
                        comm.Content = requestComment.Content;
                        db.SaveChanges();
                        TempData["message"] = "Comentariul a fost modificat!";
                        return Redirect("/Tasks/Show/" + comm.TaskId);
                    }
                    else
                    {
                        return View(requestComment);
                    }
                }
                else
                {
                    return View(requestComment);
                }
            }
            catch (Exception e)
            {
                return View(requestComment);
            }

        }

    }
}
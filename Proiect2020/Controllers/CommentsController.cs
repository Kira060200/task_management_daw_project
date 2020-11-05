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
        private Proiect2020.Models.AppContext db = new Proiect2020.Models.AppContext();

        // GET: Comments
        public ActionResult Index()
        {
            return View();
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Comment comm = db.Comments.Find(id);
            db.Comments.Remove(comm);
            db.SaveChanges();
            return Redirect("/Tasks/Show/" + comm.TaskId);
        }
        public ActionResult New(int id)
        {

            ViewBag.IdTask = id;
            return View();
        }
        [HttpPost]
        public ActionResult New(Comment comm)
        {
            comm.Date = DateTime.Now;
            try
            {
                db.Comments.Add(comm);
                db.SaveChanges();
                return Redirect("/Tasks/Show/" + comm.TaskId);
            }

            catch (Exception e)
            {
                return Redirect("/Tasks/Show/" + comm.TaskId);
            }

        }

        public ActionResult Edit(int id)
        {
            Comment comm = db.Comments.Find(id);
            ViewBag.Comment = comm;
            return View();
        }

        [HttpPut]
        public ActionResult Edit(int id, Comment requestComment)
        {
            try
            {
                Comment comm = db.Comments.Find(id);
                if (TryUpdateModel(comm))
                {
                    comm.Content = requestComment.Content;
                    db.SaveChanges();
                }
                return Redirect("/Tasks/Show/" + comm.TaskId);
            }
            catch (Exception e)
            {
                return View();
            }

        }

    }
}
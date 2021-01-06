﻿using Microsoft.AspNet.Identity;
using Laborator8App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laborator8App.Controllers
{
    public class CommentsController : Controller
    {
        // GET: Comments
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        [Authorize(Roles = "Member,Leader,Admin")]
        {
            return View();
        }

        [HttpDelete]
        [Authorize(Roles = "Member,Leader,Admin")]
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
        [Authorize(Roles = "Member,Leader,Admin")]
        {
            Comment comm = new Comment();
            ViewBag.IdTask = id;
            comm.UserId = User.Identity.GetUserId();
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Member,Leader,Admin")]
        {
            comm.UserId = User.Identity.GetUserId();
            comm.UserName = (from user in db.Users
                             where user.Id == comm.UserId
                             select user.UserName).First();
            comm.User = (from user in db.Users
                         where user.Id == comm.UserId
                         select user).First();
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
                    ViewBag.IdTask = comm.TaskId;
                    return View(comm);
                }
            }

            catch (Exception e)
            {
                return Redirect("/Tasks/Show/" + comm.TaskId);
            }

        }

        [Authorize(Roles = "Member,Leader,Admin")]
        {
            Comment comm = db.Comments.Find(id);
            ViewBag.Comment = comm;
            return View(comm);
        }

        [HttpPut]
        [Authorize(Roles = "Member,Leader,Admin")]
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
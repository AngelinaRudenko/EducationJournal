using Journal.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Journal.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Преподаватель")]
        [HttpGet]
        public ActionResult Create()
        {
            string myId = User.Identity.GetUserId();
            ViewBag.Subjects = db.Users.FirstOrDefault(x => x.Id == myId).Subjects.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Group group, List<string> Enable)
        {
            string myId = User.Identity.GetUserId();
            Group newGroup = new Group
            {
                Name = group.Name,
                CreatorId = myId,
                Code = GetGuidString()
            };
            if (Enable != null && Enable.Count() != 0)
                foreach (string id in Enable)
                {
                    Subject subject = db.Subjects.FirstOrDefault(x => x.Id.ToString() == id);
                    subject.Groups.Add(newGroup);
                    newGroup.Subjects.Add(subject);
                }
            db.Groups.Add(newGroup);
            db.SaveChanges();
            return RedirectToAction("Groups", "Home");
        }

        [Authorize(Roles = "Преподаватель")]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string groupCode)
        {
            Group group = db.Groups.FirstOrDefault(x => x.Code == groupCode);
            if (group != null)
            {
                string myId = User.Identity.GetUserId();
                group.CreatorId = myId;
                group.Code = GetGuidString();
                db.Groups.Add(group);

                //надо подумать
                ApplicationUser me = db.Users.FirstOrDefault(x => x.Id == myId);
                me.Groups.Add(group);

                db.SaveChanges();
                return RedirectToAction("Groups", "Home");
            }
            return HttpNotFound();
        }

        [Authorize(Roles = "Преподаватель")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            string myId = User.Identity.GetUserId();
            ViewBag.Subjects = db.Users.FirstOrDefault(x => x.Id == myId).Subjects.ToList();
            Group group = db.Groups.FirstOrDefault(x => x.Id == id);
            return View(group);
        }

        [HttpPost]
        public ActionResult Edit(Group group, List<string> Enable)
        {
            Group oldGroup = db.Groups.FirstOrDefault(x => x.Id == group.Id);
            if (oldGroup != null)
            {
                oldGroup.Name = group.Name;
                oldGroup.Subjects.Clear();
                if (Enable != null && Enable.Count() != 0)
                {
                    foreach (string id in Enable)
                    {
                        Subject subject = db.Subjects.FirstOrDefault(x => x.Id.ToString() == id);
                        oldGroup.Subjects.Add(subject);
                        subject.Groups.Add(oldGroup);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Groups", "Home");
            }
            return HttpNotFound();
        }

        [Authorize(Roles = "Преподаватель")]
        [HttpGet]
        public ActionResult Details(int id)
        {
            Group group = db.Groups.FirstOrDefault(x => x.Id == id);
            return View(group);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Group group = db.Groups.FirstOrDefault(x => x.Id == id);
            return View(group);
        }

        [Authorize(Roles = "Преподаватель")]
        [HttpPost]
        public ActionResult Delete(Group group)
        {
            Group gr = db.Groups.FirstOrDefault(x => x.Id == group.Id);
            if (gr != null)
            {
                db.Groups.Remove(gr);
                db.SaveChanges();
                return RedirectToAction("Groups", "Home");
            }
            return HttpNotFound();
        }

        [Authorize(Roles = "Ученик")]
        [HttpGet]
        public ActionResult Join()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Join(string code)
        {
            Group group = db.Groups.FirstOrDefault(x => x.Code == code);
            if (group != null)
            {
                string myId = User.Identity.GetUserId();
                ApplicationUser me = db.Users.FirstOrDefault(x => x.Id == myId);
                group.Members.Add(me);
                me.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Groups", "Home");
            }
            return HttpNotFound();
        }

        private string GetGuidString()
        {
            Guid g = Guid.NewGuid();
            string codeString = Convert.ToBase64String(g.ToByteArray());
            codeString = codeString.Replace("=", "");
            codeString = codeString.Replace("+", "");
            return codeString;
        }
    }
}
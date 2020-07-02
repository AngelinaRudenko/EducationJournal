using Journal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using Group = Journal.Models.Group;

namespace Journal.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;

        public HomeController()
        {
            db = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Groups()
        {
            string myId = User.Identity.GetUserId();
            if (User.IsInRole("Преподаватель"))
            {
                return View(db.Groups.Where(x => x.CreatorId == myId).ToList());
            }
            else
            {
                return View(db.Users.FirstOrDefault(x => x.Id == myId).Groups.ToList());
            }
        }

        [Authorize]
        public ActionResult Marks()
        {
            string myId = User.Identity.GetUserId();
            if (User.IsInRole("Преподаватель"))
            {
                List<Group> groups = db.Groups.Where(x => x.CreatorId == myId).ToList();
                return View(groups);
            }
            else
            {
                List<Group> groups = db.Groups.Where(x => x.Members.Any(u => u.Id == myId)).ToList();
                return View(groups);
            }           
        }

        [Authorize]
        public ActionResult Subjects()
        {
            string myId = User.Identity.GetUserId();
            if (User.IsInRole("Преподаватель"))
            {
                return View(db.Subjects.Where(x => x.TeacherId == myId).ToList());
            }
            else
            {
                List<Group> myGroups = db.Users.FirstOrDefault(x => x.Id == myId).Groups.ToList();
                List<Subject> subjects = new List<Subject>();
                foreach (Group group in myGroups)
                {
                    subjects.Add(db.Subjects.FirstOrDefault(x => x.Groups.Any(y=>y.Id==group.Id)));
                }
                return View(subjects);
            }           
        }
    }
}
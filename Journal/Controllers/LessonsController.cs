using Journal.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Journal.Controllers
{
    public class LessonsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Преподаватель")]
        [HttpGet]
        public ActionResult Create(string subjectId)
        {
            ViewBag.SubjectId = Convert.ToInt32(subjectId);
            return View();
        }

        [HttpPost]
        public ActionResult Create(Lesson lesson)
        {
            Subject subject = db.Subjects.FirstOrDefault(x => x.Id == lesson.SubjectId);
            if (subject != null)
            {
                Lesson newLesson = new Lesson
                {
                    Theme = lesson.Theme,
                    Date = lesson.Date,
                    SubjectId = lesson.SubjectId
                };
                db.Lessons.Add(newLesson);
                subject.Lessons.Add(newLesson);
                db.SaveChanges();
                return RedirectToAction("Marks", "Home");
            }
            return HttpNotFound();
        }

        [Authorize(Roles = "Преподаватель")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Lesson lesson = db.Lessons.FirstOrDefault(x => x.Id == id);
            return View(lesson);
        }

        [HttpPost]
        public ActionResult Edit(Lesson lesson)
        {
            Lesson oldLesson = db.Lessons.FirstOrDefault(x => x.Id == lesson.Id);
            if (oldLesson != null)
            {
                oldLesson.Theme = lesson.Theme;
                oldLesson.Date = lesson.Date;
                db.SaveChanges();
                return RedirectToAction("Marks", "Home");
            }
            return HttpNotFound();
        }

        [Authorize(Roles = "Преподаватель")]
        [HttpGet]
        public ActionResult Details(int id)
        {
            Lesson lesson = db.Lessons.FirstOrDefault(x => x.Id == id);
            return View(lesson);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Lesson lesson = db.Lessons.FirstOrDefault(x => x.Id == id);
            return View(lesson);
        }

        [Authorize(Roles = "Преподаватель")]
        [HttpPost]
        public ActionResult Delete(Lesson lesson)
        {
            Lesson lsn = db.Lessons.FirstOrDefault(x => x.Id == lesson.Id);
            if (lsn != null)
            {
                db.Lessons.Remove(lsn);
                db.SaveChanges();
                return RedirectToAction("Marks", "Home");
            }
            return HttpNotFound();
        }
    }
}
using Journal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Journal.Controllers
{
    public class MarksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Преподаватель")]
        [HttpGet]
        public ActionResult Create(int lessonId, string studentId)
        {
            ViewBag.LessonId = Convert.ToInt32(lessonId);
            ViewBag.StudentId = studentId;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Mark mark)
        {
            Lesson lesson = db.Lessons.FirstOrDefault(x => x.Id == mark.LessonId);
            if (lesson != null)
            {
                Mark newMark = new Mark
                {
                    LessonId = mark.LessonId,
                    StudentId = mark.StudentId,
                    Value = mark.Value
                };
                db.Marks.Add(newMark);
                lesson.Marks.Add(newMark);
                db.SaveChanges();
                return RedirectToAction("Marks", "Home");
            }
            return HttpNotFound();
        }

        [Authorize(Roles = "Преподаватель")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Mark mark = db.Marks.FirstOrDefault(x => x.Id == id);
            return View(mark);
        }

        [HttpPost]
        public ActionResult Edit(Mark mark)
        {
            Mark oldMark = db.Marks.FirstOrDefault(x => x.Id == mark.Id);
            if (oldMark != null)
            {
                oldMark.Value = mark.Value;
                db.SaveChanges();
                return RedirectToAction("Marks", "Home");
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Mark mark = db.Marks.FirstOrDefault(x => x.Id == id);
            return View(mark);
        }

        [Authorize(Roles = "Преподаватель")]
        [HttpPost]
        public ActionResult Delete(Mark mark)
        {
            Mark mrk = db.Marks.FirstOrDefault(x => x.Id == mark.Id);
            if (mrk != null)
            {
                db.Marks.Remove(mrk);
                db.SaveChanges();
                return RedirectToAction("Marks", "Home");
            }
            return HttpNotFound();
        }
    }
}
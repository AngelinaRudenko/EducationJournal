using Journal.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace Journal.Controllers
{
    public class SubjectsController : Controller
    {
        private ApplicationDbContext db;

        public SubjectsController()
        {
            db = new ApplicationDbContext();
        }

        [Authorize(Roles = "Преподаватель")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Subject subject)
        {
            string myId = User.Identity.GetUserId();
            Subject newSubject = new Subject
            {
                Name = subject.Name,
                TeacherId = myId
            };
            db.Subjects.Add(newSubject);

            //надо подумать
            ApplicationUser me = db.Users.FirstOrDefault(x => x.Id == myId);
            me.Subjects.Add(newSubject);

            db.SaveChanges();
            return RedirectToAction("Subjects", "Home");
        }       

        [Authorize(Roles = "Преподаватель")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Subject subject = db.Subjects.FirstOrDefault(x => x.Id == id);
            return View(subject);
        }

        [HttpPost]
        public ActionResult Edit(Subject subject)
        {
            Subject oldSubject = db.Subjects.FirstOrDefault(x => x.Id == subject.Id);
            if (oldSubject != null)
            {
                oldSubject.Name = subject.Name;
                db.SaveChanges();
                return RedirectToAction("Subjects", "Home");
            }
            return HttpNotFound();
        }

        [Authorize(Roles = "Преподаватель")]
        [HttpGet]
        public ActionResult Details(int id)
        {
            Subject subject = db.Subjects.FirstOrDefault(x => x.Id == id);
            return View(subject);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Subject subject = db.Subjects.FirstOrDefault(x => x.Id == id);
            return View(subject);
        }

        [Authorize(Roles = "Преподаватель")]
        [HttpPost]
        public ActionResult Delete(Subject subject)
        {
            Subject sb = db.Subjects.FirstOrDefault(x => x.Id == subject.Id);
            if (sb != null)
            {
                db.Subjects.Remove(sb);
                db.SaveChanges();
                return RedirectToAction("Subjects", "Home");
            }
            return HttpNotFound();
        }
    }
}
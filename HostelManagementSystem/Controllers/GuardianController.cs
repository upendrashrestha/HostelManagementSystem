using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HostelManagementSystem.Data;
using HostelManagementSystem.Filters;

namespace HostelManagementSystem.Controllers
{

   // [ValidateSessionAttribute]
    public class GuardianController : Controller
    {
        private HMSEntities db = new HMSEntities();

        // GET: Guardian
        public ActionResult Index()
        {
            var t_guardian = db.t_guardian.Include(t => t.t_staff).Include(t => t.t_student);
            return View(t_guardian.ToList());
        }

        // GET: Guardian/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_guardian t_guardian = db.t_guardian.Find(id);
            if (t_guardian == null)
            {
                return HttpNotFound();
            }
            return View(t_guardian);
        }

        // GET: Guardian/Create
        public ActionResult Create()
        {
            ViewBag.staff_id = new SelectList(db.t_staff, "staff_id", "first_name");
            ViewBag.stud_id = new SelectList(db.t_student, "stud_id", "first_name");
            return View();
        }

        // POST: Guardian/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "guard_id,first_name,last_name,address,phone,email,relationship,stud_id,staff_id")] t_guardian t_guardian)
        {
            if (ModelState.IsValid)
            {
                db.t_guardian.Add(t_guardian);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.staff_id = new SelectList(db.t_staff, "staff_id", "first_name", t_guardian.staff_id);
            ViewBag.stud_id = new SelectList(db.t_student, "stud_id", "first_name", t_guardian.stud_id);
            return View(t_guardian);
        }

        // GET: Guardian/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_guardian t_guardian = db.t_guardian.Find(id);
            if (t_guardian == null)
            {
                return HttpNotFound();
            }
            ViewBag.staff_id = new SelectList(db.t_staff, "staff_id", "first_name", t_guardian.staff_id);
            ViewBag.stud_id = new SelectList(db.t_student, "stud_id", "first_name", t_guardian.stud_id);
            return View(t_guardian);
        }

        // POST: Guardian/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "guard_id,first_name,last_name,address,phone,email,relationship,stud_id,staff_id")] t_guardian t_guardian)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_guardian).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.staff_id = new SelectList(db.t_staff, "staff_id", "first_name", t_guardian.staff_id);
            ViewBag.stud_id = new SelectList(db.t_student, "stud_id", "first_name", t_guardian.stud_id);
            return View(t_guardian);
        }

        // GET: Guardian/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_guardian t_guardian = db.t_guardian.Find(id);
            if (t_guardian == null)
            {
                return HttpNotFound();
            }
            return View(t_guardian);
        }

        // POST: Guardian/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            t_guardian t_guardian = db.t_guardian.Find(id);
            db.t_guardian.Remove(t_guardian);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

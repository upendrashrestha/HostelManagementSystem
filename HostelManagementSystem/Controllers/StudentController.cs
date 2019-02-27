using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HostelManagementSystem.Data;
using HostelManagementSystem.Filters;
using HostelManagementSystem.Models;
using HostelManagementSystem.Services;

namespace HostelManagementSystem.Controllers
{
    [ValidateSessionAttribute]
    public class StudentController : Controller
    {
        private IStudentManager studentMgr = new StudentManager();
        private HMSEntities db = new HMSEntities();

        // GET: Student
        [Route("Dashboard")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View(studentMgr.GetStudentList());
        }


        public ActionResult Search()
        {
            IStudentManager studentMgr = new StudentManager();
            var students = studentMgr.GetStudentList();
            ViewBag.Title = "Home Page";
            SearchCriteria srcCri = new SearchCriteria();
            return View(srcCri);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search([Bind(Include = "FirstName,LastName,Phone,Email,RoomNo,Inactive")] SearchCriteria searchCriteria)
        {
            if (ModelState.IsValid)
            {
                SearchManager srchManager = new SearchManager();
                var searchResult = srchManager.SearchStudent(searchCriteria);
                if (searchResult != null && searchResult.Count > 0)
                {
                    TempData["SearchResult"] = searchResult;
                    return RedirectToAction("Result");
                }
                ViewData["Error"] = "! Please search again with valid data.";
                return View();

            }

            return View(searchCriteria);
        }

        public ActionResult Result()
        {
            var model = TempData["SearchResult"];
            if (model != null)
            {
                return View(model);
            }
            return RedirectToAction("Search");
        }

        // GET: Student/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_student t_student = db.t_student.Find(id);
            if (t_student == null)
            {
                return HttpNotFound();
            }
            return View(t_student);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "stud_id,first_name,last_name,address,dob,phone,email,gender,monthly_fee,school_info,room_no,building_info, guardian_name, guardian_contact_info, guardian_relationship, enrollment_date")] t_student t_student, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                    t_student.img_file = ConvertToBytes(image);
                studentMgr.InsertStudent(t_student);
                //db.t_student.Add(t_student);
                //db.SaveChanges();
                return RedirectToAction("Details", "Student", new { id = t_student.stud_id });
            }

            return View(t_student);
        }


        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadImage([Bind(Include = "stud_id")] t_student t_student, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    t_student.img_file = ConvertToBytes(image);
                    studentMgr.UpdateStudentImage(t_student);
                }
                //db.t_student.Add(t_student);
                //db.SaveChanges();
                return RedirectToAction("Details", "Student", new { id = t_student.stud_id });
            }

            return View(t_student);
        }

        public ActionResult UploadImage(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_student t_student = db.t_student.Find(id);
            if (t_student == null)
            {
                return HttpNotFound();
            }
            return View(t_student);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_student t_student = db.t_student.Find(id);
            if (t_student == null)
            {
                return HttpNotFound();
            }
            return View(t_student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "stud_id,first_name,last_name,address,dob,phone,email,gender,monthly_fee,school_info,room_no,building_info,guardian_name, guardian_contact_info, guardian_relationship")] t_student t_student)
        {
            if (ModelState.IsValid)
            {
                studentMgr.UpdateStudent(t_student);
                //db.Entry(t_student).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Details", "Student", new { id = t_student.stud_id });
            }
            return View(t_student);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_student t_student = db.t_student.Find(id);
            if (t_student == null)
            {
                return HttpNotFound();
            }
            return View(t_student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            t_student t_student = db.t_student.Find(id);
            db.t_student.Remove(t_student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Deactivate(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_student t_student = db.t_student.Find(id);
            if (t_student == null)
            {
                return HttpNotFound();
            }
            studentMgr.DeactivateStudent(t_student);
            return RedirectToAction("List", "Student");
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

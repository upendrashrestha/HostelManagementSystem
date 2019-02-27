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
using HostelManagementSystem.Services;

namespace HostelManagementSystem.Controllers
{

   [ValidateSessionAttribute]
    public class StaffController : Controller
    {
        private IStaffManager staffMgr = new StaffManager();
        private HMSEntities db = new HMSEntities();

        // GET: Staff
        public ActionResult Index()
        {
            return View(staffMgr.GetStaffList());
        }

        public ActionResult List()
        {
            return View(staffMgr.GetStaffList());
        }

        // GET: Staff/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          var staff= staffMgr.GetStaff(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // GET: Staff/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Staff/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "first_name,last_name,address,dob,phone,email,gender,salary, join_date, building_info, citizenship_no, img_file, guardian_name, guardian_contact_info, guardian_relationship")] t_staff t_staff)
        {
            if (ModelState.IsValid)
            {
                staffMgr.InsertStaff(t_staff);
                return RedirectToAction("List");
            }

            return View(t_staff);
        }

        // GET: Staff/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_staff t_staff = db.t_staff.Find(id);
            if (t_staff == null)
            {
                return HttpNotFound();
            }
            return View(t_staff);
        }

        // POST: Staff/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "staff_id,first_name,last_name,address,dob,phone,email,gender,salary, join_date, citizenship_no, building_info,guardian_name, guardian_contact_info, guardian_relationship")] t_staff t_staff)
        {
            if (ModelState.IsValid)
            {
                staffMgr.UpdateStaff(t_staff);
                return RedirectToAction("Index");
            }
            return View(t_staff);
        }

        // GET: Staff/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_staff t_staff = db.t_staff.Find(id);
            if (t_staff == null)
            {
                return HttpNotFound();
            }
            return View(t_staff);
        }



        // POST: Staff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            t_staff t_staff = db.t_staff.Find(id);
            db.t_staff.Remove(t_staff);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Deactivate(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_staff t_staff = db.t_staff.Find(id);
            if (t_staff == null)
            {
                return HttpNotFound();
            }
            staffMgr.DeactivateStaff(t_staff);
            return View("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadImage([Bind(Include = "staff_id")] t_staff t_staff, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    t_staff.img_file = ConvertToBytes(image);
                    staffMgr.UpdateStaffImage(t_staff);
                }
                //db.t_student.Add(t_student);
                //db.SaveChanges();
                return RedirectToAction("Details", "Staff", new { id = t_staff.staff_id });
            }

            return View(t_staff);
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        public ActionResult UploadImage(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_staff t_staff = db.t_staff.Find(id);
            if (t_staff == null)
            {
                return HttpNotFound();
            }
            return View(t_staff);
        }

    }
}

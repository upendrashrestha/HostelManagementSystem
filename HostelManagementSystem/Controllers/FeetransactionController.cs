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
    public class FeetransactionController : Controller
    {
        private HMSEntities db = new HMSEntities();

        [Route("/{id}")]
        // GET: Feetransaction
        public ActionResult Index(string id)
        {
            StudentFeePayment feePayment = new StudentFeePayment();
            feePayment.Student = db.t_student.Find(id);
            var feeTran = from f in db.t_feetransaction
                          where f.stud_id == id
                          select f;
            feePayment.Transaction = feeTran.ToList();
            return View(feePayment);
        }


        // GET: Feetransaction/Details/5
        public ActionResult Details(string id)
        {
            StudentFeePayment feePayment = new StudentFeePayment();
            feePayment.Student = db.t_student.Find(id);
            var feeTran = (from f in db.t_feetransaction
                           where f.stud_id == id
                           select f).OrderByDescending(x => x.created_on);
            feePayment.Transaction = feeTran.ToList();
            return View(feePayment);
        }

        // GET: Feetransaction/AddPayment
        public ActionResult AddPayment(string id)
        {
            StudentFeePayment feePayment = new StudentFeePayment();
            feePayment.Student = db.t_student.Find(id);
            // feePayment.Transaction 

            var listTransaction = new List<t_feetransaction>();
            t_feetransaction defaultFee = new t_feetransaction();
            defaultFee.stud_id = feePayment.Student.stud_id;
            defaultFee.paid_amount = feePayment.Student.monthly_fee;
            listTransaction.Add(defaultFee);
            feePayment.Transaction = listTransaction;
            //var feeTran = from f in db.t_feetransaction
            //              where f.stud_id == id
            //              select f;
            //feePayment.Transaction = feeTran.ToList();
            return View(feePayment);
        }

        // POST: Feetransaction/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPayment([Bind(Include = "stud_id, paid_amount,due_amount,paid_for_month,paid_for_year, comments")] t_feetransaction studentFeePayment)
        {
            if (ModelState.IsValid)
            {
                //t_feetransaction fee = studentFeePayment.Transaction.First();
                ITransaction tran = new Transaction();
                studentFeePayment.created_by = Session["UserID"].ToString();
                tran.FeeTransaction(studentFeePayment);

            }
            return RedirectToAction("Details", new { id = studentFeePayment.stud_id });
        }

        // GET: Feetransaction/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_feetransaction feetransaction = db.t_feetransaction.Find(id);
            if (feetransaction == null)
            {
                return HttpNotFound();
            }

            StudentFeePayment feePayment = new StudentFeePayment();
            feePayment.Student = db.t_student.Find(feetransaction.stud_id);
            var listTransaction = new List<t_feetransaction>();
            listTransaction.Add(feetransaction);
            feePayment.Transaction = listTransaction;
            return View(feePayment);
        }

        public ActionResult PostFeePayment(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_feetransaction FeeTransaction = db.t_feetransaction.Find(id);
            ITransaction tran = new Transaction();
            string approvedBy = Session["UserID"].ToString();
            tran.PostFeeTransaction(FeeTransaction, approvedBy);
            return RedirectToAction("Details", new { id = FeeTransaction.stud_id });
        }
        // POST: Feetransaction/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "feetran_id,stud_id,paid_amount,due_amount,paid_for_month,paid_for_year,tran_approved,approved_by,comments,created_by,created_on")] t_feetransaction feetransaction)
        {
            if (ModelState.IsValid)
            {
                ITransaction tran = new Transaction();
                tran.UpdateFeeTransaction(feetransaction);

            }
            return RedirectToAction("Details", new { id = feetransaction.stud_id });
        }

        // GET: Feetransaction/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_feetransaction t_feetransaction = db.t_feetransaction.Find(id);
            if (t_feetransaction == null)
            {
                return HttpNotFound();
            }
            return View(t_feetransaction);
        }

        // POST: Feetransaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            t_feetransaction t_feetransaction = db.t_feetransaction.Find(id);
            db.t_feetransaction.Remove(t_feetransaction);
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

        public ActionResult Print(string id)
        {
            ITransaction t = new Transaction();
            var doc = t.PrintStatement(id);
            MemoryStream stream = new MemoryStream();
            doc.Save(stream, false);
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.BinaryWrite(stream.ToArray());
            Response.Flush();
            stream.Close();
            Response.End();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ViewAllFeeTransaction(FormCollection form) {
            string enteredDays = form["days"];
            int days = 0;
            if (!string.IsNullOrEmpty(enteredDays))
            {
                days = Convert.ToInt32(enteredDays);
                ViewBag.Days = days;
            }
           
            ITransaction t = new Transaction();
            var trans = t.GetTransaction(days);
            return View(trans);
        }

        public ActionResult ViewAllFeeTransaction()
        {            
            int days = 1;
            ITransaction t = new Transaction();
            var trans = t.GetTransaction(days);
            return View(trans);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using HostelManagementSystem.Data;
using HostelManagementSystem.Models;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;

namespace HostelManagementSystem.Services
{
    public class Transaction : ITransaction
    {
        private HMSEntities _hmsDB = null;
        public Transaction()
        {
            _hmsDB = new HMSEntities();
        }
        public void FeeTransaction(t_feetransaction fee)
        {
            _hmsDB.FeePayment(fee.stud_id, fee.paid_amount, fee.due_amount, fee.paid_for_month, fee.paid_for_year, fee.comments, fee.created_by, DateTime.Now.ToString());
        }

        public void UpdateFeeTransaction(t_feetransaction fee)
        {
            _hmsDB.UpdateFeePayment(fee.feetran_id, fee.stud_id, fee.paid_amount, fee.due_amount, fee.paid_for_month, fee.paid_for_year, fee.comments, fee.created_by, DateTime.Now.ToString());
        }


        public void PostFeeTransaction(t_feetransaction fee, string approvedBy)
        {
            _hmsDB.FeePaymentApproval(fee.feetran_id, approvedBy);
        }


        public void SalaryTransaction(t_salarytransaction salary)
        {
            _hmsDB.FeePayment(salary.staff_id, salary.paid_amount, salary.advance_amount, salary.paid_for_month, salary.paid_for_year, "", salary.created_by, DateTime.Now.ToString());

        }

        public PdfDocument PrintStatement(string tranId)
        {

            var transaction = _hmsDB.t_feetransaction.Where(x => x.feetran_id == tranId).FirstOrDefault();
            var student = _hmsDB.t_student.Where(x => x.stud_id == transaction.stud_id).FirstOrDefault();
            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("Verdana", 20, XFontStyle.Bold);
            XFont datefont = new XFont("Verdana", 12, XFontStyle.Regular);
            XFont Receiptfont = new XFont("Verdana", 16, XFontStyle.Bold);
            DateTime dateTime = DateTime.Now.Date;
            string dateText = "Date : " + dateTime.ToString("dd/MM/yyyy");


            XTextFormatter tf = new XTextFormatter(gfx);

            string receivedText = "Received From  " + student.first_name + " " + student.last_name + " the amount of Rs. " + transaction.paid_amount + "\nFor " + transaction.paid_for_month + " " + transaction.paid_for_year + ".";


            XRect rect = new XRect(0, 10, page.Width - 30, 220);
            //  gfx.DrawRectangle(XBrushes.SeaShell, rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString("Receipt", font, XBrushes.Black, rect, XStringFormats.TopLeft);

            rect = new XRect(10, 40, page.Width - 30, 210);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString("Receipt No : " + transaction.feetran_id, datefont, XBrushes.Black, rect, XStringFormats.TopLeft);

            rect = new XRect(10, 55, page.Width - 30, 210);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(dateText, datefont, XBrushes.Black, rect, XStringFormats.TopLeft);

            rect = new XRect(10, 105, page.Width - 30, 160);
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(receivedText, Receiptfont, XBrushes.Black, rect, XStringFormats.TopLeft);

            rect = new XRect(10, 170, page.Width - 30, 160);
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString("Due Amount : " + transaction.due_amount, datefont, XBrushes.Black, rect, XStringFormats.TopLeft);

            rect = new XRect(10, 190, page.Width - 30, 160);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString("Received By : " + transaction.created_by, datefont, XBrushes.Black, rect, XStringFormats.TopLeft);


            document.Info.Title = "Receipt";
            document.Info.Author = "Hostel Management Engine";
            return document;
        }

        public List<FeePayment> GetTransaction(int daysBefore)
        {
            var listFeePayment = _hmsDB.GetFeePaymentTransaction(daysBefore);
         
            List<FeePayment> feePayments = new List<FeePayment>();
            if (listFeePayment != null)
            {
                
                    foreach (var v in listFeePayment)
                    {
                        feePayments.Add(new FeePayment()
                        {
                            stud_id = v.stud_id,
                            first_name = v.name,
                            due_amount = Convert.ToDecimal(v.due_amount),
                            paid_amount = Convert.ToDecimal(v.paid_amount),
                            created_on = v.created_on,
                            created_by = v.created_by,
                            approved_by = v.approved_by,
                            tran_approved = v.tran_approved,
                            paid_for_month = v.monthYear
                        });
                    
                }
            }
                return feePayments;
            
        }
    }
}
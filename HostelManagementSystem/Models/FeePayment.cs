using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HostelManagementSystem.Models
{
    public class FeePayment
    {
        public string stud_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string building_info { get; set; }
        public string room_no { get; set; }
        public decimal monthly_fee { get; set; }
        public byte[] img_file { get; set; }
        public decimal paid_amount { get; set; }
        public decimal due_amount { get; set; }
        public string paid_for_month { get; set; }
        public string paid_for_year { get; set; }
        public string tran_approved { get; set; }
        public string approved_by { get; set; }
        public string created_by { get; set; }
        public string created_on { get; set; }
    }
}
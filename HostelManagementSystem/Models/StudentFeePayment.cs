using HostelManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HostelManagementSystem.Models
{
    public class StudentFeePayment
    {
        public t_student Student { get; set; }
        public List<t_feetransaction> Transaction { get; set; }
    }
}
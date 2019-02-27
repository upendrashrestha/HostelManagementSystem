using HostelManagementSystem.Data;
using HostelManagementSystem.Models;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostelManagementSystem.Services
{
    public interface ITransaction
    {
        void FeeTransaction(t_feetransaction fee);
        void SalaryTransaction(t_salarytransaction salary);
        void PostFeeTransaction(t_feetransaction fee, string approvedBy);
        void UpdateFeeTransaction(t_feetransaction fee);
        PdfDocument PrintStatement(string tranId);
        List<FeePayment> GetTransaction(int daysBefore);
    }
}

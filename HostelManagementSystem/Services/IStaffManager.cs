using HostelManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostelManagementSystem.Services
{
    interface IStaffManager
    {
        List<t_staff> GetStaffList();
        t_staff GetStaff(string staffId);
        t_staff InsertStaff(t_staff staff);
        t_staff UpdateStaff(t_staff staff);
        bool RemoveStaff(t_staff staff);
        bool UpdateStaffImage(t_staff staff);
        void DeactivateStaff(t_staff staff);
    }
}

using HostelManagementSystem.Data;
using HostelManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostelManagementSystem.Services
{
    public interface IStudentManager
    {
        List<t_student> GetStudentList();
        t_student GetStudent(string studId);
        List<t_student> SearchStudent(SearchCriteria searchItem);
        t_student InsertStudent(t_student student);
        t_student UpdateStudent(t_student student);
        bool RemoveStudent(t_student student);
        bool UpdateStudentImage(t_student student);
        void DeactivateStudent(t_student student);
    }
}

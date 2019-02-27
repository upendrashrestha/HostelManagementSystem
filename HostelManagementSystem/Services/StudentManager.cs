using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HostelManagementSystem.Data;
using HostelManagementSystem.Models;

namespace HostelManagementSystem.Services
{
    public class StudentManager : IStudentManager
    {
        private HMSEntities _hmsDB = null;
        public StudentManager()
        {
            _hmsDB = new HMSEntities();
        }

        public t_student GetStudent(string studId)
        {
            throw new NotImplementedException();
        }

        public List<t_student> GetStudentList()
        {
            try
            {

                if (_hmsDB != null)
                {
                    return _hmsDB.t_student.Where(x => x.Active == "Y").ToList();
                }
                return null;


            }
            catch (Exception)
            {

                throw;
            }
        }


        public t_student InsertStudent(t_student student)
        {
            _hmsDB.CreateStudent(student.first_name, student.last_name, student.address, student.dob, student.phone, student.email, student.gender, student.monthly_fee, student.school_info, student.room_no, student.building_info, student.guardian_name, student.guardian_contact_info, student.guardian_relationship, student.enrollment_date, student.img_file);
            return student;
           // throw new NotImplementedException();
        }

        public bool RemoveStudent(t_student student)
        {
            throw new NotImplementedException();
        }

        public List<t_student> SearchStudent(SearchCriteria searchItem)
        {
            throw new NotImplementedException();
        }

        public t_student UpdateStudent(t_student student)
        {
            try
            {
                _hmsDB.UpdateStudent(student.stud_id,student.first_name, student.last_name, student.address, student.dob, student.phone, student.email, student.gender, student.monthly_fee, student.school_info, student.room_no, student.building_info, student.guardian_name, student.guardian_contact_info, student.guardian_relationship, student.enrollment_date);
                return student;
            }
            catch
            {
                return null;
            }

        }

        public bool UpdateStudentImage(t_student student)
        {
            try
            {
                _hmsDB.UpdateStudentImage(student.stud_id, student.img_file);
                return true;
            }
            catch {
                return false;
            }

        }

        public void DeactivateStudent(t_student student) {
            _hmsDB.DeactivateStudent(student.stud_id, DateTime.Now);
        }
    }
}
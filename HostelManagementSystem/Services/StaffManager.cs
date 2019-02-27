using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HostelManagementSystem.Data;

namespace HostelManagementSystem.Services
{
    public class StaffManager : IStaffManager
    {
        private HMSEntities _hmsDB = null;
        public StaffManager()
        {
            _hmsDB = new HMSEntities();
        }

        public void DeactivateStaff(t_staff staff)
        {
            _hmsDB.DeactivateStaff(staff.staff_id);
        }

        public t_staff GetStaff(string staffId)
        {
            try
            {

                if (_hmsDB != null)
                {
                    return _hmsDB.t_staff.Where(x => x.staff_id == staffId).FirstOrDefault();
                }
                return null;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<t_staff> GetStaffList()
        {
            try
            {

                if (_hmsDB != null)
                {
                    return _hmsDB.t_staff.Where(x => x.active == "Y").ToList();
                }
                return null;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public t_staff InsertStaff(t_staff staff)
        {
            try
            {

                if (_hmsDB != null)
                {
                     _hmsDB.CreateStaff(staff.first_name, staff.last_name, staff.address, staff.dob, staff.phone, staff.email, staff.gender, staff.salary, staff.building_info, staff.guardian_name, staff.guardian_contact_info, staff.guardian_relationship, staff.join_date, staff.citizenship_no, staff.img_file);
                    return staff;
                }
                return null;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool RemoveStaff(t_staff staff)
        {
            throw new NotImplementedException();
        }

        public t_staff UpdateStaff(t_staff staff)
        {
            try
            {

                if (_hmsDB != null)
                {
                    _hmsDB.UpdateStaff(staff.staff_id, staff.first_name, staff.last_name, staff.address, staff.dob, staff.phone, staff.email, staff.gender, staff.salary, staff.building_info, staff.guardian_name, staff.guardian_contact_info, staff.guardian_relationship, staff.join_date, staff.citizenship_no);
                    return staff;
                }
                return null;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool UpdateStaffImage(t_staff staff)
        {
            try
            {
                _hmsDB.UpdateStaffImage(staff.staff_id, staff.img_file);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
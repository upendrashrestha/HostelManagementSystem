using HostelManagementSystem.AES246Encrpytion;
using HostelManagementSystem.Data;
using HostelManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HostelManagementSystem.Services
{
    public class UserManager
    {
        private HMSEntities _hmsDB = null;
        public UserManager()
        {
            _hmsDB = new HMSEntities();
        }

        public void CreateUser(User user)
        {
            user.Password = EncryptionLibrary.EncryptText(user.Password);
            _hmsDB.RegisterUser(user.Username, user.Password, user.Staff.staff_id, DateTime.Now);
        }

        public string GetUserFullName(string UserId)
        {
            var id = _hmsDB.t_RegisterUser.Where(x => x.user_id == UserId).FirstOrDefault().staff_id;
            var staff = _hmsDB.t_staff.Where(x => x.staff_id == id).FirstOrDefault();
            return staff.first_name + " " + staff.last_name;
        }

        public string GetUserImage(string UserId)
        {
            var id = _hmsDB.t_RegisterUser.Where(x => x.user_id == UserId).FirstOrDefault().staff_id;
            var staff = _hmsDB.t_staff.Where(x => x.staff_id == id).FirstOrDefault();
            return Convert.ToBase64String(staff.img_file);
        }

        public bool GetUserStatus(string UserId)
        {
            var user = _hmsDB.t_RegisterUser.Where(x => x.user_id == UserId).FirstOrDefault();
            if (!string.IsNullOrEmpty(user.Active) && user.Active == "Y")
            {
                var staffStatus = _hmsDB.t_staff.Where(x => x.staff_id == user.staff_id).FirstOrDefault().active;
                if (!string.IsNullOrEmpty(staffStatus) && staffStatus == "Y")
                    return true;
            }
            return false;
        }

        public bool ValidateRegisteredUser(User loginUser)
        {
            var usercount = _hmsDB.t_RegisterUser.Where(x => x.username == loginUser.Username && x.password == loginUser.Password).Count();

            if (usercount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetLoggedUserID(User loginUser)
        {
            var user = _hmsDB.t_RegisterUser.Where(x => x.username == loginUser.Username && x.password == loginUser.Password).FirstOrDefault();
            return user.user_id;
        }

        public List<User> GetUsers()
        {
            var regsUsers = _hmsDB.t_RegisterUser.ToList();
            List<User> liUser = new List<User>();
            User user = null;
            foreach (t_RegisterUser regsUser in regsUsers)
            {
                user = new User();
                user.UserId = regsUser.user_id;
                user.Username = regsUser.username;
                user.Password = regsUser.password;
                user.Active = regsUser.Active == "Y" ? true : false;
                user.Staff = _hmsDB.t_staff.Where(x => x.staff_id == regsUser.staff_id).FirstOrDefault();
                liUser.Add(user);
            }
            return liUser;
        }

        public User GetUser(string userId)
        {
            t_RegisterUser regsUser = null;
            regsUser = _hmsDB.t_RegisterUser.Where(x => x.staff_id == userId).FirstOrDefault();
            // var users = GetUsers();
            // var user = users.Where(x => x.UserId == userId).SingleOrDefault();
            if (regsUser == null)
                regsUser = _hmsDB.t_RegisterUser.Where(x => x.user_id == userId).SingleOrDefault();
            User user = null;
            if (regsUser != null)
            {
                user = new User();
                user.UserId = regsUser.user_id;
                user.Username = regsUser.username;
                user.Password = regsUser.password;
                user.Active = regsUser.Active == "Y" ? true : false;
                user.Staff = _hmsDB.t_staff.Where(x => x.staff_id == regsUser.staff_id).FirstOrDefault();
            }

            return user;
        }

        public void DeactivateUser(string userId)
        {
            var user = _hmsDB.t_RegisterUser.Where(x => x.user_id == userId).FirstOrDefault();
            user.Active = "N";
            _hmsDB.Entry(user).State = EntityState.Modified;
            _hmsDB.SaveChanges();
        }

        public void ActivateUser(string userId)
        {
            var user = _hmsDB.t_RegisterUser.Where(x => x.user_id == userId).FirstOrDefault();
            user.Active = "Y";
            _hmsDB.Entry(user).State = EntityState.Modified;
            _hmsDB.SaveChanges();
        }
        public void UpdateUser(User user) {
            user.Password = EncryptionLibrary.EncryptText(user.Password);
            _hmsDB.UpdateUser(user.UserId,user.Username, user.Password, "", user.Active?"Y":"N");
        }
    }
}
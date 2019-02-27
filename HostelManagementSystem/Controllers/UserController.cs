using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HostelManagementSystem.AES246Encrpytion;
using HostelManagementSystem.Data;
using HostelManagementSystem.Filters;
using HostelManagementSystem.Models;
using HostelManagementSystem.Services;

namespace HostelManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private HMSEntities db = new HMSEntities();
        private UserManager userManager = new UserManager();

        // GET: User

        [ValidateSessionAttribute]
        public ActionResult Index()
        {
            var t_RegisterUser = db.t_RegisterUser.Include(t => t.t_staff);
            return View(t_RegisterUser.ToList());
        }


        [ValidateSessionAttribute]
        public ActionResult List()
        {
           var users=userManager.GetUsers();
          //  var t_RegisterUser = db.t_RegisterUser.Include(t => t.t_staff);         
            return View(users);
        }

        // GET: User/Details/5

        [ValidateSessionAttribute]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            var User = userManager.GetUser(id);
            if (User == null)
            {
                return HttpNotFound();
            }
            return View(User);
        }


        [ValidateSessionAttribute]
        public ActionResult Deactivate(string id) {
            userManager.DeactivateUser(id);
            return RedirectToAction("Details", new { id = id });
        }


       [ValidateSessionAttribute]
        public ActionResult Activate(string id)
        {
            userManager.ActivateUser(id);
            return RedirectToAction("Details", new { id = id });
        }

        // GET: User/Create

        [ValidateSessionAttribute]
        public ActionResult Create()
        {
            ViewBag.staff_id = new SelectList(db.t_staff, "staff_id", "first_name");
            return View();
        }

        // GET: User/RegisterUser

       [ValidateSessionAttribute]
        public ActionResult RegisterUser(string  id)
        {
            var staff = db.t_staff.Where(s => s.staff_id == id).FirstOrDefault();
            var staffUserCount = db.t_RegisterUser.Where(s => s.staff_id == staff.staff_id).Count();
            if (staffUserCount == 0)
            {
                User user = new User();
                user.Staff = staff;
                TempData["Staff"] = staff;
                //send user model
                return View(user);
            }
            else {
                var staffUser = db.t_RegisterUser.Where(s => s.staff_id == staff.staff_id).FirstOrDefault();
                 return RedirectToAction("Edit",new {id=staffUser.user_id});
               // return RedirectToAction("Index");
            }
           
        }


        [ValidateSessionAttribute]
        [HttpPost]
        [ValidateAntiForgeryToken]
       // [ValidateSessionAttribute]
        public ActionResult RegisterUser([Bind(Include = "username,password,confirmpassword,Create_On,staff_id,Staff")] User User)
        {
            var staff = TempData["Staff"];
            User.Staff = (t_staff)staff;
            if (ModelState.IsValid)
            {
                User.CreateOn = DateTime.Now;        
                UserManager userManager = new UserManager();                
                userManager.CreateUser(User);
                return RedirectToAction("Index");
            }
            return View(User);
        }


        [ValidateSessionAttribute]
        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_id,username,password,Create_On,staff_id")] t_RegisterUser t_RegisterUser)
        {
            if (ModelState.IsValid)
            {
                db.t_RegisterUser.Add(t_RegisterUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.staff_id = new SelectList(db.t_staff, "staff_id", "first_name", t_RegisterUser.staff_id);
            return View(t_RegisterUser);
        }

        // GET: User/Edit/5

       [ValidateSessionAttribute]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           var user= userManager.GetUser(id);
            if (user == null)
            {
                return HttpNotFound();
            }
           // ViewBag.staff_id = new SelectList(db.t_staff, "staff_id", "first_name", t_RegisterUser.staff_id);
            return View(user);
        }

        [ValidateSessionAttribute]
        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userId,username,password, confirmpassword,active,CreateOn")] User user)
        {
            userManager.UpdateUser(user);
            return RedirectToAction("List","User");
            //if (ModelState.IsValid)
            //{
            //    db.Entry(t_RegisterUser).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.staff_id = new SelectList(db.t_staff, "staff_id", "first_name", t_RegisterUser.staff_id);
            //return View(t_RegisterUser);
        }


        public ActionResult Login()
        {
            return View(new User());
        }

        [HttpPost]
        public ActionResult Login(User loginUser)
        {
            try
            {

                if (string.IsNullOrEmpty(loginUser.Username) && (string.IsNullOrEmpty(loginUser.Password)))
                {
                    ModelState.AddModelError("", "Enter Username and Password");
                }
                else if (string.IsNullOrEmpty(loginUser.Username))
                {
                    ModelState.AddModelError("", "Enter Username");
                }
                else if (string.IsNullOrEmpty(loginUser.Password))
                {
                    ModelState.AddModelError("", "Enter Password");
                }
                else
                {

                    loginUser.Password = EncryptionLibrary.EncryptText(loginUser.Password);
                    UserManager usermgr = new UserManager();
                    if (usermgr.ValidateRegisteredUser(loginUser))
                    {
                        var UserID = usermgr.GetLoggedUserID(loginUser);
                        var UserFullName = usermgr.GetUserFullName(UserID);
                        var UserImage = usermgr.GetUserImage(UserID);
                        var UserStatus = usermgr.GetUserStatus(UserID);
                        if (UserStatus)
                        {
                            Session["UserID"] = UserID;
                            HttpContext.Session["UserFullName"] = UserFullName;
                            Session["UserImg"] = UserImage;
                            return RedirectToAction("Index", "Home");
                        }else
                        {
                            ModelState.AddModelError("", "Inactive User Access. Please contact Administrator.");
                            return View("Login", loginUser);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid Username or Password.");
                        loginUser.Password = "";
                        loginUser.ConfirmPassword = "";
                        return View("Login", loginUser);
                    }
                }

                return View("Login", loginUser);
            }
            catch
            {
                return View();
            }
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [ValidateSessionAttribute]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "User");
        }

        [ValidateSessionAttribute]
        public ActionResult SignOff()
        {
            Session.Abandon();
            return View();
        }
    }
}

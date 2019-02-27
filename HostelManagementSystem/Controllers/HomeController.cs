using HostelManagementSystem.Data;
using HostelManagementSystem.Filters;
using HostelManagementSystem.Models;
using HostelManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HostelManagementSystem.Controllers
{

   // [ValidateSessionAttribute]
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            IStudentManager studentMgr = new StudentManager();
            var students = studentMgr.GetStudentList();
            ViewBag.Title = "Home Page";
            SearchCriteria srcCri = new SearchCriteria();
            return View(srcCri);
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "FirstName,LastName,Phone,Email,RoomNo")] SearchCriteria searchCriteria)
        {
            if (ModelState.IsValid)
            {
                SearchManager srchManager = new SearchManager();
                var searchResult = srchManager.SearchStudent(searchCriteria);
                if (searchResult!=null && searchResult.Count > 0)
                {
                    TempData["SearchResult"] = searchResult;
                    return RedirectToAction("Result");
                }
                
            }

            return View(searchCriteria);
        }

        public ActionResult Result()
        {
            var model = TempData["SearchResult"];
            if ( model != null) {
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HostelManagementSystem.Filters
{
    public class ValidateSessionAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session["UserID"])))
            {
                
                ViewResult result = new ViewResult();
                result.ViewName = "SignOff";

                filterContext.RouteData.DataTokens["area"] = "";
                filterContext.RouteData.Values["controller"] = "User";
                filterContext.RouteData.Values["action"] = "SignOff";
                filterContext.RouteData.Values["view"] = "";

                result.ViewBag.ErrorMessage = "!Cannot Access without Login.";

                filterContext.Result = result;


            }
        }

    }
}
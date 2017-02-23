using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SNCRegistration.Controllers
{
    public class CustomAuthorize : AuthorizeAttribute {

        public override void OnAuthorization(AuthorizationContext filterContext) {
            base.OnAuthorization(filterContext);
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated) {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }

            if (filterContext.Result is HttpUnauthorizedResult) {
                filterContext.Result = new RedirectResult("~/Account/AccessDenied");
            }
        }

    }
}
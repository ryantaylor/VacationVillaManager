using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VacationVillaManager.Controllers
{
    public class ErrorController : BootstrapBaseController
    {
        //
        // GET: /Error/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}

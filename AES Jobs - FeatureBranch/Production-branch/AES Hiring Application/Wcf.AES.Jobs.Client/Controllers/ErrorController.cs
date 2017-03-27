using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WcfAESJobs.Client.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult unauthorized()
        {
            return View();
        }

        public ActionResult InvalidURL()
        {
            return View();
        }

        public ActionResult ServerError()
        {
            return View();
        }
    }
}
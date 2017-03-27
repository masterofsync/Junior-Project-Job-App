using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WcfAESJobs.Client.WebService;

namespace WcfAESJobs.Client.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        JobServiceClient js = new JobServiceClient();
        // GET: Profile
        public ActionResult Index()
        {
            string UserId = User.Identity.GetUserId();
            List<Job_Application> Applications = js.Get_Applications_by_User_ID(UserId).ToList();

            return View(Applications);

        }
    }
}
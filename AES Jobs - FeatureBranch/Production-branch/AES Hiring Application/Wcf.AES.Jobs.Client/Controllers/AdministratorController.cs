using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WcfAESJobs.Client.Models;
using WcfAESJobs.AccountLibrary;
using reCAPTCHA.MVC;
using WcfAESJobs.Client.UserService;

namespace WcfAESJobs.Client.Controllers
{
    [AuthorizeRedirect(Roles="SuperUser")]
    public class AdministratorController : Controller
    {
        LoginServiceClient ls = new LoginServiceClient();
        // GET: Administrator
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
  
        public AdministratorController()
        {
        }

        public AdministratorController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index(MessageId? message)
        {
            ViewBag.StatusMessage =
               message == MessageId.RoleChanegSuccess ? "Roles were sucessfully changed"
               : message == MessageId.LocationChangeSuccess ? "Locations were sucessfully changed"
               : "";
            ViewBag.ErrorMessage =
               message == MessageId.InvalidUser? "Only Hiring Managers and Store Managers can be assigned Locations"
               : message == MessageId.NoUser ? "User does not exist"
               : "";

            var model = new AdminViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string Username, ActionType option)
        {
            System.Web.HttpContext.Current.Session["UserName"] = Username;
            if(option == ActionType.Role)
            {
                return RedirectToAction("UpdateRoles");
            }
            if (option == ActionType.Location)
            {
                return RedirectToAction("UpdateLocations");
            }
            else
                return View();      
        }

        public ActionResult UpdateRoles()
        {
            string username = System.Web.HttpContext.Current.Session["UserName"] as string;
            if(username != null)
            {
                ApplicationUser user = UserManager.FindByName(username);

                if (user != null)
                {
                    RoleForm[] form = ls.GetRoleForm(user);
                    return View(form.ToList());
                }
                else
                {
                    ViewBag.StatusMessage = "Cannot Find User";
                    return RedirectToAction("Index", new { message = MessageId.NoUser });
                }    
            }
            else 
            {
                return RedirectToAction("Index");
            }
           
        }

        [HttpPost]
        public ActionResult UpdateRoles(IEnumerable<RoleForm> form)
        {
            string username = System.Web.HttpContext.Current.Session["UserName"] as string;
            ls.AddUserToRoleByForm(username, form.ToArray());
            return RedirectToAction("Index", new { message = MessageId.RoleChanegSuccess });

        }

        public ActionResult UpdateLocations()
        {
            string username = System.Web.HttpContext.Current.Session["UserName"] as string;
            
            if (username != null)
            {
                ApplicationUser user = UserManager.FindByName(username);
            
                // if user exists
                if (user != null)
                {
                    // Must be in store manager
                    if (ls.IsInRole(user, "HiringManager") || ls.IsInRole(user, "StoreManager"))
                    {
                        LocationForm[] form = ls.GetLocationForm(user);
                        return View(form.ToList());
                    }
                    else
                    {
                        return RedirectToAction("Index", new { message = MessageId.InvalidUser });
                    }
                }
                else
                    return RedirectToAction("Index", new { message = MessageId.NoUser });
                
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult UpdateLocations(IEnumerable<LocationForm> form)
        {
            string username = System.Web.HttpContext.Current.Session["UserName"] as string;
            ls.AddUserToLocationByForm(username, form.ToArray());
            return RedirectToAction("Index", new { message = MessageId.LocationChangeSuccess });
        }

        public enum MessageId
        {
            RoleChanegSuccess,
            LocationChangeSuccess,
            NoUser,
            InvalidUser,
            Error
        }

    }
}
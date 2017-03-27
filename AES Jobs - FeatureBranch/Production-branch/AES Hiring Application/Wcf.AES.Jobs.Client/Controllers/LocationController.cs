using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WcfAESJobs.Client.WebService;

namespace WcfAESJobs.Client.Controllers
{
    [AuthorizeRedirect(Roles="SuperUser")]
    public class LocationController : Controller
    {
        JobServiceClient js = new JobServiceClient();
        // GET: Location
        public ActionResult Index()
        {
            List<StoreLocations> locations = js.Get_Store_Location_List().ToList();
            return View(locations);
        }

        // GET: Location/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Location/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Location_Name,Location_City")] StoreLocations location)
        {
            try
            {
                js.Create_Location(location.Location_City, location.Location_Name);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Location/Edit/5
       // [HandleError]
        public ActionResult Edit(int Location_ID)
        {
          
            StoreLocations location = js.Get_Location_By_ID(Location_ID);
            return View(location);
        }

        // POST: Location/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Location_Name,Location_City,Location_ID")] StoreLocations location)
        {
            try
            {
                js.Update_Location(location);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Location/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Location/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

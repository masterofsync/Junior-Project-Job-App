using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using WcfAESJobs.Client.WebService;
using WcfAESJobs.Client.ViewModels;
using WcfAESJobs.Client.Models;
using WcfAESJobs.AccountLibrary;
using Microsoft.AspNet.Identity;
using WcfAESJobs.Client.UserService;


namespace WcfAESJobs.Client.Controllers
{
    [AuthorizeRedirect(Roles="HiringSpecialist, SuperUser, HiringManager, StoreManager")]
    public class JobsPostingController : Controller
    {
        JobServiceClient js = new JobServiceClient();
        LoginServiceClient ls = new LoginServiceClient();
     
        // GET: JobsPosting
        public ActionResult Index(string searchString,string selectedLocation)
        {
            OpenJobs[] All_Jobs;
            List<OpenJobs> Pending_Jobs = new List<OpenJobs>();
            if (User.IsInRole("SuperUser")) 
            {
                All_Jobs = js.Get_Job_Posting_List();
                Pending_Jobs = js.Get_Job_Posting_List_By_User(User.Identity.GetUserId(), PostingStatus.Submitted).ToList();
            }
            else
            {
                All_Jobs = js.Get_Job_Posting_List_By_User(User.Identity.GetUserId(), PostingStatus.Active);
                Pending_Jobs = js.Get_Job_Posting_List_By_User(User.Identity.GetUserId(), PostingStatus.Submitted).ToList();
            }
                

            IEnumerable<OpenJobs> jobList = All_Jobs.ToList();
                        

            var allUniqueLocations = jobList.Select(x => x.Job_Location).Distinct().ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                if (selectedLocation != "All Locations")
                {
                    jobList =
                        jobList.Where(
                            x => x.Job_Location.Contains(selectedLocation) && x.Job_Title.Contains(searchString));
                }
                if (selectedLocation == "All Locations")
                {
                    jobList = jobList.Where(x => x.Job_Title.Contains(searchString));
                }

                //jobList = jobList.Where(s => s.Job_Title.Contains(searchString)
                //                       || s.Job_Location.Contains(searchString));
            }
            else
            {
                if (selectedLocation == null)
                {
                    selectedLocation = "All Locations";
                }

                if (selectedLocation != "All Locations")
                {
                    jobList = jobList.Where(x => x.Job_Location.Contains(selectedLocation));
                }
            }

            JobPostingViewModel finalModel = new JobPostingViewModel {AllJobs = jobList , Locations = allUniqueLocations, PendingJobs = Pending_Jobs };
            
            return View(finalModel);
        }


        // GET: JobsPosting/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OpenJobModel jobModel = new OpenJobModel();
            jobModel.Job = js.Get_Posting_By_ID(id);
            jobModel.ApplicationQuestions = js.getQuestionsByJobID(jobModel.Job.Job_ID, QuestionType.Application).ToList();
            jobModel.PhoneQuestions = js.getQuestionsByJobID(jobModel.Job.Job_ID, QuestionType.PhoneInterview).ToList();
            //OpenJobs job = js.Get_Posting_By_ID(id);
            if (jobModel == null)
            {
                return HttpNotFound();
            }
            return View(jobModel);
        }

        public ActionResult QuestionDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question[] question = js.getQuestionsByJobID(id, QuestionType.Application);

            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }


        // GET: JobsPosting/Create
        public ActionResult Create()
        {
            List<UserService.StoreLocations> temp;
            IEnumerable<WebService.StoreLocations> locations;
            if(User.IsInRole("SuperUser") || User.IsInRole("HiringSpecialist"))
            {
                locations = js.Get_Store_Location_List().ToList();
                locations.OrderByDescending(x => x.Location_Name);
                ViewBag.Location_ID = new SelectList(locations, "Location_ID", "Location_Name");
            }
            else
            {
                temp = ls.GetLocationsByUserID(User.Identity.GetUserId()).ToList();
                temp.OrderByDescending(x => x.Location_Name);
                ViewBag.Location_ID = new SelectList(temp, "Location_ID", "Location_Name");
                
            }
                
            
            /*((from l in js.get()
                                                        select new StoreLocations
                                                        {
                                                            Location_ID = l.Location_ID,
                                                            Location_Name = l.Location_City + " - " + l.Location_Name
                                                        }));*/
            
            ViewBag.Job_ID = new SelectList(js.Get_Job_Template_List(), "Job_ID", "Job_Title");
            ViewBag.Close_Date = new DateTime();
            return View();
        }


        // POST: JobsPosting/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Location_ID, Job_ID, Close_Date")] NewPosting job)
        { 
            try
            {
                job.Status = PostingStatus.Submitted;
                js.Create_Job_Posting(job);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Location_ID = new SelectList(js.Get_Store_Location_List(), "Location_ID", "Location_Name");
                ViewBag.Job_ID = new SelectList(js.Get_Job_Template_List(), "Job_ID", "Job_Title");
                ViewBag.Close_Date = new DateTime();
                return View(job);
            }
        }


        // GET: JobsPosting/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpenJobs job = js.Get_Posting_By_ID(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }


        // POST: JobsPosting/Edit/5
        [HttpPost]
        public ActionResult Edit(OpenJobs job)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: JobsPosting/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpenJobs job = js.Get_Posting_By_ID(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }


        // POST: JobsPosting/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                js.Delete_Job_Posting(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ApproveRequest()
        {
            var jobs = js.Get_Job_Posting_List_By_User(User.Identity.GetUserId(), PostingStatus.Submitted);
            return View(jobs.ToList());
        }

        public ActionResult SubmitApproval(int id, PostingStatus status)
        {
            js.Update_Posting_Status(id, status);
            if(status == PostingStatus.NotActive)
                js.Delete_Job_Posting(id);

            return RedirectToAction("ApproveRequest");
        }
    }
}

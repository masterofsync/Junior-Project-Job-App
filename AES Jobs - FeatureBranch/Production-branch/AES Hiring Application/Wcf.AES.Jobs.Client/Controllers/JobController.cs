using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WcfAESJobs.Client.WebService;
using WcfAESJobs.Client.Models;

namespace WcfAESJobs.Client.Controllers
{

    [AuthorizeRedirect(Roles = "HiringSpecialist, SuperUser")]
    public class JobController : Controller
    {

        JobServiceClient js = new JobServiceClient();

        // GET: Job
        public ActionResult Index()
        {
            JobTemplates[] All_Jobs = js.Get_Job_Template_List();
            IEnumerable<JobTemplates> jobList = All_Jobs.ToList();

            return View(jobList);
        }

        // GET: Job/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTemplates jobTemp = js.Get_JobTemplate_By_ID(id);
            if (jobTemp == null)
            {
                return HttpNotFound();
            }
            return View(jobTemp);
        }

        // GET: Job/Create
        public ActionResult Create()
        {
            JobCreate model = new JobCreate();
            model.ApplicationQuestions = js.Get_Question_Form(null, QuestionType.Application).ToList();
            model.PhoneQuestions = js.Get_Question_Form(null, QuestionType.PhoneInterview).ToList();
            return View(model);
        }

        // POST: Job/Create
        [HttpPost]
        public ActionResult Create(JobCreate jobTemp)
        {
            try
            {
                QuestionForm[] AppQs = jobTemp.ApplicationQuestions.ToArray();
                QuestionForm[] phoneQs = jobTemp.PhoneQuestions.ToArray();
                int jobID = js.Create_Job_Template(jobTemp.template.ToWCF());
                js.Update_Job_Questions_By_Form(jobID, AppQs);
                js.Update_Job_Questions_By_Form(jobID, phoneQs);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Job/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTemplates jobTemp = js.Get_JobTemplate_By_ID(id);
            if (jobTemp == null)
            {
                return HttpNotFound();
            }
            return View(jobTemp);
        }

        // POST: Job/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Job_ID, Job_Title,Job_Description,Job_Qualifications")] JobTemplates job)
        {
            try
            {
                js.Update_Job_Template(job);

                return RedirectToAction("Details", "Job", new { id = job.Job_ID });
            }
            catch
            {
                return View();
            }
        }

        // GET: Job/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTemplates job = js.Get_JobTemplate_By_ID(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Job/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            
            try
            {
                js.Delete_Job_Template(id);
                return RedirectToAction("Index");
            }
            catch
            {
                JobTemplates job = js.Get_JobTemplate_By_ID(id);
                ViewBag.StatusMessage = "WARNING !Cannot delete jobs with current openings";
                if (job == null)
                {
                    return HttpNotFound();
                }
                return View(job);
            }
        }
    }
}

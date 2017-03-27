using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WcfAESJobs.Client.Models;
using WcfAESJobs.Client.WebService;

namespace WcfAESJobs.Client.Controllers
{
    [AuthorizeRedirect(Roles = "HiringSpecialist, SuperUser")]
    public class QuestionController : Controller
    {
        JobServiceClient js = new JobServiceClient();
        // GET: Question
        public ActionResult Index()
        {
            Question[] All_Questions = js.getAllQuestions(QuestionType.Application);
            Question[] PreAppQuestions = js.getAllQuestions(QuestionType.PreApplication);
            Question[] PhoneQuestions = js.getAllQuestions(QuestionType.PhoneInterview);

            //IEnumerable<Question> paQLIst = All_Questions.ToList();
            QuestionView qView = new QuestionView();
            qView.ApplicationQuestions = All_Questions.ToList();
            qView.PreApplicationQuestions = PreAppQuestions.ToList();
            qView.PhoneQuestions = PhoneQuestions.ToList();

            return View(qView);
        }

        // GET: Question/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Question/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Question/Create
        [HttpPost]
        public ActionResult Create(Question question, QuestionSelectType Type)
        {
            try
            {
                js.addNewQuestion(question, (QuestionType)Type);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Question/Create
        /*public ActionResult CreatePreAppQuestion()
        {
            return View();
        }

        // POST: Question/Create
        [HttpPost]
        public ActionResult CreatePreAppQuestion([Bind(Include = "QuestionTitle,FullQuestion")] Question question)
        {
            try
            {
                js.addNewQuestion(question, QuestionType.PreApp);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }*/

        // GET: Question/Create
        public ActionResult AddQuestionToJobByForm(int id, QuestionType QType)
        {
            ViewBag.QuestionID = new SelectList(js.getAllQuestions(QType), "QuestionID", "QuestionTitle");

            ViewBag.Job_ID = id;
            //QuestionIDJobID ids = new QuestionIDJobID();
            //ids.Job_ID = id;

            QuestionForm[] qform = js.Get_Question_Form(id, QType);
            return View(qform.ToList());
        }

        // POST: Question/Create
        [HttpPost]
        public ActionResult AddQuestionToJobByForm(int id, IEnumerable<QuestionForm> qform)
        {
            QuestionForm[] form = qform.ToArray();
            try
            {
                js.Update_Job_Questions_By_Form(id, form);
                return RedirectToAction("Details", "Job", new { id = id });
            }
            catch
            {
                ViewBag.StatusMessage = "WARNING !Cannot delete jobs with current openings";
                return RedirectToAction("AddQuestionToJobByForm", new { id = id });
            }
        }

        // GET: Question/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question questionTemp = js.getQuestionByID(id);
            if (questionTemp == null)
            {
                return HttpNotFound();
            }
            return View(questionTemp);
        }

        // POST: Question/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "QuestionTitle, FullQuestion,QuestionID,C_Answer")] Question question)
        {
            try
            {
                js.Update_Question(question);
                return RedirectToAction("Index", "Question");
            }
            catch
            {
                return View();
            }
        }


        // GET: Question/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Question question = js.getQuestionByID(id);
                if (question == null)
                {
                    return HttpNotFound();
                }
                return View(question);
            }
            catch
            {
                return View();
            }
        }

        // POST: Question/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                js.deleteQuestionByID(id);
                return RedirectToAction("Index");
            }
            catch
            {
                Question job = js.getQuestionByID(id);
                ViewBag.StatusMessage = "WARNING! Cannot delete questions associated with Job Templates";
                if (job == null)
                {
                    return HttpNotFound();
                }
                return View(job);
            }
        }

    }
}

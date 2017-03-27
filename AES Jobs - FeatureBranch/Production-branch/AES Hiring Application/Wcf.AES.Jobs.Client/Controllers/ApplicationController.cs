using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WcfAESJobs.Client.Models;
using WcfAESJobs.Client.WebService;
using Microsoft.AspNet.Identity;

namespace WcfAESJobs.Client.Controllers
{
    public class ApplicationController : Controller
    {
        JobServiceClient js = new JobServiceClient();

        [Authorize]
        public ActionResult Apply(int PID)
        {
            OpenJobs jobPosting = js.Get_Posting_By_ID(PID);
            //string userID = User.Identity.GetUserId();

            
            
            // id = application id
            //Job_Application job_app = js.Get_Job_Application_By_ID(id);
            
            int Job_ID = jobPosting.Job_ID;

            Question[] AppQuestions = js.getQuestionsByJobID(Job_ID, QuestionType.Application);
            Question[] PreAppQuestions = js.getAllQuestions(QuestionType.PreApplication);

            ViewBag.Posting_ID = jobPosting.Posting_ID;

            ApplicationModel qView = new ApplicationModel();

            foreach( Question q in AppQuestions)
            {         
                qView.ApplicationQuestions.Add(new QuestionAnswer{
                    fullQuestion = q.FullQuestion,
                    questionID = q.QuestionID,
                    answer = ""
                });
            }
            foreach (Question q in PreAppQuestions)
            {
                qView.PreApplicationQuestions.Add(new QuestionAnswer
                {
                    fullQuestion = q.FullQuestion,
                    questionID = q.QuestionID, 
                    answer = ""
                });
            }


            return View(qView);

        }

        // POST: Job/Create
        [HttpPost]
        [Authorize]
        public ActionResult Apply(int Posting_ID, ApplicationModel model)
        {
            try
            {
                int ApplicationID = js.Create_Application(User.Identity.GetUserId(), Posting_ID);
                ApplicationModel localModel = model;

                List<Answer> answerList = new List<Answer>();

                foreach (QuestionAnswer qa in localModel.ApplicationQuestions)
                {
                    Answer a = new Answer
                    {
                        Answer_Text = qa.answer,
                        Question_ID = qa.questionID,
                        Application_ID = ApplicationID
                    };
                    answerList.Add(a);
                    
                }

                foreach (QuestionAnswer qa in localModel.PreApplicationQuestions)
                {
                    Answer a = new Answer
                    {
                        Answer_Text = qa.answer,
                        Question_ID = qa.questionID,
                        Application_ID = ApplicationID
                    };
                    answerList.Add(a);

                }

                js.Add_Answer_To_Table(answerList.ToArray());
                js.Grade_Answers(ApplicationID);

                //ViewBag.StatusMessage = "Sucessfully Applied to Job";
                return RedirectToAction("Index", "Applicant", new { Message = MessageId.ApplicationSuccess });
            }
            catch
            {
                return View();
            }
        }

        public enum MessageId
        {
            ApplicationSuccess,
            Error
        }

    }
}
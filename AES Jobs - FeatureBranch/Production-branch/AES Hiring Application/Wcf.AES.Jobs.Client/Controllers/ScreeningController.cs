using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WcfAESJobs.Client.WebService;
using WcfAESJobs.Client.UserService;
using WcfAESJobs.Client.Models;
using System.Threading.Tasks;
using WcfAESJobs.AccountLibrary;

namespace WcfAESJobs.Client.Controllers
{
    [AuthorizeRedirect(Roles = "Screener, SuperUser")]
    public class ScreeningController : Controller
    {
        // GET: Screening

        JobServiceClient js = new JobServiceClient();
        LoginServiceClient ls = new LoginServiceClient();
        
        public ActionResult Index()
        {
            Job_Application[] applications = js.Get_Job_Application_List_By_Stage(AppStages.Submitted);

            return View(applications.ToList());
        }

        public ActionResult PhoneScreen( MessageId? message)
        {

            ViewBag.StatusMessage =
                message == MessageId.ApplicationPassed ? "The applicant was passed on to the interview"
                : message == MessageId.ApplicationFailed ? "The applicant was failed"
                : message == MessageId.CommentsSubmitted ? "Comments were sumbitted"
                : message == MessageId.Error ? "Error processing request"
                : "";
            //Job_Application[] applications = js.Get_Job_Application_List_By_Stage(AppStages.PhoneInterview);
            //Job_Application[] reviewApplications = js.Get_Job_Application_List_By_Stage(AppStages.PhoneInterviewReview);
            PhoneInterviewModel model = new PhoneInterviewModel();
            model.PhoneInterviewReviews = js.Get_Job_Application_List_By_Stage(AppStages.PhoneInterviewReview).ToList();
            model.PhoneInterviews = js.Get_Job_Application_List_By_Stage(AppStages.PhoneInterview).ToList();

            return View(model);
        }

        public ActionResult Grade(int? id)
        {
            ViewBag.AppID = id;
            Question_Answer[] AppQA = js.Get_Answers_By_Application_ID(id, QuestionType.Application);
            Question_Answer[] PreAppQA = js.Get_Answers_By_Application_ID(id, QuestionType.PreApplication);

            Question_Answer[] QA = new Question_Answer[AppQA.Length + PreAppQA.Length];
            Array.Copy(AppQA, QA, AppQA.Length);
            Array.Copy(PreAppQA, 0, QA, AppQA.Length, PreAppQA.Length);

            return View(QA.ToList());
        }

        public ActionResult SubmitGrade(AppStages result, int App_ID)
        {
            try
            {
                if (result == AppStages.Failed)
                {
                    js.Update_Application_Stage(App_ID, AppStages.Failed);

                    return RedirectToAction("Index", "Screening", new { Message = MessageId.ApplicationFailed });
                }
                if(result == AppStages.Interview)
                {
                    js.Update_Application_Stage(App_ID, result);

                    return RedirectToAction("PhoneScreen", "Screening", new { Message = MessageId.ApplicationPassed });
                }
                else
                {
                    js.Update_Application_Stage(App_ID, result);
                    return RedirectToAction("Index", "Screening", new { Message = MessageId.ApplicationPassed });
                }
            }
            catch
            {
                return RedirectToAction("Index", "Screening", new { Message = MessageId.Error });
            }


        }

        public async Task<ActionResult> ConductPhoneScreen(int? id)
        {
            Job_Application application = js.Get_Job_Application_By_ID(id);
            //string userID = User.Identity.GetUserId();



            // id = application id
            //Job_Application job_app = js.Get_Job_Application_By_ID(id);

            int Job_ID = application.JobID;



            Question[] AppQuestions = js.getQuestionsByJobID(Job_ID, QuestionType.PhoneInterview);

            //ViewBag.Posting_ID = jobPosting.Posting_ID;
            
            ScreeningModels sView = new ScreeningModels();
            ApplicationUser user = await ls.FindByIdAsync(application.UserID);
            
            sView.FirstName = user.FirstName;
            sView.LastName = user.LastName;
            sView.PhoneNumber = user.PhoneNumber;
            sView.ApplicationID = application.ID;

            foreach (Question q in AppQuestions)
            {
                sView.PhoneQuestions.Add(new QuestionAnswer
                {
                    fullQuestion = q.FullQuestion,
                    questionID = q.QuestionID,
                    answer = ""
                });
            }


            return View(sView);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ConductPhoneScreen(ScreeningModels model)
        {
            try
            {
                ScreeningModels localModel = model;

                List<Answer> answerList = new List<Answer>();

                foreach (QuestionAnswer qa in localModel.PhoneQuestions)
                {
                    Answer a = new Answer
                    {
                        Answer_Text = qa.answer,
                        Question_ID = qa.questionID,
                        Application_ID = model.ApplicationID
                    };
                    answerList.Add(a);

                }

                js.Add_Answer_To_Table(answerList.ToArray());
                js.Update_Application_Stage(model.ApplicationID, AppStages.PhoneInterviewReview);

                //ViewBag.StatusMessage = "Sucessfully Applied to Job";
                return RedirectToAction("PhoneScreen", "Screening", new { Message = MessageId.CommentsSubmitted });
            }
            catch
            {
                return View();
            }
        }
        public ActionResult ReviewPhoneInterview(int? id)
        {
            ViewBag.AppID = id;
            Question_Answer[] QA = js.Get_Answers_By_Application_ID(id, QuestionType.PhoneInterview);

            return View(QA.ToList());
        }
    }

    public enum MessageId
    {
        
        ApplicationPassed, 
        ApplicationFailed,
        CommentsSubmitted,
        Error
    }
}
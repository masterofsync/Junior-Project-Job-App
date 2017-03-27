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
using Microsoft.AspNet.Identity;

namespace WcfAESJobs.Client.Controllers
{
    [AuthorizeRedirect(Roles = "HiringManager, SuperUser")]
    public class InterviewController : Controller
    {
        // GET: Interview
        JobServiceClient js = new JobServiceClient();
        LoginServiceClient ls = new LoginServiceClient();

        private InterviewModel Get_Interview_Model(int? id)
        {
            InterviewModel ApplicantData = new InterviewModel();
            ApplicantData.JobApplication = js.Get_Job_Application_By_ID(id);
            ApplicantData.ApplicationQuestions = js.Get_Answers_By_Application_ID(id, QuestionType.Application).ToList();
            ApplicantData.PhoneInterviewQuestions = js.Get_Answers_By_Application_ID(id, QuestionType.PhoneInterview).ToList();
            ApplicantData.PreApplicationQuestions = js.Get_Answers_By_Application_ID(id, QuestionType.PreApplication).ToList();
            ApplicantData.Applicant = ls.FindById(ApplicantData.JobApplication.UserID);
            ApplicantData.JobInformation = js.Get_JobTemplate_By_ID(ApplicantData.JobApplication.JobID);

            List<WorkHistory_User> workhist = ls.GetWorkHistoryByUser(ApplicantData.JobApplication.UserID).ToList();
            List<EducationHistory_User> eduhist = ls.GetEducationHistoryByUser(ApplicantData.JobApplication.UserID).ToList();

            ApplicantData.WorkHistory = new List<WorkHistoryModel>();
            ApplicantData.EducationHistory = new List<EducationHistoryModel>();

            if (eduhist != null)
                foreach (var item in eduhist)
                    ApplicantData.EducationHistory.Add(new EducationHistoryModel(item));
   

            if (workhist != null)
                foreach (var item in workhist)
                    ApplicantData.WorkHistory.Add(new WorkHistoryModel(item));


            ApplicantData.InterviewComment = new QuestionAnswer();
            Question interviewQuestion = js.getAllQuestions(QuestionType.InterviewComment).FirstOrDefault();
            ApplicantData.InterviewComment.fullQuestion = interviewQuestion.QuestionTitle;
            ApplicantData.InterviewComment.questionID = interviewQuestion.QuestionID;
            try
            {
                Question_Answer answer = js.Get_Answers_By_Application_ID(id, QuestionType.InterviewComment).FirstOrDefault();
                ApplicantData.InterviewComment.answer = answer.Answer;
            }
            catch
            {
                ApplicantData.InterviewComment.answer = "";
            }
           
            return ApplicantData;
        }

        public ActionResult Index(MessageId? message)
        {
            ViewBag.ReturnUrl = "Index";
            ViewBag.StatusMessage =
                message == MessageId.ApplicationPassed ? "The applicant was Hired!"
                : message == MessageId.ApplicationFailed ? "The applicant was failed"
                : message == MessageId.ApplicationFailed ? "The applicant was placed on the wait list"
                : message == MessageId.CommentsSubmitted ? "Comments were sumbitted"
                : "";

            InterviewIndexModel model = new InterviewIndexModel();
            if(User.IsInRole("SuperUser"))
            {
                model.Interviews = js.Get_Job_Application_List_By_Stage(AppStages.Interview).ToList();
                model.InterviewReviews = js.Get_Job_Application_List_By_Stage(AppStages.InterviewReview).ToList();

            }
            else
            {
                model.Interviews = js.Get_Job_Application_List_By_Stage_Location(AppStages.Interview, User.Identity.GetUserId()).ToList();
                model.InterviewReviews = js.Get_Job_Application_List_By_Stage_Location(AppStages.InterviewReview, User.Identity.GetUserId()).ToList();
            }

            

            return View(model);
        }

        public ActionResult Employees(MessageId? message)
        {
            ViewBag.returnUrl = "Employees";
            ViewBag.StatusMessageRed =
                message == MessageId.ApplicationFailed ? "The applicant was failed"
                : message == MessageId.ApplicantRemoved ? "Applicant was removed from the list"
                : "";
            ViewBag.StatusMessage =
                message == MessageId.ApplicationPassed ? "The applicant was Hired!"
                : message == MessageId.ApplicationWaiting ? "The applicant was placed on the wait list"
                : message == MessageId.CommentsSubmitted ? "Comments were sumbitted"
                : "";


            EmployeeModel model = new EmployeeModel();
            if (User.IsInRole("SuperUser"))
            {
                model.Hired = js.Get_Job_Application_List_By_Stage(AppStages.Hired).ToList();
                model.WaitList = js.Get_Job_Application_List_By_Stage(AppStages.Waiting).ToList();
            }
            else
            {
                model.Hired = js.Get_Job_Application_List_By_Stage_Location(AppStages.Hired, User.Identity.GetUserId()).ToList();
                model.WaitList = js.Get_Job_Application_List_By_Stage_Location(AppStages.Waiting, User.Identity.GetUserId()).ToList();
            }



            return View(model);
        }

        public ActionResult Details(int? id)
        {
            InterviewModel ApplicantData = Get_Interview_Model(id);
            return View(ApplicantData);
        }

        public ActionResult ConductInterview(int? id)
        {
            InterviewModel ApplicantData = Get_Interview_Model(id);
            return View(ApplicantData);
        }

        [HttpPost]
        public ActionResult ConductInterview(InterviewModel Model)
        {
            List<Answer> answerList = new List<Answer>();
            Answer a = new Answer();

            a.Answer_Text = Model.InterviewComment.answer;
            a.Application_ID = Model.JobApplication.ID;
            a.Question_ID = Model.InterviewComment.questionID;

            answerList.Add(a);

            js.Add_Answer_To_Table(answerList.ToArray());
            js.Update_Application_Stage(Model.JobApplication.ID, AppStages.InterviewReview);

            return RedirectToAction("Index", "Interview", new { Message = MessageId.CommentsSubmitted });
        }

        public ActionResult ReviewInterview(int? id, string returnURL)
        {
            ViewBag.ReturnUrl = returnURL;
            InterviewModel ApplicantData = Get_Interview_Model(id);
            return View(ApplicantData);
        }

        public ActionResult SubmitGrade(AppStages result, int App_ID, string returnURL)
        {
            try
            {
                if (returnURL == null)
                    returnURL = "Index";


                if (result == AppStages.Failed)
                {
                    js.Update_Application_Stage(App_ID, AppStages.Failed);


                    return RedirectToAction(returnURL, "Interview", new { Message = MessageId.ApplicationFailed });
                }
                else if (result == AppStages.Hired)
                {
                    js.Update_Application_Stage(App_ID, result);

                    return RedirectToAction(returnURL, "Interview", new { Message = MessageId.ApplicationPassed });
                }
                else if(result == AppStages.Waiting)
                {
                    js.Update_Application_Stage(App_ID, result);

                    return RedirectToAction(returnURL, "Interview", new { Message = MessageId.ApplicationWaiting });
                }

                else if (result == AppStages.Complete)
                {
                    js.Update_Application_Stage(App_ID, result);

                    return RedirectToAction(returnURL, "Interview", new { Message = MessageId.ApplicantRemoved });
                }
                else 
                {
                    return RedirectToAction(returnURL, "Interview", new { Message = MessageId.Error });
                }
            }
            catch
            {
                return RedirectToAction("Index", "Interview", new { Message = MessageId.Error });
            }


        }

        public enum MessageId
        {

            ApplicationPassed,
            ApplicationFailed,
            ApplicationWaiting,
            CommentsSubmitted,
            ApplicantRemoved,
            Error
        }
    }
}
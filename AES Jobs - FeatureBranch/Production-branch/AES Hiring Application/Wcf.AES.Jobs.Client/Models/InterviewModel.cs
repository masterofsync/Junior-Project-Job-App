using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WcfAESJobs.Client.WebService;
using WcfAESJobs.Client.UserService;
using WcfAESJobs.AccountLibrary;

namespace WcfAESJobs.Client.Models
{
    public class InterviewModel
    {
        public ApplicationUser Applicant { get; set; }
        public Job_Application JobApplication { get; set; }
        public List<Question_Answer> ApplicationQuestions { get; set; }
        public List<Question_Answer> PhoneInterviewQuestions { get; set; }
        public List<Question_Answer> PreApplicationQuestions { get; set; }
        public QuestionAnswer InterviewComment { get; set; }
        public JobTemplates JobInformation { get; set; }
        public List<WorkHistoryModel> WorkHistory { get; set; }
        public List<EducationHistoryModel> EducationHistory {get; set;}
    }

    public class InterviewIndexModel
    {
        public InterviewIndexModel() { Interviews = new List<Job_Application>(); InterviewReviews = new List<Job_Application>(); }
        public List<Job_Application> Interviews { get; set; }
        public List<Job_Application> InterviewReviews { get; set; }
    }

    public class EmployeeModel
    {
        public EmployeeModel() { Hired = new List<Job_Application>(); WaitList = new List<Job_Application>(); }
        public List<Job_Application> Hired { get; set; }
        public List<Job_Application> WaitList { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WcfAESJobs.Client.UserService;
using WcfAESJobs.Client.WebService;

namespace WcfAESJobs.Client.Models
{
    public class ScreeningModels
    {
        public ScreeningModels() { PhoneQuestions = new List<QuestionAnswer>(); }
        public int ApplicationID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public List<QuestionAnswer> PhoneQuestions { get; set; }
    }

    public class PhoneInterviewModel
    {
        public PhoneInterviewModel() { PhoneInterviews = new List<Job_Application>(); PhoneInterviewReviews = new List<Job_Application>(); }
        public List<Job_Application> PhoneInterviews { get; set; }
        public List<Job_Application> PhoneInterviewReviews { get; set; }
    }
}
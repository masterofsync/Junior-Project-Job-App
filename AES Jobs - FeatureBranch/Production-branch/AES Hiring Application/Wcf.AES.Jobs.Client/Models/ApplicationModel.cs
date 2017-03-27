using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WcfAESJobs.Client.WebService;

namespace WcfAESJobs.Client.Models
{

    public class ApplicationModel
    {
        public ApplicationModel() { ApplicationQuestions = new List<QuestionAnswer>(); PreApplicationQuestions = new List<QuestionAnswer>(); }
        public List<QuestionAnswer> ApplicationQuestions { get; set; }
        public List<QuestionAnswer> PreApplicationQuestions { get; set; }

    }

    public class QuestionAnswer
    {
        public string fullQuestion { get; set; }
        public int questionID { get; set; }
        public string answer { get; set; }
    }

}
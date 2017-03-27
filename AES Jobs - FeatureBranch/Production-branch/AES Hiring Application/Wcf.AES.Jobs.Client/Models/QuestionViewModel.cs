using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WcfAESJobs.Client.WebService;

namespace WcfAESJobs.Client.Models
{
    public class QuestionView
    {
        public List<Question> ApplicationQuestions;
        public List<Question> PreApplicationQuestions;
        public List<Question> PhoneQuestions;

    }


}
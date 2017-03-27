using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WcfAESJobs.Client.WebService;

namespace WcfAESJobs.Client.Models
{
    public class QuestionCreateViewModel
    {
        public Question question { get; set; }
        public QuestionSelectType Type { get; set; }
        
    }

    public enum QuestionSelectType { PreApplication = 0, Application, PhoneInterview } 
}
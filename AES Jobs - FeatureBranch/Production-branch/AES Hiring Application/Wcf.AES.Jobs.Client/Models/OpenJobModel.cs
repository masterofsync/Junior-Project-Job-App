using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WcfAESJobs.Client.WebService;

namespace WcfAESJobs.Client.Models
{
    public class OpenJobModel
    {
        public OpenJobs Job { get; set; }
        public List<Question> ApplicationQuestions {get; set;}
        public List<Question> PhoneQuestions { get; set; }
    }
}
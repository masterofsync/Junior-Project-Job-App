using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WcfAESJobs.Client.Models;
using WcfAESJobs.Client.WebService;

namespace WcfAESJobs.Client.Models
{
    public class OpenJobsModel
    {
        public string JobDescription { get; set; }
        public DateTime CloseDate { get; set; }
        public int JobId{get; set;}
        public string JobLocation { get; set; }
        public string JobLocationCity { get; set; }
        public string JobQualification { get; set; }
        public string JobTitle { get; set; }
        public int PostingId { get; set; }
    }
}
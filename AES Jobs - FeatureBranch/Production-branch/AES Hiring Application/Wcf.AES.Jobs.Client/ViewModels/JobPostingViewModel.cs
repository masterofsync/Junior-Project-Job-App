using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using WcfAESJobs.Client.WebService;


namespace WcfAESJobs.Client.ViewModels
{

    public class JobPostingViewModel
    {
        public IEnumerable<OpenJobs> AllJobs { get; set; }
        public IEnumerable<OpenJobs> PendingJobs { get; set; }
        public IEnumerable<string> Locations { get; set; }

    }
}
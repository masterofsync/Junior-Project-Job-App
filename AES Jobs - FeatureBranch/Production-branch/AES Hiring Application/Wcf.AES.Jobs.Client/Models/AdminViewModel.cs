using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WcfAESJobs.Client.Models
{
    public enum ActionType
    {
        [Display(Name = "Edit Role")]
        Role,

        [Display(Name = "Edit Location")]
        Location
    }

    public class AdminViewModel
    {
        public AdminViewModel() { Username = ""; option = ActionType.Role; }

        [Display(Name = "Username to Edit")]
        public string Username { get; set; }

        [Display(Name = "Action")]
        public ActionType option { get; set; }
    }
}
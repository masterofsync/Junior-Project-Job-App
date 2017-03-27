using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNet.Identity;

namespace WcfAESJobs.Client.Models
{
    public class ApplicantModel
    {
        
        public string ID { get; set; }
        
        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "Please Enter a name less than 50 characters")]
        public string First_Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Please Enter a name less than 50 characters")]
        [Display(Name = "Last Name")]
        public string Last_Name { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        [Required]
        [Display(Name = "Address")]
        [StringLength(50, ErrorMessage = "Only maximum of 50 characters allowed")]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        [StringLength(50, ErrorMessage = "Only maximum of 50 characters allowed")]
        public string Address2 { get; set; }

        [Required]
        [Display(Name = "City")]
        [StringLength(50, ErrorMessage = "Only maximum of 50 characters allowed")]
        public string City { get; set; }

        [Required]
        [Display(Name = "State")]
        [StringLength(50, ErrorMessage = "Only maximum of 50 characters allowed")]
        public string State { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone")]
        [StringLength(50, ErrorMessage = "Only maximum of 50 characters allowed")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        [StringLength(50, ErrorMessage = "Only maximum of 50 characters allowed")]
        public string Email { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;
using System.Web;
using WcfAESJobs.Client.UserService;

namespace WcfAESJobs.Client.Models
{
    public class WorkHistoryModel
    {
        public WorkHistoryModel() { history = new WorkHistory_User(); history.EndDate = null; StartDate = null; }
        public WorkHistoryModel(WorkHistory_User WHist) { history = WHist; StartDate = history.StartDate; }


        public int Id { get; set; }

        [Required]
        [StringLength(128, ErrorMessage = "Must be less that 128 characters", MinimumLength = 1)]
        public String Title 
        {
            get { return history.Title; }
            set { history.Title = value; }
        }

        [Required]
        [StringLength(128, ErrorMessage = "Must be less that 128 characters", MinimumLength = 1)]
        public String Employer 
        {
            get { return history.Employer; }
            set { history.Employer = value; }
        }

        [Required]
        [StringLength(128, ErrorMessage = "Must be less that 128 characters", MinimumLength = 1)]
        public String Location 
        {
            get { return history.Location; }
            set { history.Location = value; }
        }

        [Required]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> StartDate 
        {
            get; set;
            //get { return history.StartDate.Date; }
            //set { history.StartDate = value; }
        }

        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> EndDate 
        {
            get { return history.EndDate; }
            set { history.EndDate = value; }
        }

        public String Description 
        {
            get { return history.Description; }
            set { history.Description = value; }
        }

        public bool Current 
        {
            get { if (history.Current == 1) return true; else return false; }
            set { if (value == true)history.Current = 1; else history.Current = 0; }
        }
        public WorkHistory_User ToEntity()
        {
            history.StartDate = StartDate ?? new DateTime();
            return history;
        }

        private WorkHistory_User history { get; set; }
  

    }

    public class EducationHistoryModel
    {
        public EducationHistoryModel() { m_education = new EducationHistory_User(); m_education.EndDate = null; StartDate = null; }
        public EducationHistoryModel(EducationHistory_User edu) { m_education = edu; StartDate = m_education.StartDate; }

        public int Id 
        {
            get { return m_education.Id; }
            set { m_education.Id = value; }
        }

        [Required]
        [StringLength(255, ErrorMessage = "Must be less that 255 characters", MinimumLength = 1)]
        public String Institution
        {
            get { return m_education.Institution; }
            set { m_education.Institution = value; }
        }

        [Required]
        [StringLength(255, ErrorMessage = "Must be less that 255 characters", MinimumLength = 1)]
        public String Major
        {
            get { return m_education.Major; }
            set { m_education.Major = value; }
        }

        [Required]
        [StringLength(255, ErrorMessage = "Must be less that 255 characters", MinimumLength = 1)]
        public String EducationLevel
        {
            get { return m_education.EducationLevel; }
            set { m_education.EducationLevel = value; }
        }

        [Required]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
        public Nullable<DateTime> StartDate
        {
            get;
            set;
        }

        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
        public Nullable<DateTime> EndDate 
        {
            get { return m_education.EndDate; }
            set { m_education.EndDate = value; }
        }

        public bool Degree
        {
            get { if (m_education.Degree == 1) return true; else return false ; }
            set { if (value == true)m_education.Degree = 1; else m_education.Degree = 0 ; }
        }

        public EducationHistory_User ToEntity()
        {
            m_education.StartDate = StartDate ?? new DateTime();
            return m_education;
        }
        private EducationHistory_User m_education { get; set; }

    }
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            WorkHistory = new List<WorkHistoryModel>();
            EducationHistory = new List<EducationHistoryModel>();
        }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required]
        [Display(Name = "First Name")]
        public string First_Name { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string Last_Name { get; set; }

        [Required]
        [ValidateYears]
        [Display(Name = "Date of Birth")]
        public Nullable<DateTime> DOB { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Range(10000, 99999, ErrorMessage = "Please enter valid zip code")]
        [Display(Name = "Zip Code")]
        public int? Zip { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public List<WorkHistory_User> ToHistoryEntity()
        {
            List<WorkHistory_User> hist = new List<WorkHistory_User>(); 
            foreach( var item in WorkHistory)
            {
                hist.Add(item.ToEntity());
            }
            return hist;
        }


        public List<WorkHistoryModel> WorkHistory { get; set;}
        public List<EducationHistoryModel> EducationHistory { get; set; }

    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}

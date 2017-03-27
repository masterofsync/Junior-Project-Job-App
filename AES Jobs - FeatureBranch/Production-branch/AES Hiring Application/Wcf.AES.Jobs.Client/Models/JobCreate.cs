using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WcfAESJobs.Client.WebService;
using System.ComponentModel.DataAnnotations;

namespace WcfAESJobs.Client.Models
{
    public class JobCreate
    {
        [Required]
        public JobTemplate_View template { get; set; }
        public List<QuestionForm> ApplicationQuestions { get; set; }
        public List<QuestionForm> PhoneQuestions { get; set; }
    }
    public class JobTemplate_View
    {
        public JobTemplate_View() { m_template = new JobTemplates(); }

        [Required]
        public string title
        {
            get { return m_template.Job_Title; }
            set { m_template.Job_Title = value; }
        }
        [Required]
        public string description
        {
            get { return m_template.Job_Description; }
            set { m_template.Job_Description = value; }
        }
        [Required]
        public string qualifications
        {
            get { return m_template.Job_Qualifications; }
            set { m_template.Job_Qualifications = value; }
        }
        public JobTemplates ToWCF() { return m_template; }
        
        private JobTemplates m_template { get; set; }
    }
}
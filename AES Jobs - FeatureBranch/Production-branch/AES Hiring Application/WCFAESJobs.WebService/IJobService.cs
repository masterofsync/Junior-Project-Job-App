using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFAESJobs.WebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IJobService" in both code and config file together.
    [ServiceContract]
    public interface IJobService
    {
        // GET
        [OperationContract]
        List<OpenJobs> Get_Job_Posting_List();

        [OperationContract]
        List<OpenJobs> Get_Job_Posting_List_By_User(string UserID, PostingStatus status);

        [OperationContract]
        OpenJobs Get_Posting_By_ID(int? value);

        [OperationContract]
        List<StoreLocations> Get_Store_Location_List();

        [OperationContract]
        List<JobTemplates> Get_Job_Template_List();

        [OperationContract]
        JobTemplates Get_JobTemplate_By_ID(int? value);

        [OperationContract]
        List<QuestionForm> Get_Question_Form(int? value, QuestionType qType);

        //[OperationContract]
        //List<Question> Get_PreApp_Questions();

        [OperationContract]
        Job_Application Get_Job_Application_By_ID(int? value);

        [OperationContract]
        List<Job_Application> Get_Job_Application_List_By_Stage(AppStages stage);

        [OperationContract]
        List<Job_Application> Get_Job_Application_List_By_Stage_Location(AppStages stage, string UserID);

        [OperationContract]
        List<Question_Answer> Get_Answers_By_Application_ID(int? Application_ID, QuestionType qType);

        [OperationContract]
        List<Job_Application> Get_Applications_by_User_ID(string Id);

        [OperationContract]
        StoreLocations Get_Location_By_ID(int Id);
        

        // CREATE 
        [OperationContract]
        void Create_Job_Posting(NewPosting posting);

        [OperationContract]
        int Create_Job_Template(JobTemplates newJob);

        [OperationContract]
        int Create_Application(string UserID, int Posting_ID);

        [OperationContract]
        void Create_Location(string Location_City, string Location_Name);


        // DELETE
        [OperationContract]
        void Delete_Job_Posting(int? value);

        [OperationContract]
        void Delete_Job_Template(int? value);


        // QUESTIONS
        [OperationContract]
        List<Question> getQuestionsByJobID(int? JobID, QuestionType qType);

        [OperationContract]
        Question getQuestionByID(int? value);

        [OperationContract]
        void deleteQuestionByID(int? value);

        [OperationContract]
        List<Question> getAllQuestions(QuestionType qType);

        [OperationContract]
        void addNewQuestion(Question question, QuestionType Qtype);

        //[OperationContract]
        //void addNewPreAppQuestion(Question question, QuestionType Qtype);

        [OperationContract]
        void addQuestionToJob(QuestionIDJobID questionIDjobID);

        [OperationContract]
        void removeQuestionFromJob(QuestionIDJobID questionIDjobID);

        // ANSWERS
        [OperationContract]
        void Add_Answer_To_Table(List<Answer> answer);


        // UPDATE
        [OperationContract]
        void Update_Job_Template(JobTemplates jobTemp);

        [OperationContract]
        void Update_Question(Question question);

        [OperationContract]
        void Update_Job_Questions_By_Form(int Job_ID, List<QuestionForm> form);

        [OperationContract]
        void Update_Location(StoreLocations locations);

        [OperationContract]
        void Update_Posting_Status(int PostingID, PostingStatus status);
            
        // Moves application to next stage
        [OperationContract]
        void Update_Application_Stage(int? Application_ID, AppStages stage);

        [OperationContract]
        void Grade_Answers(int ApplicationID);




    }

    [DataContract(IsReference = true)]
    public class OpenJobs
    {
        [DataMember]
        public int Posting_ID { get; set; }
        [DataMember]
        public int Job_ID { get; set; }
        [DataMember]
        public string Job_Title { get; set; }
        [DataMember]
        public string Job_Description { get; set; }
        [DataMember]
        public string Job_Qualification { get; set; }
        [DataMember]
        public string Job_Location_City { get; set; }
        [DataMember]
        public string Job_Location { get; set; }
        [DataMember]
        public DateTime Close_Date { get; set; }
        [DataMember]
        public PostingStatus status { get; set; }
    }


    [DataContract(IsReference = true)]
    public class StoreLocations
    {
        [DataMember]
        public int Location_ID { get; set; }

        [DataMember]
        public string Location_Name { get; set; }

        [DataMember]
        public string Location_City { get; set; }

    }


    [DataContract(IsReference = true)]
    public class JobTemplates
    {
        [DataMember]
        public int Job_ID { get; set; }

        [DataMember]
        public string Job_Title { get; set; }

        [DataMember]
        public string Job_Description { get; set; }

        [DataMember]
        public string Job_Qualifications { get; set; }

        [DataMember]
        public List<Question> Job_Questions { get; set; }

        [DataMember]
        public List<Question> Phone_Questions { get; set; }

    }


    [DataContract(IsReference = true)]
    public class NewPosting
    {
        [DataMember]
        public int Job_ID { get; set; }

        [DataMember]
        public int Location_ID { get; set; }

        [DataMember]
        public DateTime Close_Date { get; set; }

        [DataMember]
        public PostingStatus Status { get; set; }
    }


    [DataContract(IsReference = true)]
    public class Question
    {
        public Question() { QuestionID = 0; QuestionTitle = null; FullQuestion = null; }
        public Question(int id, string title, string question, QuestionType type, CorrectAnswer answer) { QuestionID = id; QuestionTitle = title; FullQuestion = question; Type = type; C_Answer = answer; }

        [DataMember] 
        public int QuestionID { get; set; }

        [DataMember]
        public string QuestionTitle { get; set; }

        [DataMember]
        public string FullQuestion { get; set; }

        [DataMember]
        public QuestionType Type { get; set; }

        [DataMember]
        public CorrectAnswer C_Answer { get; set; }
    }


    [DataContract(IsReference = true)]
    public class JobQuestionList
    {
        [DataMember]
        List<QuestionList> JobQuestions { get; set; }
    }


    [DataContract(IsReference = true)]
    public class QuestionIDJobID
    {
        [DataMember]
        public int QuestionID { get; set; }

        [DataMember]
        public int Job_ID { get; set; }
    }

    [DataContract(IsReference = true)]
    public class QuestionForm
    {
        [DataMember]
        public bool checkedQuestion { get; set; }

        [DataMember]
        public Question question { get; set; }
    }

    [DataContract]
    public class Applicant_User
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string First_Name { get; set; }

        [DataMember]
        public string Last_Name { get; set; }

        [DataMember]
        public DateTime DOB { get; set; }

        [DataMember]
        public string Address1 { get; set; }

        [DataMember]
        public string Address2 { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Email { get; set; }
    }

    [DataContract(IsReference = true)]
    public class Job_Application
    {
        [DataMember]
        public string UserID { get; set; }

        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int JobID { get; set; }

        [DataMember]
        public int PostingID { get; set; }

        [DataMember]
        public string JobTitle { get; set; }

        [DataMember]
        public int ApplicantID { get; set; }

        [DataMember]
        public string ApplicantName { get; set; }

        [DataMember]
        public int Stage { get; set; }

        [DataMember]
        public string StageName { get; set; }

        [DataMember]
        public string StageDescription { get; set; }

        [DataMember]
        public string Location { get; set; }

        
    }

    [DataContract]
    public class Answer
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int Application_ID { get; set; }

        [DataMember]
        public int Question_ID { get; set; }

        [DataMember]
        public string Answer_Text { get; set; }
    }

    [DataContract]
    public class Question_Answer
    {
        [DataMember]
        public int QuestionID { get; set; }

        [DataMember]
        public string Question {get; set;}

        [DataMember]
        public string Answer {get; set;}

        //[DataMember]
        //public string Type { get; set; }
    }

    [DataContract]
    public class Work_History
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string UserID { get; set; }

        [DataMember]
        public string Employer { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }

        [DataMember]
        public bool Current { get; set; }

    }

    //[DataContract]
    //public class Profile
    //{
    //    [DataMember]
    //    public List<Job_Application> CurrentApplications { get; set; }
    //}
    [DataContract]
    public enum AppStages
    {
        [EnumMember]
        Failed = 0,
        [EnumMember]
        Submitted = 1,
        [EnumMember]
        PhoneInterview = 2,
        [EnumMember]
        Interview = 3,
        [EnumMember]
        Hired = 4,
        [EnumMember]
        Waiting = 5,
        [EnumMember]
        PhoneInterviewReview = 6,
        [EnumMember]
        InterviewReview = 7,
        [EnumMember]
        Complete = 8
    }

    [DataContract]
    public enum QuestionType
    {
        [EnumMember(Value = "Pre Application")]
        PreApp = 0,
        [EnumMember(Value = "Application")]
        App = 1,
        [EnumMember(Value= "Phone Interview")]
        Phone = 2,
        [EnumMember(Value = "Interview Comment")]
        Interview = 3
    }

    [DataContract]
    public enum CorrectAnswer
    {
        [EnumMember(Value = "No")]
        No = 0,
        [EnumMember(Value = "Yes")]
        Yes = 1,
        [EnumMember(Value = "None")]
        None = 2,

    }

    [DataContract]
    public enum PostingStatus
    {
        [EnumMember(Value = "NotActive")]
        NotActive = 0,
        [EnumMember(Value = "Active")]
        Active = 1,
        [EnumMember(Value = "Submitted")]
        Submitted = 2,
        [EnumMember(Value = "All")]
        All = 3,

    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace WCFAESJobs.WebService
{
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "JobService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select JobService.svc or JobService.svc.cs at the Solution Explorer and start debugging.
    public class JobService : IJobService
    {
        public const int TRUE = 1;
        public const int FALSE = 0;

        DB_9E5950_aes01Entities2 db = new DB_9E5950_aes01Entities2();

        public void Create_Job_Posting(NewPosting posting)
        {
            using (db)
            {
                
                AvailableJob job = new AvailableJob
                {
                    LocationID = posting.Location_ID,
                    JobID = posting.Job_ID,
                    CloseDate = posting.Close_Date,
                    IsActive = (int)PostingStatus.Submitted
                };

                db.AvailableJobs.Add(job);
                db.SaveChanges();
            }

        }


        public int Create_Job_Template(JobTemplates newJob)
        {
            using (db)
            {
                JobApplication job = new JobApplication
                {
                    Title = newJob.Job_Title,
                    Description = newJob.Job_Description,
                    Qualifications = newJob.Job_Qualifications,
                    IsActive = TRUE
                };

                db.JobApplications.Add(job);
                db.SaveChanges();
                return job.Id;
            }
        }


        public int Create_Application(string User_ID, int Posting_ID)
        {
            using (db)
            {
                /*ApplicantTable newApplicant = new ApplicantTable
                {
                    First_Name = applicant.First_Name,
                    Last_name = applicant.Last_Name,
                    DOB = applicant.DOB,
                    Email = applicant.Email,
                    Address1 = applicant.Address1,
                    Address2 = applicant.Address2,
                    City = applicant.City,
                    State = applicant.State,
                    Phone = applicant.Phone,

                };
                //db.SaveChanges();
                db.ApplicantTables.Add(newApplicant);*/
                Application app = new Application
                {
                    UserID = User_ID,
                    ApplicantID = 29,
                    PostingID = Posting_ID,
                    Stage = 1
                };

                db.Applications.Add(app);
                db.SaveChanges();
                return app.Id;
            }
        }

        public void Create_Location(string Location_City, string Location_Name)
        {
            using (db)
            {
                try
                {
                    Location newLocation = new Location { City = Location_City, LocationName = Location_Name };
                    db.Locations.Add(newLocation);
                    db.SaveChanges();
                }
                catch
                {
                    throw new Exception("Error Creating Location");
                }
            }
        }


        public void Delete_Job_Posting(int? value)
        {

            if (value != null)
            {
                try
                {
                    db = new DB_9E5950_aes01Entities2();
                    AvailableJob job = db.AvailableJobs.Find(value);
                    db.AvailableJobs.Remove(job);
                    db.SaveChanges();
                }
                catch
                {
                    db = new DB_9E5950_aes01Entities2();
                    AvailableJob job = db.AvailableJobs.Find(value);
                    job.IsActive = FALSE;
                    db.SaveChanges();
                }
            }
            else
                throw new Exception("Jobs not Removed!!!");

        }


        public void Delete_Job_Template(int? value)
        {
            if (value != null)
            {
                try
                {
                    db = new DB_9E5950_aes01Entities2();
                    JobApplication job = db.JobApplications.Find(value);
                    db.JobApplications.Remove(job);
                    db.SaveChanges();
                }
                catch
                {
                    db = new DB_9E5950_aes01Entities2();
                    JobApplication job = db.JobApplications.Find(value);
                    job.IsActive = FALSE;
                    db.SaveChanges();
                }
            }
            else
                throw new Exception("Jobs not Removed!!!");


        }


        public List<OpenJobs> Get_Job_Posting_List()
        {
            using (db)
            {

                IQueryable<OpenJobs> Job_Entity = db.AvailableJobs.Where(x => x.IsActive == TRUE && x.CloseDate >= DateTime.Now).Select(x => new OpenJobs
                {
                    Posting_ID = x.PostingID,
                    Job_ID = x.JobID,
                    Job_Title = x.JobApplication.Title,
                    Job_Description = x.JobApplication.Description,
                    Job_Qualification = x.JobApplication.Qualifications,
                    Job_Location = x.Location.LocationName,
                    Job_Location_City = x.Location.City,
                    Close_Date = x.CloseDate,
                    status = (PostingStatus)x.IsActive

                });

                try
                {
                    return Job_Entity.ToList();
                }
                catch
                {
                    throw new Exception("No Avaliable Jobs");
                }

            }
        }

        public List<OpenJobs> Get_Job_Posting_List_By_User(string UserID, PostingStatus status)
        {
            List<StoreLocations> locations = GetLocationsByUserID(UserID);
            if (status == PostingStatus.All)
                return Get_Job_Posting_List();

            IEnumerable<int> LocationIDs;
            

                var db2 = new DB_9E5950_aes01Entities2();
                if(IsSuperUser(UserID) || IsInRole(UserID, "HiringSpecialist"))
                {
                    LocationIDs = db2.Locations.Select(l => l.Id).ToList();
                }
                else 
                    LocationIDs = db2.Locations.Where(x => x.AspNetUsers.Any(u => u.Id == UserID)).Select(l => l.Id).ToList();

            using (db = new DB_9E5950_aes01Entities2())
            {
                IQueryable<OpenJobs> postingEntity = db.AvailableJobs
                    .Where(x => x.IsActive == (int)status && x.CloseDate >= DateTime.Now)
                    .Where(l => LocationIDs.Contains(l.Location.Id))
                    .Select(x => new OpenJobs
                    {
                        Posting_ID = x.PostingID,
                        Job_ID = x.JobID,
                        Job_Title = x.JobApplication.Title,
                        Job_Description = x.JobApplication.Description,
                        Job_Qualification = x.JobApplication.Qualifications,
                        Job_Location = x.Location.LocationName,
                        Job_Location_City = x.Location.City,
                        Close_Date = x.CloseDate,
                        status = status                        
                    });

                if (postingEntity != null)
                    return postingEntity.ToList();
                else
                    throw new Exception("No Applications");

            }
        }


        public List<JobTemplates> Get_Job_Template_List()
        {
            using (db)
            {
                IQueryable<JobTemplates> Job_Entity = db.JobApplications.Where(x => x.IsActive == TRUE).Select(x => new JobTemplates
                {
                    Job_ID = x.Id,
                    Job_Title = x.Title

                });

                if (Job_Entity != null)
                    return Job_Entity.ToList();
                else
                    throw new Exception("Job Template not found");

            }
        }


        public OpenJobs Get_Posting_By_ID(int? value)
        {
            using (db)
            {
                var JobEntity = (from j in db.AvailableJobs where j.PostingID == value select j).FirstOrDefault();
                if (JobEntity != null)
                    return Translate_JobEntity_to_Job(JobEntity);
                else
                    throw new Exception("Invalid id");

            }

        }


        public JobTemplates Get_JobTemplate_By_ID(int? value)
        {
            using (db)
            {
                var JobEntity = (from j in db.JobApplications where j.Id == value select j).FirstOrDefault();
                if (JobEntity != null)
                    return Translate_JobEntity_to_Job(JobEntity);
                else
                    throw new Exception("Invalid id");
            }
        }


        public List<QuestionForm> Get_Question_Form(int? value, QuestionType qType)
        {
            using (DB_9E5950_aes01Entities2 db = new DB_9E5950_aes01Entities2())
            {
                
                var qList = db.QuestionLists.Where(x => x.Type == qType.ToString()).ToList().Select(x => new QuestionForm
                {
                    question = new Question(x.Id, x.Title, x.Question, qType, 0),
                    checkedQuestion = false
                }).ToList();

                //List<QuestionForm> qList = questionEntity.ToList();
                if(value == null)
                {
                    return qList;
                }

                List<Question> JobQlist = getQuestionsByJobID(value, qType);
                foreach (QuestionForm item in qList)
                {
                    foreach (Question q in JobQlist)
                    {
                        if (q.QuestionID == item.question.QuestionID)
                            item.checkedQuestion = true;
                    }

                }
                //List<QuestionForm> qList = questionEntity.ToList();
                return qList;
            }
        }


        public List<StoreLocations> Get_Store_Location_List()
        {
            using (db)
            {
                IQueryable<StoreLocations> storeEntity = db.Locations.Select(x => new StoreLocations
                {
                    Location_ID = x.Id,
                    Location_Name = x.LocationName,
                    Location_City = x.City


                });
                if (storeEntity != null)
                {
                    storeEntity.OrderByDescending(x => x.Location_Name);
                    return storeEntity.ToList();
                }
                    
                    
                else
                    throw new Exception("Invalid Location id");
            }


        }

        // Gets Job Application info given the application ID.
        public Job_Application Get_Job_Application_By_ID(int? value)
        {
            using (db)
            {

                var job_entity = (from j in db.Applications where j.Id == value select j).FirstOrDefault();
                if (job_entity != null)
                {
                    Job_Application job_app = new Job_Application
                    {
                        ID = job_entity.Id,
                        ApplicantID = 0,
                        PostingID = job_entity.PostingID,
                        UserID = job_entity.UserID,
                        Location = job_entity.AvailableJob.Location.City + " - " + job_entity.AvailableJob.Location.LocationName
                                                
                        //Stage = app.stage
                    };
                    var temp = Get_Posting_By_ID(job_app.PostingID);
                    job_app.JobID = temp.Job_ID;
                    return job_app;
                }
                else
                    throw new Exception("Invalid id");
            }
            
        }


        public List<Question> getQuestionsByJobID(int? JobID, QuestionType qType)
        {
            using (DB_9E5950_aes01Entities2 db = new DB_9E5950_aes01Entities2())
            {
                IQueryable<Question> questionEntity = db.QuestionLists.Where
                    (x => x.Type == qType.ToString() 
                     && x.JobApplications.Any(c => c.Id == JobID)).Select(x => new Question
                {
                    QuestionID = x.Id,
                    QuestionTitle = x.Title,
                    FullQuestion = x.Question,
                    Type = qType

                }); ;
                //IQueryable<Question> = questionEntity.Where(x => x.JobApplications.Any(c=> c.Id == JobID))
                    
                    
                /*IQueryable<Question> query = from questions in db.QuestionLists
                                             where questions.JobApplications.Any(c => c.Id == JobID) 
                                             where q
                                             select new Question()
                                             {
                                                 QuestionID = questions.Id,
                                                 QuestionTitle = questions.Title,
                                                 FullQuestion = questions.Question,
                                                 Type = qType
                                             };*/

                return questionEntity.ToList();

            }
        }


        public Question getQuestionByID(int? value)
        {
            using (db)
            {
                var questionEntity = (from j in db.QuestionLists where j.Id == value select j).FirstOrDefault();
                if (questionEntity != null)
                {
                    Question q = new Question();
                    q.QuestionID = questionEntity.Id;
                    q.QuestionTitle = questionEntity.Title;
                    q.FullQuestion = questionEntity.Question;

                    if(questionEntity.Type == QuestionType.PreApp.ToString())
                    {
                        if (questionEntity.MultipleChoiceAnswer.CorrectAnswer == "Yes")
                            q.C_Answer = CorrectAnswer.Yes;
                        else if (questionEntity.MultipleChoiceAnswer.CorrectAnswer == "No")
                            q.C_Answer = CorrectAnswer.No;
                        else
                            q.C_Answer = CorrectAnswer.None;
                    }
                    
                    return q;
                }
                else
                    throw new Exception("Invalid id");
            }
        }


        public void deleteQuestionByID(int? value)
        {
            using (db)
            {
                try
                {
                    QuestionList question = db.QuestionLists.Find(value);
                    db.QuestionLists.Remove(question);
                    db.SaveChanges();
                }
                catch
                {
                    db = new DB_9E5950_aes01Entities2();
                    QuestionList q = db.QuestionLists.Find(value);
                    q.IsActive = FALSE;
                    db.SaveChanges();
                }


            }
        }


        public List<Question> getAllQuestions(QuestionType qType)
        {
            using (db)
            {
                IQueryable<Question> questionEntity = db.QuestionLists.Where(x => x.Type == qType.ToString() && x.IsActive == TRUE).Select(x => new Question
                {
                    QuestionID = x.Id,
                    QuestionTitle = x.Title,
                    FullQuestion = x.Question,
                    Type = qType

                });

                if (questionEntity != null)
                    return questionEntity.ToList();
                else
                    throw new Exception("No Questions");
            }
        }


        /*public List<Question> Get_PreApp_Questions()
        {
            using (db)
            {
                IQueryable<Question> questionEntity = db.QuestionLists.Where(x => x.Type == "PREAPP" && x.IsActive == TRUE).Select(x => new Question
                {
                    QuestionID = x.Id,
                    QuestionTitle = x.Title,
                    FullQuestion = x.Question,
                    Type = QuestionType.PreApp
                });

                if (questionEntity != null)
                    return questionEntity.ToList();
                else
                    throw new Exception("No Questions");
            }
        }*/


        // Gets A list of job Applications of the same stage
        public List<Job_Application> Get_Job_Application_List_By_Stage(AppStages stage)
        {
            using (db)
            {
                IQueryable<Job_Application> applicationEntity = db.Applications.Where(x => x.Stage == (int)stage ).Select(x => new Job_Application
                {
                    ID = x.Id,
                    ApplicantName = x.AspNetUser.FirstName  + " " + x.AspNetUser.LastName,
                    //JobID = x.JobID,
                    JobTitle = x.AvailableJob.JobApplication.Title,
                    ApplicantID = 0,
                    UserID = x.UserID,
                    //ApplicantName = x.ApplicantTable.First_Name + " " + x.ApplicantTable.Last_name,
                    Stage = x.Stage,
                    Location = x.AvailableJob.Location.City + " - " + x.AvailableJob.Location.LocationName
                });

                if (applicationEntity != null)
                    return applicationEntity.ToList();
                else
                    throw new Exception("No Applications");
            }
        }

        public List<Job_Application> Get_Job_Application_List_By_Stage_Location(AppStages stage, string User_ID)
        {
            IEnumerable<int> LocationIDs;
            using (db = new DB_9E5950_aes01Entities2())
            {
                var db2 = new DB_9E5950_aes01Entities2();
                //List<StoreLocations> locations = Get_Store_Location_List();
                LocationIDs = db2.Locations.Where(x => x.AspNetUsers.Any(u => u.Id == User_ID)).Select(l => l.Id).ToList();
            

                IQueryable<Job_Application> applicationEntity = db.Applications
                    .Where(x => x.Stage == (int)stage)
                    .Where(l => LocationIDs.Contains(l.AvailableJob.Location.Id))
                    .Select(x => new Job_Application
                {
                    ID = x.Id,
                    ApplicantName =  x.AspNetUser.FirstName + " " + x.AspNetUser.LastName,
                    //JobID = x.JobID,
                    JobTitle = x.AvailableJob.JobApplication.Title,
                    ApplicantID = 0,
                    UserID = x.UserID,
                    //ApplicantName = x.ApplicantTable.First_Name + " " + x.ApplicantTable.Last_name,
                    Stage = x.Stage,
                    Location = x.AvailableJob.Location.City + " - " + x.AvailableJob.Location.LocationName
                });

                if (applicationEntity != null)
                    return applicationEntity.ToList();
                else
                    throw new Exception("No Applications");
            }
        }

        public List<Question_Answer> Get_Answers_By_Application_ID(int? Application_ID, QuestionType qType)
        {
            using (db)
            {
                IQueryable<Question_Answer> answerEntity = db.AnswerTables.Where(x => x.ApplicationID == Application_ID && x.QuestionList.Type == qType.ToString()).Select(x => new Question_Answer
                {
                    QuestionID = x.QuestionID,
                    Answer = x.Answer,
                    Question = x.QuestionList.Question
                });

                if (answerEntity != null)
                    return answerEntity.ToList();
                else
                    throw new Exception("Error geting answers");
            }
        }

        public List<Job_Application> Get_Applications_by_User_ID(string UserId)
        {
            using (db)
            {
                try
                {
                    IQueryable<Job_Application> applicationEntity = db.Applications.Where(x => x.AspNetUser.Id == UserId).Select(x => new Job_Application
                    {
                        UserID = x.UserID,
                        Location = x.AvailableJob.Location.LocationName,
                        Stage = x.StageTable.Id,
                        JobTitle = x.AvailableJob.JobApplication.Title,
                        StageName = x.StageTable.StageName,
                        StageDescription = x.StageTable.StageDescription
                    });

                    return applicationEntity.ToList();
                }
                catch
                {
                    throw new Exception("Error geting Applications");
                }
               
            }
        }

        public StoreLocations Get_Location_By_ID(int Id)
        {
            using (db)
            {
                try
                {
                    var location = (from x in db.Locations where x.Id == Id select x).FirstOrDefault();
                    if(location != null)
                    {
                        StoreLocations loc = new StoreLocations
                        {
                            Location_City = location.City,
                            Location_Name = location.LocationName,
                            Location_ID = location.Id
                        };
                        return loc;
                    }
                    else
                        throw new Exception("Error geting Location");

                }
                catch
                {
                    throw new Exception("Error geting Location");
                }
            }
        }

        public void addNewQuestion(Question question, QuestionType Qtype)
        {
            using (db)
            {
                QuestionList ql = new QuestionList
                {
                    Title = question.QuestionTitle,
                    Question = question.FullQuestion,
                    Id = question.QuestionID,
                    IsActive = TRUE,
                    Type = Qtype.ToString()

                };
                db.QuestionLists.Add(ql);

                if(Qtype == QuestionType.PreApp && question.C_Answer != CorrectAnswer.None)
                {
                    int questionID = ql.Id;
                    MultipleChoiceAnswer answer = new MultipleChoiceAnswer
                    {
                        CorrectAnswer = question.C_Answer.ToString()
                    };
                    db.MultipleChoiceAnswers.Add(answer);
                }
             
                db.SaveChanges();
            }

        }

        /*public void addNewPreAppQuestion(Question question, QuestionType Qtype)
        {
            using (db)
            {
                QuestionList ql = new QuestionList
                {
                    Title = question.QuestionTitle,
                    Question = question.FullQuestion,
                    Id = question.QuestionID,
                    IsActive = TRUE,
                    Type = "PREAPP"
                };
                db.QuestionLists.Add(ql);
                db.SaveChanges();
            }
        }*/

        public void addQuestionToJob(QuestionIDJobID questionIDjobID)
        {
            using (DB_9E5950_aes01Entities2 db = new DB_9E5950_aes01Entities2())
            {
                //var jobEntity = (from p in db.JobApplications where p.Id == questionIDjobID.Job_ID select p).FirstOrDefault();
                var questionEntity = (from p in db.QuestionLists where p.Id == questionIDjobID.QuestionID select p).FirstOrDefault();

                var query =
                    from ja in db.JobApplications
                    where ja.Id == questionIDjobID.Job_ID
                    select ja;

                foreach (JobApplication japp in query)
                {
                    if (japp.Id == questionIDjobID.Job_ID)
                        japp.QuestionLists.Add(questionEntity);
                }

                db.SaveChanges();

            }
        }


        public void removeQuestionFromJob(QuestionIDJobID questionIDjobID)
        {
            using (DB_9E5950_aes01Entities2 db = new DB_9E5950_aes01Entities2())
            {

                var questionEntity = (from p in db.QuestionLists where p.Id == questionIDjobID.QuestionID select p).FirstOrDefault();

                var query =
                    from ja in db.JobApplications
                    where ja.Id == questionIDjobID.Job_ID
                    select ja;

                foreach (JobApplication japp in query)
                {
                    if (japp.Id == questionIDjobID.Job_ID)
                        japp.QuestionLists.Remove(questionEntity);
                }

                db.SaveChanges();

            }

        }

        public void Add_Answer_To_Table(List<Answer> AnswerList)
        {
            using (DB_9E5950_aes01Entities2 db = new DB_9E5950_aes01Entities2())
            {
                try
                {
                    AnswerTable entry;
                    foreach (Answer a in AnswerList)
                    {
                        entry = new AnswerTable
                        {
                            ApplicationID = a.Application_ID,
                            QuestionID = a.Question_ID,
                            Answer = a.Answer_Text
                        };
                        db.AnswerTables.Add(entry);
                        db.SaveChanges();
                    }
                    
                }
                catch
                {
                    throw new Exception("Error adding to database");
                }

            }
        }

        public void Update_Job_Template(JobTemplates jobTemp)
        {
            using (db)
            {
                JobApplication ja = (from j in db.JobApplications where j.Id == jobTemp.Job_ID select j).FirstOrDefault();
                ja.Qualifications = jobTemp.Job_Qualifications;
                ja.Description = jobTemp.Job_Description;
                ja.Title = jobTemp.Job_Title;
                db.SaveChanges();
            }
        }

        public void Update_Question(Question question)
        {
            using (db)
            {
                QuestionList ja = (from j in db.QuestionLists where j.Id == question.QuestionID select j).FirstOrDefault();
                ja.Title = question.QuestionTitle;
                ja.Question = question.FullQuestion;

                if(ja.Type == QuestionType.PreApp.ToString() && question.C_Answer != CorrectAnswer.None)
                {
                    ja.MultipleChoiceAnswer.CorrectAnswer = question.C_Answer.ToString();
                }

                db.SaveChanges();
            }
        }


        public void Update_Job_Questions_By_Form(int Job_ID, List<QuestionForm> formList)
        {
            using (db)
            {
                foreach (QuestionForm form in formList)
                {
                    QuestionIDJobID QJ = new QuestionIDJobID();
                    QJ.QuestionID = form.question.QuestionID;
                    QJ.Job_ID = Job_ID;

                    if (form.checkedQuestion == true)
                        addQuestionToJob(QJ);
                    else if (form.checkedQuestion == false)
                        removeQuestionFromJob(QJ);
                }
            }
        }

        public void Update_Application_Stage(int? Application_ID, AppStages stage)
        {
            using (db = new DB_9E5950_aes01Entities2())
            {
                try
                {
                    Application app = (from j in db.Applications where j.Id == Application_ID select j).FirstOrDefault();
                    app.Stage = (int)stage;

                    db.SaveChanges();
                }
                catch
                {
                    throw new Exception("Application Update Failed");
                }
                
            }
        }

        public void Grade_Answers(int ApplicationID)
        {
            
            List<Question_Answer> PreAppAnswers = Get_Answers_By_Application_ID(ApplicationID, QuestionType.PreApp);
            bool pass = true;
            foreach(Question_Answer PreAppQA in PreAppAnswers)
            {
                using (db = new DB_9E5950_aes01Entities2())
                {
                    var query = db.MultipleChoiceAnswers.Where(a => a.QuestionID == PreAppQA.QuestionID).Single();
                    string correctAnswer = query.CorrectAnswer;
                        
                        //(from a in db.MultipleChoiceAnswers where a.QuestionID == PreAppQA.QuestionID select a.CorrectAnswer).ToString();
                    //string temp = correctAnswer;
                    if (PreAppQA.Answer != correctAnswer)
                        pass = false;
                }
            }

            if(pass != true)
            {
                Update_Application_Stage(ApplicationID, AppStages.Failed);
            }
            
        }

        public void Update_Location(StoreLocations locations)
        {
            using (db)
            {
                try
                {
                    Location loc = db.Locations.Where(x => x.Id == locations.Location_ID).FirstOrDefault();
                    loc.LocationName = locations.Location_Name;
                    loc.City = locations.Location_City;
                    db.SaveChanges();
                }
                catch
                {
                    throw new Exception("Location Update Failed");
                }
            }
        }

        public void Update_Posting_Status(int PostingID, PostingStatus status)
        {
            using(db)
            {
                try
                {
                    var job = db.AvailableJobs.Where(x => x.PostingID == PostingID).FirstOrDefault();
                    job.IsActive = (int)status;
                    db.SaveChanges();
                }
                catch
                {
                    throw new Exception("Posting Update Failed");
                }
            }
        }


        private OpenJobs Translate_JobEntity_to_Job(AvailableJob jobentity)
        {
            OpenJobs job = new OpenJobs();
            job.Posting_ID = jobentity.PostingID;
            job.Job_ID = jobentity.JobID;
            job.Job_Title = jobentity.JobApplication.Title;
            job.Job_Description = jobentity.JobApplication.Description;
            job.Job_Qualification = jobentity.JobApplication.Qualifications;
            job.Job_Location = jobentity.Location.LocationName;
            job.Close_Date = jobentity.CloseDate;
            job.Job_Location_City = jobentity.Location.City;
            return job;
        }


        private JobTemplates Translate_JobEntity_to_Job(JobApplication jobentity)
        {
            JobTemplates job = new JobTemplates();
            job.Job_ID = jobentity.Id;
            job.Job_Title = jobentity.Title;
            job.Job_Description = jobentity.Description;
            job.Job_Qualifications = jobentity.Qualifications;
            job.Job_Questions = getQuestionsByJobID(jobentity.Id, QuestionType.App);
            job.Phone_Questions = getQuestionsByJobID(jobentity.Id, QuestionType.Phone);
            return job;
        }

        private List<StoreLocations> GetLocationsByUserID(string UserID)
        {
            AspNetUser user = db.AspNetUsers.Where(x => x.Id == UserID).FirstOrDefault();
            List<StoreLocations> loc = user.Locations.Select(x => new StoreLocations
                {
                    Location_ID = x.Id,
                    Location_Name = x.City + " - " + x.LocationName,
                    Location_City = x.City
                }).ToList();

            return loc;
        }

        private bool IsSuperUser(String UserID)
        {
            using (db = new DB_9E5950_aes01Entities2())
            {
                AspNetUser UserEntity = db.AspNetUsers.Where(c => c.Id == UserID).FirstOrDefault();
                IList<string> RoleList;
                RoleList = UserEntity.AspNetRoles.Select(s => s.Name).ToList();

                if( RoleList.Contains("SuperUser"))
                    return true;
                else return false;

            }
            

        }
        private bool IsInRole(String UserID, string role)
        {
            using (db = new DB_9E5950_aes01Entities2())
            {
                AspNetUser UserEntity = db.AspNetUsers.Where(c => c.Id == UserID).FirstOrDefault();
                IList<string> RoleList;
                RoleList = UserEntity.AspNetRoles.Select(s => s.Name).ToList();

                if (RoleList.Contains(role))
                    return true;
                else return false;

            }


        }



    }
}

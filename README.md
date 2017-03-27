Software Engineering Junior Team Project By:

Bikesh Maharjan  	- Architect/QA Lead

Zaid Rashid 		    - Scrum Master

Ryan Kennell		    - Product Owner 


The following are the Use Cases implemented in this web app using:
-	C# ASP .NET MVC and MVVM pattern.
-	WCF
-	HTML5, CSS3, Bootstrap,
-	JavaScript, jQuery
-	Continuous Integration and some Unit Testing
 

Groomed Use Cases     

Sprint 1:         

As an applicant I want to be able to see job postings and apply for them, so I can submit an application
-	An applicant at the kiosk should first be shown the list of job postings. Initially, it would be all postings at current location, but the applicant may change location and sort by job titles. 

As an applicant I want to see and answer the posted questions so i can complete the application.
-	After choosing the desired job to apply for, the kiosk will display required questions for the applicant to answer before submitting the application. 

As a hiring manager I want to post a job so that the opening is available on the kiosk.
-	After approval from the store manager, the hiring manager will be able to post a job for their position. They will select the job title from the available set of predetermined titles and the location. 

As a hiring specialist I want to enter questions and qualifications so the applicant can answer them.
-	For each original job title created, the hiring specialist will specify questions that should be asked on the application for the given job title.
 
As a hiring specialist I want to be able to read and grade responses to the written questions so pass the application to next level. 
-	Once the applicant has filled out the application and answered the questions, the hiring specialist or screener will review the questions and be able to pass them onto the next level.



 

Sprint 2 and 3: 

Epic 1: Applicant Accounts/Permissions

As an applicant I want to be able to create a username and password so I can log in to my user account. 
-	When an applicant clicks the “apply” button for a job, they will be prompted to create an account. If they already have one, they will be prompted to login in order to apply. An account will store their address, name, and other generic information.  

As an applicant I want to be able to see the status of my application so I can check on it.
-	When an applicant is logged in to their account, they will be able to view the status of previous applications they have applied for. 

As an applicant I want to be able to edit my profile so my information is up to date.
-	When logged in, an applicant can update and change information including name or address information.

Epic 2: Admin Accounts/Permissions

As a hiring manager I don’t want the screener or hiring specialist to modify any of the answered questions.
-	Once any answers or notes are submitted to the application, they cannot be changed.

As a hiring manager I want to have full access to the application process so I can decide who will be accepted.
-	When an application is moved onto stage 3 (in person interview), a hiring manager will receive all information in the applicant including answers to application questions and phone screening notes. 

As a hiring specialist, I want to be able to edit the list of locations so jobs can be posted for that store.
-	The hiring specialist, or somebody in HQ, will be able to modify the list of stores, to account for new stores and closing stores. 

Epic 3: Screening

As a screener in HR I want to be able conduct an interview so I can pass the application to the hiring manager.
-	Once an application has been passed onto stage 2 (the answers have been graded), the screener will be able to conduct a phone interview then be able to pass/fail the application.
As a screener, I want to be able to record notes or interview responses so the hiring manager can see them.
-	During the phone interview, the screener will record notes/answers to the phone interview questions.

As a hiring manager, I want to be able to conduct an in-person interview so I can decide if the applicant is good for the job.
-	Once an application is passed onto stage 3 (applicant has passed the phone interview), a hiring manager will be able to conduct the in person interview.

As a hiring manager, I want to be able to record notes or interview responses so they can be referred to later.
-	The person conducting the interview will be able to record interview notes that are kept on record and can be referenced later. 

As a hiring manager I want to be able to pass/fail/hire applicants so they can complete the interview process.
-	Once the interview is complete, the hiring manager will have the option to fail, pass (save applicant for later), or hire the applicant in the system.

Epic 4: Maintaining questions and jobs. (HQ personnel)

As a hiring specialist I want to enter questions for the phone interview so the screener knows what questions to ask.
-	For each original job title created, the hiring specialist will specify questions that should be asked during the phone interview for the given job title.

As a store manager, I want to be able to approve requests for new jobs put in by the hiring manager so they can be put in the kiosk. 
-	When a job is needed, a hiring manager will be able to submit posting requests to the store manager. The store manager has the final say whether or not the job is posted to the kiosk. 

Epic 5: Other
As a hiring manager, I want to only see applications for my location(s) so other stores can't hire or fire my applicants. 

As an IT (SuperUser) person, I want to add people and remove people to roles the the user can have the correct permissions.
-	When AES requests a new user with permissions, the IT user should be able to add the user to the specific role with the designated permissions.

As an applicant, I want to enter my work history so the interviewers and screeners can look at it when they see my application
-	Upon registering, a user will be asked to add work history and education history to their profile.
-	As a hiring manager I want to see a list of the applicant that I hired
-	-when the hiring manager clicks on hire button the hired applicant data should move to a list where all the hired applicants should be grouped in.

As a hiring specialist I want the system to verify the basic qualifications.
-	When an applicant applies for a job, they will conduct an automated questionnaire that will be graded by the system.  

As a hiring specialist, I want the system to automatically grade responses to certain questions so that I don’t have to screen them.
-	When an application is submitted, the system will grade responses that are previously specified (Pre-application Questions) by the Hiring specialist.




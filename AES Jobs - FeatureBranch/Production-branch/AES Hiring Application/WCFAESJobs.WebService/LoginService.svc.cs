using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WcfAESJobs.AccountLibrary;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WCFAESJobs.WebService
{
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LoginService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select LoginService.svc or LoginService.svc.cs at the Solution Explorer and start debugging.
    public class LoginService : ILoginService
    {
        private DB_9E5950_aes01Entities2 database = new DB_9E5950_aes01Entities2();

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            ApplicationUser user = new ApplicationUser();
            try
            {
                AspNetUser UserEntity = this.database.AspNetUsers.Where(c => c.UserName == userName).FirstOrDefault();
                return Task.FromResult(ConvertUserEntityToAplicationUser(UserEntity));
            }
            catch
            {
                return Task.FromResult(new ApplicationUser { UserName = userName });
            }

        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            AspNetUser UserEntity = this.database.AspNetUsers.Where(c => c.Id == userId).FirstOrDefault();
            var RolesEntitiy = UserEntity.AspNetRoles;
            ICollection<IdentityUserRole> roles;
            roles = UserEntity.AspNetRoles.Select(x => new IdentityUserRole
            {
                RoleId = x.Id,
                UserId = UserEntity.Id
            }).ToList();
            ApplicationUser user = ConvertUserEntityToAplicationUser(UserEntity);
            return Task.FromResult(user);
        }

        public Task CreateAsync(ApplicationUser user)
        {

            this.database.AspNetUsers.Add(ConvertApplicationUserToUserEntity(user));
            return this.database.SaveChangesAsync();
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            AspNetUser UserEntity = this.database.AspNetUsers.Where(c => c.Email == email).FirstOrDefault();
            return Task.FromResult(ConvertUserEntityToAplicationUser(UserEntity));
        }

        public IList<string> GetRolesAsync(ApplicationUser user)
        {
            AspNetUser UserEntity = this.database.AspNetUsers.Where(c => c.Id == user.Id).FirstOrDefault();
            IList<string> RoleList;
            return RoleList = UserEntity.AspNetRoles.Select(s => s.Name).ToList();

        }

        public Task UpdateAsync(ApplicationUser user)
        {

            AspNetUser AUser = (from x in database.AspNetUsers
                                where x.Id == user.Id
                                select x).First();
            AUser.Email = user.Email;
            AUser.PasswordHash = user.PasswordHash;
            AUser.SecurityStamp = user.SecurityStamp;
            AUser.PhoneNumber = user.PhoneNumber;
            AUser.Address1 = user.Address1;
            AUser.Address2 = user.Address2;
            AUser.City = user.City;
            AUser.State = user.State;
            AUser.zip = user.Zip;
            AUser.EmailConfirmed = user.EmailConfirmed;
            AUser.FirstName = user.FirstName;
            AUser.LastName = user.LastName;

            return this.database.SaveChangesAsync();
        }


        private ApplicationUser ConvertUserEntityToAplicationUser(AspNetUser user)
        {
            if (user == null)
                return null;
            ApplicationUser AppUser = new ApplicationUser
            {
                UserName = user.UserName,
                Id = user.Id,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                SecurityStamp = user.SecurityStamp,
                PhoneNumber = user.PhoneNumber,
                Address1 = user.Address1,
                Address2 = user.Address2,
                City = user.City,
                State = user.State,
                Zip = user.zip,
                EmailConfirmed = user.EmailConfirmed,
                FirstName = user.FirstName,
                LastName = user.LastName,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEndDateUtc = user.LockoutEndDateUtc,
                AccessFailedCount = user.AccessFailedCount,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,

            };
            /*AppUser.UserName = user.UserName;
            AppUser.Id = user.Id;
            AppUser.Email = user.Email;
            AppUser.PasswordHash = user.PasswordHash;
            AppUser.SecurityStamp = user.SecurityStamp;
            AppUser.PhoneNumber = user.PhoneNumber;
            AppUser.Address1 = user.Address1;
            AppUser.Address2 = user.Address2;
            AppUser.DOB*/
            return AppUser;
        }
        private AspNetUser ConvertApplicationUserToUserEntity(ApplicationUser user)
        {
            if (user == null)
                return null;
            AspNetUser userEntity = new AspNetUser
            {
                UserName = user.UserName,
                Id = user.Id,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                SecurityStamp = user.SecurityStamp,
                PhoneNumber = user.PhoneNumber,
                Address1 = user.Address1,
                Address2 = user.Address2,
                City = user.City,
                State = user.State,
                zip = user.Zip,
                EmailConfirmed = user.EmailConfirmed,
                FirstName = user.FirstName,
                LastName = user.LastName,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEndDateUtc = user.LockoutEndDateUtc,
                AccessFailedCount = user.AccessFailedCount,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled
            };
            /*
            userEntity.Id = user.Id;
            userEntity.Email = user.Email;
            userEntity.EmailConfirmed = user.EmailConfirmed;
            userEntity.PasswordHash = user.PasswordHash;
            userEntity.SecurityStamp = user.SecurityStamp;
            userEntity.Address1 = user.Address1;
            userEntity.Address2 = user.Address2;
            userEntity.City = user.City;
            userEntity.zip = user.Zip;
            userEntity.State = user.State;
            userEntity.FirstName = user.FirstName;
            userEntity.LastName = user.LastName;
            userEntity.PhoneNumber = user.PhoneNumber;
            userEntity.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            userEntity.LockoutEnabled = user.LockoutEnabled;
            userEntity.UserName = user.UserName;
             * */

            return userEntity;
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string role)
        {
            IList<string> roles = GetRolesAsync(user);

            if (roles.Contains(role))
                return Task.FromResult(true);
            else return Task.FromResult(false);

        }

        public List<RoleForm> GetRoleForm(ApplicationUser user)
        {
            AspNetUser UserEntity = this.database.AspNetUsers.Where(c => c.Id == user.Id).FirstOrDefault();
            IEnumerable<URole> UserRoleList = UserEntity.AspNetRoles.Select(x => new URole
            {
                ID = x.Id,
                Title = x.Name
            });

            List<RoleForm> AllRoles = this.database.AspNetRoles.Select(x => new RoleForm
            {
                ischecked = false,
                role = new URole{ ID = x.Id, Title = x.Name}
                
            }).ToList();

            foreach (RoleForm item in AllRoles)
            {
                foreach (URole r in UserRoleList)
                {
                    if (r.ID == item.role.ID)
                        item.ischecked = true;
                }

            }

            return(AllRoles);

        }

        public void AddUserToRoleByForm(string UserID, List<RoleForm> role_form)
        {
            using(var db = new DB_9E5950_aes01Entities2())
            {
                foreach (RoleForm form in role_form)
                {
                    if (form.ischecked == true)
                        AddUserToRole(UserID, form.role.ID);
                    else if (form.ischecked == false)
                        RemoveUserFromRole(UserID, form.role.ID);
                }
            }

        }

        public void AddUserToRole(String UserID, String roleID)
        {
            using (var db = new DB_9E5950_aes01Entities2())
            {
                var roleEntity = (from p in db.AspNetRoles where p.Id == roleID select p).FirstOrDefault();
                var userEntity = (from ja in db.AspNetUsers where ja.UserName == UserID select ja).FirstOrDefault();
                if (roleEntity.Id == roleID)
                {
                    userEntity.AspNetRoles.Add(roleEntity);
                    db.SaveChanges();

                }

             }
            
        }

        public void RemoveUserFromRole(String UserID, String roleID)
        {
            using (var db = new DB_9E5950_aes01Entities2())
            {
                var roleEntity = (from p in db.AspNetRoles where p.Id == roleID select p).FirstOrDefault();

                var userEntity = (from ja in db.AspNetUsers where ja.UserName == UserID select ja).FirstOrDefault();

                if(roleEntity.Id == roleID)
                {
                    userEntity.AspNetRoles.Remove(roleEntity);
                    db.SaveChanges();

                }
                
                
                //foreach (AspNetUser u in userEntity)
                 //   if (u.UserName == UserID)
                  //      u.AspNetRoles.Remove(roleEntity);
                
                
            }

   
        }

        public List<LocationForm> GetLocationForm(ApplicationUser user)
        {
            AspNetUser UserEntity = this.database.AspNetUsers.Where(c => c.Id == user.Id).FirstOrDefault();
            IEnumerable<ULocation> UserLocationList = UserEntity.Locations.Select(x => new ULocation
            {
                ID = x.Id,
                Title = x.City + " - " + x.LocationName
            });

            List<LocationForm> AllLocations = this.database.Locations.Select(x => new LocationForm
            {
                ischecked = false,
                location = new ULocation { ID = x.Id, Title = x.City + " - " + x.LocationName }

            }).ToList();

            foreach (LocationForm item in AllLocations)
            {
                foreach (ULocation r in UserLocationList)
                {
                    if (r.ID == item.location.ID)
                        item.ischecked = true;
                }

            }

            return (AllLocations);

        }

        public void AddUserToLocationByForm(string UserID, List<LocationForm> Location_form)
        {
            using (var db = new DB_9E5950_aes01Entities2())
            {
                foreach (LocationForm form in Location_form)
                {
                    if (form.ischecked == true)
                        AddUserToLocation(UserID, form.location.ID);
                    else if (form.ischecked == false)
                        RemoveUserFromLocation(UserID, form.location.ID);
                }
            }
            
        }

        private void AddUserToLocation(String UserID, int LocationID)
        {
            using (var db = new DB_9E5950_aes01Entities2())
            {
                var locEntity = (from p in db.Locations where p.Id == LocationID select p).FirstOrDefault();

                var userEntity =
                    from ja in db.AspNetUsers
                    where ja.UserName == UserID
                    select ja;

                foreach (AspNetUser u in userEntity)
                    if (u.UserName == UserID)
                        u.Locations.Add(locEntity);

                db.SaveChanges();
            }

        }

        private void RemoveUserFromLocation(String UserID, int LocationID)
        {
            using (var db = new DB_9E5950_aes01Entities2())
            {
                var locEntity = (from p in db.Locations where p.Id == LocationID select p).FirstOrDefault();

                var userEntity =
                    from u in db.AspNetUsers
                    where u.UserName == UserID
                    select u;

                foreach (AspNetUser u in userEntity)
                    if (u.UserName == UserID)
                        u.Locations.Remove(locEntity);

                db.SaveChanges();
            }


        }

        public List<StoreLocations> GetLocationsByUserID(string UserID)
        {
            AspNetUser user = this.database.AspNetUsers.Where(x => x.Id == UserID).FirstOrDefault();
            List<StoreLocations> loc = user.Locations.Select(x => new StoreLocations
                {
                    Location_ID = x.Id,
                    Location_Name = x.City + " - " + x.LocationName,
                    Location_City = x.City
                }).ToList();
            return loc;
        }


        public List<WorkHistory_User> GetWorkHistoryByUser(string UserID)
        {
            List<WorkHistory_User> work = this.database.WorkHistories.Where(x=>x.AspNetUser.Id == UserID).Select(x=> new WorkHistory_User
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    StartDate = x.StartDate,
                    EndDate = (DateTime)x.EndDate,
                    Employer = x.Employer,
                    Location = x.Location,
                    Current =x.Current
                }
                ).ToList();

            return (work);
        }


        public List<EducationHistory_User> GetEducationHistoryByUser(string UserID)
        {
            List<EducationHistory_User> education = this.database.Educations.Where(x => x.AspNetUser.Id == UserID).Select(x => new EducationHistory_User
            {
                Id = x.Id,
                Institution = x.Institution,
                Major = x.Major,
                EducationLevel = x.EducationLevel,
                StartDate = x.StartDate,
                EndDate = (DateTime)x.EndDate,
                Degree = (int)x.Degree
            }).ToList();

            return (education);
        }


        public void AddEducationHistory(string UserEmail, List<EducationHistory_User> education)
        {
            using (this.database)
            {
                AspNetUser UserEntity = this.database.AspNetUsers.Where(c => c.UserName == UserEmail).FirstOrDefault();

                foreach (EducationHistory_User e in education)
                {
                    Education edu = new Education();
                    edu = new Education
                    {
                        UserID = UserEntity.Id,
                        Institution = e.Institution,
                        Major = e.Major,
                        EducationLevel = e.EducationLevel,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        Degree = e.Degree,
                        //AspNetUser = UserEntity
                    };

                    this.database.Educations.Add(edu);
                }

                this.database.SaveChanges();
            }

        }

        public void AddWorkHistory(string UserEmail, List<WorkHistory_User> work)
        {
            using (var db = new DB_9E5950_aes01Entities2())
            {
                AspNetUser UserEntity = db.AspNetUsers.Where(c => c.UserName == UserEmail).FirstOrDefault();
                foreach (WorkHistory_User x in work)
                {
                    WorkHistory wor = new WorkHistory();
                    wor = new WorkHistory
                    {
                        UserID = UserEntity.Id,
                        Title = x.Title,
                        Description = x.Description,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        Employer = x.Employer,
                        Location = x.Location,
                        Current = x.Current,
                        //AspNetUser = UserEntity
                    };

                    db.WorkHistories.Add(wor);
                }

                db.SaveChanges();
            }
            
        }


        public void DeleteWorkHistoryByID(int Id)
        {
            using(var db = new DB_9E5950_aes01Entities2())
            {
                WorkHistory work = db.WorkHistories.Where(x => x.Id == Id).FirstOrDefault();
                db.WorkHistories.Remove(work);
                db.SaveChanges();
            }
            
        }


        public void DeleteEducationHistoryByID(int Id)
        {
            using(var db = new DB_9E5950_aes01Entities2())
            {
                Education edu = db.Educations.Where(x => x.Id == Id).FirstOrDefault();
                db.Educations.Remove(edu);
                db.SaveChanges();
            }
            
        }

        public void UpdateEducationHistory(List<EducationHistory_User> EduHist, string UserID)
        {
            AspNetUser UserEntity = this.database.AspNetUsers.Where(c => c.Id == UserID).FirstOrDefault();

            var db = new DB_9E5950_aes01Entities2();
            IEnumerable<Education> EduEntity = db.Educations.Where(c => c.UserID == UserID);
            if(EduEntity.Count() > 0)
            foreach (var item in EduEntity)
                DeleteEducationHistoryByID(item.Id);

            //this.database.SaveChanges();

            if(EduHist.Count() > 0)
                AddEducationHistory(UserEntity.UserName, EduHist);
        }

        public void UpdateWorkHistory(List<WorkHistory_User> WorkHist, string UserID)
        {
            AspNetUser UserEntity = this.database.AspNetUsers.Where(c => c.Id == UserID).FirstOrDefault();

            var db = new DB_9E5950_aes01Entities2();
            IEnumerable<WorkHistory> WorkEntity = db.WorkHistories.Where(c => c.UserID == UserID);
            if (WorkEntity.Count() > 0)
                foreach (var item in WorkEntity)
                    DeleteWorkHistoryByID(item.Id);

            //this.database.SaveChanges();

            if(WorkHist.Count() > 0)
                AddWorkHistory(UserEntity.UserName, WorkHist);
        }

    }
}

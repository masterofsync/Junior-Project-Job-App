using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WcfAESJobs.AccountLibrary;

namespace WCFAESJobs.WebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ILoginService" in both code and config file together.

    [ServiceContract]
    public interface ILoginService
    {
        [OperationContract]
        Task<ApplicationUser> FindByIdAsync(string userId);

        [OperationContract]
        Task<ApplicationUser> FindByNameAsync(string userName);

        [OperationContract]
        Task CreateAsync(ApplicationUser user);

        [OperationContract]
        Task<ApplicationUser> FindByEmailAsync(string email);

        [OperationContract]
        IList<string> GetRolesAsync(ApplicationUser user);

        [OperationContract]
        Task UpdateAsync(ApplicationUser user);

        [OperationContract]
        Task<bool> IsInRoleAsync(ApplicationUser user, string role);

        [OperationContract]
        List<RoleForm> GetRoleForm(ApplicationUser user);

        [OperationContract]
        void AddUserToRoleByForm(string UserID, List<RoleForm> form);

        [OperationContract]
        List<LocationForm> GetLocationForm(ApplicationUser user);

        [OperationContract]
        void AddUserToLocationByForm(string UserID, List<LocationForm> form);

        [OperationContract]
        List<StoreLocations> GetLocationsByUserID(string UserID);

        [OperationContract]
        List<WorkHistory_User> GetWorkHistoryByUser(string UserID);

        [OperationContract]
        List<EducationHistory_User> GetEducationHistoryByUser(string UserID);

        [OperationContract]
        void AddEducationHistory(string UserID, List<EducationHistory_User> education);

        [OperationContract]
        void AddWorkHistory(string UserID, List<WorkHistory_User> work);

        [OperationContract]
        void DeleteWorkHistoryByID(int Id);

        [OperationContract]
        void DeleteEducationHistoryByID(int Id);

        [OperationContract]
        void UpdateEducationHistory(List<EducationHistory_User> EduHist, string UserID);

        [OperationContract]
        void UpdateWorkHistory(List<WorkHistory_User> WorkHist, string UserID);
    }


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
public class RoleForm
{
    [DataMember]
    public bool ischecked { get; set; }

    [DataMember]
    public URole role { get; set; }
}

[DataContract(IsReference = true)]
public class URole
{
    [DataMember]
    public string ID { get; set; }

    [DataMember]
    public String Title { get; set; }
}


[DataContract(IsReference = true)]
public class LocationForm
{
    [DataMember]
    public bool ischecked { get; set; }

    [DataMember]
    public ULocation location { get; set; }
}

[DataContract(IsReference = true)]
public class ULocation
{
    [DataMember]
    public int ID { get; set; }

    [DataMember]
    public String Title { get; set; }
}

[DataContract(IsReference = true)]
public class WorkHistory_User
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public String Title { get; set; }

        [DataMember]
        public String Employer { get; set; }

        [DataMember]
        public String Location { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime? EndDate { get; set; }

        [DataMember]
        public String Description { get; set; }

        [DataMember]
        public int Current { get; set; }
  

    }

[DataContract(IsReference = true)]
public class EducationHistory_User
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public String Institution { get; set; }

        [DataMember]
        public String Major { get; set; }

        [DataMember]
        public String EducationLevel { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime? EndDate { get; set; }

        [DataMember]
        public int Degree { get; set; }

    }



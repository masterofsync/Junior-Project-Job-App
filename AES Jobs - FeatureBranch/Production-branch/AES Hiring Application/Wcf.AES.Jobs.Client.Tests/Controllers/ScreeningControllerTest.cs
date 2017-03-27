using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WcfAESJobs.Client.Controllers;
using WcfAESJobs.Client.WebService;

namespace Wcf.AES.Jobs.Client.Tests.Controllers
{
    [TestClass]
    public class ScreeningControllerTest
    {
        // GET: Screening

        JobServiceClient js = new JobServiceClient();

        [TestMethod]
        public void Index()
        {
            // Arrange
            ScreeningController controller = new ScreeningController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PhoneScreen()
        {
           
        }

        [TestMethod]
        public void Grade(int? id)
        {
            
        }

        [TestMethod]
        public void SubmitGrade(string result, int App_ID)
        {
          
          

        }
    }
}
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WcfAESJobs.Client.Controllers;
using WCFAESJobs.WebService;

namespace Wcf.AES.Jobs.Client.Tests.Controllers
{
    [TestClass()]
    public class LocationControllerTests
    {
        [TestMethod()]
        public void IndexTest()
        {
            // Arrange
            LocationController controller = new LocationController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void DetailsTest()
        {

        }

        [TestMethod()]
        public void CreateTest()
        {

        }

        [TestMethod()]
        public void CreateTest1()
        {

        }

        [TestMethod()]
        public void EditTest()
        {

        }

        [TestMethod()]
        public void EditTest1()
        {

        }

        [TestMethod()]
        public void DeleteTest()
        {

        }

        [TestMethod()]
        public void DeleteTest1()
        {

        }
    }
}

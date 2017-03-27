using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WcfAESJobs.Client.Controllers;

namespace Wcf.AES.Jobs.Client.Tests.Controllers
{
    [TestClass()]
    public class QuestionControllerTest
    {
        [TestMethod()]
        public void IndexTest()
        {
            // Arrange
            QuestionController controller = new QuestionController();

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
        public void AddQuestionToJobByFormTest()
        {

        }

        [TestMethod()]
        public void AddQuestionToJobByFormTest1()
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

        [TestMethod()]
        public void IndexTest1()
        {

        }

        [TestMethod()]
        public void PhoneScreenTest()
        {

        }

        [TestMethod()]
        public void GradeTest()
        {

        }

        [TestMethod()]
        public void SubmitGradeTest()
        {

        }

        [TestMethod()]
        public void ConductPhoneScreenTest()
        {

        }

        [TestMethod()]
        public void ConductPhoneScreenTest1()
        {

        }

        [TestMethod()]
        public void ReviewPhoneInterviewTest()
        {

        }
    }
}


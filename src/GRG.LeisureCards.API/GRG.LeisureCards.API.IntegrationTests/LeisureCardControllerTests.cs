using System;
using GRG.LeisureCards.WebAPI.ClientContract;
using NUnit.Framework;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class LeisureCardControllerTests : ControllerTests
    {
        [Test]
        public void Registration_Ok()
        {
            RegistrationTest("Unregistered", "Ok");
        }
        [Test]
        public void Registration_Ok_Admin()
        {
            RegistrationTest("Admin", "Ok");
        }

        [Test]
        public void Registration_CardAlreadyRegistered()
        {
            RegistrationTest("Registered1", "Ok");
        }

        [Test]
        public void Registration_CardSuspended()
        {
            RegistrationTest("Cancelled", "CardSuspended");
        }

        [Test]
        public void Registration_CardNotFound()
        {
            RegistrationTest("xxx", "CodeNotFound");
        }

        public void RegistrationTest(string code, string expectedStatus)
        {
            ISession session;
            var response = LoginService.Login(code, out session);

            Assert.AreEqual(expectedStatus, response.Status);

            if(code == "Admin")
                Assert.IsTrue(response.SessionInfo.IsAdmin);
        }

        [Test]
        public void Update()
        {
            Assert.AreEqual(1, AdminSession.GetLeisureCardService().Update("Registered1", DateTime.Now, false).CardsUpdated);
        }

        [Test]
        public void GetSessionInfoTest()
        {
            Assert.IsNotNull(UserSession.GetLeisureCardService().GetSessionInfo().CardRenewalDate);
        }

        [Test]
        public void GetAdminSessionInfoTest()
        {
            Assert.IsTrue(AdminSession.GetLeisureCardService().GetSessionInfo().IsAdmin);
        }
       
        [Test]
        public void CardGenerationTest()
        {
            var result = AdminSession.GetLeisureCardService().GenerateCards("TEST", 10, 12);
            
            Assert.AreEqual("TEST", result.CardGenerationLog.Ref);
            Assert.IsTrue(result.Success);
        }
    }
}

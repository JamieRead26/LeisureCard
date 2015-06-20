﻿using System;
using GRG.LeisureCards.WebAPI.ClientContract;
using GRG.LeisureCards.WebAPI.Model;
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
        public void Registration_Ok_PopupNotMandatory()
        {
            var response = RegistrationTest("PopupNotMandatory", "Ok");

            Assert.IsTrue(response.DisplayMemberLoginPopup);
            Assert.IsFalse(response.MemberLoginPopupAcceptanceMandatory);
        }

        [Test]
        public void Registration_Ok_PopupMandatory()
        {
            var response = RegistrationTest("PopupMandatory", "Ok");

            Assert.IsTrue(response.DisplayMemberLoginPopup);
            Assert.IsTrue(response.MemberLoginPopupAcceptanceMandatory);

            UserSession.GetLeisureCardService().AcceptTerms();
        }

        [Test]
        public void Registration_InactiveClient()
        {
            RegistrationTest("InactiveClient", "ClientInactive");
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

        public LeisureCardRegistrationResponse RegistrationTest(string code, string expectedStatus)
        {
            ISession session;
            var response = LoginService.Login(code, out session);

            Assert.AreEqual(expectedStatus, response.Status);

            if(code == "Admin")
                Assert.IsTrue(response.SessionInfo.IsAdmin);

            return response;
        }

        [Test]
        [Ignore("Works in app but in test env has error to do with null renewalDate in message? FIX PENDING")]
        public void Update()
        {
            var response = AdminSession.GetLeisureCardService().Update("Registered1", DateTime.Now, false);

            Assert.AreEqual(1, response.CardsUpdated);
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
            var result = AdminSession.GetLeisureCardService().GenerateCards("TEST", 10, 12, "GRG");
            
            Assert.AreEqual("TEST", result.CardGenerationLog.Ref);
            Assert.IsTrue(result.Success);
        }
    }
}

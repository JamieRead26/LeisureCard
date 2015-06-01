using System;
using System.Collections.Generic;
using System.Linq;
using GRG.LeisureCards.WebAPI.Model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class ReportsControllerTests : ControllerTests
    {
        [Test]
        public void GetLoginHistory()
        {
            var history = AdminSession.GetReportsService().GetloginHistory(new DateTime(2000, 1, 1), new DateTime(2001, 1, 1));
            Assert.IsTrue(history.Count()>1);
            Assert.IsTrue(history.FirstOrDefault().LoginDateTime>history.LastOrDefault().LoginDateTime);
            Assert.IsFalse(string.IsNullOrWhiteSpace(history.FirstOrDefault().LeisureCardCode));
        }

        [Test]
        public void GetSelectedOfferHistory()
        {
            var history = AdminSession.GetReportsService()
                .GetSelectedOfferHistory(new DateTime(2000, 1, 1), new DateTime(2001, 1, 1));

            Assert.IsTrue(history.Count() > 1);
            Assert.IsTrue(history.FirstOrDefault().SelectedDateTime > history.LastOrDefault().SelectedDateTime);
            Assert.IsFalse(string.IsNullOrWhiteSpace(history.FirstOrDefault().LeisureCardCode));
        }

        [Test]
        public void GetCardActivationHistory()
        {
            var history = AdminSession.GetReportsService().GetCardActivationHistory(new DateTime(2000, 1, 1), new DateTime(2020, 1, 1));

            Assert.IsTrue(history.Count() > 1);
            Assert.IsTrue(history.FirstOrDefault().RegistrationDate > history.LastOrDefault().RegistrationDate);
        }
    }
}

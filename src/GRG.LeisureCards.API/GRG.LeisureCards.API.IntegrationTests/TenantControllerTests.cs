using System.Linq;
using GRG.LeisureCards.WebAPI.Model;
using NUnit.Framework;

namespace GRG.LeisureCards.API.IntegrationTests
{
    [TestFixture]
    public class TenantControllerTests : ControllerTests
    {
        [Test]
        public void CRUD()
        {
            var service = AdminSession.GetTenantService();

            Assert.IsTrue(service.GetAll().Any());

            var tenant = new Tenant {Key = "123"};
            service.Save(tenant);

            Assert.IsFalse(service.GetAll().First(x=>x.Key=="123").Active);

            tenant.Active = true;
            service.Update(tenant);

            tenant = service.GetAll().First(x => x.Key == "123");

            Assert.IsTrue(tenant.Active);
            Assert.AreEqual(0, tenant.UrnCount);

        }
    }
}

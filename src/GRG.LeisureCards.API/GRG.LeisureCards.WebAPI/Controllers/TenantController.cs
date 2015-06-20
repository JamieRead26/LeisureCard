using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.WebAPI.Filters;
using GRG.LeisureCards.WebAPI.Model;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    [RoutePrefix("Tenant")]
    public class TenantController : ApiController
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly ILeisureCardRepository _leisureCardRepository;

        public TenantController(
            ITenantRepository tenantRepository,
            ILeisureCardRepository leisureCardRepository)
        {
            _tenantRepository = tenantRepository;
            _leisureCardRepository = leisureCardRepository;
        }

        [HttpGet]
        [Route("GetAll")]
        public List<Tenant> GetAll()
        {
            return _tenantRepository.GetAll().Select(Mapper.Map<Tenant>).ToList();
        }

        [HttpGet]
        [Route("Get/{key}")]
        public Tenant Get(string key)
        {
            var tenant = Mapper.Map<Tenant>(_tenantRepository.Get(key));

            tenant.UrnCount = _leisureCardRepository.CountUrns(key);

            return tenant;
        }

        [SessionAuthFilter(true)]
        [HttpPost]
        [Route("Update")]
        public void Update(Tenant tenant)
        {
            _tenantRepository.Update(Mapper.Map<DomainModel.Tenant>(tenant));
        }

        [SessionAuthFilter(true)]
        [HttpPost]
        [Route("Save")]
        public void Save(Tenant tenant)
        {
            _tenantRepository.Save(Mapper.Map<DomainModel.Tenant>(tenant));
        }
    }
}
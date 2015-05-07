using System.Collections.Generic;
using System.Web.Http;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.WebAPI.Filters;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    [SessionAuthFilter]
    public class TwoForOneController : ApiController
    {
        private readonly ITwoForOneRepository _repository;

        public TwoForOneController(ITwoForOneRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("TwoForOne/GetAll")]
        public IEnumerable<TwoForOneOffer> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Web.Http;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.WebAPI.Filters;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    [AdminSessionAuthFilter]
    public class LoginHistoryController : ApiController
    {
        private readonly ILeisureCardUsageRepository _leisureCardUsageRepository;

        public LoginHistoryController(ILeisureCardUsageRepository leisureCardUsageRepository)
        {
            _leisureCardUsageRepository = leisureCardUsageRepository;
        }

        [HttpGet]
        [Route("LoginHistory/Get/{count}/{toId}")]
        public IEnumerable<LeisureCardUsage> Get(int count, int toId)
        {
            return _leisureCardUsageRepository.Get(count, toId);
        }

        [HttpGet]
        [Route("LoginHistory/GetForCard/{cardId}/{count}/{toId}")]
        public IEnumerable<LeisureCardUsage> GetForCard(string cardId, int count, int toId)
        {
            return _leisureCardUsageRepository.Get(cardId, count, toId);
        }
    }
}
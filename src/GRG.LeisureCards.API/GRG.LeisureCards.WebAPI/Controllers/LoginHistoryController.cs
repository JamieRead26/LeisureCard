using System.Collections;
using System.Collections.Generic;
using System.Web.Http;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.WebAPI.Filters;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    [SessionAuthFilter(true)]
    public class LoginHistoryController : ApiController
    {
        private readonly ILeisureCardUsageRepository _leisureCardUsageRepository;

        public LoginHistoryController(ILeisureCardUsageRepository leisureCardUsageRepository)
        {
            _leisureCardUsageRepository = leisureCardUsageRepository;
        }

        /// <summary>
        /// Pagable login history in reverse chronalogical order.  Pass 0 for toId to get the latest history.
        /// </summary>
        /// <param name="count">Number of records to return</param>
        /// <param name="toId">The max id to return.  When paging set this to the last id of the previous list return else set to 0 for most recent history.</param>
        /// <returns></returns>
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
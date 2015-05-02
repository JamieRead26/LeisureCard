using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Persistence;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    public class RedLetterController : ApiController
    {
        private readonly IRedLetterProductRepository _redLetterProductRepository;

        public RedLetterController(IRedLetterProductRepository redLetterProductRepository)
        {
            _redLetterProductRepository = redLetterProductRepository;
        }

        [HttpGet]
        [Route("RedLetter/Find/{keyword}")]
        public List<RedLetterProductSummary> Find(string keyword)
        {
            return _redLetterProductRepository.Find(keyword).Select(p => new RedLetterProductSummary(p)).ToList();
        }

        [HttpGet]
        [Route("RedLetter/Get/{id}")]
        public RedLetterProduct Get(int id)
        {
            var result =  _redLetterProductRepository.Get(id);

            return result;
        }
    }
}
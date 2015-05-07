using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.WebAPI.Filters;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    [SessionAuthFilter]
    public class RedLetterController : ApiController
    {
        private readonly IRedLetterProductRepository _redLetterProductRepository;

        public RedLetterController(IRedLetterProductRepository redLetterProductRepository)
        {
            _redLetterProductRepository = redLetterProductRepository;
        }

        [HttpGet]
        [Route("RedLetter/FindByKeyword/{keyword}")]
        public List<RedLetterProductSummary> Find(string keyword)
        {
            return _redLetterProductRepository.FindByKeyword(keyword).Select(p => new RedLetterProductSummary(p)).ToList();
        }

        [HttpGet]
        [Route("RedLetter/Get/{id}")]
        public RedLetterProduct Get(int id)
        {
            var result =  _redLetterProductRepository.Get(id);

            return result;
        }

        [HttpGet]
        [Route("RedLetter/GetRandomSpecialOffers/{count}")]
        public IEnumerable<RedLetterProductSummary> GetRandomSpecialOffers(int count)
        {
            var products = _redLetterProductRepository.Find(product => product.IsSpecialOffer);

            var shuffled = Shuffle(products.ToArray(), count);

            return shuffled.Select(p => new RedLetterProductSummary(p));
        }

        static T[] Shuffle<T>(T[] array, int max = -1)
        {
            var random = new Random();
            var n = array.Length;
            for (var i = 0; i < n; i++)
            {
                if (i == max)
                    break;
                // NextDouble returns a random number between 0 and 1.
                // ... It is equivalent to Math.random() in Java.
                var r = i + (int)(random.NextDouble() * (n - i));
                var t = array[r];
                array[r] = array[i];
                array[i] = t;
            }

            return (max > -1) ? array.Take(Math.Min(max, array.Length)).ToArray() : array;
        }
    }
}
#if DEBUG

using System.Web.Http;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Service;

namespace GRG.LeisureCards.WebAPI.Controllers
{
    public class TestController : ApiController
    {
        private readonly ITestService _testService;
        private readonly ISettingRepository _settingRepository;

        public TestController(ITestService testService, ISettingRepository settingRepository)
        {
            _testService = testService;
            _settingRepository = settingRepository;
        }

        [HttpGet]
        [Route("Test/UOW")]
        public string UOW()
        {
            var testSetting = _settingRepository.Get("TEST");

            if (testSetting!=null)
                _settingRepository.Delete(testSetting);

            try
            {
                _testService.TestUOW();
            }
            catch{}

            var result = _settingRepository.Get("TEST") == null;

            return result.ToString();
        }
    }
}
#endif

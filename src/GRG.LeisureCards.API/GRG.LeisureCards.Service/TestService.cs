#if DEBUG

    using System;
    using GRG.LeisureCards.Model;
    using GRG.LeisureCards.Persistence;
    using GRG.LeisureCards.Persistence.NHibernate;

namespace GRG.LeisureCards.Service
{
    public interface ITestService
    {
        void TestUOW();
    }

    public class TestService : ITestService
    {
        private readonly ISettingRepository _settingRepository;

        public TestService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        [UnitOfWork]
        public void TestUOW()
        {
            _settingRepository.SaveOrUpdate(new Setting {SettingKey = "TEST", Value = "TEST"});

            throw new Exception();
        }
    }
}

#endif

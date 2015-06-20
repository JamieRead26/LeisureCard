using GRG.LeisureCards.WebAPI.ClientContract;
using GRG.LeisureCards.WebAPI.Model;

namespace GRG.LeisureCards.WebAPI.Client
{
    public class Session : ISession
    {
        private readonly string _baseurl;
        private readonly SessionInfo _sessionInfo;

        public Session(string baseurl, SessionInfo sessionInfo)
        {
            _baseurl = baseurl;
            _sessionInfo = sessionInfo;
        }

        public bool IsAdmin
        {
            get { return _sessionInfo.IsAdmin; }
        }

        public ILeisureCardService GetLeisureCardService()
        {
            return new LeisureCardService(_baseurl, _sessionInfo.SessionToken);
        }

        public IDataImportService GetDataImportService()
        {
            return new DataImportService(_baseurl, _sessionInfo.SessionToken);
        }

        public IRedLetterService GetRedLetterService()
        {
            return new RedLetterService(_baseurl, _sessionInfo.SessionToken);
        }

        public ITwoForOneService GetTwoforOneService()
        {
            return new TwoForOneService(_baseurl, _sessionInfo.SessionToken);
        }

        public IShortBreakService GetShortBreakService()
        {
            return new ShortBreakService(_baseurl, _sessionInfo.SessionToken);
        }

        public IReportService GetReportsService()
        {
            return new ReportService(_baseurl, _sessionInfo.SessionToken);
        }

        public ITenantService GetTenantService()
        {
            return new TenantService(_baseurl, _sessionInfo.SessionToken);
        }
    }
}

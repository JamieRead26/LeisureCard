using GRG.leisureCards.WebAPI.ClientContract;
using GRG.LeisureCards.WebAPI.Model;

namespace GRG.LeisureCards.WebAPI.Client
{
    public class Session : ISession
    {
        private readonly string _baseurl;
        private readonly LeisureCard _leisureCard;
        private readonly SessionInfo _sessionInfo;

        public Session(string baseurl, LeisureCard leisureCard, SessionInfo sessionInfo)
        {
            _baseurl = baseurl;
            _leisureCard = leisureCard;
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
    }
}

using System.Security.Claims;
using System.Security.Principal;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.WebAPI.Authentication
{
    public class LeisureCardPrincipal : ClaimsPrincipal
    {
        public SessionInfo SessionInfo { get; private set; }

        public LeisureCardPrincipal(string cardCode, SessionInfo sessionInfo)
            : base(new GenericIdentity(cardCode))
        {
            SessionInfo = sessionInfo;
        }
    }
}
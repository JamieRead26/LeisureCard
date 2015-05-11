using System.Security.Claims;
using System.Security.Principal;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.WebAPI.Authentication
{
    public class LeisureCardPrincipal : ClaimsPrincipal
    {
        public SessionInfo SessionInfo { get; private set; }

        public LeisureCardPrincipal(LeisureCard card, SessionInfo sessionInfo) : base(new GenericIdentity(card.Code))
        {
            SessionInfo = sessionInfo;
        }
    }
}
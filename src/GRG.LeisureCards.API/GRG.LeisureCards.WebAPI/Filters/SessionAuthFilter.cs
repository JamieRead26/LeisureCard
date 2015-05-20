using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Service;
using GRG.LeisureCards.WebAPI.Authentication;

namespace GRG.LeisureCards.WebAPI.Filters
{
    public class SessionAuthFilter : Attribute, IAuthenticationFilter
    {
        private readonly bool _admin;
        private readonly IUserSessionService _userSessionService;

        public SessionAuthFilter(bool admin = false)
        {
            _admin = admin;
            _userSessionService = UserSessionService.Instance;
        }

        public bool AllowMultiple { get; private set; }
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            IEnumerable<string> vals;

            if (!context.Request.Headers.TryGetValues("SessionToken", out vals))
            {
                context.ErrorResult = new AuthenticationFailureResult("No SessionToken header", context.Request);
                return;
            }

            var card = _userSessionService.GetCard(vals.First());

            if (card == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("No session matching token", context.Request);
                return;
            }

            if (_admin&&!card.IsAdmin)
            {
                context.ErrorResult = new AuthenticationFailureResult("Admin access denied", context.Request);
                return;
            }

            context.Principal = new LeisureCardPrincipal(card, new SessionInfo { SessionToken = _userSessionService.GetToken(card), CardRenewalDate = card.RenewalDate.Value, IsAdmin = card.IsAdmin});
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return;
        }
    }
}
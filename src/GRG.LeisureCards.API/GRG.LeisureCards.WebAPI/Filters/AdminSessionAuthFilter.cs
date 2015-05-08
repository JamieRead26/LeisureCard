using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace GRG.LeisureCards.WebAPI.Filters
{
    public class AdminSessionAuthFilter : Attribute, IAuthenticationFilter
    {
        private readonly string _adminCode;

        public AdminSessionAuthFilter()
        {
            _adminCode = ConfigurationManager.AppSettings["AdminCode"];
        }

        public bool AllowMultiple { get; private set; }
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            IEnumerable<string> vals;

            if (context.Request.Headers.TryGetValues("AdminCode", out vals) && vals.FirstOrDefault() == _adminCode)
            {
                context.Principal = new GenericPrincipal(new GenericIdentity("Code"), new string[0]);

                return;
            }

            context.ErrorResult = new AuthenticationFailureResult("", context.Request);
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return;
        }
    }
}
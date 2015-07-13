
using GRG.LeisureCards.WebAPI.Model;

namespace GRG.LeisureCards.WebAPI.ClientContract
{
    public interface ILoginService
    {
        LeisureCardRegistrationResponse Login(string code, string tenantKey, out ISession session);
    }
}


using GRG.LeisureCards.WebAPI.Model;

namespace GRG.leisureCards.WebAPI.ClientContract
{
    public interface ILoginService
    {
        LeisureCardRegistrationResponse Login(string code, out ISession session);
    }
}

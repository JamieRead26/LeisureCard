using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Service
{
    public interface ISession
    {
        string Token { get; }
        LeisureCard LeisureCard { get; }
        bool HasExpired { get; }
        bool IsAdmin { get; }
    }
}
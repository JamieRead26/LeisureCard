using System.Collections.Generic;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence
{
    public  interface ISessionRepository : IRepository<Session, string>
    {
        Session GetLiveByCardCode(string code);
    }
}

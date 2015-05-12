using System;
using System.Collections.Generic;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Persistence
{
    public interface ILeisureCardRepository : IRepository<LeisureCard,string>
    {
        IEnumerable<LeisureCard> GetRegistrationHistory(DateTime @from, DateTime to);
    }
}

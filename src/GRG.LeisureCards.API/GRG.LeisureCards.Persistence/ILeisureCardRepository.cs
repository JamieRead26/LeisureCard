using System;
using System.Collections.Generic;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence
{
    public interface ILeisureCardRepository : IRepository<LeisureCard,string>
    {
        IEnumerable<LeisureCard> GetRegistrationHistory(DateTime @from, DateTime to);
        IEnumerable<LeisureCard> GetAllIncludingDeleted();
    }
}

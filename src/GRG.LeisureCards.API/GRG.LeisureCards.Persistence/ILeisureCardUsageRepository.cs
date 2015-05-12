using System;
using System.Collections.Generic;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Persistence
{
    public interface ILeisureCardUsageRepository : IRepository<LeisureCardUsage, int>
    {
        IEnumerable<LeisureCardUsage> Get(int count, int toId);
        IEnumerable<LeisureCardUsage> Get(string cardId, int count, int toId);
        IEnumerable<LeisureCardUsage> Get(DateTime from, DateTime to);
    }
}

using System;
using System.Collections.Generic;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence
{
    public interface ICardGenerationLogRepository : IRepository<CardGenerationLog, string>
    {
        IEnumerable<CardGenerationLog> Get(DateTime @from, DateTime to);
    }
}

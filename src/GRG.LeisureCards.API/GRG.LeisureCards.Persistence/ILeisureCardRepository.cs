using System;
using System.Collections.Generic;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence
{
    public interface ILeisureCardRepository : IRepository<LeisureCard,string>
    {
        IEnumerable<LeisureCard> GetRegistrationHistory(DateTime @from, DateTime to);
        IEnumerable<LeisureCard> GetAllIncludingDeleted();
        IEnumerable<LeisureCard> GetByRef(string cardNumberOrRef);
        int CountUrns(string tenantKey);
        IEnumerable<LeisureCard> GetLoginPopupReportIncludingNotAccepted(string tenantKey, DateTime @from, DateTime to);
        IEnumerable<LeisureCard> GetLoginPopupReport(string tenantKey, DateTime @from, DateTime to);
        bool Exists(string code);
        ICollection<LeisureCard> GetPrototypeByRef(string searchTerm, int max);
    }
}

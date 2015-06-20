using System;
using System.Collections.Generic;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Persistence.NHibernate;

namespace GRG.LeisureCards.Service
{
    public interface IReportService
    {
        IEnumerable<LeisureCard> GetLoginPopupReport(string tenantKey, DateTime from, DateTime to);
    }

    public class ReportService : IReportService
    {
        private readonly ILeisureCardRepository _leisureCardRepository;
        private readonly ITenantRepository _tenantRepository;

        public ReportService(ILeisureCardRepository leisureCardRepository, ITenantRepository tenantRepository)
        {
            _leisureCardRepository = leisureCardRepository;
            _tenantRepository = tenantRepository;
        }

        [UnitOfWork]
        public IEnumerable<LeisureCard> GetLoginPopupReport(string tenantKey, DateTime from, DateTime to)
        {
            var tenant = _tenantRepository.Get(tenantKey);

            return tenant.MemberLoginPopupMandatory ? 
                _leisureCardRepository.GetLoginPopupReportIncludingNotAccepted(tenantKey, @from, to) : 
                _leisureCardRepository.GetLoginPopupReport(tenantKey, @from, to);
        }
    }
}

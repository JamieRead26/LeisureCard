using System;
using System.Collections.Generic;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class LeisureCardRepository : Repository<LeisureCard, string>, ILeisureCardRepository
    {
        public IEnumerable<LeisureCard> GetRegistrationHistory(DateTime @from, DateTime to)
        {
            return Session.QueryOver<LeisureCard>()
                .Where(u => u.RegistrationDate >= from)
                .Where(u => u.RegistrationDate <= to)
                .Where(u=>!u.Deleted)
                .OrderBy(f=>f.RegistrationDate).Desc
                .List();
        }

        public override IEnumerable<LeisureCard> GetAll()
        {
            return Session.QueryOver<LeisureCard>().Where(u=>!u.Deleted).List();
        }

        public IEnumerable<LeisureCard> GetAllIncludingDeleted()
        {
            return Session.QueryOver<LeisureCard>().List();
        }

        public IEnumerable<LeisureCard> GetByRef(string reference)
        {
            return Session.QueryOver<LeisureCard>()
                .Where(u => !u.Deleted)
                .Where(u => u.Reference == reference)
                .List();
        }

        public int CountUrns(string tenantKey)
        {
            return Session.QueryOver<LeisureCard>()
                .Where(u => !u.Deleted)
                .Where(u => u.TenantKey == tenantKey)
                .RowCount();
        }

        public IEnumerable<LeisureCard> GetLoginPopupReportIncludingNotAccepted(string tenantKey, DateTime @from, DateTime to)
        {
            return Session.QueryOver<LeisureCard>()
                .Where(u => !u.Deleted)
                .Where(u => u.TenantKey == tenantKey)
                .Where(u => u.RegistrationDate.HasValue)
                .List();
        }

        public IEnumerable<LeisureCard> GetLoginPopupReport(string tenantKey, DateTime @from, DateTime to)
        {
            return Session.QueryOver<LeisureCard>()
                .Where(u => !u.Deleted)
                .Where(u => u.TenantKey == tenantKey)
                .Where(u => u.MembershipTermsAccepted.HasValue)
                .List();
        }

        public override void Delete(LeisureCard entity)
        {
            entity.Deleted = true;
            SaveOrUpdate(entity);
        }
    }
}

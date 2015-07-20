using System;
using System.Collections.Generic;
using System.Linq;
using GRG.LeisureCards.DomainModel;
using NHibernate.Criterion;
using NHibernate.Linq;

//using NHibernate.Linq;

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

        public bool Exists(string code)
        {
            return Session.Query<LeisureCard>().Any(c=>c.Code==code);
        }

        public IEnumerable<LeisureCard> FindByCodeAndRef(string searchTerm, int max)
        {
            LeisureCard inner = null;
            // this alias is for outer query, and will be used in 
            // inner query as a condition in the HAVING clause
            LeisureCard outer = null;

            var minIdSubquery = QueryOver.Of(() => inner)
                .SelectList(l => l
                    .SelectGroup(() => inner.Reference) // here we GROUP BY
                    .SelectMin(() => inner.Code)
                )

                // HAVING to get just Min(id) match
                .Where(Restrictions.EqProperty(
                  Projections.Min<LeisureCard>(i => i.Code),
                  Projections.Property(() => outer.Code)
                ));

            // outer query
            IEnumerable<LeisureCard> urns = (Session.QueryOver(() => outer)
                .WithSubquery
                // we can now use EXISTS, because we applied match in subquery
                .WhereExists(minIdSubquery)
                .Where(Restrictions.On<LeisureCard>(x => x.Reference).IsLike(searchTerm, MatchMode.Start))
                .Take(max)
                .List());

            if (urns.Count() < 100)
            {
                urns = urns.Union(Session.QueryOver<LeisureCard>()
                .Where(Restrictions.On<LeisureCard>(x => x.Code).IsLike(searchTerm, MatchMode.Start))
                .Take(100)
                .List());
            }

            return urns;
        }

        public override void Delete(LeisureCard entity)
        {
            entity.Deleted = true;
            SaveOrUpdate(entity);
        }
    }
}

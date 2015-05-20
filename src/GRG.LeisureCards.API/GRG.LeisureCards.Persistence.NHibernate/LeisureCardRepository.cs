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

        public override void Delete(LeisureCard entity)
        {
            entity.Deleted = true;
            SaveOrUpdate(entity);
        }
    }
}

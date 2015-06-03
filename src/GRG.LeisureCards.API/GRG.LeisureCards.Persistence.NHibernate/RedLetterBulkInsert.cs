using System;
using System.Collections.Generic;
using GRG.LeisureCards.DomainModel;
using log4net;
using NHibernate;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class RedLetterBulkInsert : IRedLetterBulkInsert
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RedLetterBulkInsert));

        private readonly ISessionFactory _sessionFactory;

        public RedLetterBulkInsert(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Insert(IEnumerable<RedLetterProduct> inserts, IEnumerable<RedLetterProduct> updates, IEnumerable<RedLetterProduct> deletes, DataImportJournalEntry journalEntry)
        {
            try
            {
                using (var session = _sessionFactory.OpenStatelessSession())
                using (var trans = session.BeginTransaction())
                {
                    journalEntry.Success = true;

                    foreach (var product in deletes)
                        session.Delete(product);

                    foreach (var redLetterProduct in inserts)
                        session.Insert(redLetterProduct);

                    foreach (var redLetterProduct in updates)
                        session.Update(redLetterProduct);

                    journalEntry.Success = true;
                    journalEntry.LastRun = DateTime.Now;
                    journalEntry.Status = "Success";
                    session.Update(journalEntry);

                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error on file import", ex);

                using (var session = _sessionFactory.OpenStatelessSession())
                {
                    journalEntry.Success = false;
                    journalEntry.Message = ex.Message;
                    journalEntry.Status = "Failure";

                    session.Update(journalEntry);
                }
            }
        }

        public IEnumerable<RedLetterProduct> GetAll()
        {
            using (var session = _sessionFactory.OpenStatelessSession())
                return session.QueryOver<RedLetterProduct>().List();
        }
    }
}

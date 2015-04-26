using System;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
    {
        private readonly ISessionFactory _sessionFactory;

        public Repository()
        {
            var dbConf = PostgreSQLConfiguration.PostgreSQL82.ConnectionString(c => c
                    .Database("LeisureCards")
                    .Host("localhost")
                    .Port(5432)
                    .Username("postgres")
                    .Password(""));

            var classMapAssembly = Assembly.GetCallingAssembly();

            _sessionFactory = Fluently.Configure()
                .Database(() => dbConf)
                .Mappings(m => m.FluentMappings.AddFromAssembly(classMapAssembly))
                .BuildSessionFactory();
        }

        //protected ISession Session { get { return UnitOfWork.Current.Session; } }
       

        public TEntity SaveOrUpdate(TEntity entity)
        {
            using (var session =_sessionFactory.OpenSession())
            {
                session.SaveOrUpdate(entity);
            }

            return entity;
        }

        public void Delete(TKey entityKey)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(TKey key)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Get<TEntity>(key);
            }
        }
    }
}

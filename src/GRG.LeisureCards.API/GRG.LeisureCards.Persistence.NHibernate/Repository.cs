﻿using System;
using System.Collections.Generic;
using NHibernate;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected ISession Session { get { return UnitOfWork.Current.Session; } }

        public IEnumerable<TEntity> GetAll()
        {
            return Session.QueryOver<TEntity>().List();
        }

        public TEntity SaveOrUpdate(TEntity entity)
        {
            Session.SaveOrUpdate(entity);

            return entity;
        }

        public void Delete(TKey entityKey)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            Session.Delete(entity);
        }

        public virtual TEntity Get(TKey key)
        {
            return Session.Get<TEntity>(key);
        }
    }
}

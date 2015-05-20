﻿using System;
using System.Collections.Generic;
using NHibernate;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected ISession Session { get { return UnitOfWork.Current.Session; } }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Session.QueryOver<TEntity>().List();
        }

        public TEntity SaveOrUpdate(TEntity entity)
        {
            Session.SaveOrUpdate(entity);

            return entity;
        }

        public virtual void Delete(TEntity entity)
        {
            Session.Delete(entity);
        }

        public void Purge()
        {
            foreach(var entity in GetAll())
                Delete(entity);
        }

        public virtual TEntity Get(TKey key)
        {
            return Session.Get<TEntity>(key);
        }
    }
}

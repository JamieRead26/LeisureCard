﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
    {

        public TEntity SaveOrUpdate(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(TKey entityKey)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(TKey key)
        {
            throw new NotImplementedException();
        }
    }
}

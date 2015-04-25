using System;
using FluentNHibernate.Mapping;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class LeisureCardClassMap : ClassMap<LeisureCard>
    {
        public LeisureCardClassMap()
        {
            Id(x => x.Code);
        }
    }
}

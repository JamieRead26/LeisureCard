using FluentNHibernate.Mapping;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class SettingClassMap : ClassMap<Setting>
    {
        public SettingClassMap()
        {
            Id(x => x.SettingKey);
            Map(x => x.Value);
        }
    }
}

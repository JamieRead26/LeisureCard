using FluentNHibernate.Mapping;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class SettingClassMap : ClassMap<Setting>
    {
        public SettingClassMap()
        {
            Id(x => x.Key);
            Map(x => x.Value);
        }
    }
}

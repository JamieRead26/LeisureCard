using FluentNHibernate.Mapping;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate.ClassMaps
{
    public class TenantClassMap : ClassMap<Tenant>
    {
        public TenantClassMap()
        {
            Id(x => x.Key);
            Map(x => x.Name);
            Map(x => x.Domain);
            Map(x => x.Comments);
            Map(x => x.Active);
            Map(x => x.MemberLoginPopupDisplayed);
            Map(x => x.MemberLoginPopupMandatory);
            Map(x => x.FtpPassword);
            Map(x => x.FtpServer);
            Map(x => x.FtpUsername);
            Map(x => x.FtpAddFilePath);
            Map(x => x.FtpDeactivateFilePath);
        }
    }
}

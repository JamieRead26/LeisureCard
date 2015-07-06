using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Data.Test
{
    public class TenantDataFixture : DataFixture
    {
        public Tenant GRG { get; private set; }

        public Tenant Base { get; private set; }

        public Tenant NPower { get; private set; }
        public Tenant Inactive { get; private set; }
        public Tenant PopupNotMandatory { get; private set; }
        public Tenant PopupMandatory { get; private set; }

        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            GRG = new Tenant { TenantKey = "GRG", Name = "Grass Roots Group", Active = true, Domain = "grg.leisurecards.shiftkey.uk.com", FtpServer = "ftp://grg.leisurecards.shiftkey.uk.com/", FtpAddFilePath = "/GRG/newUrns.csv", FtpDeactivateFilePath = "/GRG/deactiveUrns.csv", FtpUsername = "anon"};
            Base = new Tenant { TenantKey = "Base", Name = "Base Implementation", Active = true, Domain = "base.leisurecards.shiftkey.uk.com" };
            NPower = new Tenant { TenantKey = "NPower", Name = "NPower", Active = true, Domain = "npower.leisurecards.shiftkey.uk.com" };
            Inactive = new Tenant { TenantKey = "Inactive", Name = "Inactive", Active = false };
            PopupNotMandatory = new Tenant { TenantKey = "PopupNotMandatory", Name = "Inactive", Active = true, MemberLoginPopupDisplayed = true };
            PopupMandatory = new Tenant { TenantKey = "PopupMandatory", Name = "Inactive", Active = true, MemberLoginPopupDisplayed = true, MemberLoginPopupMandatory = true };
            
            return new[]{GRG, Base, NPower, Inactive, PopupMandatory, PopupNotMandatory};
        }
    }
}

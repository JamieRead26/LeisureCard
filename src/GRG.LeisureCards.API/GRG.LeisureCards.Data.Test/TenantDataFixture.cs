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
            GRG = new Tenant { Key = "GRG", Name = "Grass Roots Group", Active = true, Domain = "grg.leisurecards.shiftkey.uk.com" };
            Base = new Tenant { Key = "Base", Name = "Base Implementation", Active = true, Domain = "base.leisurecards.shiftkey.uk.com" };
            NPower = new Tenant { Key = "NPower", Name = "NPower", Active = true, Domain = "npower.leisurecards.shiftkey.uk.com" };
            Inactive = new Tenant { Key = "Inactive", Name = "Inactive", Active = false };
            PopupNotMandatory = new Tenant { Key = "PopupNotMandatory", Name = "Inactive", Active = true, MemberLoginPopupDisplayed = true };
            PopupMandatory = new Tenant { Key = "PopupMandatory", Name = "Inactive", Active = true, MemberLoginPopupDisplayed = true, MemberLoginPopupMandatory = true };
            
            return new[]{GRG, Base, NPower, Inactive, PopupMandatory, PopupNotMandatory};
        }
    }
}

namespace GRG.LeisureCards.Model
{
    public class MembershipTier
    {
        public virtual string Key { get; set; }

        public virtual string Name { get; set; }

        public virtual bool IsDefault { get; set; }
    }
}

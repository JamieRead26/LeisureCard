﻿namespace GRG.LeisureCards.Model
{
    public class MembershipTier
    {
        public virtual string TierKey { get; set; }

        public virtual string Name { get; set; }

        public virtual bool IsDefault { get; set; }
    }
}

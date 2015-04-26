using System;

namespace GRG.LeisureCards.Model
{
    public class LeisureCard
    {
        public virtual string Code { get; set; }
        public virtual DateTime RenewalDate { get; set; }
        public virtual bool Suspended { get; set; }
        public virtual DateTime? Registered { get; set; }
    }
}

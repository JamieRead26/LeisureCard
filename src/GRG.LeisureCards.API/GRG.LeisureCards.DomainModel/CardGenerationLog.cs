using System;

namespace GRG.LeisureCards.DomainModel
{
    public class CardGenerationLog
    {
        public virtual string Ref { get; set; }
        public virtual DateTime GeneratedDate { get; set; }
    }
}

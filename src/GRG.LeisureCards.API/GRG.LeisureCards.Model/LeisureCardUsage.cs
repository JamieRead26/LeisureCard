using System;

namespace GRG.LeisureCards.Model
{
    public class LeisureCardUsage
    {
        public virtual int Id { get; set; }
        public virtual LeisureCard LeisureCard { get; set; }
        public virtual DateTime LoginDateTime { get; set; }
    }
}

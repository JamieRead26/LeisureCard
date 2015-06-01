using System;

namespace GRG.LeisureCards.DomainModel
{
    public class Session
    {
        public virtual string Token { get; set; }
        public virtual string CardCode { get; set; }
        public virtual bool ExpiryUtc { get; set; }
        public virtual bool IsAdmin { get; set; }
        public virtual DateTime? CardRenewalDate { get; set; }
    }
}

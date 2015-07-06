using System;

namespace GRG.LeisureCards.DomainModel
{
    public class Session
    {
        public virtual string Token { get; set; }
        public virtual string CardCode { get; set; }
        public virtual DateTime ExpiryUtc { get; set; }
        public virtual bool IsAdmin { get; set; }
        public virtual string TenantKey { get; set; }
    }
}

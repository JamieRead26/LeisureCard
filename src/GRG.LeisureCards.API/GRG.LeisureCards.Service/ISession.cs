using System;

namespace GRG.LeisureCards.Service
{
    public interface ISession
    {
        string Token { get; }
        string CardCode { get; }
        bool HasExpired { get; }
        bool IsAdmin { get; }
        DateTime ExpiryUtc { get;  }
        string TenantKey { get; }
    }
}
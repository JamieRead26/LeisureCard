using System;

namespace GRG.LeisureCards.DomainModel
{
    public class SessionInfo
    {
        public DateTime CardRenewalDate { get; set; }
        public string SessionToken { get; set; }

        public bool IsAdmin { get; set; }
    }
}

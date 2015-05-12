using System;

namespace GRG.LeisureCards.Model
{
    public class SessionInfo
    {
        public DateTime CardRenewalDate { get; set; }
        public string SessionToken { get; set; }

        public bool IsAdmin { get; set; }
    }
}

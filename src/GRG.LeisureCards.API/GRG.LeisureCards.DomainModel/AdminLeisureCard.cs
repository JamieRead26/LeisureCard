using System;

namespace GRG.LeisureCards.DomainModel
{
    public class AdminLeisureCard
    {
        public static readonly LeisureCard Instance = new LeisureCard
        {
            Code = "Admin",
            ExpiryDate = DateTime.Parse( "1/1/3000"), 
            RenewalDate = DateTime.Parse( "1/1/3000"),
            RegistrationDate = DateTime.Parse("1/1/2000"),
            TenantKey = "GRG"
        };

        private AdminLeisureCard() { }
    }
}

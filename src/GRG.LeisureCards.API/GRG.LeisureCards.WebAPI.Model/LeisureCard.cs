using System;

namespace GRG.LeisureCards.WebAPI.Model
{
    public class LeisureCard
    {
        public string Code { get; set; }

        public DateTime UploadedDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public DateTime? RenewalDate { get; set; }

        public bool Suspended { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public string Status { get; set; }

        public string Reference { get; set; }

        public int RenewalPeriodMonths { get; set; }

        public DateTime? MembershipTermsAccepted { get; set; }

       // public bool IsMembershipTermsAccepted { get; set; }
    }
}

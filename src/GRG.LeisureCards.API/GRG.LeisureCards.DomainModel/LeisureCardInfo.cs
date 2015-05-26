using System;

namespace GRG.LeisureCards.DomainModel
{
    public class LeisureCardInfo
    {
        public LeisureCardInfo(){}

        public LeisureCardInfo(LeisureCard leisureCard)
        {
            Code = leisureCard.Code;
            UploadedDate = leisureCard.UploadedDate;
            ExpiryDate = leisureCard.ExpiryDate;
            RenewalDate = leisureCard.RenewalDate;
            CancellationDate = leisureCard.Suspended;
            RegistrationDate = leisureCard.RegistrationDate;
            Status = leisureCard.Status;
        }
        public string Status { get; set; }

        public string Code { get; set; }
        public DateTime UploadedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? RenewalDate { get; set; }
        public bool CancellationDate { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public int RenewalPeriodMonths { get; set; }

        public string Reference { get; set; }

        public bool Suspended { get; set; }

    }
}

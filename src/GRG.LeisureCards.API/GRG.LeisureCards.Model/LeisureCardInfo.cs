using System;

namespace GRG.LeisureCards.Model
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
            CancellationDate = leisureCard.CancellationDate;
            RegistrationDate = leisureCard.RegistrationDate;
        }

        public string Code { get; set; }
        public DateTime UploadedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? RenewalDate { get; set; }
        public bool CancellationDate { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}

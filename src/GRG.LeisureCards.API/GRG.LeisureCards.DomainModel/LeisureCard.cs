using System;
using System.Collections.Generic;

namespace GRG.LeisureCards.DomainModel
{
    public class LeisureCard
    {
        public LeisureCard()
        {
            OfferCategories = new List<OfferCategory>();
            LoginHistory = new List<LeisureCardUsage>();
        }

        public virtual IList<LeisureCardUsage> LoginHistory { get; set; }

        public virtual string Code { get; set; }

        public virtual DateTime UploadedDate { get; set; }

        public virtual DateTime? ExpiryDate { get; set; }

        public virtual DateTime? RenewalDate { get; set; }

        public virtual bool Suspended { get; set; }

        public virtual DateTime? RegistrationDate { get; set; }

        public virtual string TenantKey { get; set; }

        public virtual LeisureCardStatus StatusEnum
        {
            get
            {
                if (Suspended)
                    return LeisureCardStatus.Suspended;

                if ((ExpiryDate.HasValue && ExpiryDate.Value <= DateTime.Now))
                    return LeisureCardStatus.Expired;

                if (!RegistrationDate.HasValue)
                    return LeisureCardStatus.Inactive;

                return LeisureCardStatus.Active;
            }
            set { }
        }

        public virtual string Status
        {
            get
            {
                return Enum.GetName(typeof (LeisureCardStatus), StatusEnum);
                ;}
            set { }
        }

        public virtual string Reference { get; set; }

        public virtual int RenewalPeriodMonths { get; set; }

        public virtual MembershipTier MembershipTier { get; set; }

        public virtual IList<OfferCategory> OfferCategories { get; set; }

        public virtual bool Deleted { get; set; }

        public virtual DateTime? MembershipTermsAccepted { get; set; }

        public virtual void AddOfferCategory(OfferCategory offerCategory)
        {
            OfferCategories.Add(offerCategory);
            offerCategory.LeisureCards.Add(this);
        }

        public virtual void AddUsage(LeisureCardUsage leisureCardUsage)
        {
            leisureCardUsage.LeisureCard = this;
            LoginHistory.Add(leisureCardUsage);
        }
    }

    public enum LeisureCardStatus
    {
        Inactive,
        Active,
        Suspended,
        Expired
    }
}
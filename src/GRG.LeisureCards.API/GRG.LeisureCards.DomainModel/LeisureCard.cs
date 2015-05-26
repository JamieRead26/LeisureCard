using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GRG.LeisureCards.DomainModel
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class LeisureCard
    {
        public LeisureCard()
        {
            OfferCategories = new List<OfferCategory>();
            LoginHistory = new List<LeisureCardUsage>();
        }

        public virtual IList<LeisureCardUsage> LoginHistory { get; set; }

        [DataMember]
        public virtual string Code { get; set; }

        [DataMember]
        public virtual DateTime UploadedDate { get; set; }

        [DataMember]
        public virtual DateTime? ExpiryDate { get; set; }

        [DataMember]
        public virtual DateTime? RenewalDate { get; set; }

        [DataMember]
        public virtual bool Suspended { get; set; }

        [DataMember]
        public virtual DateTime? RegistrationDate { get; set; }

        [DataMember]
        public virtual string Status
        {
            get
            {
                if (Suspended)
                    return "Suspended";

                if ((ExpiryDate.HasValue && ExpiryDate.Value <= DateTime.Now) ||
                    (RenewalDate.HasValue && RenewalDate.Value <= DateTime.Now))
                    return "Expired";

                if (!RegistrationDate.HasValue)
                    return "Inactive";

                return "Active";
            }
            set { }
        }

        public virtual MembershipTier MembershipTier { get; set; }

        public virtual IList<OfferCategory> OfferCategories { get; set; }

        public virtual bool Deleted { get; set; }

       
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

        public virtual string Reference { get; set; }
        public virtual int? RenewalPeriodMonths { get; set; }
    }
}
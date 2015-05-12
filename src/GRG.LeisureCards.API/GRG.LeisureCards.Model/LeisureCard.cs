using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GRG.LeisureCards.Model
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
        public virtual bool CancellationDate { get; set; }

        [DataMember]
        public virtual DateTime? RegistrationDate { get; set; }

        public virtual MembershipTier MembershipTier { get; set; }

        public virtual IList<OfferCategory> OfferCategories { get; set; }

        [DataMember]
        public virtual bool IsAdmin { get; set; }

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
}
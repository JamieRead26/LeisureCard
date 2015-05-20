using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GRG.LeisureCards.DomainModel
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class RedLetterProduct
    {
        public RedLetterProduct()
        {
            Facts = new List<RedLetterFact>();
            Venues = new List<RedLetterVenue>();
            Keywords = new List<RedLetterKeyword>();
        }

        [DataMember]
        public virtual int Id { get; set; }
        [DataMember]
        public virtual string Title { get; set; }
        [DataMember]
        public virtual string InspirationalDescription { get; set; }
        [DataMember]
        public virtual string VoucherText { get; set; }
        [DataMember]
        public virtual string ExpRef { get; set; }
        [DataMember]
        public virtual string Type { get; set; }
        [DataMember]
        public virtual decimal GeneralPrice { get; set; }
        [DataMember]
        public virtual decimal PriceBeforeVAT { get; set; }
        [DataMember]
        public virtual IList<RedLetterVenue> Venues { get; set; }
        [DataMember]
        public virtual string Territory { get; set; }
        [DataMember]
        public virtual string DisplayLocations { get; set; }
        [DataMember]
        public virtual string MainSectionName { get; set; }
        [DataMember]
        public virtual string SectionName { get; set; }
        [DataMember]
        public virtual int Priority { get; set; }
        [DataMember]
        public virtual IList<RedLetterFact> Facts { get; set; }
        [DataMember]
        public virtual string WhatsIncluded { get; set; }
        [DataMember]
        public virtual string Availability { get; set; }
        [DataMember]
        public virtual string Weather { get; set; }
        [DataMember]
        public virtual string Duration { get; set; }
        [DataMember]
        public virtual string ShortDuration { get; set; }
        [DataMember]
        public virtual string HowManyPeople { get; set; }
        [DataMember]
        public virtual string FriendsAndFamily { get; set; }
        [DataMember]
        public virtual string DressCode { get; set; }
        [DataMember]
        public virtual string AnyOtherInfo { get; set; }
        [DataMember]
        public virtual string WhoCanTakePart { get; set; }
        [DataMember]
        public virtual string WhereIsItHeld { get; set; }
        [DataMember]
        public virtual string HowToGetThere { get; set; }
        [DataMember]
        public virtual string PermaLink { get; set; }
        [DataMember]
        public virtual string Url { get; set; }
        [DataMember]
        public virtual string ImageUrl { get; set; }
        [DataMember]
        public virtual string ThumbnailUrl { get; set; }
        [DataMember]
        public virtual string LargeImageName { get; set; }
        [DataMember]
        public virtual bool IsSpecialOffer { get; set; }
        [DataMember]
        public virtual string DeliveryTime { get; set; }
        [DataMember]
        public virtual string DeliveryCost { get; set; }
        public virtual IList<RedLetterKeyword> Keywords { get; set; }

        public virtual RedLetterFact AddFact(RedLetterFact redLetterFact)
        {
            Facts.Add(redLetterFact);
            redLetterFact.RedLetterProduct = this;
            return redLetterFact;
        }

        public virtual RedLetterVenue AddVenue(RedLetterVenue redLetterVenue)
        {
            Venues.Add(redLetterVenue);
            redLetterVenue.RedLetterProduct = this;
            return redLetterVenue;
        }

        public virtual RedLetterKeyword AddKeyword(RedLetterKeyword redLetterKeyword)
        {
            redLetterKeyword.Products.Add(this);
            Keywords.Add(redLetterKeyword);
            return redLetterKeyword;
        }
    }
}

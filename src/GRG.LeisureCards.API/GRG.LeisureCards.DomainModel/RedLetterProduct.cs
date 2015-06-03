using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GRG.LeisureCards.DomainModel
{
    public class RedLetterProduct
    {
        //public RedLetterProduct()
        //{
        //    Facts = new List<RedLetterFact>();
        //    Venues = new List<RedLetterVenue>();
        //    Keywords = new List<RedLetterKeyword>();
        //}

        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string InspirationalDescription { get; set; }
        public virtual string VoucherText { get; set; }
        public virtual string ExpRef { get; set; }
        public virtual string Type { get; set; }
        public virtual decimal GeneralPrice { get; set; }
        public virtual decimal PriceBeforeVAT { get; set; }
        //public virtual IList<RedLetterVenue> Venues { get; set; }
        public virtual string Territory { get; set; }
        public virtual string DisplayLocations { get; set; }
        public virtual string MainSectionName { get; set; }
        public virtual string SectionName { get; set; }
        public virtual int Priority { get; set; }
        //public virtual IList<RedLetterFact> Facts { get; set; }
        public virtual string WhatsIncluded { get; set; }
        public virtual string Availability { get; set; }
        public virtual string Weather { get; set; }
        public virtual string Duration { get; set; }
        public virtual string ShortDuration { get; set; }
        public virtual string HowManyPeople { get; set; }
        public virtual string FriendsAndFamily { get; set; }
        public virtual string DressCode { get; set; }
        public virtual string AnyOtherInfo { get; set; }
        public virtual string WhoCanTakePart { get; set; }
        public virtual string WhereIsItHeld { get; set; }
        public virtual string HowToGetThere { get; set; }
        public virtual string PermaLink { get; set; }
        public virtual string Url { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual string ThumbnailUrl { get; set; }
        public virtual string LargeImageName { get; set; }
        public virtual bool IsSpecialOffer { get; set; }
        public virtual string DeliveryTime { get; set; }
        public virtual string DeliveryCost { get; set; }
        //public virtual IList<RedLetterKeyword> Keywords { get; set; }

        //public virtual RedLetterFact AddFact(RedLetterFact redLetterFact)
        //{
        //    Facts.Add(redLetterFact);
        //    redLetterFact.RedLetterProduct = this;
        //    return redLetterFact;
        //}

        //public virtual RedLetterVenue AddVenue(RedLetterVenue redLetterVenue)
        //{
        //    Venues.Add(redLetterVenue);
        //    redLetterVenue.RedLetterProduct = this;
        //    return redLetterVenue;
        //}

        //public virtual RedLetterKeyword AddKeyword(RedLetterKeyword redLetterKeyword)
        //{
        //    redLetterKeyword.Products.Add(this);
        //    Keywords.Add(redLetterKeyword);
        //    return redLetterKeyword;
        //}
    }
}

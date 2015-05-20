namespace GRG.LeisureCards.DomainModel
{
    public class TwoForOneOffer : ILatLong
    {
        public virtual int Id { get; set; }
        public virtual string OutletName { get; set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string TownCity { get; set; }
        public virtual string County { get; set; }
        public virtual string PostCode { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Website { get; set; }
        public virtual string Description { get; set; }
        public virtual bool DisabledAccess { get; set; }
        public virtual string ClaimCode { get; set; }
        public virtual string BookingInstructions { get; set; }
        public virtual string Email { get; set; }
        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }
    }
}

namespace GRG.LeisureCards.WebAPI.Model
{
    public class TwoForOneOffer
    {
        public int Id { get; set; }
        public string OutletName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string TownCity { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string DisabledAccess { get; set; }
        public string ClaimCode { get; set; }
        public string BookingInstructions { get; set; }
        public string CategoryKey { get; set; }
    }
}

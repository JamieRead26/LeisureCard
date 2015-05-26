namespace GRG.LeisureCards.WebAPI.Model
{
    public class RedLetterProductSummary
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal GeneralPrice { get; set; }
        public string InspirationalDescription { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string LargeImageName { get; set; }
    }
}

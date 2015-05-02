using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Model
{
    public class RedLetterProductSummary
    {
        public RedLetterProductSummary() { }

        public RedLetterProductSummary(RedLetterProduct redLetterProduct)
        {
            Id = redLetterProduct.Id;
            Title = redLetterProduct.Title;
            GeneralPrice = redLetterProduct.GeneralPrice;
            InspirationalDescription = redLetterProduct.InspirationalDescription;
            Url = redLetterProduct.Url;
            ImageUrl = redLetterProduct.ImageUrl;
            ThumbnailUrl = redLetterProduct.ThumbnailUrl;
            LargeImageName = redLetterProduct.LargeImageName;
        }

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
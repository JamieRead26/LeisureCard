namespace GRG.LeisureCards.WebAPI.Model
{
    public class CardGenerationResponse
    {
        public CardGenerationLog CardGenerationLog { get; set; }

        public bool Success { get; set; }

        public string ErrorMessage { get; set; }
    }
}

namespace GRG.LeisureCards.Model
{
    public class LeisureCardRegistrationResponse
    {
        public string Status { get; set; }
        public LeisureCard LeisureCard { get; set; }

        public string SessionToken { get; set; }
    }
}

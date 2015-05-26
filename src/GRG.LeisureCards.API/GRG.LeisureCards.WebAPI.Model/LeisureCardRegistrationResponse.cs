namespace GRG.LeisureCards.WebAPI.Model
{
    public class LeisureCardRegistrationResponse
    {
        public string Status { get; set; }
        public LeisureCard LeisureCard { get; set; }
        public SessionInfo SessionInfo { get; set; }
    }
}

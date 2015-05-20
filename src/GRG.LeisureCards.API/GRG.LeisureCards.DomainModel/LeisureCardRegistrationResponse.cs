namespace GRG.LeisureCards.DomainModel
{
    public class LeisureCardRegistrationResponse
    {
        public string Status { get; set; }
        public LeisureCard LeisureCard { get; set; }

        public SessionInfo SessionInfo { get; set; }
    }
}

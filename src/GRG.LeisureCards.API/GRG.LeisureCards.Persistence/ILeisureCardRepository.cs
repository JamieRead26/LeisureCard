namespace GRG.LeisureCards.Persistence
{
    public interface ILeisureCardRepository : IRepository<LeisureCard,string>
    {
        LeisureCard Find(string code);
    }

    public class LeisureCard
    {
        public string Code { get; set; }
    }
}

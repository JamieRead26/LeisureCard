namespace GRG.leisureCards.WebAPI.ClientContract
{
    public interface ISession
    {
        bool IsAdmin { get; }
        ILeisureCardService GetLeisureCardService();
    }
}

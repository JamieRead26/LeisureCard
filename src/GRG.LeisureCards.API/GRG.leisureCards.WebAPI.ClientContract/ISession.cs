namespace GRG.LeisureCards.WebAPI.ClientContract
{
    public interface ISession
    {
        bool IsAdmin { get; }
        ILeisureCardService GetLeisureCardService();
        IDataImportService GetDataImportService();
    }
}

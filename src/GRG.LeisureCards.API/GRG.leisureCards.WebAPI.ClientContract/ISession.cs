namespace GRG.LeisureCards.WebAPI.ClientContract
{
    public interface ISession
    {
        bool IsAdmin { get; }
        ILeisureCardService GetLeisureCardService();
        IDataImportService GetDataImportService();
        IRedLetterService GetRedLetterService();
        ITwoForOneService GetTwoforOneService();
        IShortBreakService GetShortBreakService();
        IReportService GetReportsService();
        ITenantService GetTenantService();
    }
}

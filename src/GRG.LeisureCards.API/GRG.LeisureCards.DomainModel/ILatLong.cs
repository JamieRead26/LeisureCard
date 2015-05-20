namespace GRG.LeisureCards.DomainModel
{
    public interface ILatLong
    {
        string UkPostCodeOrTown { get; }
        double? Latitude { get; set; }
        double? Longitude { get; set; } 
    }
}

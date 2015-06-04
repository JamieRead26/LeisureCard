namespace GRG.LeisureCards.DomainModel
{
    public interface ILatLong
    {
        string[] Locations { get; }
        double? Latitude { get; set; }
        double? Longitude { get; set; } 
    }
}

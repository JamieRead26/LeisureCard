namespace GRG.LeisureCards.DomainModel
{
    public class Location
    {
        public virtual int Id { get; set; }
        public virtual string UkPostcodeOrTown { get; set; }
        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }
    }
}

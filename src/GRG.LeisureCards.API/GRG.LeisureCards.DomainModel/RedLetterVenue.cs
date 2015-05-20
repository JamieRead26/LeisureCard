using System;
using System.Runtime.Serialization;

namespace GRG.LeisureCards.DomainModel
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class RedLetterVenue
    {
        [DataMember]
        public virtual int Id { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string County { get; set; }
        [DataMember]
        public virtual string Town { get; set; }
        [DataMember]
        public virtual string PostCode { get; set; }
        [DataMember]
        public virtual decimal Latitude { get; set; }
        [DataMember]
        public virtual decimal Longitude { get; set; }
        [DataMember]
        public virtual RedLetterProduct RedLetterProduct { get; set; }
        [DataMember]
        public virtual int RedLetterId { get; set; }
    }
}

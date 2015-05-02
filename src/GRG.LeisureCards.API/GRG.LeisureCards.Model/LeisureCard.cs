using System;
using System.Runtime.Serialization;

namespace GRG.LeisureCards.Model
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class LeisureCard
    {
        [DataMember]
        public virtual string Code { get; set; }
        [DataMember]
        public virtual DateTime RenewalDate { get; set; }
        [DataMember]
        public virtual bool Suspended { get; set; }
        [DataMember]
        public virtual DateTime? Registered { get; set; }
    }
}

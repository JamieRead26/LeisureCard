using System;
using System.Runtime.Serialization;

namespace GRG.LeisureCards.Model
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class RedLetterFact
    {
        public virtual int Id { get; set; }

        [DataMember]
        public virtual string Fact { get; set; }

        public virtual RedLetterProduct RedLetterProduct{get;set;}
    }
}

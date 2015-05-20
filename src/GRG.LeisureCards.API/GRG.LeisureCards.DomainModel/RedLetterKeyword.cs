using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GRG.LeisureCards.DomainModel
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class RedLetterKeyword
    {
        public RedLetterKeyword()
        {
            Products = new List<RedLetterProduct>();
        }

        [DataMember]
        public virtual string Keyword { get; set; }

        public virtual IList<RedLetterProduct> Products { get; set; }
    }
}

using System;

namespace GRG.LeisureCards.DomainModel
{
    public class LeisureCardUsage
    {
        public virtual int Id { get; set; }
        public virtual LeisureCard LeisureCard { get; set; }
        public virtual DateTime LoginDateTime { get; set; }
    }

    public class LeisureCardUsageInfo
    {
        public LeisureCardUsageInfo() { }

        public LeisureCardUsageInfo(LeisureCardUsage usage)
        {
            Id = usage.Id;
           // LeisureCardCode = usage.LeisureCard.Code;
            LoginDateTime = usage.LoginDateTime;
        }

        public int Id { get; set; }
        public string LeisureCardCode { get; set; }
        public DateTime LoginDateTime { get; set; }
    }
}

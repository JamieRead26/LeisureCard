using System;

namespace GRG.LeisureCards.WebAPI.Model
{
    public class LeisureCardUsage
    {
        public int Id { get; set; }
        public string LeisureCardCode { get; set; }
        public DateTime LoginDateTime { get; set; }
    }
}

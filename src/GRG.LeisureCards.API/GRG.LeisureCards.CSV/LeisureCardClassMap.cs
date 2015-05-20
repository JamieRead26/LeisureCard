using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsvHelper.Configuration;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.CSV
{
    public sealed class LeisureCardClassMap : CsvClassMap<LeisureCard>
    {
        public LeisureCardClassMap()
        {
            Map(m => m.Code).Name("Code");
            Map(m => m.ExpiryDate).Name("ExpiryDate");
            Map(m => m.RenewalDate).Name("RenewalDate");
            Map(m => m.Suspended).Name("Suspended");
            Map(m => m.IsAdmin).Name("IsAdmin");
            Map(m => m.MembershipTier).Ignore();
            Map(m => m.OfferCategories).Ignore();
            Map(m => m.RegistrationDate).Ignore();
            Map(m => m.UploadedDate).Ignore();
        }
    }
}

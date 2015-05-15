using System;
using System.Collections.Generic;
using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Data.Test
{
    public class DataImportJournalEntryDataFixture : DataFixture
    {
        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            var journalEntries = new List<DataImportJournalEntry>();
            var from = DateTime.Now - TimeSpan.FromDays(30);

            for (var i = 0; i < 25; i++)
                journalEntries.Add(new DataImportJournalEntry
                {
                    OfferTypeKey = DataImportKey.RedLetter.Key, 
                    ImportedDateTime = from + TimeSpan.FromDays(i),
                    Success = true
                });

            for (var i = 0; i < 25; i++)
                journalEntries.Add(new DataImportJournalEntry
                {
                    OfferTypeKey = DataImportKey.TwoForOne.Key,
                    ImportedDateTime = from + TimeSpan.FromDays(i),
                    Success = true
                });

            return journalEntries.ToArray();
        }
    }
}

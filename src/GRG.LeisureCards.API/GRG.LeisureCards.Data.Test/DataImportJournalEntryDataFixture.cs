﻿using System;
using System.Collections.Generic;
using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.DomainModel;

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
                    UploadKey = DataImportKey.RedLetter.Key, 
                    LastRun = from + TimeSpan.FromDays(i),
                    Success = true,
                    FileName = "placeholder"
                });

            for (var i = 0; i < 25; i++)
                journalEntries.Add(new DataImportJournalEntry
                {
                    UploadKey = DataImportKey.TwoForOne.Key,
                    LastRun = from + TimeSpan.FromDays(i),
                    Success = true,
                    FileName = "placeholder"
                });

            return journalEntries.ToArray();
        }
    }
}

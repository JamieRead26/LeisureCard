using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper.Configuration;

namespace GRG.LeisureCards.CSV
{
    public class CsvReader
    {
        private static readonly CsvConfiguration Config;

        public static ICsvReader Create(TextReader reader)
        {
            return new CsvReaderProxy(new CsvHelper.CsvReader(reader, Config));
        }

        static CsvReader()
        {
            Config = new CsvConfiguration();
            Config.RegisterClassMap<TwoForOneOfferClassMap>();
            Config.RegisterClassMap<LeisureCardClassMap>();
            Config.TrimFields = true;
        }
    }

    public interface ICsvReader : IDisposable
    {
        IEnumerable<T> GetRecords<T>();
    }

    public class CsvReaderProxy : ICsvReader
    {
        private readonly CsvHelper.CsvReader _reader;

        public CsvReaderProxy(CsvHelper.CsvReader reader)
        {
            _reader = reader;
        }

        public IEnumerable<T> GetRecords<T>() 
        {
            return _reader.GetRecords<T>();
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}

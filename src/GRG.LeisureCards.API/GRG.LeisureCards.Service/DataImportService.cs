using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using GRG.LeisureCards.CSV;
using GRG.LeisureCards.Model;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Persistence.NHibernate;

namespace GRG.LeisureCards.Service
{
    public interface IDataImportService
    {
        DataImportJournalEntry ImportRedLetterOffers(byte[] file);
        DataImportJournalEntry ImportTwoForOneOffers(byte[] file);
    }

    public class DataImportService : IDataImportService
    {
        private readonly IDataImportJournalEntryRepository _dataImportJournalEntryRepository;
        private readonly IRedLetterProductRepository _redLetterProductRepository;
        private readonly ITwoForOneRepository _twoForOneRepository;

        public DataImportService(
            IDataImportJournalEntryRepository dataImportJournalEntryRepository, 
            IRedLetterProductRepository redLetterProductRepository,
            ITwoForOneRepository _twoForOneRepository)
        {
            _dataImportJournalEntryRepository = dataImportJournalEntryRepository;
            _redLetterProductRepository = redLetterProductRepository;
            this._twoForOneRepository = _twoForOneRepository;
        }

        [UnitOfWork]
        public DataImportJournalEntry ImportRedLetterOffers(byte[] file)
        {
            return ImportData("Red Letter Days", () =>
            {
                var keywords = new Dictionary<string, RedLetterKeyword>();

                using (var stream = new MemoryStream(file))
                using (var txtReader = new StreamReader(stream))
                {
                    var xmlDoc = new XmlDocument();

                    xmlDoc.Load(txtReader);

                    foreach (XmlNode productNode in xmlDoc.GetElementsByTagName("RedLetterProduct"))
                    {
                        var product = new RedLetterProduct
                        {
                            Id = int.Parse(productNode.SelectSingleNode("Key").InnerText),
                            Title = productNode.SelectSingleNode("Title").InnerText,
                            InspirationalDescription =
                                productNode.SelectSingleNode("InspirationalDescription").InnerText,
                            VoucherText = productNode.SelectSingleNode("VoucherText").InnerText,
                            ExpRef = productNode.SelectSingleNode("ExpRef").InnerText,
                            Type = productNode.SelectSingleNode("Type").InnerText,
                            GeneralPrice = decimal.Parse(productNode.SelectSingleNode("GeneralPrice").InnerText),
                            PriceBeforeVAT = decimal.Parse(productNode.SelectSingleNode("PriceBeforeVAT").InnerText),
                            Territory = productNode.SelectSingleNode("Territory").InnerText,
                            DisplayLocations = productNode.SelectSingleNode("DisplayLocations").InnerText,
                            MainSectionName = productNode.SelectSingleNode("MainSectionName").InnerText,
                            SectionName = productNode.SelectSingleNode("SectionName").InnerText,
                            Priority = int.Parse(productNode.SelectSingleNode("Priority").InnerText),
                            WhatsIncluded = productNode.SelectSingleNode("WhatsIncluded").InnerText,
                            Availability = productNode.SelectSingleNode("Availability").InnerText,
                            Weather = productNode.SelectSingleNode("Weather").InnerText,
                            Duration = productNode.SelectSingleNode("Duration").InnerText,
                            ShortDuration = productNode.SelectSingleNode("ShortDuration").InnerText,
                            HowManyPeople = productNode.SelectSingleNode("HowManyPeople").InnerText,
                            FriendsAndFamily = productNode.SelectSingleNode("FriendsAndFamily").InnerText,
                            DressCode = productNode.SelectSingleNode("DressCode").InnerText,
                            AnyOtherInfo = productNode.SelectSingleNode("AnyOtherInfo").InnerText,
                            WhoCanTakePart = productNode.SelectSingleNode("WhoCanTakePart").InnerText,
                            WhereIsItHeld = productNode.SelectSingleNode("WhereIsItHeld").InnerText,
                            HowToGetThere = productNode.SelectSingleNode("HowToGetThere").InnerText,
                            PermaLink = productNode.SelectSingleNode("PermaLink").InnerText,
                            Url = productNode.SelectSingleNode("Url").InnerText,
                            ImageUrl = productNode.SelectSingleNode("ImageUrl").InnerText,
                            ThumbnailUrl = productNode.SelectSingleNode("ThumbnailUrl").InnerText,
                            LargeImageName = productNode.SelectSingleNode("LargeImageName").InnerText,
                            IsSpecialOffer = bool.Parse(productNode.SelectSingleNode("IsSpecialOffer").InnerText),
                            DeliveryTime = productNode.SelectSingleNode("DeliveryTime").InnerText,
                            DeliveryCost = productNode.SelectSingleNode("DeliveryCost").InnerText
                        };

                        foreach (
                            var keyword in productNode.SelectSingleNode("Keywords").InnerText.Split(",".ToCharArray()))
                        {
                            if (!keywords.ContainsKey(keyword))
                                keywords.Add(keyword, new RedLetterKeyword {Keyword = keyword});

                            product.AddKeyword(keywords[keyword]);
                        }

                        foreach (XmlNode venueNode in productNode.SelectSingleNode("Venues").ChildNodes)
                        {
                            product.AddVenue(new RedLetterVenue
                            {
                                RedLetterId = int.Parse(venueNode.SelectSingleNode("Key").InnerText),
                                Name = venueNode.SelectSingleNode("Name").InnerText,
                                County = venueNode.SelectSingleNode("County").InnerText,
                                Town = venueNode.SelectSingleNode("Town").InnerText,
                                PostCode = venueNode.SelectSingleNode("PostCode").InnerText,
                                Latitude = decimal.Parse(venueNode.SelectSingleNode("Latitude").InnerText),
                                Longitude = decimal.Parse(venueNode.SelectSingleNode("Longitude").InnerText)
                            });
                        }

                        _redLetterProductRepository.Purge();

                        foreach (var fact in from XmlNode factNode in productNode.SelectSingleNode("Facts").ChildNodes
                            select new RedLetterFact {Fact = factNode.InnerText})
                            product.AddFact(fact);


                        _redLetterProductRepository.SaveOrUpdate(product);
                    }
                }
            });
        }

        [UnitOfWork]
        public DataImportJournalEntry ImportTwoForOneOffers(byte[] file)
        {
            return ImportData("2-4-1 Offers", () =>
            {
                _twoForOneRepository.Purge();

                using (var stream = new MemoryStream(file))
                using (var csvReader = CsvReader.Create(new StreamReader(stream)))
                {
                    foreach (var offer in csvReader.GetRecords<TwoForOneOffer>().ToArray())
                        _twoForOneRepository.SaveOrUpdate(offer);
                }
            });
        }

        private DataImportJournalEntry ImportData(string key, Action importAction)
        {
            var journalEntry = new DataImportJournalEntry
            {
                ImportedUtc = DateTime.UtcNow,
                Key = key
            };

            try
            {
                importAction();

                journalEntry.Success = true;
                _dataImportJournalEntryRepository.SaveOrUpdate(journalEntry);

                return journalEntry;
            }
            catch (Exception ex)
            {
                journalEntry.Success = false;
                journalEntry.Message = ex.Message;
                journalEntry.StackTrace = ex.StackTrace;

                _dataImportJournalEntryRepository.SaveOrUpdate(journalEntry);

                return journalEntry;
            }
        }
    }
}

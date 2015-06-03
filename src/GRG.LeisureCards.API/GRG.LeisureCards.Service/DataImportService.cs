using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Xml;
using GRG.LeisureCards.CSV;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Persistence.NHibernate;
using log4net;

namespace GRG.LeisureCards.Service
{
    public interface IDataImportService
    {
        void Import(DataImportJournalEntry journalEntry, Func<string,string> mapPath );
    }

    public class DataImportService : IDataImportService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataImportService));

        private readonly IDataImportJournalEntryRepository _dataImportJournalEntryRepository;
        private readonly IRedLetterProductRepository _redLetterProductRepository;
        private readonly ITwoForOneRepository _twoForOneRepository;
        private readonly IUkLocationService _locationService;
        private readonly IRedLetterBulkInsert _redLetterBulkInsert;

        public DataImportService(
            IDataImportJournalEntryRepository dataImportJournalEntryRepository, 
            IRedLetterProductRepository redLetterProductRepository,
            ITwoForOneRepository twoForOneRepository,
            IUkLocationService locationService,
            IRedLetterBulkInsert redLetterBulkInsert)
        {
            _dataImportJournalEntryRepository = dataImportJournalEntryRepository;
            _redLetterProductRepository = redLetterProductRepository;
            _twoForOneRepository = twoForOneRepository;
            _locationService = locationService;
            _redLetterBulkInsert = redLetterBulkInsert;
        }

        public DataImportJournalEntry ImportRedLetterOffers(Stream fileStream, DataImportJournalEntry journalEntry)
        {
            journalEntry.Status = "Running";
            journalEntry.LastRun = DateTime.Now;
            _dataImportJournalEntryRepository.SaveOrUpdate(journalEntry);

            var inserts = new List<RedLetterProduct>();
            var updates = new List<RedLetterProduct>();
            var allProducts = _redLetterBulkInsert.GetAll().ToDictionary(p => p.Id, p => p);
                
            //var keywords = _redLetterProductRepository.GetAllKeywords().ToDictionary(k => k.Keyword, k => k);

            using (var txtReader = new StreamReader(fileStream))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(txtReader);

                foreach (XmlNode productNode in xmlDoc.GetElementsByTagName("Product"))
                {
                    try
                    {
                        var id = int.Parse(productNode.SelectSingleNode("Id").InnerText);
                        RedLetterProduct product;

                        if (allProducts.ContainsKey(id))
                        {
                            product = allProducts[id];
                            updates.Add(product);
                        }
                        else
                        {
                            product =  new RedLetterProduct { Id = id };
                            inserts.Add(product);
                        }

                        product.Title = productNode.SelectSingleNode("Title").InnerText;
                        product.InspirationalDescription = productNode.SelectSingleNode("InspirationalDescription").InnerText;
                        product.VoucherText = productNode.SelectSingleNode("VoucherText").InnerText;
                        product.ExpRef = productNode.SelectSingleNode("ExpRef").InnerText;
                        product.Type = productNode.SelectSingleNode("Type").InnerText;
                        product.GeneralPrice = decimal.Parse(productNode.SelectSingleNode("GeneralPrice").InnerText);
                        product.PriceBeforeVAT = decimal.Parse(productNode.SelectSingleNode("PriceBeforeVAT").InnerText);
                        product.Territory = productNode.SelectSingleNode("Territory").InnerText;
                        product.DisplayLocations = productNode.SelectSingleNode("DisplayLocations").InnerText;
                        product.MainSectionName = productNode.SelectSingleNode("MainSectionName").InnerText;
                        product.SectionName = productNode.SelectSingleNode("SectionName").InnerText;
                        product.Priority = int.Parse(productNode.SelectSingleNode("Priority").InnerText);
                        product.WhatsIncluded = productNode.SelectSingleNode("WhatsIncluded").InnerText;
                        product.Availability = productNode.SelectSingleNode("Availability").InnerText;
                        product.Weather = productNode.SelectSingleNode("Weather").InnerText;
                        product.Duration = productNode.SelectSingleNode("Duration").InnerText;
                        product.ShortDuration = productNode.SelectSingleNode("ShortDuration").InnerText;
                        product.HowManyPeople = productNode.SelectSingleNode("HowManyPeople").InnerText;
                        product.FriendsAndFamily = productNode.SelectSingleNode("FriendsAndFamily").InnerText;
                        product.DressCode = productNode.SelectSingleNode("DressCode").InnerText;
                        product.AnyOtherInfo = productNode.SelectSingleNode("AnyOtherInfo").InnerText;
                        product.WhoCanTakePart = productNode.SelectSingleNode("WhoCanTakePart").InnerText;
                        product.WhereIsItHeld = productNode.SelectSingleNode("WhereIsItHeld").InnerText;
                        product.HowToGetThere = productNode.SelectSingleNode("HowToGetThere").InnerText;
                        product.PermaLink = productNode.SelectSingleNode("PermaLink").InnerText;
                        product.Url = productNode.SelectSingleNode("Url").InnerText;
                        product.ImageUrl = productNode.SelectSingleNode("ImageUrl").InnerText;
                        product.ThumbnailUrl = productNode.SelectSingleNode("ThumbnailUrl").InnerText;
                        product.LargeImageName = productNode.SelectSingleNode("LargeImageName").InnerText;
                        product.IsSpecialOffer = bool.Parse(productNode.SelectSingleNode("IsSpecialOffer").InnerText);
                        product.DeliveryTime = productNode.SelectSingleNode("DeliveryTime").InnerText;
                        product.DeliveryCost = productNode.SelectSingleNode("DeliveryCost").InnerText;

                        //foreach (var redLetterKeyword in product.Keywords.ToArray())
                        //{
                        //    redLetterKeyword.Products.Remove(product);
                        //    product.Keywords.Remove(redLetterKeyword);
                        //}

                        //foreach (var fact in product.Facts.ToArray())
                        //{
                        //    fact.RedLetterProduct = null;
                        //    product.Facts.Remove(fact);
                        //}

                        //foreach (var venue in product.Venues.ToArray())
                        //{
                        //    venue.RedLetterProduct = null;
                        //    product.Venues.Remove(venue);
                        //}

                        //foreach (var keyword in productNode.SelectSingleNode("Keywords").InnerText.Split(",".ToCharArray()))
                        //{
                        //    if (!keywords.ContainsKey(keyword))
                        //        keywords.Add(keyword, new RedLetterKeyword { Keyword = keyword });

                        //    product.AddKeyword(keywords[keyword]);
                        //}

                        //foreach (XmlNode venueNode in productNode.SelectSingleNode("Venues").ChildNodes)
                        //{
                        //    product.AddVenue(new RedLetterVenue
                        //    {
                        //        RedLetterId = int.Parse(venueNode.SelectSingleNode("Id").InnerText),
                        //        Name = venueNode.SelectSingleNode("Name").InnerText,
                        //        County = venueNode.SelectSingleNode("County").InnerText,
                        //        Town = venueNode.SelectSingleNode("Town").InnerText,
                        //        PostCode = venueNode.SelectSingleNode("PostCode").InnerText,
                        //        Latitude = decimal.Parse(venueNode.SelectSingleNode("Latitude").InnerText),
                        //        Longitude = decimal.Parse(venueNode.SelectSingleNode("Longitude").InnerText)
                        //    });
                        //}

                        //foreach (var fact in from XmlNode factNode in productNode.SelectSingleNode("Facts").ChildNodes
                        //                     select new RedLetterFact { Fact = factNode.InnerText })
                        //    product.AddFact(fact);

                        allProducts.Remove(product.Id);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error reading product from redletter data", ex);
                    }
                }
            }

            _redLetterBulkInsert.Insert(inserts, updates, allProducts.Values, journalEntry) ;

            return journalEntry;
        }

        public DataImportJournalEntry ImportTwoForOneOffers(Stream fileStream, DataImportJournalEntry journalEntry)
        {
            try
            {
                var offers = _twoForOneRepository.GetAll().ToDictionary(o => o.Id, o => o);

                using (var csvReader = CsvReader.Create(new StreamReader(fileStream)))
                {
                    foreach (var offer in csvReader.GetRecords<TwoForOneOffer>().ToArray())
                    {
                        TwoForOneOffer offerToPersist = null;

                        if (offers.ContainsKey(offer.Id))
                            offerToPersist = offers[offer.Id];

                        if (offerToPersist != null)
                        {
                            offerToPersist.Address1 = offer.Address1;
                            offerToPersist.Address2 = offer.Address2;
                            offerToPersist.County = offer.County;
                            offerToPersist.Description = offer.Description;
                            offerToPersist.OutletName = offer.OutletName;
                            offerToPersist.PostCode = offer.PostCode;
                            offerToPersist.Phone = offer.Phone;
                            offerToPersist.TownCity = offer.TownCity;
                            offerToPersist.Website = offer.Website;

                            offers.Remove(offer.Id);
                        }
                        else
                        {
                            offerToPersist = offer;
                        }

                        var latLong =
                            _locationService.GetMapPoint(string.IsNullOrWhiteSpace(offerToPersist.PostCode)
                                ? offerToPersist.TownCity
                                : offerToPersist.PostCode);

                        if (latLong != null)
                        {
                            offerToPersist.Latitude = latLong.Latitude;
                            offerToPersist.Longitude = latLong.Longitude;
                        }

                        _twoForOneRepository.SaveOrUpdate(offerToPersist);
                    }
                }

                foreach (var twoForOneOffer in offers.Values)
                    _twoForOneRepository.Delete(twoForOneOffer);

                journalEntry.Success = true;
                journalEntry.LastRun = DateTime.Now;
                journalEntry.Status = "Success";

                _dataImportJournalEntryRepository.SaveOrUpdate(journalEntry);

                return journalEntry;
            }
            catch (Exception ex)
            {
                journalEntry.Success = false;
                journalEntry.LastRun = DateTime.Now;
                journalEntry.Status = "Failure";
                journalEntry.Message = ex.Message;

                _dataImportJournalEntryRepository.SaveOrUpdate(journalEntry);

                return journalEntry;
            }
            
        }

        public void Import(DataImportJournalEntry journalEntry, Func<string, string> mapPath)
        {
            var importKey = DataImportKey.All.First(d => d.Key == journalEntry.UploadKey);

            if (importKey==DataImportKey.RedLetter)
                ImportRedLetterOffers(
                    File.OpenRead(mapPath(importKey.UploadPath) + "\\" + journalEntry.FileName),
                    journalEntry);

            if (importKey == DataImportKey.TwoForOne)
                ImportTwoForOneOffers(
                    File.OpenRead(mapPath(importKey.UploadPath) + "\\" + journalEntry.FileName),
                    journalEntry);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using GRG.LeisureCards.CSV;
using GRG.LeisureCards.CSV.CsvModel;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Persistence.NHibernate;
using log4net;

namespace GRG.LeisureCards.Service
{
    public interface IDataImportService
    {
        DataImportJournalEntry Import(DataImportKey key, Func<string,string> mapPath, params object[] args );

        DataImportJournalEntry ImportForTenant(DataImportKey key, Func<string, string> mapPath, string tenantKey, params object[] args);
    }

    public class DataImportService : IDataImportService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataImportService));

        private readonly IDataImportJournalEntryRepository _dataImportJournalEntryRepository;
        private readonly ITwoForOneRepository _twoForOneRepository;
        private readonly IUkLocationService _locationService;
        private readonly IRedLetterBulkInsert _redLetterBulkInsert;
        private readonly ILeisureCardRepository _leisureCardRepository;

        public DataImportService(
            IDataImportJournalEntryRepository dataImportJournalEntryRepository, 
            ITwoForOneRepository twoForOneRepository,
            IUkLocationService locationService,
            IRedLetterBulkInsert redLetterBulkInsert,
            ILeisureCardRepository leisureCardRepository)
        {
            _dataImportJournalEntryRepository = dataImportJournalEntryRepository;
            _twoForOneRepository = twoForOneRepository;
            _locationService = locationService;
            _redLetterBulkInsert = redLetterBulkInsert;
            _leisureCardRepository = leisureCardRepository;
        }

        public DataImportJournalEntry ImportRedLetterOffers(Stream fileStream, DataImportJournalEntry journalEntry)
        {
            journalEntry.Status = "Running";
            journalEntry.LastRun = DateTime.Now;
            journalEntry.UploadKey = DataImportKey.RedLetter.Key;
            _dataImportJournalEntryRepository.SaveOrUpdate(journalEntry);

            var inserts = new List<RedLetterProduct>();
            var updates = new List<RedLetterProduct>();
            var allProducts = _redLetterBulkInsert.GetAll().ToDictionary(p => p.Id, p => p);
                
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
                var missingLocation = false;

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

                        offerToPersist.CategoryKey = string.IsNullOrWhiteSpace(offer.CategoryKey) ? "Daysoutgen" : offer.CategoryKey;

                        var mapPoint =
                            _locationService.GetMapPoint(new []{ offerToPersist.PostCode, offerToPersist.TownCity});

                        if (mapPoint != null)
                        {
                            offerToPersist.Latitude = mapPoint.Latitude;
                            offerToPersist.Longitude = mapPoint.Longitude;
                        }
                        else
                            missingLocation = true;

                        _twoForOneRepository.SaveOrUpdate(offerToPersist);
                    }
                }

                foreach (var twoForOneOffer in offers.Values)
                    _twoForOneRepository.Delete(twoForOneOffer);

                journalEntry.Success = true;
                journalEntry.LastRun = DateTime.Now;
                journalEntry.Status = "Success";
                journalEntry.UploadKey = DataImportKey.TwoForOne.Key;

                if (missingLocation)
                    journalEntry.Message = "Unable to determine all locations";

                _dataImportJournalEntryRepository.SaveOrUpdate(journalEntry);

                return journalEntry;
            }
            catch (Exception ex)
            {
                journalEntry.Success = false;
                journalEntry.LastRun = DateTime.Now;
                journalEntry.Status = "Failure";
                journalEntry.Message = ex.Message;
                journalEntry.UploadKey = DataImportKey.TwoForOne.Key;

                _dataImportJournalEntryRepository.SaveOrUpdate(journalEntry);

                return journalEntry;
            }
            
        }

        [UnitOfWork]
        public DataImportJournalEntry Import(DataImportKey importKey, Func<string, string> mapPath, params object[] args)
        {
            return ImportForTenant(importKey, mapPath, string.Empty, args);
        }

        public DataImportJournalEntry ImportForTenant(DataImportKey importKey, Func<string, string> mapPath,
            string tenantKey, params object[] args)
        {
            var journalEntry = _dataImportJournalEntryRepository.GetLast(importKey);

            var path = (String.IsNullOrWhiteSpace(tenantKey))
                ? mapPath(importKey.UploadPath) + "\\" + journalEntry.FileName
                : mapPath(importKey.UploadPath) + "\\" + tenantKey + "\\" + journalEntry.FileName;

            if (importKey == DataImportKey.RedLetter)
                return ImportRedLetterOffers(
                    File.OpenRead(path),
                    journalEntry);

            if (importKey == DataImportKey.TwoForOne)
                return ImportTwoForOneOffers(
                    File.OpenRead(path),
                    journalEntry);

            if (importKey == DataImportKey.NewUrns)
                return ImportNewUrns(
                    File.OpenRead(path),
                    journalEntry,
                    (int)args[0],
                    (string)args[1]);

            if (importKey == DataImportKey.DeactivatedUrns)
                return ImportDeactivatedUrns(
                    File.OpenRead(path),
                    journalEntry);

            throw new Exception("Unexpected DataImportKey : " + importKey.Key);
        }

        private DataImportJournalEntry ImportNewUrns(FileStream fileStream, DataImportJournalEntry journalEntry, int cardDurationMonths, string reference)
        {
            try
            {
                var urns = _leisureCardRepository.GetAll().Select(x=>x.Code.ToUpper());

                int affected = 0;
                using (var csvReader = CsvReader.Create(new StreamReader(fileStream)))
                {
                    foreach (var newUrn in csvReader.GetRecords<Urn>().ToArray().Where(newUrn => !urns.Contains(newUrn.Code.ToUpper())))
                    {
                        _leisureCardRepository.Save(
                            new LeisureCard
                            {
                                Code = newUrn.Code,
                                Reference = reference,
                                TenantKey = journalEntry.Tenant.Key,
                                RenewalPeriodMonths = cardDurationMonths
                            });

                        affected++;
                    }
                }

                journalEntry.Success = true;
                journalEntry.LastRun = DateTime.Now;
                journalEntry.Status = "Success";
                journalEntry.Supplemental = affected.ToString();
                journalEntry.Supplemental1 = reference;
                journalEntry.UploadKey = DataImportKey.NewUrns.Key;

                _dataImportJournalEntryRepository.SaveOrUpdate(journalEntry);

                return journalEntry;
            }
            catch (Exception ex)
            {
                journalEntry.Success = false;
                journalEntry.LastRun = DateTime.Now;
                journalEntry.Status = "Failure";
                journalEntry.Message = ex.Message;
                journalEntry.UploadKey = DataImportKey.NewUrns.Key;

                _dataImportJournalEntryRepository.SaveOrUpdate(journalEntry);

                return journalEntry;
            }
        }
        
        private DataImportJournalEntry ImportDeactivatedUrns(FileStream fileStream, DataImportJournalEntry journalEntry)
        {
            try
            {
                var urns = _leisureCardRepository.GetAll();
                int affected = 0;

                using (var csvReader = CsvReader.Create(new StreamReader(fileStream)))
                {
                    foreach (var deactiveUrn in csvReader.GetRecords<Urn>())
                    {
                        var urn =
                            urns.FirstOrDefault(
                                x => string.Equals(x.Code, deactiveUrn.Code, StringComparison.CurrentCultureIgnoreCase));
                       
                        if (urn != null)
                        {
                            urn.Suspended = true;

                            _leisureCardRepository.Update(urn);

                            affected++;
                        }
                    }
                }

                journalEntry.Success = true;
                journalEntry.LastRun = DateTime.Now;
                journalEntry.Status = "Success";
                journalEntry.Supplemental = affected.ToString();
                journalEntry.UploadKey = DataImportKey.DeactivatedUrns.Key;

                _dataImportJournalEntryRepository.SaveOrUpdate(journalEntry);

                return journalEntry;
            }
            catch (Exception ex)
            {
                journalEntry.Success = false;
                journalEntry.LastRun = DateTime.Now;
                journalEntry.Status = "Failure";
                journalEntry.Message = ex.Message;
                journalEntry.UploadKey = DataImportKey.DeactivatedUrns.Key;

                _dataImportJournalEntryRepository.SaveOrUpdate(journalEntry);

                return journalEntry;
            }
        }
    }
}

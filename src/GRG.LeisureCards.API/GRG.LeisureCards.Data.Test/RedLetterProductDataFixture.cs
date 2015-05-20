using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.TestResources;

namespace GRG.LeisureCards.Data.Test
{
    public class RedLetterProductDataFixture : DataFixture
    {
        public readonly IList<RedLetterProduct> Products = new List<RedLetterProduct>();
        private readonly IDictionary<string, RedLetterKeyword> _keywords = new Dictionary<string, RedLetterKeyword>();

        public RedLetterProductDataFixture()
        {
            using (var txtReader = new StreamReader(ResourceStreams.GetRedLetterDataStream()))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(txtReader);

                foreach (XmlNode productNode in xmlDoc.GetElementsByTagName("RedLetterProduct"))
                {
                    var product = new RedLetterProduct{
                        Id = int.Parse(productNode.SelectSingleNode("Key").InnerText),
                        Title = productNode.SelectSingleNode("Title").InnerText,
                        InspirationalDescription = productNode.SelectSingleNode("InspirationalDescription").InnerText,
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
                        IsSpecialOffer = true,// bool.Parse(productNode.SelectSingleNode("IsSpecialOffer").InnerText),
                        DeliveryTime = productNode.SelectSingleNode("DeliveryTime").InnerText,
                        DeliveryCost = productNode.SelectSingleNode("DeliveryCost").InnerText
                    };
                
                    Products.Add(product);

                    foreach (var keyword in productNode.SelectSingleNode("Keywords").InnerText.Split(",".ToCharArray()))
                    {
                        if (!_keywords.ContainsKey(keyword))
                            _keywords.Add(keyword, new RedLetterKeyword { Keyword = keyword });

                        product.AddKeyword(_keywords[keyword]);
                    }
                }
            }
        }

        public override object[] GetEntities(IFixtureContainer container)
        {
            return Products.ToArray();
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;
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

                foreach (XmlNode productNode in xmlDoc.GetElementsByTagName("Product"))
                {
                    var product = new RedLetterProduct();
                    product.Id = int.Parse(productNode.SelectSingleNode("Id").InnerText);
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
                    product.IsSpecialOffer = true;// bool.Parse(productNode.SelectSingleNode("IsSpecialOffer").InnerText),
                    product.DeliveryTime = productNode.SelectSingleNode("DeliveryTime").InnerText;
                    product.DeliveryCost = productNode.SelectSingleNode("DeliveryCost").InnerText;
                    Products.Add(product);
                }
            }
        }

        public override object[] GetEntities(IFixtureContainer container)
        {
            return Products.ToArray();
        }
    }
}

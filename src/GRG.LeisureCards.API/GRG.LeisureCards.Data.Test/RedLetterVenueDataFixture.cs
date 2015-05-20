using System;
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
    public class RedLetterVenueDataFixture: DataFixture
    {
        private readonly IDictionary<int, List<RedLetterVenue>> _venues = new Dictionary<int, List<RedLetterVenue>>();

        public override Type[] Dependencies
        {
            get { return new[]{typeof(RedLetterProductDataFixture)}; }
        }

        public RedLetterVenueDataFixture()
        {
            using (var txtReader = new StreamReader(ResourceStreams.GetRedLetterDataStream()))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(txtReader);

                foreach (XmlNode productNode in xmlDoc.GetElementsByTagName("RedLetterProduct"))
                {
                    var productId = int.Parse(productNode.SelectSingleNode("Key").InnerText);

                    var venues = new List<RedLetterVenue>();

                    foreach (XmlNode venueNode in productNode.SelectSingleNode("Venues").ChildNodes)
                    {
                        venues.Add(new RedLetterVenue
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

                    if (venues.Any())
                        _venues.Add(productId, venues);
                }
            }
        }

        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            var productFixture = fixtureContainer.Get<RedLetterProductDataFixture>();

            var venues = new List<RedLetterVenue>();
           
            foreach (var key in _venues.Keys)
            {
                var product = productFixture.Products.First(p=>p.Id==key);

                venues.AddRange(_venues[key].Select(venue => product.AddVenue(venue)));
            }

            return venues.ToArray();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.TestResources;
using NHibernate.Util;

namespace GRG.LeisureCards.Data.Test
{
    public class RedLetterFactDataFixture: DataFixture
    {
        private readonly IDictionary<int, List<RedLetterFact>> _facts = new Dictionary<int, List<RedLetterFact>>();

        public override Type[] Dependencies
        {
            get { return new[]{typeof(RedLetterProductDataFixture)}; }
        }

        public RedLetterFactDataFixture()
        {
            using (var txtReader = new StreamReader(ResourceStreams.GetRedLetterDataStream()))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(txtReader);

                foreach (XmlNode productNode in xmlDoc.GetElementsByTagName("RedLetterProduct"))
                {
                    var productId = int.Parse(productNode.SelectSingleNode("Key").InnerText);
                    var facts = (from XmlNode factNode in productNode.SelectSingleNode("Facts").ChildNodes select new RedLetterFact {Fact = factNode.InnerText}).ToList();

                    if (EnumerableExtensions.Any(facts))
                        _facts.Add(productId, facts);
                }
            }
        }
        
        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            var productFixture = fixtureContainer.Get<RedLetterProductDataFixture>();

            var facts = new List<RedLetterFact>();

            foreach (var key in _facts.Keys)
            {
                var product = productFixture.Products.First(p=>p.Id==key);

                facts.AddRange(_facts[key].Select(redLetterFact => product.AddFact(redLetterFact)));
            }

            return facts.ToArray();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Bootstrap4NHibernate.Data;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Data.Test
{
    public class RedLetterKeywordsDataFixture: DataFixture
    {
        private readonly IDictionary<string, List<int>> _keywords = new Dictionary<string, List<int>>();

        public override Type[] Dependencies
        {
            get { return new[]{typeof(RedLetterProductDataFixture)}; }
        }

        public RedLetterKeywordsDataFixture()
        {
            using (var xmlStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GRG.LeisureCards.Data.Test.RedLetter_Products.xml"))
            using (var txtReader = new StreamReader(xmlStream))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(txtReader);

                foreach (XmlNode productNode in xmlDoc.GetElementsByTagName("RedLetterProduct"))
                {
                    var productId = int.Parse(productNode.SelectSingleNode("Id").InnerText);
                    foreach (var keyword in productNode.SelectSingleNode("Keywords").InnerText.Split(",".ToCharArray()))
                    {
                        if (_keywords.ContainsKey(keyword))
                        {
                            var ids = _keywords[keyword];
                            if (!ids.Contains(productId))
                                ids.Add(productId);
                        }
                        else
                        {
                            _keywords.Add(keyword, new List<int>{productId});
                        }
                    }
                }
            }
        }

        public override object[] GetEntities(IFixtureContainer fixtureContainer)
        {
            var productFixture = fixtureContainer.Get<RedLetterProductDataFixture>();

            var keywords = new List<RedLetterKeyword>();
            //foreach (var key in _keywords.Keys)
            //{
            //    var keyword = new RedLetterKeyword { Keyword = key };

            //    foreach (var productId in _keywords[key])
            //        keyword.AddProduct(productFixture.Products.First(p=>p.Id==productId));

            //    keywords.Add(keyword);
            //}


            var keyword = new RedLetterKeyword
            {
                Keyword = "amphibious",
                Products = new List<RedLetterProduct> {productFixture.Products.FirstOrDefault()}
            };
            keywords.Add(keyword);

            return keywords.ToArray();
        }
    }
}

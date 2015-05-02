using System.Collections.Generic;

namespace GRG.LeisureCards.Data
{
    public class CustomSqlTypeStringsBase : ICustomSqlTypeStrings
    {
        protected readonly IDictionary<CustomSqlType, string> _dictionary = new Dictionary<CustomSqlType, string>();

        public string Get(CustomSqlType customSqlType)
        {
            return _dictionary[customSqlType];
        }
    }
}

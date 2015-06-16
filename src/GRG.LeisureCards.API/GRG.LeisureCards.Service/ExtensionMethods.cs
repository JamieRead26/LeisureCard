using System.Collections.Generic;
using System.Linq;

namespace GRG.LeisureCards.Service
{
    public static class ExtensionMethods
    {
        public static string GetCommaSeparatedKey(this IEnumerable<string> array)
        {
            return array.Aggregate(string.Empty, (a, s) => string.IsNullOrWhiteSpace(s) ? a : string.IsNullOrWhiteSpace(a) ? s :  a + "," + s);
        }
    }
}

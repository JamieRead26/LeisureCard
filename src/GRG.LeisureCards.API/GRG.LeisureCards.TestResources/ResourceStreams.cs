using System.IO;
using System.Linq;
using System.Reflection;

namespace GRG.LeisureCards.TestResources
{
    public static class ResourceStreams
    {
        public static readonly string BadRedLetterName;
        public static readonly string RedLetterName;
        public static readonly string TwoForOneName;
        public static readonly string LeisureCardName;
        public static readonly string NewUrns;
        public static readonly string DeactivateUrns;
        static ResourceStreams()
        {
            var names = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            RedLetterName = names.First(n => n.IndexOf("RedLetter") > 0);
            BadRedLetterName = names.First(n => n.IndexOf("BadRedLetter") > 0);
            TwoForOneName = names.First(n => n.IndexOf("241") > 0);
            LeisureCardName = names.First(n => n.IndexOf("LeisureCards.csv") > 0);
            NewUrns = names.First(n => n.IndexOf("newUrns.csv") > 0);
            DeactivateUrns = names.First(n => n.IndexOf("deactiveUrns.csv") > 0);
        }

        public static Stream GetStream(string key)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(key);
        }
    }
}

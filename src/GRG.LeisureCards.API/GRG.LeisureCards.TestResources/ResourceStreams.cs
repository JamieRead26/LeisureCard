using System.IO;
using System.Linq;
using System.Reflection;

namespace GRG.LeisureCards.TestResources
{
    public static class ResourceStreams
    {
        private static readonly string RedLetterName;
        private static readonly string TwoForOneName;
        static ResourceStreams()
        {
            var names = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            RedLetterName = names.First(n => n.IndexOf("RedLetter") > 0);
            TwoForOneName = names.First(n => n.IndexOf("241") > 0);
        }

        public static Stream GetRedLetterDataStream()
        {
            return GetStream(RedLetterName);
        }

        public static Stream Get241LetterDataStream()
        {
            return GetStream(TwoForOneName);
        }

        private static Stream GetStream(string key)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(key);
        }
    }
}

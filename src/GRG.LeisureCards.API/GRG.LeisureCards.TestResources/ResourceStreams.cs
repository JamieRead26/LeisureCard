﻿using System.IO;
using System.Linq;
using System.Reflection;

namespace GRG.LeisureCards.TestResources
{
    public static class ResourceStreams
    {
        private static readonly string BadRedLetterName;
        private static readonly string RedLetterName;
        private static readonly string TwoForOneName;
        private static readonly string LeisureCardName;
        static ResourceStreams()
        {
            var names = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            RedLetterName = names.First(n => n.IndexOf("RedLetter") > 0);
            BadRedLetterName = names.First(n => n.IndexOf("BadRedLetter") > 0);
            TwoForOneName = names.First(n => n.IndexOf("241") > 0);
            LeisureCardName = names.First(n => n.IndexOf("LeisureCards.csv") > 0);
        }

        public static Stream GetRedLetterDataStream()
        {
            return GetStream(RedLetterName);
        }
        public static Stream GetRedLetterBadDataStream()
        {
            return GetStream(BadRedLetterName);
        }

        public static Stream Get241LetterDataStream()
        {
            return GetStream(TwoForOneName);
        }

        public static Stream GetLeisureCardStream()
        {
            return GetStream(LeisureCardName);
        }

        private static Stream GetStream(string key)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(key);
        }
    }
}

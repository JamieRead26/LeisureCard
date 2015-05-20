namespace GRG.LeisureCards.DomainModel
{
    public class DataImportKey
    {
        public readonly static DataImportKey RedLetter = new DataImportKey("RedLetter");
        public readonly static DataImportKey TwoForOne = new DataImportKey("241");
        public readonly static DataImportKey LeisureCards = new DataImportKey("LeisureCards");

        public string Key { get; private set; }

        private DataImportKey(string key)
        {
            Key = key;
        }
    }
}

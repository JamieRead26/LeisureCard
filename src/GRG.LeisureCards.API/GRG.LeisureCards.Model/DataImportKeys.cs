namespace GRG.LeisureCards.Model
{
    public class DataImportKey
    {
        public readonly static DataImportKey RedLetter = new DataImportKey("Red Letter");
        public readonly static DataImportKey TwoForOne = new DataImportKey("2-4-1");

        public string Key { get; private set; }

        private DataImportKey(string key)
        {
            Key = key;
        }
    }
}

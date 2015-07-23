namespace GRG.LeisureCards.DomainModel
{
    public class DataImportKey
    {
        public readonly static DataImportKey RedLetter = new DataImportKey("RedLetter",  "\\RedLetter");
        public readonly static DataImportKey TwoForOne = new DataImportKey("241", "\\241");
        public readonly static DataImportKey NewUrns = new DataImportKey("NewUrns", "\\NewUrns");
        public readonly static DataImportKey DeactivatedUrns = new DataImportKey("DeactivatedUrns", "\\DeactivatedUrns");

        public static readonly DataImportKey[] All = {RedLetter, TwoForOne, NewUrns, DeactivatedUrns};

        public string Key { get; private set; }
        public string UploadPath { get; private set; }

        private DataImportKey(string key, string uploadPath)
        {
            UploadPath = uploadPath;
            Key = key;
        }
    }
}

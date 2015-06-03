namespace GRG.LeisureCards.DomainModel
{
    public class DataImportKey
    {
        public readonly static DataImportKey RedLetter = new DataImportKey("RedLetter",  "~\\UploadFiles\\RedLetter");
        public readonly static DataImportKey TwoForOne = new DataImportKey("241", "~\\UploadFiles\\241");

        public static readonly DataImportKey[] All = {RedLetter, TwoForOne};

        public string Key { get; private set; }
        public string UploadPath { get; private set; }

        private DataImportKey(string key, string uploadPath)
        {
            UploadPath = uploadPath;
            Key = key;
        }
    }
}

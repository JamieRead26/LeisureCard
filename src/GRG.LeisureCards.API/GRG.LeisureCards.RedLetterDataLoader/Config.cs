namespace GRG.LeisureCards.RedLetterDataLoader
{
    public class Config
    {
        public string FtpPath { get; set; }
        public string Uid { get; set; }
        public string Password { get; set; }

        public string ApiBaseAddress { get; set; }

        public string ApiAdminCode { get; set; }

        public bool IsValid
        {
            get
            {
                return 
                    !string.IsNullOrWhiteSpace(FtpPath) &&
                    !string.IsNullOrWhiteSpace(Uid) &&
                    !string.IsNullOrWhiteSpace(Password);
            }
        }
    }
}

namespace GRG.LeisureCards.WebAPI.Model
{
    public class Tenant
    {
        public string TenantKey { get; set; }
        
        public string Name { get; set; }

        public string Domain { get; set; }

        public string Comments { get; set; }

        public bool MemberLoginPopupDisplayed { get; set; }

        public bool MemberLoginPopupMandatory { get; set; }

        public bool Active { get; set; }
        public int UrnCount { get; set; }

        public string FtpServer { get; set; }

        public string FtpPassword { get; set; }

        public string FtpUsername { get; set; }

        public string FtpAddFilePath { get; set; }

        public string FtpDeactivateFilePath { get; set; }
    }
}

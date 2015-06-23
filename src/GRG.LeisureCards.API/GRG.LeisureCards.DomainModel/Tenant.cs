namespace GRG.LeisureCards.DomainModel
{
    public class Tenant
    {
        public virtual string Key { get; set; }
        public virtual string Name { get; set; }

        public virtual string Domain { get; set; }

        public virtual string Comments { get; set; }

        public virtual bool MemberLoginPopupDisplayed { get; set; }

        public virtual bool MemberLoginPopupMandatory { get; set; }

        public virtual bool Active { get; set; }

        public virtual string FtpServer { get; set; }
        public virtual string FtpUsername { get; set; }

        public virtual string FtpPassword { get; set; }

        public virtual string FtpPath { get; set; }
    }
}

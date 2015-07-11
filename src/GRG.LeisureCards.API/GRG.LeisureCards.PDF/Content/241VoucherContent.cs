using GRG.LeisureCards.PDF.DocumentTemplates;

namespace GRG.LeisureCards.PDF.Content
{
    public class TwoForOneVoucherContent : DocumentTemplates.Content
    {

        [ContentProperty]
        public string UiRootUrl
        {
            get;
            set;
        }

        [ContentProperty]
        public string TenantKey
        {
            get;
            set;
        }

        [ContentProperty]
        public string ValidUntil
        {
            get;
            set;
        }

        [ContentPropertyAttribute]
        public string OutletName
        {
            get;
            set;
        }

        [ContentPropertyAttribute]
        public string BookingInstructions
        {
            get;
            set;
        }

        [ContentPropertyAttribute]
        public string ClaimCode
        {
            get;
            set;
        }
    }
}

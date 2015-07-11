using GRG.LeisureCards.PDF.Content;
using GRG.LeisureCards.PDF.DocumentTemplates;

namespace GRG.LeisureCards.PDF.PDFWriters
{
    public class TwoForOneVoucherPDFWriter : PDFWriter<TwoForOneVoucherContent>
    {
        public TwoForOneVoucherPDFWriter(
            string uiRootUrl,
            string tenantKey, 
            string validUntil, 
            string bookingInstructions, 
            string claimCode, 
            string outletName, 
            DocumentTemplate<TwoForOneVoucherContent> htmlTemplate)
        {
            Init(htmlTemplate, new TwoForOneVoucherContent
            {
                UiRootUrl = uiRootUrl,
                TenantKey = tenantKey,
                ValidUntil = validUntil,
                BookingInstructions = bookingInstructions,
                ClaimCode = claimCode,
                OutletName = outletName
            });
        }
    }
}

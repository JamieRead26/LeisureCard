namespace GRG.LeisureCards.PDFDocuments.NPower
{
    public class HtmlTemplates : Global.HtmlTemplates
    {
        public override string TenantKey { get { return "NPower"; } }

        protected override string TwoForOneVoucherContentResourceKey
        {
            get { return "GRG.LeisureCards.PDFDocuments.NPower.TwoForOneVoucher.html"; }
        }
    }
}

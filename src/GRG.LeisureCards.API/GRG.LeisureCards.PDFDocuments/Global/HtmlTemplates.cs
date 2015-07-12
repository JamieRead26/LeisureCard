using System.IO;
using System.Reflection;
using GRG.LeisureCards.PDF.Content;
using GRG.LeisureCards.PDF.DocumentTemplates;

namespace GRG.LeisureCards.PDFDocuments.Global
{
    public class HtmlTemplates : IHtmlTemplates
    {
        public HtmlTemplates()
        {
            using (var reader = new StreamReader(Assembly.GetAssembly(GetType()).GetManifestResourceStream(TwoForOneVoucherContentResourceKey)))
                VoucherContent = new DocumentTemplate<TwoForOneVoucherContent>(reader.ReadToEnd());
        }

        public DocumentTemplate<TwoForOneVoucherContent> VoucherContent { get; private set; }

        protected virtual string TwoForOneVoucherContentResourceKey
        {
            get { return "GRG.LeisureCards.PDFDocuments.Global.TwoForOneVoucher.html"; }
        }

        public virtual string TenantKey
        {
            get { return "Global"; }
        }
    }
}

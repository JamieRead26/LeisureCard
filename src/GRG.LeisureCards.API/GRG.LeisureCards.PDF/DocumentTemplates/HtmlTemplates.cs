using System.IO;
using System.Reflection;
using GRG.LeisureCards.PDF.Content;

namespace GRG.LeisureCards.PDF.DocumentTemplates
{
    public class HtmlTemplates : IHtmlTemplates
    {
        public HtmlTemplates()
        {
            using (var reader = new StreamReader(Assembly.GetAssembly(GetType()).GetManifestResourceStream("GRG.LeisureCards.PDF.HtmlTemplates.TwoForOneVoucher.html")))
                VoucherContent = new DocumentTemplate<TwoForOneVoucherContent>(reader.ReadToEnd());
        }

        public DocumentTemplate<TwoForOneVoucherContent> VoucherContent { get; private set; }
    }
}

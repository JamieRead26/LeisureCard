using System.IO;
using System.Text;
using GRG.LeisureCards.PDF.DocumentTemplates;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace GRG.LeisureCards.PDF
{
    public abstract class PDFWriter<TContent> where TContent : DocumentTemplates.Content
    {
        private string _htmldata;

        protected void Init(DocumentTemplate<TContent> template, TContent content, int mtop = 0, int mbottom = 0, int mleft = 0, int mright = 0, string baseUrl = "")
        {
            _htmldata = template.BindContent(content);
        }

        public void Write(Stream stream)
        {
            var bytes = Encoding.Unicode.GetBytes(_htmldata);

            using (var input = new MemoryStream(bytes))
            {
                var document = new Document(PageSize.A4, 0, 0, 0, 0);

                var writer = PdfWriter.GetInstance(document, stream);
                writer.CompressionLevel = PdfStream.NO_COMPRESSION;
                writer.CloseStream = false;

                document.Open();

                input.Position = 0;
                var xmlWorker = XMLWorkerHelper.GetInstance();
                xmlWorker.ParseXHtml(writer, document, input, new UnicodeEncoding());
                document.Close();
            }
        }

        public void Save(string file)
        {

            using (var stream = File.OpenWrite(file))
                Write(stream);
        }
    }
}

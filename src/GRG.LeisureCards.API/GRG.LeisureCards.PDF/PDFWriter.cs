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

        protected void Init(DocumentTemplate<TContent> template, TContent content, int mtop = 60, int mbottom = 60, int mleft = 60, int mright = 60, string baseUrl = "")
        {
            _htmldata = template.BindContent(content);
        }

        public void Write(Stream stream)
        {
            var bytes = Encoding.Unicode.GetBytes(_htmldata);

            using (var input = new MemoryStream(bytes))
            {
                var document = new Document(PageSize.LETTER, 50, 50, 50, 50);

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

using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using GRG.LeisureCards.WebAPI.Client;
using iTextSharp.text.pdf;

namespace GRG.LeisureCards.UI.Controllers
{
    public class PdfController : Controller
    {
        [Route("pdf/241voucher/{offerId}/{sessionToken}")]
        public ActionResult Get241VoucherPdf(int offerId, string sessionToken)
        {
            var offerService = new TwoForOneService(ConfigurationManager.AppSettings["ApiUrl"], sessionToken);
            
            var path = Server.MapPath(string.Format("~/content/{0}/PDF/npower_voucher.pdf", Session["TenantKey"]));
            var reader = new PdfReader(path);

            var offer = offerService.Get(offerId);
            var filename = Server.MapPath(string.Format("~/content/{0}.pdf", Guid.NewGuid()));

            var bookingInsPara = new StringBuilder();

            foreach (var ins in new[]
            {
                offer.BookingInstructions1, 
                offer.BookingInstructions2, 
                offer.BookingInstructions3, 
                offer.BookingInstructions4, 
                offer.BookingInstructions5, 
                offer.BookingInstructions6, 
                offer.BookingInstructions7,
            }.Where(i=>!string.IsNullOrWhiteSpace(i)))
            {
                bookingInsPara.Append(ins + @"\r\n");
            }

            using (var output = System.IO.File.OpenWrite(filename))
            using (var stamper = new PdfStamper(reader, output))
            {
                stamper.AcroFields.SetField("expiry_date", (DateTime.Now+TimeSpan.FromDays(14)).ToString("d"));
                stamper.AcroFields.SetField("outlet_name", offer.OutletName);
                stamper.AcroFields.SetField("booking_ins_1", bookingInsPara.ToString());

                if (!string.IsNullOrWhiteSpace(offer.ClaimCode))
                    stamper.AcroFields.SetField("claim_code", "CODE: " + offer.ClaimCode);
                else
                    stamper.AcroFields.SetField("claim_code", "");

                stamper.FormFlattening = true;
            }
            
            return new FileStreamResult(System.IO.File.OpenRead(filename), "application/pdf");
        }
    }
}
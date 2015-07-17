using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using GRG.LeisureCards.WebAPI.Client;
using org.pdfclown.files;
using Stream = org.pdfclown.bytes.Stream;

namespace GRG.LeisureCards.UI.Controllers
{
    public class PdfController : Controller
    {
        [Route("pdf/241voucher/{offerId}/{sessionToken}")]
        public ActionResult Get241VoucherPdf(int offerId, string sessionToken)
        {
            var offerService = new TwoForOneService(ConfigurationManager.AppSettings["ApiUrl"], sessionToken);
            var offer = offerService.Get(offerId);
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

            var file = new org.pdfclown.files.File(Server.MapPath(string.Format("~/content/{0}/PDF/npower_voucher.pdf", Session["TenantKey"])));
     
            file.Document.Form.Fields["expiry_date"].Value = (DateTime.Now + TimeSpan.FromDays(14)).ToString("d");
            file.Document.Form.Fields["expiry_date"].ReadOnly = true;

            file.Document.Form.Fields["outlet_name"].Value = offer.OutletName;
            file.Document.Form.Fields["outlet_name"].ReadOnly = true;

            file.Document.Form.Fields["booking_ins_1"].Value = bookingInsPara.ToString();
            file.Document.Form.Fields["booking_ins_1"].ReadOnly = true;

            file.Document.Form.Fields["claim_code"].Value = string.IsNullOrWhiteSpace(offer.ClaimCode)
                ? ""
                : "CODE: " + offer.ClaimCode;
            file.Document.Form.Fields["claim_code"].ReadOnly = true;

            var output = new MemoryStream();
            file.Save(new Stream(output), SerializationModeEnum.Standard);
            output.Position = 0;

            return File(output, "application/pdf");
        }
    }
}
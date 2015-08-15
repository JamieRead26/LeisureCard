using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using GRG.LeisureCards.WebAPI.Client;
using iTextSharp.text.pdf;
using org.pdfclown.files;
using File = org.pdfclown.files.File;
using Stream = org.pdfclown.bytes.Stream;

namespace GRG.LeisureCards.UI.Controllers
{
    public class PdfController : LcController
    {
        [Route("pdf/241voucher/{offerId}/{sessionToken}")]
        public ActionResult Get241VoucherPdf(int offerId, string sessionToken)
        {
            return Dispatch(() =>
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
                    offer.BookingInstructions7
                }.Where(i => !string.IsNullOrWhiteSpace(i)))
                {
                    bookingInsPara.Append(ins + " \n");
                }

                if (Request.Browser.IsMobileDevice)
                {
                    Log.Debug("Rendering PDF voucher for mobile, Offer ID: " +offer.Id);
                    var reader =
                        new PdfReader(
                            Server.MapPath(string.Format("~/content/{0}/PDF/voucher.pdf", Session["TenantKey"])));
                    var tempfile = string.Format(Server.MapPath(string.Format("~/content/{0}.pdf", Guid.NewGuid())));

                    try
                    {
                        using (var output = System.IO.File.OpenWrite(tempfile))
                        using (var stamper = new PdfStamper(reader, output))
                        {
                            stamper.AcroFields.SetField("expiry_date",
                                (DateTime.Now + TimeSpan.FromDays(14)).ToString("d"));
                            stamper.AcroFields.SetField("outlet_name 2", offer.OutletName);
                            stamper.AcroFields.SetField("booking_ins_1", bookingInsPara.ToString());

                            if (!string.IsNullOrWhiteSpace(offer.ClaimCode))
                                stamper.AcroFields.SetField("claim_code", "CODE: " + offer.ClaimCode);
                            else
                                stamper.AcroFields.SetField("claim_code", "");

                            stamper.FormFlattening = true;
                        }

                        return new FileStreamResult(System.IO.File.OpenRead(tempfile), "application/pdf");
                    }
                    finally
                    {
                        Log.Debug("Rendering PDF voucher for non mobile, Offer ID: " + offer.Id);
                        try
                        {
                            if (System.IO.File.Exists(tempfile))
                                System.IO.File.Delete(tempfile);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                else
                {
                    var file =
                        new File(
                            Server.MapPath(string.Format("~/content/{0}/PDF/voucher.pdf", Session["TenantKey"])));

                    file.Document.Form.Fields["expiry_date"].Value = (DateTime.Now + TimeSpan.FromDays(14)).ToString("d");
                    file.Document.Form.Fields["expiry_date"].ReadOnly = true;

                    file.Document.Form.Fields["outlet_name 2"].Value = offer.OutletName;
                    file.Document.Form.Fields["outlet_name 2"].ReadOnly = true;

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
            });
        }
    }
}
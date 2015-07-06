using GRG.LeisureCards.PDF.Content;

namespace GRG.LeisureCards.PDF.DocumentTemplates
{
	public interface IHtmlTemplates
	{
		DocumentTemplate<TwoForOneVoucherContent> VoucherContent{get;}
    }
}


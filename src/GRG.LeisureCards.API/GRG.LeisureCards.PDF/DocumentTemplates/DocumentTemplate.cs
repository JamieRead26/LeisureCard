using System;
using System.Linq;

namespace GRG.LeisureCards.PDF.DocumentTemplates
{
	public class DocumentTemplate<TContent> where TContent : Content
	{
		readonly string _documentTemplate;

		public DocumentTemplate(string documentTemplate)
		{
		    foreach (var field in new ContentTemplate<TContent>().Fields.Where(field => documentTemplate.IndexOf (string.Format ("#[{0}]", field)) == -1))
		        throw new Exception(
		            string.Format("HTML template for content type {0} missing field {1}", typeof(TContent).FullName, field));

		    _documentTemplate = documentTemplate;
		}

	    public string BindContent(TContent content) {

			return content.GetContent()
				.Aggregate(_documentTemplate, (current, kvp) => current.Replace("#[" + kvp.Key + "]", kvp.Value));
		}
	}
}


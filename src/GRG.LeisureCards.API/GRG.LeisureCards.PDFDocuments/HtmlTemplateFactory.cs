using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GRG.LeisureCards.PDF.DocumentTemplates;
using GRG.LeisureCards.PDFDocuments.Global;

namespace GRG.LeisureCards.PDFDocuments
{
    public interface IHtmlTemplateFactory
    {
        IHtmlTemplates GetHtmlTemplates(string tenantKey);
    }

    public class HtmlTemplateFactory : IHtmlTemplateFactory
    {
        private readonly IHtmlTemplates _globalTemplate = new HtmlTemplates();
        private readonly IDictionary<string, HtmlTemplates> _templatesByTenant;
        
        public HtmlTemplateFactory()
        {
            _templatesByTenant = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof (HtmlTemplates).IsAssignableFrom(t))
                .Select(htmlTemplateType => (HtmlTemplates) Activator.CreateInstance(htmlTemplateType))
                .ToDictionary(t => t.TenantKey.ToUpper(), t => t);
        }

        public IHtmlTemplates GetHtmlTemplates(string tenantKey)
        {
            return _templatesByTenant.ContainsKey(tenantKey.ToUpper()) 
                ? _templatesByTenant[tenantKey.ToUpper()] 
                : _globalTemplate;
        }
    }
}

using System;
using System.Configuration;
using System.Linq;
using GRG.LeisureCards.WebAPI.Client;
using GRG.LeisureCards.WebAPI.ClientContract;
using GRG.LeisureCards.WebAPI.Model;

namespace GRG.LeisureCards.UI.Shizzle
{
    public interface ITenantCache
    {
        Tenant GetTenant(string domain);
    }

    public class TenantCache : ITenantCache
    {
        public static readonly ITenantCache Instance = new TenantCache();

        private readonly ITenantService _tenantService;
        private TenantCache()
        {
            _tenantService = new TenantService(ConfigurationManager.AppSettings["ApiUrl"]);    
        }

        public Tenant GetTenant(string domain)
        {
            var all = _tenantService.GetAll();
            
            return all.First(x => string.Equals(x.Domain, domain, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
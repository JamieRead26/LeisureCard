using System;
using System.Collections.Generic;
using GRG.LeisureCards.WebAPI.ClientContract;
using GRG.LeisureCards.WebAPI.Model;
using Newtonsoft.Json;
using RestSharp;

namespace GRG.LeisureCards.WebAPI.Client
{
    public class TenantService : Service, ITenantService
    {
        public TenantService(string baseUrl) : this(baseUrl, "none") { }
        
        public TenantService(string baseUrl, string sessionToken)
            : base(baseUrl, sessionToken)
        {
        }

        public IEnumerable<Tenant> GetAll()
        {
            var request = GetRestRequest("Tenant/GetAll", Method.GET);

            var response = new RestClient(BaseUrl).Execute(request);
            return JsonConvert.DeserializeObject<List<Tenant>>(response.Content);
        }

        public Tenant Update(Tenant tenant)
        {
            var request = GetRestRequest("Tenant/Update", Method.POST);

            request.AddBody(tenant);

            return new RestClient(BaseUrl).Execute<Tenant>(request).Data;
        }

        public Tenant Save(Tenant tenant)
        {
            var request = GetRestRequest("Tenant/Save", Method.POST);

            request.AddBody(tenant);

            return new RestClient(BaseUrl).Execute<Tenant>(request).Data;
        }
    }
}
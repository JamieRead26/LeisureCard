using System.Collections;
using System.Collections.Generic;
using GRG.LeisureCards.WebAPI.Model;

namespace GRG.LeisureCards.WebAPI.ClientContract
{
    public interface ITenantService
    {
        IEnumerable<Tenant> GetAll();

        Tenant Update(Tenant tenant);

        Tenant Save(Tenant tenant);
    }
}
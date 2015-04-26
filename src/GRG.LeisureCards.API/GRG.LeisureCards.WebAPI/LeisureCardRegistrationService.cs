using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GRG.LeisureCards.Service;

namespace GRG.LeisureCards.WebAPI
{
    public class LeisureCardRegistrationService : ILeisureCardRegistrationService
    {
        public RegistrationResult Register(string cardCode)
        {
            return RegistrationResult.Ok;
        }
    }
}
﻿using System;
using GRG.LeisureCards.WebAPI.Model;

namespace GRG.LeisureCards.WebAPI.ClientContract
{
    public interface ILeisureCardService
    {
        CardUpdateResponse Update(string codeOrRef, DateTime renewalDate, bool suspended);
        SessionInfo GetSessionInfo();
        CardGenerationResponse GenerateCards(string reference, int numOfcards, int renewalPeriodMonths);
        void AcceptTerms();
    }
}

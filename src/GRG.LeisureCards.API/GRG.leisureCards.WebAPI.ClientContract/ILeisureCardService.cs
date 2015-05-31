using System;
using GRG.LeisureCards.WebAPI.Model;

namespace GRG.leisureCards.WebAPI.ClientContract
{
    public interface ILeisureCardService
    {
        CardUpdateResponse Update(string codeOrRef, DateTime renewalDateTime, bool suspended);
        SessionInfo GetSessionInfo();
        CardGenerationResponse GenerateCards(string reference, int numOfcards, int renewalPeriodMonths);
    }
}

using AutoMapper;
using ApiModel = GRG.LeisureCards.WebAPI.Model;
using DomainModel = GRG.LeisureCards.Model;

namespace GRG.LeisureCards.WebAPI.Mappings
{
    public static class Mapping
    {
        public static void Register()
        {
            Mapper.CreateMap<ApiModel.TwoForOneOffer, DomainModel.TwoForOneOffer>();
            Mapper.CreateMap<DomainModel.TwoForOneOffer, ApiModel.TwoForOneOffer>();
        }
    }
}
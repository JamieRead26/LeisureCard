using AutoMapper;

namespace GRG.LeisureCards.WebAPI.Mappings
{
    public static class Mapping
    {
        public static void Register()
        {
            Mapper.CreateMap<DomainModel.TwoForOneOffer, Model.TwoForOneOffer>();
            
            Mapper.CreateMap<DomainModel.LeisureCardUsage, Model.LeisureCardUsage>()
                .ForMember(dest => dest.LeisureCardCode, opt => opt.MapFrom(src => src.LeisureCard.Code));

            Mapper.CreateMap<DomainModel.SelectedOffer, Model.SelectedOffer>()
               .ForMember(dest => dest.LeisureCardCode, opt => opt.MapFrom(src => src.LeisureCard.Code));
        }
    }
}
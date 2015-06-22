using AutoMapper;

namespace GRG.LeisureCards.WebAPI.Mappings
{
    public static class Mapping
    {
        public static void Register()
        {
            Mapper.CreateMap<DomainModel.DataImportJournalEntry, Model.DataImportJournalEntry>();

            Mapper.CreateMap<DomainModel.SessionInfo, Model.SessionInfo>();

            Mapper.CreateMap<DomainModel.TwoForOneOffer, Model.TwoForOneOffer>();

            Mapper.CreateMap<Service.LeisureCardRegistrationResponse, Model.LeisureCardRegistrationResponse>();

            Mapper.CreateMap<DomainModel.LeisureCard, Model.LeisureCard>();

            Mapper.CreateMap<DomainModel.LeisureCardUsage, Model.LeisureCardUsage>()
                .ForMember(dest => dest.LeisureCardCode, opt => opt.MapFrom(src => src.LeisureCard.Code));

            Mapper.CreateMap<DomainModel.SelectedOffer, Model.SelectedOffer>()
               .ForMember(dest => dest.LeisureCardCode, opt => opt.MapFrom(src => src.LeisureCardCode));

            Mapper.CreateMap<DomainModel.RedLetterProduct, Model.RedLetterProductSummary>();

            Mapper.CreateMap<DomainModel.CardGenerationLog, Model.CardGenerationLog>();

            Mapper.CreateMap<DomainModel.Tenant, Model.Tenant>();
            Mapper.CreateMap<Model.Tenant, DomainModel.Tenant>();

        }
    }
}
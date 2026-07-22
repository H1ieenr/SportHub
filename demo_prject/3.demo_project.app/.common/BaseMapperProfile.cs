using AutoMapper;
using azicloud.app.contracts;
using demo_project.app.contracts;
using demo_project.domain;

namespace demo_project.app
{
    public class BaseMapperProfile : Profile
    {
        public BaseMapperProfile()
        {
            //CreateMap<BaseParamsProcedure, BaseRequestDTO>().ReverseMap();
            //CreateMap<TenantParamsProcedure, TenantRequestDTO>().ReverseMap();
            //CreateMap<MarketParamsProcedure, MarketRequestDTO>().ReverseMap();
            //CreateMap<TenantMarketParamsProcedure, TenantMarketRequestDTO>().ReverseMap();

            CreateMap<DeletedParamsProcedure, ZaloAppDeleteRequestDTO>().ReverseMap();
            CreateMap<CreateProcedure, CreateDTO>().ReverseMap();
            CreateMap<DeletedProcedure, DeleteDTO>().ReverseMap();
            CreateMap<DeletedParamsProcedure, DeletedRequestDTO>().ReverseMap();
            CreateMap<CheckProcedure, CheckDTO>().ReverseMap();
            CreateMap<CreateResponseDTO, CreateProcedure>().ReverseMap();
            CreateMap<DeletedResponseDTO, DeletedProcedure>().ReverseMap();
            CreateMap<DeleteActionResultDTO, DeletedProcedure>().ReverseMap();
            CreateMap<UpdatedResponseDTO, UpdatedProcedure>().ReverseMap();
            CreateMap<CreateProcedure,CreateActionResultDTO>().ReverseMap();
            CreateMap<DeletedRequestDTO, CustomerDeletedParamsProcedure>().ReverseMap();


            CreateMap<ActionFunction, CreateActionResultDTO>().ForMember(e => e.id, o => o.MapFrom(x => x.record_id));
            CreateMap<ActionFunction, UpdateActionResultDTO>().ForMember(e => e.id, o => o.MapFrom(x => x.record_id));
            CreateMap<ActionFunction, DeleteActionResultDTO>().ForMember(e => e.id, o => o.MapFrom(x => x.record_id));
            //CreateMap<UpdatedProcedure, UpdateDTO>().ReverseMap();
            //CreateMap<UpdatedProcedure, CreateDTO>().ReverseMap();
        }
    }
}

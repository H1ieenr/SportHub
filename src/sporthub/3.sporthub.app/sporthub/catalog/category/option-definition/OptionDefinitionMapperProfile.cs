using AutoMapper;
using sporthub.domain;
using sporthub.app.contracts;

namespace sporthub.app
{
    public class OptionDefinitionMapperProfile : Profile
    {
        public OptionDefinitionMapperProfile()
        {
            CreateMap<OptionDefinition, OptionDefinitionDTO>()
                .ForMember(d => d.option_values, o => o.MapFrom(s => s.values));;
        }

    }
}
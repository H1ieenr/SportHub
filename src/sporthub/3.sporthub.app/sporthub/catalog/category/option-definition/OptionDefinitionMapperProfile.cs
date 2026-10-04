using AutoMapper;
using sporthub.domain;
using sporthub.app.contracts;

namespace sporthub.app
{
    public class OptionDefinitionMapperProfile : Profile
    {
        public OptionDefinitionMapperProfile()
        {
            CreateMap<OptionDefinition, OptionDefinitionDTO>();
        }

    }
}
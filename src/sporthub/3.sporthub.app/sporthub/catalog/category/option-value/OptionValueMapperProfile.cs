using AutoMapper;
using sporthub.domain;
using sporthub.app.contracts;

namespace sporthub.app
{
    public class OptionValueMapperProfile : Profile
    {
        public OptionValueMapperProfile()
        {
            CreateMap<OptionValue, OptionValueDTO>();
        }
    }
}
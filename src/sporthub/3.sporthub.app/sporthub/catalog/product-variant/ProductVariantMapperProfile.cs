using AutoMapper;
using sporthub.domain;
using sporthub.app.contracts;

namespace sporthub.app
{
    public class ProductVariantMapperProfile : Profile
    {
        public ProductVariantMapperProfile()
        {
            CreateMap<ProductVariant, ProductVariantDTO>();
            CreateMap<ProductVariantOptionValue, ProductVariantOptionValueDTO>()
                .ForMember(d => d.option_definition_name, o => o.MapFrom(s => s.option_definition.name))
                .ForMember(d => d.option_value, o => o.MapFrom(s => s.option_value.value));
        }

    }
}
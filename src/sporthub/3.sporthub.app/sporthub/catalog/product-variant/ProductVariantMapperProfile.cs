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
        }

    }
}
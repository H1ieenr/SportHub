using AutoMapper;
using sporthub.domain;
using sporthub.app.contracts;

namespace sporthub.app
{
    public class ProductImageMapperProfile : Profile
    {
        public ProductImageMapperProfile()
        {
            CreateMap<ProductImage, ProductImageDTO>();
        }

    }
}
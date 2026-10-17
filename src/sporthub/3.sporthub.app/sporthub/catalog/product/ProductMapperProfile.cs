using AutoMapper;
using sporthub.domain;
using sporthub.app.contracts;

namespace sporthub.app
{
    public class ProductMapperProfile : Profile
    {
        public ProductMapperProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(d => d.category_name, o => o.MapFrom(s => s.category.name))
                .ForMember(d => d.category_slug, o => o.MapFrom(s => s.category.slug))
                .ForMember(d => d.brand_name, o => o.MapFrom(s => s.brand.name));
            CreateMap<Product, ProductGetByIdResponseDTO>()
                .ForMember(d => d.category_name, o => o.MapFrom(s => s.category.name))
                .ForMember(d => d.category_slug, o => o.MapFrom(s => s.category.slug))
                .ForMember(d => d.brand_name, o => o.MapFrom(s => s.brand.name)); ;
        }
    }
}
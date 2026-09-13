using AutoMapper;
using sporthub.domain;
using sporthub.app.contracts;

namespace sporthub.app
{
    public class ProductMapperProfile : Profile
    {
        public ProductMapperProfile()
        {
            CreateMap<Product, ProductDTO>();
        }
    }
}
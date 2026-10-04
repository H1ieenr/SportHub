using AutoMapper;
using sporthub.domain;
using sporthub.app.contracts;

namespace sporthub.app
{
    public class CategoryOptionMapperProfile : Profile
    {
        public CategoryOptionMapperProfile()
        {
            CreateMap<CategoryOption, CategoryOptionDTO>();
        }
    }
}
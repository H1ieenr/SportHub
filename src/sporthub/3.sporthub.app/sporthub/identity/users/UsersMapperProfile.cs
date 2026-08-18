using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using sporthub.domain;
using sporthub.app.contracts;
namespace sporthub.app
{
    public class UsersMapperProfile : Profile
    {
        public UsersMapperProfile()
        {
            CreateMap<Users, UsersDTO>();

            #region GetByIdAsync
            CreateMap<Users, GetByIdAsyncDTO>();
            #endregion
        }
    }
}
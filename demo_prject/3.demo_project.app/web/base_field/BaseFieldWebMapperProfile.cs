using AutoMapper;
using demo_project.app.contracts;
using demo_project.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_project.app
{
    public class BaseFieldWebMapperProfile:Profile
    {
        public BaseFieldWebMapperProfile()
        {
            CreateMap<FnBaseFieldWebGetByIdRequestDTO, FnBaseFieldWebGetByIdParamsFunction>();
            CreateMap<BaseFieldWebItem, FnBaseFieldWebGetByIdDTO>();
            CreateMap<FnBaseFieldWebDeleteRequestDTO, FnBaseFieldWebDeleteParamsFunction>();
            CreateMap<FnBaseFieldWebCreateRequestDTO, FnBaseFieldWebCreateParamsFunction>();
        }
    }
}

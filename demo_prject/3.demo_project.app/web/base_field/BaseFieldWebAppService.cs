using AutoMapper;
using azicloud.app.contracts;
using demo_project.app.contracts;
using demo_project.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_project.app
{
    public class BaseFieldWebAppService : IBaseFieldWebAppService
    {
        private readonly IBaseFieldWebRepository _baseFieldWebRepository;
        private readonly IMapper _mapper;
        public BaseFieldWebAppService(IBaseFieldWebRepository baseFieldWebRepository,
            IMapper mapper)
        {
            _baseFieldWebRepository = baseFieldWebRepository;
            _mapper = mapper;
        }

        public async Task<CreateActionResultDTO> fn_base_field_web_create(FnBaseFieldWebCreateRequestDTO model)
        {
            var dto = new CreateActionResultDTO();
            var req = _mapper.Map<FnBaseFieldWebCreateParamsFunction>(model);
            var result = await _baseFieldWebRepository.fn_base_field_web_create(req);
            dto = _mapper.Map<CreateActionResultDTO>(result);
            return dto;
        }

        public async Task<DeleteActionResultDTO> fn_base_field_web_delete(FnBaseFieldWebDeleteRequestDTO model)
        {
            var dto = new DeleteActionResultDTO();
            var req = _mapper.Map<FnBaseFieldWebDeleteParamsFunction>(model);
            var result = await _baseFieldWebRepository.fn_base_field_web_delete(req);
            dto = _mapper.Map<DeleteActionResultDTO>(result);
            return dto;
        }

        public async Task<FnBaseFieldWebGetByIdDTO> fn_base_field_web_get_by_id(FnBaseFieldWebGetByIdRequestDTO model)
        {
            var dto = new FnBaseFieldWebGetByIdDTO();
            var req = _mapper.Map<FnBaseFieldWebGetByIdParamsFunction>(model);
            var result = await _baseFieldWebRepository.fn_base_field_web_get_by_id(req);
            dto = _mapper.Map<FnBaseFieldWebGetByIdDTO>(result.base_field);
            return dto;
        }
    }
}

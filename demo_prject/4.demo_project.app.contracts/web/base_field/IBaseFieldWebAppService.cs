using azicloud.app.contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_project.app.contracts
{
    public interface IBaseFieldWebAppService
    {
        Task<CreateActionResultDTO> fn_base_field_web_create(FnBaseFieldWebCreateRequestDTO model);
        Task<DeleteActionResultDTO> fn_base_field_web_delete(FnBaseFieldWebDeleteRequestDTO model);
        Task<FnBaseFieldWebGetByIdDTO> fn_base_field_web_get_by_id(FnBaseFieldWebGetByIdRequestDTO model);
    }
}

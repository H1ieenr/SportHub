using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace demo_project.domain
{
    public interface IBaseFieldWebRepository
    {
        Task<ActionFunction> fn_base_field_web_create(FnBaseFieldWebCreateParamsFunction paramsFunction, CancellationToken cancellation = default);
        Task<ActionFunction> fn_base_field_web_delete(FnBaseFieldWebDeleteParamsFunction paramsFunction, CancellationToken cancellation = default);
        Task<FnBaseFieldWebGetByIdFunction> fn_base_field_web_get_by_id(FnBaseFieldWebGetByIdParamsFunction paramsFunction, CancellationToken cancellation = default);
    }
}
